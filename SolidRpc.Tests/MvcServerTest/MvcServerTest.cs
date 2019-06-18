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
            using (var ctx = await StartTestHostContextAsync())
            {
                var resp = await ctx.GetResponse("/MvcServerTest/Index");
                await AssertOk(resp);

                resp = await ctx.GetResponse("/swagger/v1/swagger.json");
                var content = await AssertOk(resp);
            }
        }
    }
}