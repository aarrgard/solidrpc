using Microsoft.AspNetCore.Mvc;

namespace SolidRpc.Tests.MvcServerTest
{
    /// <summary>
    /// The default controller
    /// </summary>
    [Route("[controller]/[Action]")]
    public class MvcServerTestController : Controller
    {
        /// <summary>
        /// Returns data for the index page.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public OkResult Index()
        {
            return new OkResult();
        }
    }

}
