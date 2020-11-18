using SolidRpc.Tests.Swagger.SpecGen.OperatorOverrides.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace SolidRpc.Tests.Swagger.SpecGen.OperatorOverrides.Services
{
    /// <summary>
    /// Tests operator overrides in type
    /// </summary>
    public interface IOperatorOverrides
    {
        /// <summary>
        /// Consumes the type with overrides
        /// </summary>
        /// <param name="ct"></param>
        /// <returns></returns>
        ComplexType GetComplexType(ComplexType ct);
    }
}
