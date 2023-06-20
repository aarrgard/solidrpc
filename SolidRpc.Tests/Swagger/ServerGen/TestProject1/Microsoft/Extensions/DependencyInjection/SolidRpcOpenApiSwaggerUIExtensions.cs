namespace Microsoft.Extensions.DependencyInjection {
    /// <summary>
    /// 
    /// </summary>
    public static class SolidRpcOpenApiSwaggerUIExtensions {
        /// <summary>
        /// 
        /// </summary>
        public class SolidRpcOpenApiSwaggerUIServicesISwaggerUIProxy : Microsoft.Extensions.DependencyInjection.SolidRpcExtensions.Proxy<SolidRpc.OpenApi.SwaggerUI.Services.ISwaggerUI>,SolidRpc.OpenApi.SwaggerUI.Services.ISwaggerUI {
            /// <summary>
            /// 
            /// </summary>
            public SolidRpcOpenApiSwaggerUIServicesISwaggerUIProxy(
                System.IServiceProvider serviceProvider,
                Microsoft.Extensions.DependencyInjection.SolidRpcExtensions.IProxyConfig<SolidRpc.OpenApi.SwaggerUI.Services.ISwaggerUI> config) : base(serviceProvider, config)
            {}
            /// <summary>
            /// 
            /// </summary>
            System.Reflection.MethodInfo mi_GetIndexHtml_onlyImplemented_cancellationToken;
            /// <summary>
            /// 
            /// </summary>
            /// <param name="onlyImplemented"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<SolidRpc.OpenApi.SwaggerUI.Types.FileContent> GetIndexHtml(
                System.Boolean onlyImplemented,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (SolidRpc.OpenApi.SwaggerUI.Services.ISwaggerUI)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync<SolidRpc.OpenApi.SwaggerUI.Types.FileContent>(_serviceProvider, impl, mi_GetIndexHtml_onlyImplemented_cancellationToken, new object[] {onlyImplemented, cancellationToken}, () => impl.GetIndexHtml(onlyImplemented, cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            System.Reflection.MethodInfo mi_GetSwaggerInitializer_onlyImplemented_cancellationToken;
            /// <summary>
            /// 
            /// </summary>
            /// <param name="onlyImplemented"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<SolidRpc.OpenApi.SwaggerUI.Types.FileContent> GetSwaggerInitializer(
                System.Boolean onlyImplemented,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (SolidRpc.OpenApi.SwaggerUI.Services.ISwaggerUI)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync<SolidRpc.OpenApi.SwaggerUI.Types.FileContent>(_serviceProvider, impl, mi_GetSwaggerInitializer_onlyImplemented_cancellationToken, new object[] {onlyImplemented, cancellationToken}, () => impl.GetSwaggerInitializer(onlyImplemented, cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            System.Reflection.MethodInfo mi_GetOauth2RedirectHtml_cancellationToken;
            /// <summary>
            /// 
            /// </summary>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<SolidRpc.OpenApi.SwaggerUI.Types.FileContent> GetOauth2RedirectHtml(
                System.Threading.CancellationToken cancellationToken) {
                var impl = (SolidRpc.OpenApi.SwaggerUI.Services.ISwaggerUI)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync<SolidRpc.OpenApi.SwaggerUI.Types.FileContent>(_serviceProvider, impl, mi_GetOauth2RedirectHtml_cancellationToken, new object[] {cancellationToken}, () => impl.GetOauth2RedirectHtml(cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            System.Reflection.MethodInfo mi_GetSwaggerUrls_onlyImplemented_cancellationToken;
            /// <summary>
            /// 
            /// </summary>
            /// <param name="onlyImplemented"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<System.Collections.Generic.IEnumerable<SolidRpc.OpenApi.SwaggerUI.Types.SwaggerUrl>> GetSwaggerUrls(
                System.Boolean onlyImplemented,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (SolidRpc.OpenApi.SwaggerUI.Services.ISwaggerUI)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync<System.Collections.Generic.IEnumerable<SolidRpc.OpenApi.SwaggerUI.Types.SwaggerUrl>>(_serviceProvider, impl, mi_GetSwaggerUrls_onlyImplemented_cancellationToken, new object[] {onlyImplemented, cancellationToken}, () => impl.GetSwaggerUrls(onlyImplemented, cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            System.Reflection.MethodInfo mi_GetOpenApiSpec_assemblyName_openApiSpecResolverAddress_onlyImplemented_cancellationToken;
            /// <summary>
            /// 
            /// </summary>
            /// <param name="assemblyName"></param>
            /// <param name="openApiSpecResolverAddress"></param>
            /// <param name="onlyImplemented"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<SolidRpc.OpenApi.SwaggerUI.Types.FileContent> GetOpenApiSpec(
                System.String assemblyName,
                System.String openApiSpecResolverAddress,
                System.Boolean onlyImplemented,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (SolidRpc.OpenApi.SwaggerUI.Services.ISwaggerUI)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync<SolidRpc.OpenApi.SwaggerUI.Types.FileContent>(_serviceProvider, impl, mi_GetOpenApiSpec_assemblyName_openApiSpecResolverAddress_onlyImplemented_cancellationToken, new object[] {assemblyName, openApiSpecResolverAddress, onlyImplemented, cancellationToken}, () => impl.GetOpenApiSpec(assemblyName, openApiSpecResolverAddress, onlyImplemented, cancellationToken));
            }
        
        }
    
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sc"></param>
        /// <param name="configure"></param>
        public static Microsoft.Extensions.DependencyInjection.IServiceCollection AddSolidRpcOpenApiSwaggerUI(
            this Microsoft.Extensions.DependencyInjection.IServiceCollection sc,
            System.Func<Microsoft.Extensions.DependencyInjection.SolidRpcExtensions.IProxyConfig,Microsoft.Extensions.DependencyInjection.SolidRpcExtensions.IProxyConfig> configure) {
            sc.SetupProxy<SolidRpc.OpenApi.SwaggerUI.Services.ISwaggerUI,Microsoft.Extensions.DependencyInjection.SolidRpcOpenApiSwaggerUIExtensions.SolidRpcOpenApiSwaggerUIServicesISwaggerUIProxy>(configure);
            return sc;
        }
    
    }
}