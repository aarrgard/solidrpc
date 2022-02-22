
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using SolidRpc.OpenApi.AzFunctions.Bindings;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
namespace SolidRpc.OpenApi.AzFunctions
{


    public class Queue_SolidRpc_OpenApi_AzQueue_Services_IAzTableQueue_ProcessMessageAsync_arg0_arg1_arg2
    {
        [FunctionName("Queue_SolidRpc_OpenApi_AzQueue_Services_IAzTableQueue_ProcessMessageAsync_arg0_arg1_arg2")]
        public static Task Run(
            [QueueTrigger("s8c-o7i-a7e-s8s-iaztablequeue-processmessageasync-arg-arg-arg", Connection = "AzureWebJobsStorage")] string message,
            string id,
            [Inject] IServiceProvider serviceProvider,
            ILogger log,
            CancellationToken cancellationToken)
        {
            return QueueFunction.Run(message, id, log, serviceProvider, cancellationToken);
        }
    }


    public class Timer_SolidRpc_OpenApi_AzQueue_Services_IAzTableQueue_DoScheduledScanAsync_1
    {
        [FunctionName("Timer_SolidRpc_OpenApi_AzQueue_Services_IAzTableQueue_DoScheduledScanAsync_1")]
        public static Task Run(
            [TimerTrigger("0 * * * * *", RunOnStartup = false)] TimerInfo timerInfo,
            [Inject] IServiceProvider serviceProvider,
            [Constant("Timer_SolidRpc_OpenApi_AzQueue_Services_IAzTableQueue_DoScheduledScanAsync_1")] string timerId,
            ILogger log,
            CancellationToken cancellationToken)
        {
            return TimerFunction.Run(timerInfo, log, serviceProvider, timerId, cancellationToken);
        }
    }


    public class WildcardFunc
    {
        [FunctionName("WildcardFunc")]
        public static Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "options", "post", Route = "{*restOfPath}")] HttpRequestMessage req,
            [Inject] IServiceProvider serviceProvider,
            ILogger log,
            CancellationToken cancellationToken)
        {
            return HttpFunction.Run(req, log, serviceProvider, cancellationToken);
        }
    }

}
