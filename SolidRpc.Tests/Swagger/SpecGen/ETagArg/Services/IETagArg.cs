using SolidRpc.Tests.Swagger.SpecGen.ETagArg.Types;

namespace SolidRpc.Tests.Swagger.SpecGen.ETagArg.Services
{
    /// <summary>
    /// Tests method with two complex types
    /// </summary>
    public interface IETagArg
    {
        /// <summary>
        /// Consumes two complex types an a simple string...
        /// </summary>
        /// <param name="fileType"></param>
        /// <returns></returns>
        FileType GetEtagStruct(FileType fileType);
    }
}
