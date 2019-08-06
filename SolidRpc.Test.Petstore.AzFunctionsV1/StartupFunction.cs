using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using SolidRpc.OpenApi.AzFunctions.Bindings;
using SolidRpc.OpenApi.AzFunctions.Services;
using SolidRpc.Test.Petstore.AzFunctionsV1;

namespace SolidRpc.Test.Petstore.AzFunctionsV1
{
    public static class StartupFuntion
    {
        [FunctionName("StartupTimer")]
        public static Task RunTimer([TimerTrigger("0 0 0 1 1 0", RunOnStartup = true)] TimerInfo timerInfo, [Inject] ISolidRpcHost startup)
        {
            return startup.CheckConfig();
        }

        //[FunctionName("StartupHttp")]
        //public static Task RunHttp([HttpTrigger(AuthorizationLevel.Function, "get", Route = null)]HttpRequestMessage req, TraceWriter log, [Inject]ISolidRpcHost startup, CancellationToken cancellationToken)
        //{
        //    startup.CheckConfig();
        //    return Task.CompletedTask;
        //}
    }
}
