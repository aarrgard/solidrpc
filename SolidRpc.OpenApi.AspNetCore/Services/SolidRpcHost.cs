using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SolidRpc.Abstractions;
using SolidRpc.Abstractions.InternalServices;
using SolidRpc.Abstractions.OpenApi.Binder;
using SolidRpc.Abstractions.OpenApi.Invoker;
using SolidRpc.Abstractions.Services;
using SolidRpc.Abstractions.Types;
using SolidRpc.OpenApi.AspNetCore.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

[assembly: SolidRpcService(typeof(ISolidRpcHost), typeof(SolidRpcHost), SolidRpcServiceLifetime.Scoped)]
namespace SolidRpc.OpenApi.AspNetCore.Services
{
    /// <summary>
    /// Implements the logic for a solid rpc host
    /// </summary>
    public class SolidRpcHost : ISolidRpcHost
    {
        /// <summary>
        /// Constructs a new instance
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="serviceProvider"></param>
        public SolidRpcHost(
            ILogger<SolidRpcHost> logger,
            IServiceProvider serviceProvider)
        {
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            ServiceProvider = serviceProvider;
         }

        /// <summary>
        /// The logger
        /// </summary>
        protected ILogger Logger { get; }

        /// <summary>
        /// The service provider
        /// </summary>
        protected IServiceProvider ServiceProvider { get; }

        /// <summary>
        /// The configuration
        /// </summary>
        protected IConfiguration Configuration => ServiceProvider.GetRequiredService<IConfiguration>();

        /// <summary>
        /// The application
        /// </summary>
        protected ISolidRpcApplication SolidRpcApplication => ServiceProvider.GetRequiredService<ISolidRpcApplication>();

        private IMethodAddressTransformer MethodAddressResolver => ServiceProvider.GetRequiredService<IMethodAddressTransformer>();

        /// <summary>
        /// The host stores
        /// </summary>
        protected IEnumerable<ISolidRpcHostStore> HostStores => ServiceProvider.GetRequiredService<IEnumerable<ISolidRpcHostStore>>();

        ///// <summary>
        ///// Checks the hosts
        ///// </summary>
        //protected ConcurrentDictionary<Guid, Task> HostChecks { get; }

        /// <summary>
        /// The cookies
        /// </summary>
        protected IDictionary<string, string> HttpCookies { get; set; }

        private async void RegisterHost()
        {
            // we need to complete the construction...
            await Task.Yield();

            var cancellationToken = SolidRpcApplication.ShutdownToken;

            if (Logger.IsEnabled(LogLevel.Information))
            {
                Logger.LogInformation("Registering host " + SolidRpcApplication.HostId);
            }

            //
            // register this instance in all the stores.
            //
            var hostInstance = await GetHostInstance(cancellationToken);
            var registerTasks = HostStores.Select(o => o.AddHostInstanceAsync(hostInstance, cancellationToken));
            await Task.WhenAll(registerTasks);
        }

        private async Task ShutdownHost()
        {
            try
            {
                var cancellationToken = CancellationToken.None;
                var hostInstance = await GetHostInstance(cancellationToken);
                var removeTasks = HostStores.Select(o => o.RemoveHostInstanceAsync(hostInstance, cancellationToken));
                await Task.WhenAll(removeTasks);
            }
            catch (Exception e)
            {
                Logger.LogError(e, "Failed to shutdown host gracefully.");
            }
            finally
            {
                SolidRpcApplication.StopApplication();
            }
        }

        /// <summary>
        /// Syncronizes the hosts
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<IEnumerable<SolidRpcHostInstance>> SyncHostsFromStore(CancellationToken cancellationToken = default(CancellationToken))
        {
            //
            // list hosts from store
            //
            var listTasks = HostStores.Select(o => o.ListHostInstancesAsync(cancellationToken));
            var registrations = await Task.WhenAll(listTasks);
            var allHosts = registrations.SelectMany(o => o).GroupBy(o => o.HostId).Select(o => o.First());

            return allHosts;
        }

        /// <summary>
        /// Returns the host configuration
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<IEnumerable<NameValuePair>> GetHostConfiguration(CancellationToken cancellationToken = default(CancellationToken))
        {
            var lst = new List<NameValuePair>();
            foreach(var o in Configuration.GetChildren())
            {
                lst.Add(new NameValuePair() {
                    Name = $"{o.Key}",
                    Value = o.Value
                });
            }
            return Task.FromResult<IEnumerable<NameValuePair>>(lst);
        }

        /// <summary>
        /// Returns the host id
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<Guid> GetHostId(CancellationToken cancellationToken = default(CancellationToken))
        {
            return Task.FromResult(SolidRpcApplication.HostId);
        }

        /// <summary>
        /// Returns the host instance
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<SolidRpcHostInstance> GetHostInstance(CancellationToken cancellationToken = default(CancellationToken))
        {
            return Task.FromResult(new SolidRpcHostInstance()
            {
                HostId = SolidRpcApplication.HostId,
                Started = SolidRpcApplication.Started,
                LastAlive = DateTimeOffset.Now,
                HttpCookies = new Dictionary<string, string>(),
                BaseAddress = MethodAddressResolver.BaseAddress
            });
        }

        /// <summary>
        /// determines if this host is alive.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual Task IsAlive(CancellationToken cancellationToken = default(CancellationToken))
        {
            return Task.CompletedTask;
        }

        /// <summary>
        /// proxy the request
        /// </summary>
        /// <param name="hostInstance"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<SolidRpcHostInstance> CheckHost(SolidRpcHostInstance hostInstance, CancellationToken cancellationToken = default(CancellationToken))
        {
            var httpInvoker = ServiceProvider.GetRequiredService<IInvoker<ISolidRpcHost>>();
            return httpInvoker.InvokeAsync(o => o.GetHostInstance(cancellationToken));

        }

        /// <summary>
        /// Returns the base address
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<Uri> BaseAddress(CancellationToken cancellationToken = default)
        {
            return Task.FromResult(MethodAddressResolver.BaseAddress);
        }

        /// <summary>
        /// The allowed cors origins
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<IEnumerable<string>> AllowedCorsOrigins(CancellationToken cancellationToken = default)
        {
            return Task.FromResult(MethodAddressResolver.AllowedCorsOrigins);
        }
    }
}
