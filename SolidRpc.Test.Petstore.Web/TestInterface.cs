using System.Threading;
using System.Threading.Tasks;

namespace SolidRpc.Test.Petstore.Web
{
    public interface ITestInterface
    {
        Task<string> ProxyString(string s, CancellationToken cancellationToken);
    }
    public class TestInterface : ITestInterface
    {
        public Task<string> ProxyString(string s, CancellationToken cancellationToken)
        {
            return Task.FromResult(s);
        }
    }
}
