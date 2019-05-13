using System.Net;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Tests.MvcServerTest
{
    public class MvcServerTest : WebHostMvcTest
    {
        [Test]
        public async Task Test1()
        {
            using (var ctx = new TestHostContext(GetWebHost()))
            {
                var resp = await ctx.GetResponse("/MvcServerTest/Index");
                Assert.AreEqual(HttpStatusCode.OK, resp.StatusCode);
            }
        }
    }
}