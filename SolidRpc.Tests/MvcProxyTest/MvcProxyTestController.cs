using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace SolidRpc.Tests.MvcProxyTest
{
    /// <summary>
    /// The test controller.
    /// </summary>
    [Route("[controller]/[Action]")]
    public class MvcProxyTestController : Controller
    {
        /// <summary>
        /// Sends an integer back and forth between client and server
        /// </summary>
        /// <param name="i">The integer to proxy</param>
        /// <returns></returns>
        [HttpGet]
        public Task<int> ProxyInt(int i)
        {
            return Task.FromResult(i);
        }

        /// <summary>
        /// Sends a string back and forth between client and server
        /// </summary>
        /// <param name="s">The string to proxy</param>
        /// <returns></returns>
        [HttpGet]
        public Task<string> ProxyString(string s)
        {
            return Task.FromResult(s);
        }
    }

}
