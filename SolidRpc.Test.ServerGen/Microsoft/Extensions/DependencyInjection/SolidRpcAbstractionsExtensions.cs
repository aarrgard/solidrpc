namespace Microsoft.Extensions.DependencyInjection {
    /// <summary>
    /// 
    /// </summary>
    public static class SolidRpcAbstractionsExtensions {
        /// <summary>
        /// 
        /// </summary>
        public class SolidRpcAbstractionsServicesISolidRpcAcmeChallengeProxy : Microsoft.Extensions.DependencyInjection.SolidRpcExtensions.Proxy<SolidRpc.Abstractions.Services.ISolidRpcAcmeChallenge>,SolidRpc.Abstractions.Services.ISolidRpcAcmeChallenge {
            /// <summary>
            /// 
            /// </summary>
            public SolidRpcAbstractionsServicesISolidRpcAcmeChallengeProxy(
                System.IServiceProvider serviceProvider,
                Microsoft.Extensions.DependencyInjection.SolidRpcExtensions.IProxyConfig<SolidRpc.Abstractions.Services.ISolidRpcAcmeChallenge> config) : base(serviceProvider, config)
            {}
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_SetAcmeChallengeAsync_challenge_cancellation = GetMethodInfo("SetAcmeChallengeAsync", new System.Type[] {typeof(System.String), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="challenge"></param>
            /// <param name="cancellation"></param>
            public System.Threading.Tasks.Task SetAcmeChallengeAsync(
                System.String challenge,
                System.Threading.CancellationToken cancellation) {
                var impl = (SolidRpc.Abstractions.Services.ISolidRpcAcmeChallenge)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync(_serviceProvider, impl, mi_SetAcmeChallengeAsync_challenge_cancellation, new object[] {challenge, cancellation}, () => impl.SetAcmeChallengeAsync(challenge, cancellation));
            }
        
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_GetAcmeChallengeAsync_key_cancellation = GetMethodInfo("GetAcmeChallengeAsync", new System.Type[] {typeof(System.String), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="key"></param>
            /// <param name="cancellation"></param>
            [SolidRpc.Abstractions.OpenApi(Path="/.well-known/acme-challenge/{key}")]
            public System.Threading.Tasks.Task<SolidRpc.Abstractions.Types.FileContent> GetAcmeChallengeAsync(
                System.String key,
                System.Threading.CancellationToken cancellation) {
                var impl = (SolidRpc.Abstractions.Services.ISolidRpcAcmeChallenge)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync<SolidRpc.Abstractions.Types.FileContent>(_serviceProvider, impl, mi_GetAcmeChallengeAsync_key_cancellation, new object[] {key, cancellation}, () => impl.GetAcmeChallengeAsync(key, cancellation));
            }
        
        }
    
        /// <summary>
        /// 
        /// </summary>
        public class SolidRpcAbstractionsServicesISolidRpcContentHandlerProxy : Microsoft.Extensions.DependencyInjection.SolidRpcExtensions.Proxy<SolidRpc.Abstractions.Services.ISolidRpcContentHandler>,SolidRpc.Abstractions.Services.ISolidRpcContentHandler {
            /// <summary>
            /// 
            /// </summary>
            public SolidRpcAbstractionsServicesISolidRpcContentHandlerProxy(
                System.IServiceProvider serviceProvider,
                Microsoft.Extensions.DependencyInjection.SolidRpcExtensions.IProxyConfig<SolidRpc.Abstractions.Services.ISolidRpcContentHandler> config) : base(serviceProvider, config)
            {}
            /// <summary>
            /// 
            /// </summary>
            public System.Collections.Generic.IEnumerable<System.String> PathPrefixes {
                get {
                    throw new System.NotImplementedException();
                }
             }
        
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_GetContent_path_cancellationToken = GetMethodInfo("GetContent", new System.Type[] {typeof(System.String), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="path"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<SolidRpc.Abstractions.Types.FileContent> GetContent(
                System.String path,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (SolidRpc.Abstractions.Services.ISolidRpcContentHandler)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync<SolidRpc.Abstractions.Types.FileContent>(_serviceProvider, impl, mi_GetContent_path_cancellationToken, new object[] {path, cancellationToken}, () => impl.GetContent(path, cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_GetPathMappingsAsync_redirects_cancellationToken = GetMethodInfo("GetPathMappingsAsync", new System.Type[] {typeof(System.Boolean), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="redirects"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<System.Collections.Generic.IEnumerable<SolidRpc.Abstractions.Types.NameValuePair>> GetPathMappingsAsync(
                System.Boolean redirects,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (SolidRpc.Abstractions.Services.ISolidRpcContentHandler)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync<System.Collections.Generic.IEnumerable<SolidRpc.Abstractions.Types.NameValuePair>>(_serviceProvider, impl, mi_GetPathMappingsAsync_redirects_cancellationToken, new object[] {redirects, cancellationToken}, () => impl.GetPathMappingsAsync(redirects, cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_GetProtectedContentAsync_resource_cancellationToken = GetMethodInfo("GetProtectedContentAsync", new System.Type[] {typeof(System.Byte[]), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="resource"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<SolidRpc.Abstractions.Types.FileContent> GetProtectedContentAsync(
                System.Byte[] resource,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (SolidRpc.Abstractions.Services.ISolidRpcContentHandler)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync<SolidRpc.Abstractions.Types.FileContent>(_serviceProvider, impl, mi_GetProtectedContentAsync_resource_cancellationToken, new object[] {resource, cancellationToken}, () => impl.GetProtectedContentAsync(resource, cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_GetProtectedContentAsync_resource_fileName_cancellationToken = GetMethodInfo("GetProtectedContentAsync", new System.Type[] {typeof(System.Byte[]), typeof(System.String), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="resource"></param>
            /// <param name="fileName"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<SolidRpc.Abstractions.Types.FileContent> GetProtectedContentAsync(
                System.Byte[] resource,
                System.String fileName,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (SolidRpc.Abstractions.Services.ISolidRpcContentHandler)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync<SolidRpc.Abstractions.Types.FileContent>(_serviceProvider, impl, mi_GetProtectedContentAsync_resource_fileName_cancellationToken, new object[] {resource, fileName, cancellationToken}, () => impl.GetProtectedContentAsync(resource, fileName, cancellationToken));
            }
        
        }
    
        /// <summary>
        /// 
        /// </summary>
        public class SolidRpcAbstractionsServicesISolidRpcHostProxy : Microsoft.Extensions.DependencyInjection.SolidRpcExtensions.Proxy<SolidRpc.Abstractions.Services.ISolidRpcHost>,SolidRpc.Abstractions.Services.ISolidRpcHost {
            /// <summary>
            /// 
            /// </summary>
            public SolidRpcAbstractionsServicesISolidRpcHostProxy(
                System.IServiceProvider serviceProvider,
                Microsoft.Extensions.DependencyInjection.SolidRpcExtensions.IProxyConfig<SolidRpc.Abstractions.Services.ISolidRpcHost> config) : base(serviceProvider, config)
            {}
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_GetHostId_cancellationToken = GetMethodInfo("GetHostId", new System.Type[] {typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="cancellationToken"></param>
            [SolidRpc.Abstractions.Security(new [] {"SolidRpcHostPermission"})]
            public System.Threading.Tasks.Task<System.Guid> GetHostId(
                System.Threading.CancellationToken cancellationToken) {
                var impl = (SolidRpc.Abstractions.Services.ISolidRpcHost)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync<System.Guid>(_serviceProvider, impl, mi_GetHostId_cancellationToken, new object[] {cancellationToken}, () => impl.GetHostId(cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_GetHostInstance_cancellationToken = GetMethodInfo("GetHostInstance", new System.Type[] {typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="cancellationToken"></param>
            [SolidRpc.Abstractions.Security(new [] {"SolidRpcHostPermission"})]
            public System.Threading.Tasks.Task<SolidRpc.Abstractions.Types.SolidRpcHostInstance> GetHostInstance(
                System.Threading.CancellationToken cancellationToken) {
                var impl = (SolidRpc.Abstractions.Services.ISolidRpcHost)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync<SolidRpc.Abstractions.Types.SolidRpcHostInstance>(_serviceProvider, impl, mi_GetHostInstance_cancellationToken, new object[] {cancellationToken}, () => impl.GetHostInstance(cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_SyncHostsFromStore_cancellationToken = GetMethodInfo("SyncHostsFromStore", new System.Type[] {typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="cancellationToken"></param>
            [SolidRpc.Abstractions.Security(new [] {"SolidRpcHostPermission"})]
            public System.Threading.Tasks.Task<System.Collections.Generic.IEnumerable<SolidRpc.Abstractions.Types.SolidRpcHostInstance>> SyncHostsFromStore(
                System.Threading.CancellationToken cancellationToken) {
                var impl = (SolidRpc.Abstractions.Services.ISolidRpcHost)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync<System.Collections.Generic.IEnumerable<SolidRpc.Abstractions.Types.SolidRpcHostInstance>>(_serviceProvider, impl, mi_SyncHostsFromStore_cancellationToken, new object[] {cancellationToken}, () => impl.SyncHostsFromStore(cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_CheckHost_hostInstance_cancellationToken = GetMethodInfo("CheckHost", new System.Type[] {typeof(SolidRpc.Abstractions.Types.SolidRpcHostInstance), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="hostInstance"></param>
            /// <param name="cancellationToken"></param>
            [SolidRpc.Abstractions.Security(new [] {"SolidRpcHostPermission"})]
            public System.Threading.Tasks.Task<SolidRpc.Abstractions.Types.SolidRpcHostInstance> CheckHost(
                SolidRpc.Abstractions.Types.SolidRpcHostInstance hostInstance,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (SolidRpc.Abstractions.Services.ISolidRpcHost)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync<SolidRpc.Abstractions.Types.SolidRpcHostInstance>(_serviceProvider, impl, mi_CheckHost_hostInstance_cancellationToken, new object[] {hostInstance, cancellationToken}, () => impl.CheckHost(hostInstance, cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_GetHostConfiguration_cancellationToken = GetMethodInfo("GetHostConfiguration", new System.Type[] {typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="cancellationToken"></param>
            [SolidRpc.Abstractions.Security(new [] {"SolidRpcHostPermission"})]
            public System.Threading.Tasks.Task<System.Collections.Generic.IEnumerable<SolidRpc.Abstractions.Types.NameValuePair>> GetHostConfiguration(
                System.Threading.CancellationToken cancellationToken) {
                var impl = (SolidRpc.Abstractions.Services.ISolidRpcHost)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync<System.Collections.Generic.IEnumerable<SolidRpc.Abstractions.Types.NameValuePair>>(_serviceProvider, impl, mi_GetHostConfiguration_cancellationToken, new object[] {cancellationToken}, () => impl.GetHostConfiguration(cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_IsAlive_cancellationToken = GetMethodInfo("IsAlive", new System.Type[] {typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="cancellationToken"></param>
            [SolidRpc.Abstractions.Security(new [] {"SolidRpcHostPermission"})]
            public System.Threading.Tasks.Task IsAlive(
                System.Threading.CancellationToken cancellationToken) {
                var impl = (SolidRpc.Abstractions.Services.ISolidRpcHost)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync(_serviceProvider, impl, mi_IsAlive_cancellationToken, new object[] {cancellationToken}, () => impl.IsAlive(cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_BaseAddress_cancellationToken = GetMethodInfo("BaseAddress", new System.Type[] {typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="cancellationToken"></param>
            [SolidRpc.Abstractions.Security(new [] {"SolidRpcHostPermission"})]
            public System.Threading.Tasks.Task<System.Uri> BaseAddress(
                System.Threading.CancellationToken cancellationToken) {
                var impl = (SolidRpc.Abstractions.Services.ISolidRpcHost)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync<System.Uri>(_serviceProvider, impl, mi_BaseAddress_cancellationToken, new object[] {cancellationToken}, () => impl.BaseAddress(cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_AllowedCorsOrigins_cancellationToken = GetMethodInfo("AllowedCorsOrigins", new System.Type[] {typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="cancellationToken"></param>
            [SolidRpc.Abstractions.Security(new [] {"SolidRpcHostPermission"})]
            public System.Threading.Tasks.Task<System.Collections.Generic.IEnumerable<System.String>> AllowedCorsOrigins(
                System.Threading.CancellationToken cancellationToken) {
                var impl = (SolidRpc.Abstractions.Services.ISolidRpcHost)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync<System.Collections.Generic.IEnumerable<System.String>>(_serviceProvider, impl, mi_AllowedCorsOrigins_cancellationToken, new object[] {cancellationToken}, () => impl.AllowedCorsOrigins(cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_DefaultTimezone_cancellationToken = GetMethodInfo("DefaultTimezone", new System.Type[] {typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="cancellationToken"></param>
            [SolidRpc.Abstractions.Security(new [] {"SolidRpcHostPermission"})]
            public System.Threading.Tasks.Task<System.String> DefaultTimezone(
                System.Threading.CancellationToken cancellationToken) {
                var impl = (SolidRpc.Abstractions.Services.ISolidRpcHost)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync<System.String>(_serviceProvider, impl, mi_DefaultTimezone_cancellationToken, new object[] {cancellationToken}, () => impl.DefaultTimezone(cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_ParseDateTime_dateTime_cancellationToken = GetMethodInfo("ParseDateTime", new System.Type[] {typeof(System.String), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="dateTime"></param>
            /// <param name="cancellationToken"></param>
            [SolidRpc.Abstractions.Security(new [] {"SolidRpcHostPermission"})]
            public System.Threading.Tasks.Task<System.DateTimeOffset> ParseDateTime(
                System.String dateTime,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (SolidRpc.Abstractions.Services.ISolidRpcHost)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync<System.DateTimeOffset>(_serviceProvider, impl, mi_ParseDateTime_dateTime_cancellationToken, new object[] {dateTime, cancellationToken}, () => impl.ParseDateTime(dateTime, cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_ListAssemblyNames_cancellationToken = GetMethodInfo("ListAssemblyNames", new System.Type[] {typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="cancellationToken"></param>
            [SolidRpc.Abstractions.Security(new [] {"SolidRpcHostPermission"})]
            public System.Threading.Tasks.Task<System.Collections.Generic.IEnumerable<System.String>> ListAssemblyNames(
                System.Threading.CancellationToken cancellationToken) {
                var impl = (SolidRpc.Abstractions.Services.ISolidRpcHost)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync<System.Collections.Generic.IEnumerable<System.String>>(_serviceProvider, impl, mi_ListAssemblyNames_cancellationToken, new object[] {cancellationToken}, () => impl.ListAssemblyNames(cancellationToken));
            }
        
        }
    
        /// <summary>
        /// 
        /// </summary>
        public class SolidRpcAbstractionsServicesISolidRpcHostStoreProxy : Microsoft.Extensions.DependencyInjection.SolidRpcExtensions.Proxy<SolidRpc.Abstractions.Services.ISolidRpcHostStore>,SolidRpc.Abstractions.Services.ISolidRpcHostStore {
            /// <summary>
            /// 
            /// </summary>
            public SolidRpcAbstractionsServicesISolidRpcHostStoreProxy(
                System.IServiceProvider serviceProvider,
                Microsoft.Extensions.DependencyInjection.SolidRpcExtensions.IProxyConfig<SolidRpc.Abstractions.Services.ISolidRpcHostStore> config) : base(serviceProvider, config)
            {}
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_AddHostInstanceAsync_hostInstance_cancellationToken = GetMethodInfo("AddHostInstanceAsync", new System.Type[] {typeof(SolidRpc.Abstractions.Types.SolidRpcHostInstance), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="hostInstance"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task AddHostInstanceAsync(
                SolidRpc.Abstractions.Types.SolidRpcHostInstance hostInstance,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (SolidRpc.Abstractions.Services.ISolidRpcHostStore)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync(_serviceProvider, impl, mi_AddHostInstanceAsync_hostInstance_cancellationToken, new object[] {hostInstance, cancellationToken}, () => impl.AddHostInstanceAsync(hostInstance, cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_RemoveHostInstanceAsync_hostInstance_cancellationToken = GetMethodInfo("RemoveHostInstanceAsync", new System.Type[] {typeof(SolidRpc.Abstractions.Types.SolidRpcHostInstance), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="hostInstance"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task RemoveHostInstanceAsync(
                SolidRpc.Abstractions.Types.SolidRpcHostInstance hostInstance,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (SolidRpc.Abstractions.Services.ISolidRpcHostStore)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync(_serviceProvider, impl, mi_RemoveHostInstanceAsync_hostInstance_cancellationToken, new object[] {hostInstance, cancellationToken}, () => impl.RemoveHostInstanceAsync(hostInstance, cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_ListHostInstancesAsync_cancellationToken = GetMethodInfo("ListHostInstancesAsync", new System.Type[] {typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<System.Collections.Generic.IEnumerable<SolidRpc.Abstractions.Types.SolidRpcHostInstance>> ListHostInstancesAsync(
                System.Threading.CancellationToken cancellationToken) {
                var impl = (SolidRpc.Abstractions.Services.ISolidRpcHostStore)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync<System.Collections.Generic.IEnumerable<SolidRpc.Abstractions.Types.SolidRpcHostInstance>>(_serviceProvider, impl, mi_ListHostInstancesAsync_cancellationToken, new object[] {cancellationToken}, () => impl.ListHostInstancesAsync(cancellationToken));
            }
        
        }
    
        /// <summary>
        /// 
        /// </summary>
        public class SolidRpcAbstractionsServicesISolidRpcMethodStoreProxy : Microsoft.Extensions.DependencyInjection.SolidRpcExtensions.Proxy<SolidRpc.Abstractions.Services.ISolidRpcMethodStore>,SolidRpc.Abstractions.Services.ISolidRpcMethodStore {
            /// <summary>
            /// 
            /// </summary>
            public SolidRpcAbstractionsServicesISolidRpcMethodStoreProxy(
                System.IServiceProvider serviceProvider,
                Microsoft.Extensions.DependencyInjection.SolidRpcExtensions.IProxyConfig<SolidRpc.Abstractions.Services.ISolidRpcMethodStore> config) : base(serviceProvider, config)
            {}
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_GetHttpRequestAsync_takeCount_cancellation = GetMethodInfo("GetHttpRequestAsync", new System.Type[] {typeof(System.Nullable<System.Int32>), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="takeCount"></param>
            /// <param name="cancellation"></param>
            public System.Threading.Tasks.Task<System.Collections.Generic.IEnumerable<SolidRpc.Abstractions.Types.HttpRequest>> GetHttpRequestAsync(
                System.Int32? takeCount,
                System.Threading.CancellationToken cancellation) {
                var impl = (SolidRpc.Abstractions.Services.ISolidRpcMethodStore)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync<System.Collections.Generic.IEnumerable<SolidRpc.Abstractions.Types.HttpRequest>>(_serviceProvider, impl, mi_GetHttpRequestAsync_takeCount_cancellation, new object[] {takeCount, cancellation}, () => impl.GetHttpRequestAsync(takeCount, cancellation));
            }
        
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_RemoveHttpRequestAsync_solidRpcCallId_cancellation = GetMethodInfo("RemoveHttpRequestAsync", new System.Type[] {typeof(System.String), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="solidRpcCallId"></param>
            /// <param name="cancellation"></param>
            public System.Threading.Tasks.Task RemoveHttpRequestAsync(
                System.String solidRpcCallId,
                System.Threading.CancellationToken cancellation) {
                var impl = (SolidRpc.Abstractions.Services.ISolidRpcMethodStore)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync(_serviceProvider, impl, mi_RemoveHttpRequestAsync_solidRpcCallId_cancellation, new object[] {solidRpcCallId, cancellation}, () => impl.RemoveHttpRequestAsync(solidRpcCallId, cancellation));
            }
        
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_GetHttpRequestsAsync_sessionId_takeCount_cancellation = GetMethodInfo("GetHttpRequestsAsync", new System.Type[] {typeof(System.String), typeof(System.Nullable<System.Int32>), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="sessionId"></param>
            /// <param name="takeCount"></param>
            /// <param name="cancellation"></param>
            public System.Threading.Tasks.Task<System.Collections.Generic.IEnumerable<SolidRpc.Abstractions.Types.HttpRequest>> GetHttpRequestsAsync(
                System.String sessionId,
                System.Int32? takeCount,
                System.Threading.CancellationToken cancellation) {
                var impl = (SolidRpc.Abstractions.Services.ISolidRpcMethodStore)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync<System.Collections.Generic.IEnumerable<SolidRpc.Abstractions.Types.HttpRequest>>(_serviceProvider, impl, mi_GetHttpRequestsAsync_sessionId_takeCount_cancellation, new object[] {sessionId, takeCount, cancellation}, () => impl.GetHttpRequestsAsync(sessionId, takeCount, cancellation));
            }
        
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_RemoveHttpRequestAsync_sessionId_solidRpcCallId_cancellation = GetMethodInfo("RemoveHttpRequestAsync", new System.Type[] {typeof(System.String), typeof(System.String), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="sessionId"></param>
            /// <param name="solidRpcCallId"></param>
            /// <param name="cancellation"></param>
            public System.Threading.Tasks.Task RemoveHttpRequestAsync(
                System.String sessionId,
                System.String solidRpcCallId,
                System.Threading.CancellationToken cancellation) {
                var impl = (SolidRpc.Abstractions.Services.ISolidRpcMethodStore)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync(_serviceProvider, impl, mi_RemoveHttpRequestAsync_sessionId_solidRpcCallId_cancellation, new object[] {sessionId, solidRpcCallId, cancellation}, () => impl.RemoveHttpRequestAsync(sessionId, solidRpcCallId, cancellation));
            }
        
        }
    
        /// <summary>
        /// 
        /// </summary>
        public class SolidRpcAbstractionsServicesISolidRpcOAuth2Proxy : Microsoft.Extensions.DependencyInjection.SolidRpcExtensions.Proxy<SolidRpc.Abstractions.Services.ISolidRpcOAuth2>,SolidRpc.Abstractions.Services.ISolidRpcOAuth2 {
            /// <summary>
            /// 
            /// </summary>
            public SolidRpcAbstractionsServicesISolidRpcOAuth2Proxy(
                System.IServiceProvider serviceProvider,
                Microsoft.Extensions.DependencyInjection.SolidRpcExtensions.IProxyConfig<SolidRpc.Abstractions.Services.ISolidRpcOAuth2> config) : base(serviceProvider, config)
            {}
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_GetAuthorizationCodeTokenAsync_callbackUri_state_scopes_cancellationToken = GetMethodInfo("GetAuthorizationCodeTokenAsync", new System.Type[] {typeof(System.Uri), typeof(System.String), typeof(System.Collections.Generic.IEnumerable<System.String>), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="callbackUri"></param>
            /// <param name="state"></param>
            /// <param name="scopes"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<SolidRpc.Abstractions.Types.FileContent> GetAuthorizationCodeTokenAsync(
                System.Uri callbackUri,
                System.String state,
                System.Collections.Generic.IEnumerable<System.String> scopes,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (SolidRpc.Abstractions.Services.ISolidRpcOAuth2)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync<SolidRpc.Abstractions.Types.FileContent>(_serviceProvider, impl, mi_GetAuthorizationCodeTokenAsync_callbackUri_state_scopes_cancellationToken, new object[] {callbackUri, state, scopes, cancellationToken}, () => impl.GetAuthorizationCodeTokenAsync(callbackUri, state, scopes, cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_TokenCallbackAsync_code_state_cancellation = GetMethodInfo("TokenCallbackAsync", new System.Type[] {typeof(System.String), typeof(System.String), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="code"></param>
            /// <param name="state"></param>
            /// <param name="cancellation"></param>
            public System.Threading.Tasks.Task<SolidRpc.Abstractions.Types.FileContent> TokenCallbackAsync(
                System.String code,
                System.String state,
                System.Threading.CancellationToken cancellation) {
                var impl = (SolidRpc.Abstractions.Services.ISolidRpcOAuth2)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync<SolidRpc.Abstractions.Types.FileContent>(_serviceProvider, impl, mi_TokenCallbackAsync_code_state_cancellation, new object[] {code, state, cancellation}, () => impl.TokenCallbackAsync(code, state, cancellation));
            }
        
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_RefreshTokenAsync_accessToken_cancellation = GetMethodInfo("RefreshTokenAsync", new System.Type[] {typeof(System.String), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="accessToken"></param>
            /// <param name="cancellation"></param>
            public System.Threading.Tasks.Task<SolidRpc.Abstractions.Types.FileContent> RefreshTokenAsync(
                System.String accessToken,
                System.Threading.CancellationToken cancellation) {
                var impl = (SolidRpc.Abstractions.Services.ISolidRpcOAuth2)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync<SolidRpc.Abstractions.Types.FileContent>(_serviceProvider, impl, mi_RefreshTokenAsync_accessToken_cancellation, new object[] {accessToken, cancellation}, () => impl.RefreshTokenAsync(accessToken, cancellation));
            }
        
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_LogoutAsync_callbackUri_accessToken_cancellationToken = GetMethodInfo("LogoutAsync", new System.Type[] {typeof(System.Uri), typeof(System.String), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="callbackUri"></param>
            /// <param name="accessToken"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<SolidRpc.Abstractions.Types.FileContent> LogoutAsync(
                System.Uri callbackUri,
                System.String accessToken,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (SolidRpc.Abstractions.Services.ISolidRpcOAuth2)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync<SolidRpc.Abstractions.Types.FileContent>(_serviceProvider, impl, mi_LogoutAsync_callbackUri_accessToken_cancellationToken, new object[] {callbackUri, accessToken, cancellationToken}, () => impl.LogoutAsync(callbackUri, accessToken, cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_PostLogoutAsync_state_cancellationToken = GetMethodInfo("PostLogoutAsync", new System.Type[] {typeof(System.String), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="state"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<SolidRpc.Abstractions.Types.FileContent> PostLogoutAsync(
                System.String state,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (SolidRpc.Abstractions.Services.ISolidRpcOAuth2)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync<SolidRpc.Abstractions.Types.FileContent>(_serviceProvider, impl, mi_PostLogoutAsync_state_cancellationToken, new object[] {state, cancellationToken}, () => impl.PostLogoutAsync(state, cancellationToken));
            }
        
        }
    
        /// <summary>
        /// 
        /// </summary>
        public class SolidRpcAbstractionsServicesISolidRpcOidcProxy : Microsoft.Extensions.DependencyInjection.SolidRpcExtensions.Proxy<SolidRpc.Abstractions.Services.ISolidRpcOidc>,SolidRpc.Abstractions.Services.ISolidRpcOidc {
            /// <summary>
            /// 
            /// </summary>
            public SolidRpcAbstractionsServicesISolidRpcOidcProxy(
                System.IServiceProvider serviceProvider,
                Microsoft.Extensions.DependencyInjection.SolidRpcExtensions.IProxyConfig<SolidRpc.Abstractions.Services.ISolidRpcOidc> config) : base(serviceProvider, config)
            {}
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_GetDiscoveryDocumentAsync_cancellationToken = GetMethodInfo("GetDiscoveryDocumentAsync", new System.Type[] {typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="cancellationToken"></param>
            [SolidRpc.Abstractions.OpenApi(Path="/.well-known/openid-configuration")]
            public System.Threading.Tasks.Task<SolidRpc.Abstractions.Types.OAuth2.OpenIDConnectDiscovery> GetDiscoveryDocumentAsync(
                System.Threading.CancellationToken cancellationToken) {
                var impl = (SolidRpc.Abstractions.Services.ISolidRpcOidc)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync<SolidRpc.Abstractions.Types.OAuth2.OpenIDConnectDiscovery>(_serviceProvider, impl, mi_GetDiscoveryDocumentAsync_cancellationToken, new object[] {cancellationToken}, () => impl.GetDiscoveryDocumentAsync(cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_GetKeysAsync_cancellationToken = GetMethodInfo("GetKeysAsync", new System.Type[] {typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<SolidRpc.Abstractions.Types.OAuth2.OpenIDKeys> GetKeysAsync(
                System.Threading.CancellationToken cancellationToken) {
                var impl = (SolidRpc.Abstractions.Services.ISolidRpcOidc)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync<SolidRpc.Abstractions.Types.OAuth2.OpenIDKeys>(_serviceProvider, impl, mi_GetKeysAsync_cancellationToken, new object[] {cancellationToken}, () => impl.GetKeysAsync(cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_GetTokenAsync_grantType_clientId_clientSecret_username_password_scope_code_redirectUri_codeVerifier_refreshToken_cancellationToken = GetMethodInfo("GetTokenAsync", new System.Type[] {typeof(System.String), typeof(System.String), typeof(System.String), typeof(System.String), typeof(System.String), typeof(System.Collections.Generic.IEnumerable<System.String>), typeof(System.String), typeof(System.String), typeof(System.String), typeof(System.String), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="grantType"></param>
            /// <param name="clientId"></param>
            /// <param name="clientSecret"></param>
            /// <param name="username"></param>
            /// <param name="password"></param>
            /// <param name="scope"></param>
            /// <param name="code"></param>
            /// <param name="redirectUri"></param>
            /// <param name="codeVerifier"></param>
            /// <param name="refreshToken"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<SolidRpc.Abstractions.Types.OAuth2.TokenResponse> GetTokenAsync(
                System.String grantType,
                System.String clientId,
                System.String clientSecret,
                System.String username,
                System.String password,
                System.Collections.Generic.IEnumerable<System.String> scope,
                System.String code,
                System.String redirectUri,
                System.String codeVerifier,
                System.String refreshToken,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (SolidRpc.Abstractions.Services.ISolidRpcOidc)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync<SolidRpc.Abstractions.Types.OAuth2.TokenResponse>(_serviceProvider, impl, mi_GetTokenAsync_grantType_clientId_clientSecret_username_password_scope_code_redirectUri_codeVerifier_refreshToken_cancellationToken, new object[] {grantType, clientId, clientSecret, username, password, scope, code, redirectUri, codeVerifier, refreshToken, cancellationToken}, () => impl.GetTokenAsync(grantType, clientId, clientSecret, username, password, scope, code, redirectUri, codeVerifier, refreshToken, cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_AuthorizeAsync_scope_responseType_clientId_redirectUri_state_responseMode_nonce_cancellationToken = GetMethodInfo("AuthorizeAsync", new System.Type[] {typeof(System.Collections.Generic.IEnumerable<System.String>), typeof(System.String), typeof(System.String), typeof(System.String), typeof(System.String), typeof(System.String), typeof(System.String), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="scope"></param>
            /// <param name="responseType"></param>
            /// <param name="clientId"></param>
            /// <param name="redirectUri"></param>
            /// <param name="state"></param>
            /// <param name="responseMode"></param>
            /// <param name="nonce"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<SolidRpc.Abstractions.Types.FileContent> AuthorizeAsync(
                System.Collections.Generic.IEnumerable<System.String> scope,
                System.String responseType,
                System.String clientId,
                System.String redirectUri,
                System.String state,
                System.String responseMode,
                System.String nonce,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (SolidRpc.Abstractions.Services.ISolidRpcOidc)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync<SolidRpc.Abstractions.Types.FileContent>(_serviceProvider, impl, mi_AuthorizeAsync_scope_responseType_clientId_redirectUri_state_responseMode_nonce_cancellationToken, new object[] {scope, responseType, clientId, redirectUri, state, responseMode, nonce, cancellationToken}, () => impl.AuthorizeAsync(scope, responseType, clientId, redirectUri, state, responseMode, nonce, cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_RevokeAsync_clientId_clientSecret_token_tokenHint_cancellationToken = GetMethodInfo("RevokeAsync", new System.Type[] {typeof(System.String), typeof(System.String), typeof(System.String), typeof(System.String), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="clientId"></param>
            /// <param name="clientSecret"></param>
            /// <param name="token"></param>
            /// <param name="tokenHint"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task RevokeAsync(
                System.String clientId,
                System.String clientSecret,
                System.String token,
                System.String tokenHint,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (SolidRpc.Abstractions.Services.ISolidRpcOidc)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync(_serviceProvider, impl, mi_RevokeAsync_clientId_clientSecret_token_tokenHint_cancellationToken, new object[] {clientId, clientSecret, token, tokenHint, cancellationToken}, () => impl.RevokeAsync(clientId, clientSecret, token, tokenHint, cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_EndSessionAsync_idTokenHint_postLogoutRedirectUri_state_cancellationToken = GetMethodInfo("EndSessionAsync", new System.Type[] {typeof(System.String), typeof(System.String), typeof(System.String), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="idTokenHint"></param>
            /// <param name="postLogoutRedirectUri"></param>
            /// <param name="state"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<SolidRpc.Abstractions.Types.FileContent> EndSessionAsync(
                System.String idTokenHint,
                System.String postLogoutRedirectUri,
                System.String state,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (SolidRpc.Abstractions.Services.ISolidRpcOidc)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync<SolidRpc.Abstractions.Types.FileContent>(_serviceProvider, impl, mi_EndSessionAsync_idTokenHint_postLogoutRedirectUri_state_cancellationToken, new object[] {idTokenHint, postLogoutRedirectUri, state, cancellationToken}, () => impl.EndSessionAsync(idTokenHint, postLogoutRedirectUri, state, cancellationToken));
            }
        
        }
    
        /// <summary>
        /// 
        /// </summary>
        public class SolidRpcAbstractionsServicesISolidRpcProtectedContentProxy : Microsoft.Extensions.DependencyInjection.SolidRpcExtensions.Proxy<SolidRpc.Abstractions.Services.ISolidRpcProtectedContent>,SolidRpc.Abstractions.Services.ISolidRpcProtectedContent {
            /// <summary>
            /// 
            /// </summary>
            public SolidRpcAbstractionsServicesISolidRpcProtectedContentProxy(
                System.IServiceProvider serviceProvider,
                Microsoft.Extensions.DependencyInjection.SolidRpcExtensions.IProxyConfig<SolidRpc.Abstractions.Services.ISolidRpcProtectedContent> config) : base(serviceProvider, config)
            {}
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_GetProtectedContentAsync_resource_cancellationToken = GetMethodInfo("GetProtectedContentAsync", new System.Type[] {typeof(System.String), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="resource"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<SolidRpc.Abstractions.Types.FileContent> GetProtectedContentAsync(
                System.String resource,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (SolidRpc.Abstractions.Services.ISolidRpcProtectedContent)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync<SolidRpc.Abstractions.Types.FileContent>(_serviceProvider, impl, mi_GetProtectedContentAsync_resource_cancellationToken, new object[] {resource, cancellationToken}, () => impl.GetProtectedContentAsync(resource, cancellationToken));
            }
        
        }
    
        /// <summary>
        /// 
        /// </summary>
        public class SolidRpcAbstractionsServicesRateLimitISolidRpcRateLimitProxy : Microsoft.Extensions.DependencyInjection.SolidRpcExtensions.Proxy<SolidRpc.Abstractions.Services.RateLimit.ISolidRpcRateLimit>,SolidRpc.Abstractions.Services.RateLimit.ISolidRpcRateLimit {
            /// <summary>
            /// 
            /// </summary>
            public SolidRpcAbstractionsServicesRateLimitISolidRpcRateLimitProxy(
                System.IServiceProvider serviceProvider,
                Microsoft.Extensions.DependencyInjection.SolidRpcExtensions.IProxyConfig<SolidRpc.Abstractions.Services.RateLimit.ISolidRpcRateLimit> config) : base(serviceProvider, config)
            {}
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_GetRateLimitTokenAsync_resourceName_timeout_cancellationToken = GetMethodInfo("GetRateLimitTokenAsync", new System.Type[] {typeof(System.String), typeof(System.TimeSpan), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="resourceName"></param>
            /// <param name="timeout"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<SolidRpc.Abstractions.Types.RateLimit.RateLimitToken> GetRateLimitTokenAsync(
                System.String resourceName,
                System.TimeSpan timeout,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (SolidRpc.Abstractions.Services.RateLimit.ISolidRpcRateLimit)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync<SolidRpc.Abstractions.Types.RateLimit.RateLimitToken>(_serviceProvider, impl, mi_GetRateLimitTokenAsync_resourceName_timeout_cancellationToken, new object[] {resourceName, timeout, cancellationToken}, () => impl.GetRateLimitTokenAsync(resourceName, timeout, cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_GetSingeltonTokenAsync_resourceName_timeout_cancellationToken = GetMethodInfo("GetSingeltonTokenAsync", new System.Type[] {typeof(System.String), typeof(System.TimeSpan), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="resourceName"></param>
            /// <param name="timeout"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<SolidRpc.Abstractions.Types.RateLimit.RateLimitToken> GetSingeltonTokenAsync(
                System.String resourceName,
                System.TimeSpan timeout,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (SolidRpc.Abstractions.Services.RateLimit.ISolidRpcRateLimit)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync<SolidRpc.Abstractions.Types.RateLimit.RateLimitToken>(_serviceProvider, impl, mi_GetSingeltonTokenAsync_resourceName_timeout_cancellationToken, new object[] {resourceName, timeout, cancellationToken}, () => impl.GetSingeltonTokenAsync(resourceName, timeout, cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_ReturnRateLimitTokenAsync_rateLimitToken_cancellationToken = GetMethodInfo("ReturnRateLimitTokenAsync", new System.Type[] {typeof(SolidRpc.Abstractions.Types.RateLimit.RateLimitToken), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="rateLimitToken"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task ReturnRateLimitTokenAsync(
                SolidRpc.Abstractions.Types.RateLimit.RateLimitToken rateLimitToken,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (SolidRpc.Abstractions.Services.RateLimit.ISolidRpcRateLimit)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync(_serviceProvider, impl, mi_ReturnRateLimitTokenAsync_rateLimitToken_cancellationToken, new object[] {rateLimitToken, cancellationToken}, () => impl.ReturnRateLimitTokenAsync(rateLimitToken, cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_GetRateLimitSettingsAsync_cancellationToken = GetMethodInfo("GetRateLimitSettingsAsync", new System.Type[] {typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<System.Collections.Generic.IEnumerable<SolidRpc.Abstractions.Types.RateLimit.RateLimitSetting>> GetRateLimitSettingsAsync(
                System.Threading.CancellationToken cancellationToken) {
                var impl = (SolidRpc.Abstractions.Services.RateLimit.ISolidRpcRateLimit)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync<System.Collections.Generic.IEnumerable<SolidRpc.Abstractions.Types.RateLimit.RateLimitSetting>>(_serviceProvider, impl, mi_GetRateLimitSettingsAsync_cancellationToken, new object[] {cancellationToken}, () => impl.GetRateLimitSettingsAsync(cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_UpdateRateLimitSetting_setting_cancellationToken = GetMethodInfo("UpdateRateLimitSetting", new System.Type[] {typeof(SolidRpc.Abstractions.Types.RateLimit.RateLimitSetting), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="setting"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task UpdateRateLimitSetting(
                SolidRpc.Abstractions.Types.RateLimit.RateLimitSetting setting,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (SolidRpc.Abstractions.Services.RateLimit.ISolidRpcRateLimit)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync(_serviceProvider, impl, mi_UpdateRateLimitSetting_setting_cancellationToken, new object[] {setting, cancellationToken}, () => impl.UpdateRateLimitSetting(setting, cancellationToken));
            }
        
        }
    
        /// <summary>
        /// 
        /// </summary>
        public class SolidRpcAbstractionsServicesCodeICodeNamespaceGeneratorProxy : Microsoft.Extensions.DependencyInjection.SolidRpcExtensions.Proxy<SolidRpc.Abstractions.Services.Code.ICodeNamespaceGenerator>,SolidRpc.Abstractions.Services.Code.ICodeNamespaceGenerator {
            /// <summary>
            /// 
            /// </summary>
            public SolidRpcAbstractionsServicesCodeICodeNamespaceGeneratorProxy(
                System.IServiceProvider serviceProvider,
                Microsoft.Extensions.DependencyInjection.SolidRpcExtensions.IProxyConfig<SolidRpc.Abstractions.Services.Code.ICodeNamespaceGenerator> config) : base(serviceProvider, config)
            {}
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_CreateCodeNamespace_assemblyName_cancellationToken = GetMethodInfo("CreateCodeNamespace", new System.Type[] {typeof(System.String), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="assemblyName"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<SolidRpc.Abstractions.Types.Code.CodeNamespace> CreateCodeNamespace(
                System.String assemblyName,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (SolidRpc.Abstractions.Services.Code.ICodeNamespaceGenerator)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync<SolidRpc.Abstractions.Types.Code.CodeNamespace>(_serviceProvider, impl, mi_CreateCodeNamespace_assemblyName_cancellationToken, new object[] {assemblyName, cancellationToken}, () => impl.CreateCodeNamespace(assemblyName, cancellationToken));
            }
        
        }
    
        /// <summary>
        /// 
        /// </summary>
        public class SolidRpcAbstractionsServicesCodeINpmGeneratorProxy : Microsoft.Extensions.DependencyInjection.SolidRpcExtensions.Proxy<SolidRpc.Abstractions.Services.Code.INpmGenerator>,SolidRpc.Abstractions.Services.Code.INpmGenerator {
            /// <summary>
            /// 
            /// </summary>
            public SolidRpcAbstractionsServicesCodeINpmGeneratorProxy(
                System.IServiceProvider serviceProvider,
                Microsoft.Extensions.DependencyInjection.SolidRpcExtensions.IProxyConfig<SolidRpc.Abstractions.Services.Code.INpmGenerator> config) : base(serviceProvider, config)
            {}
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_CreateNpmPackage_assemblyNames_cancellationToken = GetMethodInfo("CreateNpmPackage", new System.Type[] {typeof(System.Collections.Generic.IEnumerable<System.String>), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="assemblyNames"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<System.Collections.Generic.IEnumerable<SolidRpc.Abstractions.Types.Code.NpmPackage>> CreateNpmPackage(
                System.Collections.Generic.IEnumerable<System.String> assemblyNames,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (SolidRpc.Abstractions.Services.Code.INpmGenerator)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync<System.Collections.Generic.IEnumerable<SolidRpc.Abstractions.Types.Code.NpmPackage>>(_serviceProvider, impl, mi_CreateNpmPackage_assemblyNames_cancellationToken, new object[] {assemblyNames, cancellationToken}, () => impl.CreateNpmPackage(assemblyNames, cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_CreateInitialZip_cancellationToken = GetMethodInfo("CreateInitialZip", new System.Type[] {typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<SolidRpc.Abstractions.Types.FileContent> CreateInitialZip(
                System.Threading.CancellationToken cancellationToken) {
                var impl = (SolidRpc.Abstractions.Services.Code.INpmGenerator)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync<SolidRpc.Abstractions.Types.FileContent>(_serviceProvider, impl, mi_CreateInitialZip_cancellationToken, new object[] {cancellationToken}, () => impl.CreateInitialZip(cancellationToken));
            }
        
        }
    
        /// <summary>
        /// 
        /// </summary>
        public class SolidRpcAbstractionsServicesCodeITypescriptGeneratorProxy : Microsoft.Extensions.DependencyInjection.SolidRpcExtensions.Proxy<SolidRpc.Abstractions.Services.Code.ITypescriptGenerator>,SolidRpc.Abstractions.Services.Code.ITypescriptGenerator {
            /// <summary>
            /// 
            /// </summary>
            public SolidRpcAbstractionsServicesCodeITypescriptGeneratorProxy(
                System.IServiceProvider serviceProvider,
                Microsoft.Extensions.DependencyInjection.SolidRpcExtensions.IProxyConfig<SolidRpc.Abstractions.Services.Code.ITypescriptGenerator> config) : base(serviceProvider, config)
            {}
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_CreateTypesTsForAssemblyAsync_assemblyName_cancellationToken = GetMethodInfo("CreateTypesTsForAssemblyAsync", new System.Type[] {typeof(System.String), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="assemblyName"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<System.String> CreateTypesTsForAssemblyAsync(
                System.String assemblyName,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (SolidRpc.Abstractions.Services.Code.ITypescriptGenerator)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync<System.String>(_serviceProvider, impl, mi_CreateTypesTsForAssemblyAsync_assemblyName_cancellationToken, new object[] {assemblyName, cancellationToken}, () => impl.CreateTypesTsForAssemblyAsync(assemblyName, cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_CreateTypesTsForCodeNamespaceAsync_codeNamespace_cancellationToken = GetMethodInfo("CreateTypesTsForCodeNamespaceAsync", new System.Type[] {typeof(SolidRpc.Abstractions.Types.Code.CodeNamespace), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="codeNamespace"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<System.String> CreateTypesTsForCodeNamespaceAsync(
                SolidRpc.Abstractions.Types.Code.CodeNamespace codeNamespace,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (SolidRpc.Abstractions.Services.Code.ITypescriptGenerator)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync<System.String>(_serviceProvider, impl, mi_CreateTypesTsForCodeNamespaceAsync_codeNamespace_cancellationToken, new object[] {codeNamespace, cancellationToken}, () => impl.CreateTypesTsForCodeNamespaceAsync(codeNamespace, cancellationToken));
            }
        
        }
    
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sc"></param>
        /// <param name="configure"></param>
        public static Microsoft.Extensions.DependencyInjection.IServiceCollection AddSolidRpcAbstractions(
            this Microsoft.Extensions.DependencyInjection.IServiceCollection sc,
            System.Func<Microsoft.Extensions.DependencyInjection.SolidRpcExtensions.IProxyConfig,Microsoft.Extensions.DependencyInjection.SolidRpcExtensions.IProxyConfig> configure) {
            sc.SetupProxy<SolidRpc.Abstractions.Services.ISolidRpcAcmeChallenge,Microsoft.Extensions.DependencyInjection.SolidRpcAbstractionsExtensions.SolidRpcAbstractionsServicesISolidRpcAcmeChallengeProxy>(configure);
            sc.SetupProxy<SolidRpc.Abstractions.Services.ISolidRpcContentHandler,Microsoft.Extensions.DependencyInjection.SolidRpcAbstractionsExtensions.SolidRpcAbstractionsServicesISolidRpcContentHandlerProxy>(configure);
            sc.SetupProxy<SolidRpc.Abstractions.Services.ISolidRpcHost,Microsoft.Extensions.DependencyInjection.SolidRpcAbstractionsExtensions.SolidRpcAbstractionsServicesISolidRpcHostProxy>(configure);
            sc.SetupProxy<SolidRpc.Abstractions.Services.ISolidRpcHostStore,Microsoft.Extensions.DependencyInjection.SolidRpcAbstractionsExtensions.SolidRpcAbstractionsServicesISolidRpcHostStoreProxy>(configure);
            sc.SetupProxy<SolidRpc.Abstractions.Services.ISolidRpcMethodStore,Microsoft.Extensions.DependencyInjection.SolidRpcAbstractionsExtensions.SolidRpcAbstractionsServicesISolidRpcMethodStoreProxy>(configure);
            sc.SetupProxy<SolidRpc.Abstractions.Services.ISolidRpcOAuth2,Microsoft.Extensions.DependencyInjection.SolidRpcAbstractionsExtensions.SolidRpcAbstractionsServicesISolidRpcOAuth2Proxy>(configure);
            sc.SetupProxy<SolidRpc.Abstractions.Services.ISolidRpcOidc,Microsoft.Extensions.DependencyInjection.SolidRpcAbstractionsExtensions.SolidRpcAbstractionsServicesISolidRpcOidcProxy>(configure);
            sc.SetupProxy<SolidRpc.Abstractions.Services.ISolidRpcProtectedContent,Microsoft.Extensions.DependencyInjection.SolidRpcAbstractionsExtensions.SolidRpcAbstractionsServicesISolidRpcProtectedContentProxy>(configure);
            sc.SetupProxy<SolidRpc.Abstractions.Services.RateLimit.ISolidRpcRateLimit,Microsoft.Extensions.DependencyInjection.SolidRpcAbstractionsExtensions.SolidRpcAbstractionsServicesRateLimitISolidRpcRateLimitProxy>(configure);
            sc.SetupProxy<SolidRpc.Abstractions.Services.Code.ICodeNamespaceGenerator,Microsoft.Extensions.DependencyInjection.SolidRpcAbstractionsExtensions.SolidRpcAbstractionsServicesCodeICodeNamespaceGeneratorProxy>(configure);
            sc.SetupProxy<SolidRpc.Abstractions.Services.Code.INpmGenerator,Microsoft.Extensions.DependencyInjection.SolidRpcAbstractionsExtensions.SolidRpcAbstractionsServicesCodeINpmGeneratorProxy>(configure);
            sc.SetupProxy<SolidRpc.Abstractions.Services.Code.ITypescriptGenerator,Microsoft.Extensions.DependencyInjection.SolidRpcAbstractionsExtensions.SolidRpcAbstractionsServicesCodeITypescriptGeneratorProxy>(configure);
            return sc;
        }
    
    }
}