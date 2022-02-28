using System;
using System.Collections.Generic;
using System.Text;

namespace SolidRpc.Tests.Swagger.SpecGen.TypesImplementsInterface.Types.Sub
{
    /// <summary>
    /// ComplexType
    /// </summary>
    public class ComplexType : ITestInterface
    {
        /// <summary>
        /// ComplexType1
        /// </summary>
        public string Data { get; set; }
    }
}
