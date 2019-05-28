using System;
using System.Collections.Generic;
using System.Text;

namespace SolidRpc.Swagger.Generator.Model.CSharp
{
    /// <summary>
    /// The CSharp repository contains all the objects
    /// </summary>
    public interface ICSharpRepository
    {
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
