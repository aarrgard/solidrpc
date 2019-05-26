using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
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
        public Task<bool> ProxyBooleanInQuery([FromQuery]bool b)
        {
            return Task.FromResult(b);
        }

        /// <summary>
        /// Sends a short back and forth between client and server
        /// </summary>
        /// <param name="s">The short to proxy</param>
        /// <returns>the supplied short</returns>
        [HttpGet]
        public Task<short> ProxyShortInQuery([FromQuery]short s)
        {
            return Task.FromResult(s);
        }

        /// <summary>
        /// Sends a byte back and forth between client and server
        /// </summary>
        /// <param name="b">The byte to proxy</param>
        /// <returns>the supplied byte</returns>
        [HttpGet]
        public Task<byte> ProxyByteInQuery([FromQuery]byte b)
        {
            return Task.FromResult(b);
        }


        /// <summary>
        /// Sends an integer back and forth between client and server
        /// </summary>
        /// <param name="i">The interger to proxy</param>
        /// <returns>the supplied int</returns>
        [HttpGet]
        public Task<int> ProxyIntInQuery([FromQuery]int i)
        {
            return Task.FromResult(i);
        }

        /// <summary>
        /// Sends an integer back and forth between client and server
        /// </summary>
        /// <param name="i">The interger to proxy</param>
        /// <returns>the supplied int</returns>
        [HttpPost]
        public Task<int> ProxyIntInForm([FromForm]int i)
        {
            return Task.FromResult(i);
        }

        /// <summary>
        /// Sends an integer back and forth between client and server
        /// </summary>
        /// <param name="i">The interger to proxy</param>
        /// <returns>the supplied int</returns>
        [HttpGet]
        public Task<int[][]> ProxyIntArrArrInQuery([FromQuery]int[][] iarr)
        {
            return Task.FromResult(iarr);
        }

        /// <summary>
        /// Sends an long back and forth between client and server
        /// </summary>
        /// <param name="l">The long to proxy</param>
        /// <returns>the supplied long</returns>
        [HttpGet]
        public Task<long> ProxyLongInQuery([FromQuery]long l)
        {
            return Task.FromResult(l);
        }

        /// <summary>
        /// Sends a float back and forth between client and server
        /// </summary>
        /// <param name="f">The float to proxy</param>
        /// <returns>the supplied float</returns>
        [HttpGet]
        public Task<float> ProxyFloatInQuery([FromQuery]float f)
        {
            return Task.FromResult(f);
        }

        /// <summary>
        /// Sends a double back and forth between client and server
        /// </summary>
        /// <param name="d">The double to proxy</param>
        /// <returns>the supplied double</returns>
        [HttpGet]
        public Task<double> ProxyDoubleInQuery([FromQuery]double d)
        {
            return Task.FromResult(d);
        }

        /// <summary>
        /// Sends a string back and forth between client and server
        /// </summary>
        /// <param name="s">The string to proxy</param>
        /// <returns>the supplied string</returns>
        [HttpGet]
        public Task<string> ProxyStringInQuery([FromQuery]string s)
        {
            return Task.FromResult(s);
        }

        /// <summary>
        /// Sends a guid back and forth between client and server
        /// </summary>
        /// <param name="g">The guid to proxy</param>
        /// <returns>the supplied guid</returns>
        [HttpGet]
        public Task<Guid> ProxyGuidInQuery([FromQuery]Guid g)
        {
            return Task.FromResult(g);
        }

        /// <summary>
        /// Sends a datetime back and forth between client and server
        /// </summary>
        /// <param name="g">The datetime to proxy</param>
        /// <returns>the supplied datetime</returns>
        [HttpGet]
        public Task<DateTime> ProxyDateTimeInQuery([FromQuery]DateTime d)
        {
            return Task.FromResult(d);
        }

        /// <summary>
        /// Sends a datetime back and forth between client and server
        /// </summary>
        /// <param name="dArr">The datetime to proxy</param>
        /// <returns>the supplied datetime</returns>
        [HttpGet]
        public Task<IEnumerable<DateTime>> ProxyDateTimeArrayInQuery([FromQuery]IEnumerable<DateTime> dArr)
        {
            return Task.FromResult(dArr);
        }

        /// <summary>
        /// Sends a datetime back and forth between client and server
        /// </summary>
        /// <param name="dArr">The datetime to proxy</param>
        /// <returns>the supplied datetime</returns>
        [HttpPost]
        public Task<IEnumerable<DateTime>> ProxyDateTimeArrayInForm([FromForm]IEnumerable<DateTime> dArr)
        {
            return Task.FromResult(dArr);
        }

        /// <summary>
        /// Sends a stream back and forth between client and server
        /// </summary>
        /// <param name="ff">The stream to proxy</param>
        /// <returns>the supplied stream</returns>
        [HttpPost]
        public async Task<FileStreamResult> ProxyStream(IFormFile ff)
        {
            var ms = new MemoryStream();
            using (var s = ff.OpenReadStream())
            {
                await s.CopyToAsync(ms);
            }
            return new FileStreamResult(new MemoryStream(ms.ToArray()), ff.ContentType);
        }
    }

}
