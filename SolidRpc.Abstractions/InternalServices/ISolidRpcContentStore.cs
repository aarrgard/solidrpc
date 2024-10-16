﻿using System;
using System.Reflection;
using System.Threading.Tasks;

namespace SolidRpc.Abstractions.InternalServices
{
    /// <summary>
    /// The implementation of this interface is a singleton service
    /// that provides access to static content. 
    /// 
    /// Static resources may be configured(added) during setup of the IoC container.
    /// 
    /// If no path prefix is specified all the registered paths for the assembly are used at
    /// runtime to determine if the content should be delivered.
    /// </summary>
    public interface ISolidRpcContentStore
    {
        /// <summary>
        /// Adds a content mapping. 
        /// </summary>
        /// <param name="contentAssembly">The assebly that contains the content</param>
        /// <param name="assemblyRelativeName">The string to append to the name of the assembly. All resoures that start with that name are added</param>
        /// <param name="pathPrefix">The absolute path to use to obtain the content.</param>
        void AddContent(Assembly contentAssembly, string assemblyRelativeName, string pathPrefix);

        /// <summary>
        /// Adds a content mapping. 
        /// </summary>
        /// <param name="contentAssembly">The assebly that contains the content</param>
        /// <param name="assemblyRelativeName">The string to append to the name of the assembly. All resoures that start with that name are added</param>
        /// <param name="apiAssembly">The interface assembly containing the openapi spec.</param>
        void AddContent(Assembly contentAssembly, string assemblyRelativeName, Assembly apiAssembly);

        /// <summary>
        /// Use this method to expose a static path to a dynamic resource.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="mapping"></param>
        /// <param name="isRedirect"></param>
        void AddMapping(string path, Func<IServiceProvider, Task<Uri>> mapping, bool isRedirect = false);
    }
}