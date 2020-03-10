
using SolidRpc.Tests.Swagger.SpecGen.ComplexAndSimpleArgs.Types;
using System;

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
        ComplexType1 GetSimpleAndComplexType(string simpleType, ComplexType1 ct1);

        /// <summary>
        /// Proxies an integer
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        int GetInteger(int i);

        /// <summary>
        /// Proxies a decimal
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        decimal GetDecimal(decimal d);

        /// <summary>
        /// Proxies a uri
        /// </summary>
        /// <param name="u"></param>
        /// <returns></returns>
        Uri GetUri(Uri u);

        /// <summary>
        /// Proxies a guid
        /// </summary>
        /// <param name="g"></param>
        /// <returns></returns>
        Guid GetGuid(Guid g);
    }
}
