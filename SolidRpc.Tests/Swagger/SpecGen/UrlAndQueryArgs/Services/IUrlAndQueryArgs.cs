using System.Threading.Tasks;

namespace SolidRpc.Tests.Swagger.SpecGen.TestUrlAndQueryArgs.Services
{
    /// <summary>
    /// Tests method with one complex type
    /// </summary>
    public interface IUrlAndQueryArgs
    {
        /// <summary>
        /// Consumes one complex type an a simple string...
        /// </summary>
        /// <param name="urlArg"></param>
        /// <param name="queryArg"></param>
        /// <returns></returns>
        Task DoSometingAsync(string urlArg, string queryArg = "");
    }
}
