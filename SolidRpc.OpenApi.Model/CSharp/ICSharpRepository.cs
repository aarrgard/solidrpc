using System;
using System.Collections.Generic;
using System.Text;

namespace SolidRpc.OpenApi.Model.CSharp
{
    /// <summary>
    /// The CSharp repository contains all the objects
    /// </summary>
    public interface ICSharpRepository : ICSharpMember
    {
        /// <summary>
        /// Returns all the classes in this repo.
        /// </summary>
        IEnumerable<ICSharpClass> Classes { get; }

        /// <summary>
        /// Returns all the interfaces in this repo.
        /// </summary>
        IEnumerable<ICSharpInterface> Interfaces { get; }

        /// <summary>
        /// Returns all the namespaces in this repo.
        /// </summary>
        IEnumerable<ICSharpNamespace> Namespaces { get; }

        /// <summary>
        /// Returns the namespace for supplied name if it exists
        /// </summary>
        /// <param name="fullName"></param>
        /// <param name="ns"></param>
        /// <returns></returns>
        bool TryGetNamespace(string fullName, out ICSharpNamespace ns);

        /// <summary>
        /// Returns the type with full name.
        /// </summary>
        /// <param name="fullName"></param>
        /// <returns></returns>
        ICSharpType GetType(string fullName);

        /// <summary>
        /// Returns the interface for supplied name
        /// </summary>
        /// <param name="fullName">The name of the interface</param>
        /// <returns></returns>
        ICSharpInterface GetInterface(string fullName);

        /// <summary>
        /// Returns the class for supplied name
        /// </summary>
        /// <param name="fullName">The name of the class</param>
        /// <returns></returns>
        ICSharpClass GetClass(string fullName);
    }
}
