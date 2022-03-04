
using SolidRpc.Tests.Swagger.SpecGen.ComplexAndSimpleArgs.Types;
using System;

namespace SolidRpc.Tests.Swagger.SpecGen.ByteArrArgs.Services
{
    /// <summary>
    /// Tests method with one complex type
    /// </summary>
    public interface IByteArrArgs
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="arr"></param>
        /// <returns></returns>
        byte[] ProxyByteArray(byte[] arr);
    }
}
