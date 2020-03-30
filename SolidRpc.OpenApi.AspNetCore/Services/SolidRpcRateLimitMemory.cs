using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SolidRpc.Abstractions;
using SolidRpc.Abstractions.Services.RateLimit;
using SolidRpc.Abstractions.Types.RateLimit;
using SolidRpc.OpenApi.AspNetCore.Services;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

[assembly: SolidRpcService(typeof(ISolidRpcRateLimit), typeof(SolidRpcRateLimitMemory))]
namespace SolidRpc.OpenApi.AspNetCore.Services
{
    /// <summary>
    /// Implements the logic for an inmemory rate limiting service.
    /// </summary>
    public class SolidRpcRateLimitMemory : ISolidRpcRateLimit
    {

        private class ResourceItem
        {
            private SemaphoreSlim _semaphore = new SemaphoreSlim(0);
            private HashSet<Guid> _leases = new HashSet<Guid>();

            public ResourceItem(string resourceName)
            {
                ResourceName = resourceName;
            }
            public string ResourceName { get; }
            public int? MaxConcurrentCalls { get; set; }

            public void RemoveToken(Guid id)
            {
                lock(_leases)
                {
                    _leases.Remove(id);
                }
                _semaphore.Release();
            }

            public async Task<Guid> GetTokenAsync(TimeSpan timeout, CancellationToken cancellationToken)
            {
                var timedOut = DateTime.Now.Add(timeout);
                do
                {
                    lock (_leases)
                    {
                        if(CanAddLease())
                        {
                            var lease = Guid.NewGuid();
                            _leases.Add(lease);
                            return lease;
                        }
                    }
                    await _semaphore.WaitAsync(timedOut - DateTime.Now, cancellationToken);
                }
                while (DateTime.Now < timedOut);

                return Guid.Empty;
            }

            private bool CanAddLease()
            {
                if(MaxConcurrentCalls != null)
                {
                    if (_leases.Count >= MaxConcurrentCalls)
                    {
                        return false;
                    }
                }
                return true;
            }
        }

        public SolidRpcRateLimitMemory(ILogger<SolidRpcRateLimitMemory> logger, IConfiguration configuration)
        {
            Logger = logger;
            Configuration = configuration;
            ResourceItems = new ConcurrentDictionary<string, ResourceItem>();
        }

        private ILogger Logger { get; }
        public IConfiguration Configuration { get; }
        private ConcurrentDictionary<string, ResourceItem> ResourceItems { get; }

        private ResourceItem CreateResourceItem(string resourceName)
        {
            if(Logger.IsEnabled(LogLevel.Trace))
            {
                Logger.LogTrace($"Looking for configuration setting SolidRpcRateLimit:{resourceName}:");
            }
            var resourceItem = new ResourceItem(resourceName);
           
            if(int.TryParse(Configuration[$"SolidRpcRateLimit:{resourceName}:{nameof(RateLimitSetting.MaxConcurrentCalls)}"], out int maxConcurrentCalls))
            {
                resourceItem.MaxConcurrentCalls = maxConcurrentCalls;
            }

            return resourceItem;
            
        }

        public Task<IEnumerable<RateLimitSetting>> GetRateLimitSettingsAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            var settings = ResourceItems.Values.Select(o => new RateLimitSetting()
            {
                ResourceName = o.ResourceName,
                MaxConcurrentCalls = o.MaxConcurrentCalls
            });
            return Task.FromResult(settings);
        }

        public async Task<RateLimitToken> GetRateLimitTokenAsync(string resourceName, TimeSpan timeout, CancellationToken cancellationToken = default(CancellationToken))
        {
            var resourceItem = ResourceItems.GetOrAdd(resourceName, CreateResourceItem);
            var token = await resourceItem.GetTokenAsync(timeout, cancellationToken);
            return new RateLimitToken()
            {
                ResourceName = resourceName,
                Id = token
            };
        }

        public Task ReturnRateLimitTokenAsync(RateLimitToken rateLimitToken, CancellationToken cancellationToken = default(CancellationToken))
        {
            var resourceItem = ResourceItems.GetOrAdd(rateLimitToken.ResourceName, CreateResourceItem);
            resourceItem.RemoveToken(rateLimitToken.Id);
            return Task.CompletedTask;
        }

        public Task UpdateRateLimitSetting(RateLimitSetting setting, CancellationToken cancellationToken = default(CancellationToken))
        {
            var resourceItem = ResourceItems.GetOrAdd(setting.ResourceName, CreateResourceItem);
            resourceItem.MaxConcurrentCalls = setting.MaxConcurrentCalls;
            return Task.CompletedTask;
        }
    }
}
