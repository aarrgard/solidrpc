using SolidRpc.Tests.Swagger.SpecGen.TwoComplexArgs.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace SolidRpc.Tests.Swagger.SpecGen.TwoComplexArgs.Services
{
    /// <summary>
    /// Tests method with two complex types
    /// </summary>
    public interface ITwoComplexArgs
    {
        /// <summary>
        /// Consumes two complex types an a simple string...
        /// </summary>
        /// <param name="simpleType"></param>
        /// <param name="ct1"></param>
        /// <param name="ct2"></param>
        /// <returns></returns>
        ComplexType1 GetComplexType(string simpleType, ComplexType1 ct1, ComplexType2 ct2);
    }
}
