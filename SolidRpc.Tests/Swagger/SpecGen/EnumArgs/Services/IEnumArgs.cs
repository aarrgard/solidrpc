using SolidRpc.Tests.Swagger.SpecGen.EnumArgs.Types;
using System.Threading;
using System.Threading.Tasks;

namespace SolidRpc.Tests.Swagger.SpecGen.EnumArgs.Services
{
    /// <summary>
    /// Tests method with two complex types
    /// </summary>
    public interface IEnumArgs
    {
        /// <summary>
        /// Consumes an enum.
        /// </summary>
        /// <param name="testEnum"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<TestEnum> GetEnumTypeAsync(TestEnum testEnum, CancellationToken cancellationToken = default(CancellationToken));
    }
}
