using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SolidRpc.Abstractions;
using SolidRpc.Abstractions.Services;
using SolidRpc.Abstractions.Types;
using SolidRpc.OpenApi.AspNetCore.Services;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

[assembly: SolidRpcService(typeof(ISolidRpcHost), typeof(SolidRpcHost))]
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
        /// <param name="configuration"></param>
        public SolidRpcHost(ILogger<SolidRpcHost> logger, IConfiguration configuration)
        {
            HostId = Guid.NewGuid();
            Logger = logger;
            Configuration = configuration;
        }

        protected ILogger Logger { get; }
        protected IConfiguration Configuration { get; }
        protected Guid HostId { get; }

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
            return Task.FromResult(HostId);
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
    }
}
