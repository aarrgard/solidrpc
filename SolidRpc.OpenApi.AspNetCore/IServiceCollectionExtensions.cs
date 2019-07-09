using Microsoft.Extensions.DependencyInjection;
using SolidRpc.OpenApi.AspNetCore;
using SolidRpc.OpenApi.Binder;
using System.IO;
using System.Linq;
using System.Reflection;

namespace System
{
    public static class IServiceCollectionExtensions
    {
        /// <summary>
        /// Configures all the interfaces in supplied assembly
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sp"></param>
        /// <param name="message"></param>
        public static IServiceCollection AddSolidRpcBindings(this IServiceCollection sc, Assembly assembly)
        {
            foreach (var t in assembly.GetTypes())
            {
                if(!t.IsInterface)
                {
                    continue;
                }
                sc.AddSolidRpcBinding(t);
            }
            return sc;
        }

        /// <summary>
        /// Configures the supplied type so that it is exposed through the .net core http binding.
        /// </summary>
        /// <param name="sc"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public static IServiceCollection AddSolidRpcBinding(this IServiceCollection sc, Type t)
        {
            foreach (var m in t.GetMethods())
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
        /// <returns></returns>
        public static IServiceCollection AddSolidRpcBinding(this IServiceCollection sc, MethodInfo mi)
        {
            //
            // make sure that the method invoker is registered.
            //
            if (!sc.Any(o => typeof(IMethodInvoker) == o.ServiceType))
            {
                sc.AddScoped<IMethodInvoker, MethodInvoker>();
            }

            //
            // make sure that the declaring type has a registration
            //
            if(!sc.Any(o => o.ServiceType == mi.DeclaringType))
            {
                return sc;
            }

            //
            // read configuration file
            //
            string config;
            var assembly = mi.DeclaringType.Assembly;
            var assemblyName = assembly.GetName().Name;
            var resourceName = assembly.GetManifestResourceNames().FirstOrDefault(o => o.EndsWith($".{assemblyName}.json"));
            if (resourceName == null)
            {
                throw new Exception($"Solid proxy advice config does not contain a swagger spec for {mi.DeclaringType.FullName}.");
            }
            using (var s = assembly.GetManifestResourceStream(resourceName))
            {
                using (var sr = new StreamReader(s))
                {
                    config = sr.ReadToEnd();
                }
            }

            //
            // configure method
            //
            var mc = sc.GetSolidConfigurationBuilder()
                .ConfigureInterfaceAssembly(mi.DeclaringType.Assembly)
                .ConfigureInterface(mi.DeclaringType)
                .ConfigureMethod(mi);

            mc.ConfigureAdvice<ISolidRpcAspNetCoreConfig>()
                .OpenApiConfiguration = config;

            //
            // make sure that the implementation is wrapped in a proxy by adding the invocation advice.
            // 
            mc.AddAdvice(typeof(SolidProxy.Core.Proxy.SolidProxyInvocationImplAdvice<,,>));

            return sc;
        }
    }
}
