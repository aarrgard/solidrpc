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
            /// <param name="onlyImplemented"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<SolidRpc.OpenApi.SwaggerUI.Types.FileContent> GetIndexHtml(
                System.Boolean onlyImplemented,
                System.Threading.CancellationToken cancellationToken) {
                return GetImplementation().GetIndexHtml(onlyImplemented, cancellationToken);
            }
        
            /// <summary>
            /// 
            /// </summary>
            /// <param name="onlyImplemented"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<SolidRpc.OpenApi.SwaggerUI.Types.FileContent> GetSwaggerInitializer(
                System.Boolean onlyImplemented,
                System.Threading.CancellationToken cancellationToken) {
                return GetImplementation().GetSwaggerInitializer(onlyImplemented, cancellationToken);
            }
        
            /// <summary>
            /// 
            /// </summary>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<SolidRpc.OpenApi.SwaggerUI.Types.FileContent> GetOauth2RedirectHtml(
                System.Threading.CancellationToken cancellationToken) {
                return GetImplementation().GetOauth2RedirectHtml(cancellationToken);
            }
        
            /// <summary>
            /// 
            /// </summary>
            /// <param name="onlyImplemented"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<System.Collections.Generic.IEnumerable<SolidRpc.OpenApi.SwaggerUI.Types.SwaggerUrl>> GetSwaggerUrls(
                System.Boolean onlyImplemented,
                System.Threading.CancellationToken cancellationToken) {
                return GetImplementation().GetSwaggerUrls(onlyImplemented, cancellationToken);
            }
        
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
                return GetImplementation().GetOpenApiSpec(assemblyName, openApiSpecResolverAddress, onlyImplemented, cancellationToken);
            }
        
        }
    
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sc"></param>
        /// <param name="configure"></param>
        public static Microsoft.Extensions.DependencyInjection.IServiceCollection AddSolidRpcOpenApiSwaggerUI(
            Microsoft.Extensions.DependencyInjection.IServiceCollection sc,
            System.Func<Microsoft.Extensions.DependencyInjection.SolidRpcExtensions.IProxyConfig,Microsoft.Extensions.DependencyInjection.SolidRpcExtensions.IProxyConfig> configure) {
            sc.SetupProxy<SolidRpc.OpenApi.SwaggerUI.Services.ISwaggerUI,Microsoft.Extensions.DependencyInjection.SolidRpcOpenApiSwaggerUIExtensions.SolidRpcOpenApiSwaggerUIServicesISwaggerUIProxy>(configure);
            return sc;
        }
    
    }
}