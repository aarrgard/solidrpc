namespace Microsoft.Extensions.DependencyInjection {
    /// <summary>
    /// 
    /// </summary>
    public static class SolidRpcOpenApiGeneratorExtensions {
        /// <summary>
        /// 
        /// </summary>
        public class SolidRpcOpenApiGeneratorServicesIOpenApiGeneratorProxy : Microsoft.Extensions.DependencyInjection.SolidRpcExtensions.Proxy<SolidRpc.OpenApi.Generator.Services.IOpenApiGenerator>,SolidRpc.OpenApi.Generator.Services.IOpenApiGenerator {
            /// <summary>
            /// 
            /// </summary>
            public SolidRpcOpenApiGeneratorServicesIOpenApiGeneratorProxy(
                System.IServiceProvider serviceProvider,
                Microsoft.Extensions.DependencyInjection.SolidRpcExtensions.IProxyConfig<SolidRpc.OpenApi.Generator.Services.IOpenApiGenerator> config) : base(serviceProvider, config)
            {}
            /// <summary>
            /// 
            /// </summary>
            /// <param name="projectZip"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<SolidRpc.OpenApi.Generator.Types.Project.Project> ParseProjectZip(
                SolidRpc.OpenApi.Generator.Types.FileData projectZip,
                System.Threading.CancellationToken cancellationToken) {
                return GetImplementation().ParseProjectZip(projectZip, cancellationToken);
            }
        
            /// <summary>
            /// 
            /// </summary>
            /// <param name="project"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<SolidRpc.OpenApi.Generator.Types.FileData> CreateProjectZip(
                SolidRpc.OpenApi.Generator.Types.Project.Project project,
                System.Threading.CancellationToken cancellationToken) {
                return GetImplementation().CreateProjectZip(project, cancellationToken);
            }
        
            /// <summary>
            /// 
            /// </summary>
            /// <param name="settings"></param>
            /// <param name="project"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<SolidRpc.OpenApi.Generator.Types.FileData> CreateOpenApiSpecFromCode(
                SolidRpc.OpenApi.Generator.Types.SettingsSpecGen settings,
                SolidRpc.OpenApi.Generator.Types.Project.Project project,
                System.Threading.CancellationToken cancellationToken) {
                return GetImplementation().CreateOpenApiSpecFromCode(settings, project, cancellationToken);
            }
        
            /// <summary>
            /// 
            /// </summary>
            /// <param name="settings"></param>
            /// <param name="project"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<SolidRpc.OpenApi.Generator.Types.Project.Project> CreateCodeFromOpenApiSpec(
                SolidRpc.OpenApi.Generator.Types.SettingsCodeGen settings,
                SolidRpc.OpenApi.Generator.Types.Project.Project project,
                System.Threading.CancellationToken cancellationToken) {
                return GetImplementation().CreateCodeFromOpenApiSpec(settings, project, cancellationToken);
            }
        
            /// <summary>
            /// 
            /// </summary>
            /// <param name="settings"></param>
            /// <param name="project"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<SolidRpc.OpenApi.Generator.Types.Project.Project> CreateServerCode(
                SolidRpc.OpenApi.Generator.Types.SettingsServerGen settings,
                SolidRpc.OpenApi.Generator.Types.Project.Project project,
                System.Threading.CancellationToken cancellationToken) {
                return GetImplementation().CreateServerCode(settings, project, cancellationToken);
            }
        
            /// <summary>
            /// 
            /// </summary>
            /// <param name="csproj"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<SolidRpc.OpenApi.Generator.Types.SettingsCodeGen> GetSettingsCodeGenFromCsproj(
                SolidRpc.OpenApi.Generator.Types.FileData csproj,
                System.Threading.CancellationToken cancellationToken) {
                return GetImplementation().GetSettingsCodeGenFromCsproj(csproj, cancellationToken);
            }
        
            /// <summary>
            /// 
            /// </summary>
            /// <param name="csproj"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<SolidRpc.OpenApi.Generator.Types.SettingsSpecGen> GetSettingsSpecGenFromCsproj(
                SolidRpc.OpenApi.Generator.Types.FileData csproj,
                System.Threading.CancellationToken cancellationToken) {
                return GetImplementation().GetSettingsSpecGenFromCsproj(csproj, cancellationToken);
            }
        
        }
    
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sc"></param>
        /// <param name="configure"></param>
        public static Microsoft.Extensions.DependencyInjection.IServiceCollection AddSolidRpcOpenApiGenerator(
            this Microsoft.Extensions.DependencyInjection.IServiceCollection sc,
            System.Func<Microsoft.Extensions.DependencyInjection.SolidRpcExtensions.IProxyConfig,Microsoft.Extensions.DependencyInjection.SolidRpcExtensions.IProxyConfig> configure) {
            sc.SetupProxy<SolidRpc.OpenApi.Generator.Services.IOpenApiGenerator,Microsoft.Extensions.DependencyInjection.SolidRpcOpenApiGeneratorExtensions.SolidRpcOpenApiGeneratorServicesIOpenApiGeneratorProxy>(configure);
            return sc;
        }
    
    }
}