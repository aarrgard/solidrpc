using SolidRpc.Tests.Swagger.SpecGen.MoreComplexArgs.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace SolidRpc.Tests.Swagger.SpecGen.MoreComplexArgs.Services
{
    /// <summary>
    /// Tests method with two complex types
    /// </summary>
    public interface IMoreComplexArgs
    {
        /// <summary>
        /// Consumes two complex types an a simple string...
        /// </summary>
        /// <param name="simpleType"></param>
        /// <param name="date"></param>
        /// <param name="ct1Enum"></param>
        /// <param name="ct2Enum"></param>
        /// <returns></returns>
        string GetComplexType(string simpleType, DateTime? date, IEnumerable<ComplexType1> ct1Enum, IEnumerable<ComplexType2> ct2Enum);
    }
}
