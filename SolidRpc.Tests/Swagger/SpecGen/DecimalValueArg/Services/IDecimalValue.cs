
namespace SolidRpc.Tests.Swagger.SpecGen.DecimalValueArg.Services
{
    /// <summary>
    /// Tests method with one complex type
    /// </summary>
    public interface IDecimalValue
    {
        /// <summary>
        /// Consumes a decimal value
        /// </summary>
        /// <param name="d">The string values</param>
        /// <returns></returns>
        decimal GetStringValueArgs(decimal d);
    }
}
