using System.Threading;
using System.Threading.Tasks;

namespace SolidRpc.Test.Petstore.Web
{
    public interface ITestInterface
    {
        Task<string> ProxyString(string s = null, CancellationToken cancellationToken = default);
    }
    public class TestInterface : ITestInterface
    {
        public Task<string> ProxyString(string s = null, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(s);
        }
    }
}
