using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using SolidRpc.Abstractions.OpenApi.Invoker;
using SolidRpc.Abstractions.Services;
using SolidRpc.OpenApi.Binder.Invoker;

namespace SolidRpc.Test.Petstore.AzFunctionsV4
{
    public class StartupFunction
    {
        private IInvoker<ISolidRpcHost> _methodInvoker;

        public StartupFunction(IInvoker<ISolidRpcHost> methodInvoker)
        {
            _methodInvoker = methodInvoker;
        }

        [FunctionName("Startup")]
        public Task Run([TimerTrigger("0 0 0 1 1 0", RunOnStartup = true)] TimerInfo timerInfo)
        {
            return _methodInvoker.InvokeAsync(o => o.IsAlive(CancellationToken.None), opt => opt.SetTransport(LocalHandler.TransportType));
        }

    }
}
