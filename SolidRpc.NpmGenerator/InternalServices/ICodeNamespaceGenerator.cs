using SolidRpc.NpmGenerator.Types;

namespace SolidRpc.NpmGenerator.InternalServices
{
    /// <summary>
    /// instance responsible for generating code structures
    /// </summary>
    public interface ICodeNamespaceGenerator
    {
        /// <summary>
        /// Creates a code namespace for supplied assembly name
        /// </summary>
        /// <param name="assemblyName"></param>
        /// <returns></returns>
        CodeNamespace CreateCodeNamespace(string assemblyName);
    }
}
