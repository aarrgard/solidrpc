using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SolidRpc.Abstractions
{
    /// <summary>
    /// Attribute that can be set on the SolidRpc hosting assemblies to define the 
    /// </summary>
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
    public class SolidRpcAbstractionProviderAttribute : Attribute
    {
        /// <summary>
        /// Constructs a new attribute
        /// </summary>
        /// <param name="serviceType"></param>
        /// <param name="implementationType"></param>
        public SolidRpcAbstractionProviderAttribute(Type serviceType, Type implementationType, ServiceLifetime serviceLifetime = ServiceLifetime.Singleton)
        {
            ServiceType = serviceType ?? throw new ArgumentNullException(nameof(serviceType));
            ImplementationType = implementationType ?? throw new ArgumentNullException(nameof(implementationType));
            ServiceLifetime = serviceLifetime;
            if (serviceType.IsGenericType)
            {
                return;
            }
            if(!ServiceType.IsAssignableFrom(ImplementationType))
            {
                throw new Exception("ServiceType not assignable from implementation");
            }
        }

        /// <summary>
        /// The interface type
        /// </summary>
        public Type ServiceType { get; }

        /// <summary>
        /// The implementation type.
        /// </summary>
        public Type ImplementationType { get; }

        /// <summary>
        /// The service lifetime
        /// </summary>
        public ServiceLifetime ServiceLifetime { get; }
    }
}
