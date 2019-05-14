using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace SolidRpc.Tests.MvcProxyTest
{
    [Route("[controller]/[Action]")]
    public class MvcProxyTestController : Controller
    {
        [HttpGet]
        public Task<ActionResult<int>> ProxyInt(int i)
        {
            return Task.FromResult(new ActionResult<int>(i));
        }
    }

}
