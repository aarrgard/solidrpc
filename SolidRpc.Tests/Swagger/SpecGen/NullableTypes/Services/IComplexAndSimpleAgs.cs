
using SolidRpc.Tests.Swagger.SpecGen.NullableTypes.Types;

namespace SolidRpc.Tests.Swagger.SpecGen.NullableTypes.Services
{
    /// <summary>
    /// Tests method with one complex type
    /// </summary>
    public interface INullableTypes
    {
        /// <summary>
        /// Consumes one complex type an a simple string...
        /// </summary>
        /// <param name="ct"></param>
        /// <returns></returns>
        ComplexType GetComplexType(ComplexType ct);

        /// <summary>
        /// Consumes one nullable int and returns it.
        /// </summary>
        /// <param name="nullableInt"></param>
        /// <returns></returns>
        int? GetNullableType(int? nullableInt);
    }
}
