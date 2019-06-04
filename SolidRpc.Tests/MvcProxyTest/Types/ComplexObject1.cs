using System.Collections.Generic;

namespace SolidRpc.Tests.MvcProxyTest.Types
{
    /// <summary>
    /// ComplexObject1
    /// </summary>
    public class ComplexObject1
    {
        /// <summary>
        /// Value1
        /// </summary>
        public string Value1 { get; set; }

        /// <summary>
        /// Value2
        /// </summary>
        public string Value2 { get; set; }

        /// <summary>
        /// The children
        /// </summary>
        public IEnumerable<ComplexObject1> Children { get; set; }
    }
}