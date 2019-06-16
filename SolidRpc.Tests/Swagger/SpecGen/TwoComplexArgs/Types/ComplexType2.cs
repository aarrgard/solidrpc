using System;
using System.Collections.Generic;
using System.Text;

namespace SolidRpc.Tests.Swagger.SpecGen.TwoComplexArgs.Types
{
    /// <summary>
    /// ComplexType2
    /// </summary>
    public class ComplexType2
    {
        /// <summary>
        /// ComplexType1
        /// </summary>
        public ComplexType1 CT1 { get; set; }

        /// <summary>
        /// ComplexType2
        /// </summary>
        public ComplexType2 CT2 { get; set; }
    }
}
