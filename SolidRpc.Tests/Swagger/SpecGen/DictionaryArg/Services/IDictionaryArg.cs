using SolidRpc.Tests.Swagger.SpecGen.DictionaryArg.Types;
using System.Collections.Generic;

namespace SolidRpc.Tests.Swagger.SpecGen.DictionaryArg.Services
{
    /// <summary>
    /// Tests method with one complex type
    /// </summary>
    public interface IDictionaryArg
    {
        /// <summary>
        /// Consumes one complex type an a simple string...
        /// </summary>
        /// <param name="strings">The strings</param>
        /// <returns></returns>
        IDictionary<string, ComplexType> GetDictionaryValues(IDictionary<string, ComplexType> strings);
    }
}
