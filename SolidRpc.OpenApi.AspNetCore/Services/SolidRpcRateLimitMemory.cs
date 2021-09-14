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

[assembly: SolidRpcService(typeof(ISolidRpcRateLimit), typeof(SolidRpcRateLimitMemory), SolidRpcServiceLifetime.Singleton, SolidRpcServiceInstances.Many)]
namespace SolidRpc.OpenApi.AspNetCore.Services
{
    /// <summary>
    /// Implements the logic for an inmemory rate limiting service.
    /// </summary>
    public class SolidRpcRateLimitMemory : ISolidRpcRateLimit
    {
        private static readonly LeaseToken EmptyLease = new LeaseToken(Guid.Empty);

        private class LeaseToken
        {
            public LeaseToken()
            {
                Id = Guid.NewGuid();
                Expires = DateTimeOffset.Now.AddMinutes(1);
            }
            internal LeaseToken(Guid id)
            {
                Id = id;
                Expires = DateTimeOffset.MinValue;
            }
            public Guid Id { get; }
            public DateTimeOffset Expires { get; }
        }


        private class ResourceItem
        {
            private object _mutex = new object();
            private SemaphoreSlim _semaphore = new SemaphoreSlim(0);
            private LinkedList<LeaseToken> _leases = new LinkedList<LeaseToken>();
            private Dictionary<Guid, LinkedListNode<LeaseToken>> _leasesById = new Dictionary<Guid, LinkedListNode<LeaseToken>>();

            public ResourceItem(string resourceName)
            {
                ResourceName = resourceName;
            }
            public string ResourceName { get; }
            public int? MaxConcurrentCalls { get; set; }

            public void RemoveToken(Guid id)
            {
                lock(_mutex)
                {
                    if(_leasesById.TryGetValue(id, out LinkedListNode<LeaseToken> leaseToken))
                    {
                        _leases.Remove(leaseToken);
                        _leasesById.Remove(id);
                    }
                }
                _semaphore.Release();
            }

            public async Task<LeaseToken> GetTokenAsync(TimeSpan timeout, CancellationToken cancellationToken)
            {
                var timedOut = DateTime.Now.Add(timeout);
                do
                {
                    lock (_mutex)
                    {
                        if(CanAddLease())
                        {
                            var lease = new LeaseToken();
                            var leaseNode = _leases.AddLast(lease);
                            _leasesById.Add(lease.Id, leaseNode);
                            return lease;
                        }
                    }

                    //
                    // wait until next lease expires or until we time out.
                    //
                    var wait = timedOut - DateTime.Now;
                    var nextLeaseExpires = _leases.First?.Value?.Expires;
                    if(nextLeaseExpires != null)
                    {
                        var expireWait = nextLeaseExpires.Value - DateTime.Now;
                        if(expireWait < wait)
                        {
                            wait = expireWait;
                        }
                    }

                    await _semaphore.WaitAsync(wait, cancellationToken);
                }
                while (DateTime.Now < timedOut);

                return EmptyLease;
            }

            private bool CanAddLease()
            {
                while(_leases.First?.Value?.Expires < DateTimeOffset.Now)
                {
                    var leaseNode = _leases.First;
                    _leases.Remove(leaseNode);
                    _leasesById.Remove(leaseNode.Value.Id);
                }
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

        /// <summary>
        /// Constructs a new instance
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="configuration"></param>
        public SolidRpcRateLimitMemory(ILogger<SolidRpcRateLimitMemory> logger, IConfiguration configuration)
        {
            Logger = logger;
            Configuration = configuration;
            ResourceItems = new ConcurrentDictionary<string, ResourceItem>();
        }

        private ILogger Logger { get; }
        private IConfiguration Configuration { get; }
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

        /// <summary>
        /// Returns the rate limit settings
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<IEnumerable<RateLimitSetting>> GetRateLimitSettingsAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            var settings = ResourceItems.Values.Select(o => new RateLimitSetting()
            {
                ResourceName = o.ResourceName,
                MaxConcurrentCalls = o.MaxConcurrentCalls
            });
            return Task.FromResult(settings);
        }
        /// <summary>
        /// Returns the rate limit token
        /// </summary>
        /// <param name="resourceName"></param>
        /// <param name="timeout"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<RateLimitToken> GetRateLimitTokenAsync(string resourceName, TimeSpan timeout, CancellationToken cancellationToken = default(CancellationToken))
        {
            var resourceItem = ResourceItems.GetOrAdd(resourceName, CreateResourceItem);
            var token = await resourceItem.GetTokenAsync(timeout, cancellationToken);
            return new RateLimitToken()
            {
                ResourceName = resourceName,
                Id = token.Id,
                Expires = token.Expires
            };
        }
        /// <summary>
        /// Returns the rate limit settings
        /// </summary>
        /// <param name="rateLimitToken"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task ReturnRateLimitTokenAsync(RateLimitToken rateLimitToken, CancellationToken cancellationToken = default(CancellationToken))
        {
            var resourceItem = ResourceItems.GetOrAdd(rateLimitToken.ResourceName, CreateResourceItem);
            resourceItem.RemoveToken(rateLimitToken.Id);
            return Task.CompletedTask;
        }

        /// <summary>
        /// Updates the rate limit settings
        /// </summary>
        /// <param name="setting"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task UpdateRateLimitSetting(RateLimitSetting setting, CancellationToken cancellationToken = default(CancellationToken))
        {
            var resourceItem = ResourceItems.GetOrAdd(setting.ResourceName, CreateResourceItem);
            resourceItem.MaxConcurrentCalls = setting.MaxConcurrentCalls;
            return Task.CompletedTask;
        }

        /// <summary>
        /// Returns the singleton token
        /// </summary>
        /// <param name="resourceName"></param>
        /// <param name="timeout"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<RateLimitToken> GetSingeltonTokenAsync(string resourceName, TimeSpan timeout, CancellationToken cancellationToken = default(CancellationToken))
        {
            var resourceItem = ResourceItems.GetOrAdd(resourceName, CreateResourceItem);
            resourceItem.MaxConcurrentCalls = 1;
            var token = await resourceItem.GetTokenAsync(timeout, cancellationToken);
            return new RateLimitToken()
            {
                ResourceName = resourceName,
                Id = token.Id,
                Expires = token.Expires
            };
        }
    }
}
