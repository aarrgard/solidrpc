using Microsoft.AspNetCore.Mvc;

namespace SolidRpc.Tests.MvcServerTest
{
    [Route("[controller]/[Action]")]
    public class MvcServerTestController : Controller
    {
        [HttpGet]
        public OkResult Index()
        {
            return new OkResult();
        }
    }

}
