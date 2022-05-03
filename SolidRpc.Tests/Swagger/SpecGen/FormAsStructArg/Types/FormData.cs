using Microsoft.Extensions.Primitives;
using System;

namespace SolidRpc.Tests.Swagger.SpecGen.FormAsStructArg.Types
{
    /// <summary>
    /// ComplexType1
    /// </summary>
    public class FormData
    {
        /// <summary>
        /// String value
        /// </summary>
        public string StringValue { get; set; }

        /// <summary>
        /// Guid value
        /// </summary>
        public Guid GuidValue { get; set; }

        /// <summary>
        /// int value
        /// </summary>
        public int IntValue { get; set; }
    }
}
