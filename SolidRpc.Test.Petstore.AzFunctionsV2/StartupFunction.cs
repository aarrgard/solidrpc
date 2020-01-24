using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using SolidRpc.Abstractions.Services;
using SolidRpc.OpenApi.AzFunctions.Bindings;

namespace SolidRpc.Test.Petstore.AzFunctionsV2
{
    public static class StartupFunction
    {
        [FunctionName("Startup")]
        public static Task Run([TimerTrigger("0 0 0 1 1 0", RunOnStartup = true)] TimerInfo timerInfo, [Inject] ISolidRpcHost startup)
        {
            return startup.IsAlive();
        }
    }
}
