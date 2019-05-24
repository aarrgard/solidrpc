using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace SolidRpc.Tests.MvcProxyTest
{
    /// <summary>
    /// The test controller.
    /// </summary>
    [Route("[controller]/[Action]")]
    [Produces("application/json")]
    public class MvcProxyTestController : Controller
    {

        /// <summary>
        /// Sends a boolean back and forth between client and server
        /// </summary>
        /// <param name="b">The boolean to proxy</param>
        /// <returns>the supplied boolean</returns>
        [HttpGet]
        public Task<bool> ProxyBoolean(bool b)
        {
            return Task.FromResult(b);
        }

        /// <summary>
        /// Sends a short back and forth between client and server
        /// </summary>
        /// <param name="s">The short to proxy</param>
        /// <returns>the supplied short</returns>
        [HttpGet]
        public Task<short> ProxyShort(short s)
        {
            return Task.FromResult(s);
        }


        /// <summary>
        /// Sends an integer back and forth between client and server
        /// </summary>
        /// <param name="i">The interger to proxy</param>
        /// <returns>the supplied int</returns>
        [HttpGet]
        public Task<int> ProxyInt(int i)
        {
            return Task.FromResult(i);
        }

        /// <summary>
        /// Sends an long back and forth between client and server
        /// </summary>
        /// <param name="l">The long to proxy</param>
        /// <returns>the supplied long</returns>
        [HttpGet]
        public Task<long> ProxyLong(long l)
        {
            return Task.FromResult(l);
        }

        /// <summary>
        /// Sends a string back and forth between client and server
        /// </summary>
        /// <param name="s">The string to proxy</param>
        /// <returns>the supplied string</returns>
        [HttpGet]
        public Task<string> ProxyString(string s)
        {
            return Task.FromResult(s);
        }

        /// <summary>
        /// Sends a guid back and forth between client and server
        /// </summary>
        /// <param name="g">The guid to proxy</param>
        /// <returns>the supplied guid</returns>
        [HttpGet]
        public Task<Guid> ProxyGuid(Guid g)
        {
            return Task.FromResult(g);
        }
    }

}
