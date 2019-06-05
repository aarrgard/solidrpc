using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace SolidRpc.Tests.MvcServerTest
{
    /// <summary>
    /// 
    /// </summary>
    public class MvcServerTest : WebHostMvcTest
    {
        /// <summary>
        /// 
        /// </summary>
        [Test]
        public async Task Test1()
        {
            using (var ctx = new TestHostContext(GetWebHost()))
            {
                var resp = await ctx.GetResponse("/MvcServerTest/Index");
                await AssertOk(resp);

                resp = await ctx.GetResponse("/swagger/v1/swagger.json");
                var content = await AssertOk(resp);
            }
        }
    }
}