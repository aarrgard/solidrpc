using SolidRpc.NpmGenerator.Types;

namespace SolidRpc.NpmGenerator.InternalServices
{
    /// <summary>
    /// instance responsible for generating code structures
    /// </summary>
    public interface ITypescriptGenerator
    {
        /// <summary>
        /// Creates a types.ts file from supplied 
        /// </summary>
        /// <param name="codeNamespace"></param>
        /// <returns></returns>
        string CreateTypesTs(CodeNamespace codeNamespace);
    }
}
