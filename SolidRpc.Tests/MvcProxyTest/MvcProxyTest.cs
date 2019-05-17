using System.Threading.Tasks;
using NUnit.Framework;
using SolidRpc.Swagger.Model.V2;

namespace SolidRpc.Tests.MvcProxyTest
{
    public class MvcProxyTest : WebHostMvcTest
    {
        [Test]
        public async Task TestProxyInt()
        {
            using (var ctx = new TestHostContext(GetWebHost()))
            {
                var resp = await ctx.GetResponse("/swagger/v1/swagger.json");
                var content = await AssertOk(resp);
                var spec = new SwaggerParserV2().ParseSwaggerDoc(content);

                resp = await ctx.GetResponse("/MvcProxyTest/ProxyInt?i=10");
                Assert.AreEqual("10", await AssertOk(resp));

            }
        }
    }
}