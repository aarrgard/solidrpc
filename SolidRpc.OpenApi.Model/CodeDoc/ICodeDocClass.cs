using System.Collections.Generic;
using System.Reflection;

namespace SolidRpc.OpenApi.Model.CodeDoc
{
    /// <summary>
    /// Defines support for accessing class documentation
    /// </summary>
    public interface ICodeDocClass
    {
        /// <summary>
        /// The assembly documentation that this class belongs to.
        /// </summary>
        ICodeDocAssembly AssemblyDocumentation { get; }

        /// <summary>
        /// Returns the class name for this documentation
        /// </summary>
        string ClassName { get; }

        /// <summary>
        /// The method documentations.
        /// </summary>
        IEnumerable<ICodeDocMethod> MethodDocumentation { get; }

        /// <summary>
        /// The method documentations.
        /// </summary>
        IEnumerable<ICodeDocProperty> PropertyDocumentation { get; }

        /// <summary>
        /// Returns the documentation for supplied method.
        /// </summary>
        /// <param name="methodInfo"></param>
        /// <returns></returns>
        ICodeDocMethod GetMethodDocumentation(MethodInfo methodInfo);

        /// <summary>
        /// Returns the property documentation for supplied property.
        /// </summary>
        /// <param name="pi"></param>
        /// <returns></returns>
        ICodeDocProperty GetPropertyDocumentation(PropertyInfo pi);

        /// <summary>
        /// Returns the summary
        /// </summary>
        string Summary { get; }

        /// <summary>
        /// Returns the code comments. These are the comments associated with the class.
        /// </summary>
        string CodeComments { get; }
    }
}