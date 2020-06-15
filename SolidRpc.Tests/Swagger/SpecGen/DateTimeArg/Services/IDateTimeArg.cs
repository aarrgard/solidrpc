using System;

namespace SolidRpc.Tests.Swagger.SpecGen.DateTimeArg.Services
{
    /// <summary>
    /// Tests method with two complex types
    /// </summary>
    public interface IDateTimeArg
    {
        /// <summary>
        /// Proxies the date time
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        DateTimeOffset ProxyDateTimeOffset(DateTimeOffset dt);
    }
}
