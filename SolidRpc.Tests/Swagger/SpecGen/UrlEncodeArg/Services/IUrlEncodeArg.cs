using SolidRpc.Tests.Swagger.SpecGen.TwoComplexArgs.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace SolidRpc.Tests.Swagger.SpecGen.UrlEncodeArg.Services
{
    /// <summary>
    /// Tests method with two complex types
    /// </summary>
    public interface IUrlEncodeArg
    {
        /// <summary>
        /// Proxies the string
        /// </summary>
        /// <param name="s1"></param>
        /// <param name="s2"></param>
        /// <returns></returns>
        string ProxyStrings(string s1, string s2);

        /// <summary>
        /// Proxies the string
        /// </summary>
        /// <param name="arr"></param>
        /// <returns></returns>
        byte[] ProxyByteArray(byte[] arr);
    }
}
