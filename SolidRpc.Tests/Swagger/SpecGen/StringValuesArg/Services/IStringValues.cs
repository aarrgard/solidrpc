
using Microsoft.Extensions.Primitives;
using SolidRpc.Tests.Swagger.SpecGen.StringValuesArg.Types;
using System;

namespace SolidRpc.Tests.Swagger.SpecGen.StringValuesArg.Services
{
    /// <summary>
    /// Tests method with one complex type
    /// </summary>
    public interface IStringValuesArg
    {
        /// <summary>
        /// Consumes one complex type an a simple string...
        /// </summary>
        /// <param name="stringValues">The string values</param>
        /// <param name="ct">The complex type</param>
        /// <returns></returns>
        ComplexType GetStringValueArgs(StringValues stringValues, ComplexType ct);
    }
}
