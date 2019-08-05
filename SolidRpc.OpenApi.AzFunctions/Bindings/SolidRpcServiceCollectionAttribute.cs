using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SolidRpc.OpenApi.AzFunctions.Bindings
{
    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Assembly)]
    public class SolidRpcServiceCollectionAttribute : Attribute
    {
        private static readonly HashSet<Type> ServiceConfigs = new HashSet<Type>();
        
        /// <summary>
        /// Uses all the registered types to configure the services.
        /// </summary>
        /// <param name="services"></param>
        public static void ConfigureServices(IServiceCollection services)
        {
            ServiceConfigs.ToList()
                .SelectMany(o => o.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
                .Where(o => o.GetParameters().Length == 1)
                .Where(o => typeof(IServiceCollection).IsAssignableFrom(o.GetParameters()[0].ParameterType))
                .ToList().ForEach(o =>
                {
                    var target = Activator.CreateInstance(o.DeclaringType);
                    var del = (Action<IServiceCollection>)o.CreateDelegate(typeof(Action<IServiceCollection>), target);
                    del(services);
                });
        }

        /// <summary>
        /// Constructs a new instance
        /// </summary>
        /// <param name="serviceConfig"></param>
        public SolidRpcServiceCollectionAttribute(Type serviceConfig)
        {
            ServiceConfigs.Add(serviceConfig);
        } 
    }
}
