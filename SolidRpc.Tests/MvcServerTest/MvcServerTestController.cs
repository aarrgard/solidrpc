using Microsoft.AspNetCore.Mvc;

namespace SolidRpc.Tests.MvcServerTest
{
    public class MvcServerTestController : Controller
    {
        public OkResult Index()
        {
            return new OkResult();
        }
    }

}
