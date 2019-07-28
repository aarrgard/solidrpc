using System;
using System.Linq;

namespace SolidRpc.Abstractions
{
    /// <summary>
    /// Attribute that can be set on the SolidRpc hosting assemblies to define the 
    /// </summary>
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
    public class SolidRpcAbstractionProviderAttribute : Attribute
    {
        /// <summary>
        /// Returns the implementation type for suplied generic type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static Type GetImplemenationType<T>()
        {
            var attrs = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(o => o.GetCustomAttributes(true))
                .OfType<SolidRpcAbstractionProviderAttribute>()
                .Where(o => o.ServiceType == typeof(T))
                .ToList();
            if (!attrs.Any())
            {
                throw new Exception($"Cannot find service implementation for {typeof(T)}");
            }
            return attrs.First().ImplementationType;
        }

        /// <summary>
        /// Constructs a new instance of specified type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T CreateInstance<T>()
        {
            return (T)Activator.CreateInstance(GetImplemenationType<T>());
        }

        /// <summary>
        /// Constructs a new attribute
        /// </summary>
        /// <param name="serviceType"></param>
        /// <param name="implementationType"></param>
        public SolidRpcAbstractionProviderAttribute(Type serviceType, Type implementationType)
        {
            ServiceType = serviceType ?? throw new ArgumentNullException(nameof(serviceType));
            ImplementationType = implementationType ?? throw new ArgumentNullException(nameof(implementationType));
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
    }
}
