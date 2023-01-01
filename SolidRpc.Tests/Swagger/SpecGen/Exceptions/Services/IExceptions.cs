using SolidRpc.Tests.Swagger.SpecGen.ETagArg.Types;

namespace SolidRpc.Tests.Swagger.SpecGen.Exceptions.Services
{
    /// <summary>
    /// The service
    /// </summary>
    public interface IExceptions
    {
        /// <summary>
        /// the method that declares the exception
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        /// <exception cref="SolidRpc.Tests.Swagger.SpecGen.Exceptions.Types.TestException">Test exception!</exception>
        int GetEtagStruct(int arg);
    }
}
