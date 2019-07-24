using SolidRpc.OpenApi.Binder;
using SolidRpc.OpenApi.Binder.Proxy;
using System;
using System.Linq;
using System.Reflection;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Extension methods for the service collection.
    /// </summary>
    public static class IServiceCollectionExtensions
    {
        /// <summary>
        /// Configures all the interfaces in supplied assembly
        /// </summary>
        /// <param name="sc"></param>
        /// <param name="interfaceAssembly"></param>
        /// <param name="implementationAssembly"></param>
        public static IServiceCollection AddSolidRpcBindings(this IServiceCollection sc, Assembly interfaceAssembly, Assembly implementationAssembly = null)
        {
            foreach (var t in interfaceAssembly.GetTypes())
            {
                if(!t.IsInterface)
                {
                    continue;
                }
                Type impl = null;
                if(implementationAssembly != null)
                {
                    impl = implementationAssembly.GetTypes()
                        .Where(o => t.IsAssignableFrom(o))
                        .SingleOrDefault();
                }
                sc.AddSolidRpcBinding(t, impl);
            }
            return sc;
        }

        /// <summary>
        /// Configures the supplied type so that it is exposed in the binder.
        /// </summary>
        /// <param name="sc"></param>
        /// <param name="interfaze"></param>
        /// <param name="impl"></param>
        /// <returns></returns>
        public static IServiceCollection AddSolidRpcBinding(this IServiceCollection sc, Type interfaze, Type impl = null)
        {

            //
            // make sure that the type is registered
            //
            if(impl != null && !sc.Any(o => o.ServiceType == interfaze)) 
            {
                sc.AddTransient(interfaze, impl);
            }

            foreach (var m in interfaze.GetMethods())
            {
                sc.AddSolidRpcBinding(m);
            }
            return sc;
        }

        /// <summary>
        /// Configures the suppllied method so that it is exposed through the .net core http binding.
        /// </summary>
        /// <param name="sc"></param>
        /// <param name="mi"></param>
        /// <param name="openApiConfiguration">The open api configuration to use - may be null to use the embedded api config.</param>
        /// <returns></returns>
        public static IServiceCollection AddSolidRpcBinding(this IServiceCollection sc, MethodInfo mi, string openApiConfiguration = null)
        {
            //
            // make sure that the method invoker is registered.
            //
            if (!sc.Any(o => typeof(IMethodInvoker) == o.ServiceType))
            {
                sc.AddSingleton<IMethodInvoker, MethodInvoker>();
            }
            if (!sc.Any(o => typeof(IMethodBinderStore) == o.ServiceType))
            {
                sc.AddSingleton<IMethodBinderStore, MethodBinderStore>();
            }

            //
            // make sure that the declaring type has a registration
            //
            if (!sc.Any(o => o.ServiceType == mi.DeclaringType))
            {
                return sc;
            }

            //
            // configure method
            //
            var mc = sc.GetSolidConfigurationBuilder()
                .ConfigureInterfaceAssembly(mi.DeclaringType.Assembly)
                .ConfigureInterface(mi.DeclaringType)
                .ConfigureMethod(mi);

            //
            // make sure that the implementation is wrapped in a proxy by adding the invocation advice.
            // 
            mc.ConfigureAdvice<ISolidRpcOpenApiConfig>().OpenApiConfiguration = openApiConfiguration;
            mc.AddAdvice(typeof(SolidProxy.Core.Proxy.SolidProxyInvocationImplAdvice<,,>));

            return sc;
        }
    }
}
