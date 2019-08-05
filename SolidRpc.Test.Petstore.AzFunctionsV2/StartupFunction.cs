using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using SolidRpc.OpenApi.AzFunctions.Bindings;
using SolidRpc.OpenApi.AzFunctions.Services;
using SolidRpc.Test.Petstore.AzFunctionsV2;

[assembly: SolidRpcServiceCollection(typeof(StartupServices))]

namespace SolidRpc.Test.Petstore.AzFunctionsV2
{
    public static class Startup
    {
        [FunctionName("Startup")]
        public static Task Run([TimerTrigger("0 0 0 1 1 0", RunOnStartup=true)] TimerInfo timerInfo, [Inject] ISolidRpcHost startup)
        {
            return startup.CheckConfig();
        }
    }
}
