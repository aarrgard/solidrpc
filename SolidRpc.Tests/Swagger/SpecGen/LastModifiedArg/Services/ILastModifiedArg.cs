using SolidRpc.Tests.Swagger.SpecGen.LastModifiedArg.Types;

namespace SolidRpc.Tests.Swagger.SpecGen.LastModifiedArg.Services
{
    /// <summary>
    /// Tests method with two complex types
    /// </summary>
    public interface ILastModifiedArg
    {
        /// <summary>
        /// Consumes two complex types an a simple string...
        /// </summary>
        /// <param name="fileType"></param>
        /// <returns></returns>
        FileType GetLastModifiedStruct(FileType fileType);
    }
}
