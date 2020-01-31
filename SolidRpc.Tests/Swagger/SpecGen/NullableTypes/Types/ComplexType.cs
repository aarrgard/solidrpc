using System;
using System.Collections.Generic;
using System.Text;

namespace SolidRpc.Tests.Swagger.SpecGen.NullableTypes.Types
{
    /// <summary>
    /// ComplexType1
    /// </summary>
    public class ComplexType
    {
        /// <summary>
        /// nullable int
        /// </summary>
        public int? NullableInt { get; set; }

        /// <summary>
        /// nullable int
        /// </summary>
        public long? NullableLong { get; set; }
    }
}
