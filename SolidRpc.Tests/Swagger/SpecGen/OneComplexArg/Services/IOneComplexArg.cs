using SolidRpc.Tests.Swagger.SpecGen.OneComplexArg.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace SolidRpc.Tests.Swagger.SpecGen.OneComplexArg.Services
{
    /// <summary>
    /// Tests method with one complex type
    /// </summary>
    public interface IOneComplexArg
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
