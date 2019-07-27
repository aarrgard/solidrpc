
using SolidRpc.Tests.Swagger.SpecGen.ComplexAndSimpleArgs.Types;

namespace SolidRpc.Tests.Swagger.SpecGen.ComplexAndSimpleArgs.Services
{
    /// <summary>
    /// Tests method with one complex type
    /// </summary>
    public interface IComplexAndSimpleArgs
    {
        /// <summary>
        /// Consumes one complex type an a simple string...
        /// </summary>
        /// <param name="simpleType"></param>
        /// <param name="ct1"></param>
        /// <returns></returns>
        ComplexType1 GetComplexType(string simpleType, ComplexType1 ct1);
    }
}
