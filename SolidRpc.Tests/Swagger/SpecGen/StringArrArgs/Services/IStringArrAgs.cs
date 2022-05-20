
using SolidRpc.Tests.Swagger.SpecGen.ComplexAndSimpleArgs.Types;
using System;
using System.Collections.Generic;

namespace SolidRpc.Tests.Swagger.SpecGen.StringArrArgs.Services
{
    /// <summary>
    /// Tests method with one complex type
    /// </summary>
    public interface IStringArrArgs
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="arr"></param>
        /// <returns></returns>
        IEnumerable<string> ProxyStringArray(IEnumerable<string> arr);
    }
}
