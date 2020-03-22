using Microsoft.Extensions.Primitives;
using System.Collections.Generic;

namespace SolidRpc.Tests.Swagger.SpecGen.DictionaryArg.Types
{
    /// <summary>
    /// ComplexType1
    /// </summary>
    public class ComplexType
    {
        /// <summary>
        /// ComplexType1
        /// </summary>
        public IDictionary<string, ComplexType> ComplexTypes { get; set; }
    }
}
