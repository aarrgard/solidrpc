﻿using System;

namespace SolidRpc.Abstractions
{
    /// <summary>
    /// The lifetime of the service
    /// </summary>
    public enum SolidRpcServiceLifetime
    {
        /// <summary>
        /// Should the service be registered as a singleton
        /// </summary>
        Singleton, 
        
        /// <summary>
        /// Should the service be scoped
        /// </summary>
        Scoped, 
        
        /// <summary>
        /// Create an instance every time the service is resolved
        /// </summary>
        Transient
    }

    /// <summary>
    /// The lifetime of the service
    /// </summary>
    public enum SolidRpcServiceInstances
    {
        /// <summary>
        /// We only resolve one intance at a time
        /// </summary>
        One, 
        
        /// <summary>
        /// There may be many implementation types
        /// </summary>
        Many
    }

    /// <summary>
    /// Attribute that can be set on the SolidRpc hosting assemblies to define the 
    /// </summary>
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
    public class SolidRpcServiceAttribute : Attribute
    {
        /// <summary>
        /// Constructs a new attribute
        /// </summary>
        /// <param name="serviceType"></param>
        /// <param name="implementationType"></param>
        /// <param name="serviceLifetime"></param>
        /// <param name="serviceInstances"></param>
        public SolidRpcServiceAttribute(
            Type serviceType, 
            Type implementationType, 
            SolidRpcServiceLifetime serviceLifetime = SolidRpcServiceLifetime.Singleton,
            SolidRpcServiceInstances serviceInstances = SolidRpcServiceInstances.One)
        {
            ServiceType = serviceType ?? throw new ArgumentNullException(nameof(serviceType));
            ImplementationType = implementationType ?? throw new ArgumentNullException(nameof(implementationType));
            ServiceLifetime = serviceLifetime;
            ServiceInstances = serviceInstances;
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
        public SolidRpcServiceLifetime ServiceLifetime { get; }
        /// <summary>
        /// The service lifetime
        /// </summary>
        public SolidRpcServiceInstances ServiceInstances { get; }
    }
}
