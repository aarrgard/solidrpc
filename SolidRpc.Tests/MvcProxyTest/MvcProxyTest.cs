using System.Threading.Tasks;
using Newtonsoft.Json;
using NUnit.Framework;
using SolidRpc.Swagger.V2;

namespace Tests.MvcProxyTest
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
                var settings = new JsonSerializerSettings()
                {
                    ContractResolver = SolidRpc.Swagger.NewtonsoftContractResolver.Instance
                };
                var obj = JsonConvert.DeserializeObject<SwaggerObject>(content, settings);

                resp = await ctx.GetResponse("/MvcProxyTest/ProxyInt?i=10");
                Assert.AreEqual("10", await AssertOk(resp));

            }
        }
    }
}