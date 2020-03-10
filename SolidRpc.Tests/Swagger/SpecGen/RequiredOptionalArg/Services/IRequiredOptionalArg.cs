using SolidRpc.Tests.Swagger.SpecGen.RequiredOptionalArg.Types;

namespace SolidRpc.Tests.Swagger.SpecGen.RequiredOptionalArg.Services
{
    /// <summary>
    /// Tests method with one complex type
    /// </summary>
    public interface IRequiredOptionalArg
    {
        /// <summary>
        /// This argument should be required
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        string GetRequiredString(string s);

        /// <summary>
        /// This argument should be optional
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        string GetOptionalString(string s = null);

        /// <summary>
        /// This argument should be required
        /// </summary>
        /// <param name="ct1"></param>
        /// <returns></returns>
        ComplexType1 GetRequiredComplexType(ComplexType1 ct1);

        /// <summary>
        /// This argument should be optional
        /// </summary>
        /// <param name="ct1"></param>
        /// <returns></returns>
        ComplexType1 GetOptionalComplexType(ComplexType1 ct1 = null);
    }
}
