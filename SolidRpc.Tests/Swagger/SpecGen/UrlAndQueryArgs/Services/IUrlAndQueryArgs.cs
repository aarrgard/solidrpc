using System.Threading.Tasks;

namespace SolidRpc.Tests.Swagger.SpecGen.UrlAndQueryArgs.Services
{
    /// <summary>
    /// Tests method with one complex type
    /// </summary>
    public interface IUrlAndQueryArgs
    {
        /// <summary>
        /// Consumes a string arg from path and query
        /// </summary>
        /// <param name="pathArg"></param>
        /// <param name="queryArg"></param>
        /// <returns></returns>
        Task<string> DoSometingAsync(string pathArg, string queryArg = "");
    }
}
