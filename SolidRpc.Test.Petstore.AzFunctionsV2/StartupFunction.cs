using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using SolidRpc.Abstractions.OpenApi.Invoker;
using SolidRpc.Abstractions.Services;
using SolidRpc.OpenApi.AzFunctions.Bindings;

namespace SolidRpc.Test.Petstore.AzFunctionsV2
{
    public static class StartupFunction
    {
        [FunctionName("Startup")]
        public static Task Run([TimerTrigger("0 0 0 1 1 0", RunOnStartup = true)] TimerInfo timerInfo, [Inject] IInvoker<ISolidRpcHost> methodInvoker, CancellationToken cancellationToken)
        {
            return methodInvoker.InvokeAsync(o => o.IsAlive(cancellationToken), opt => InvocationOptions.Local);
        }
    }
}
