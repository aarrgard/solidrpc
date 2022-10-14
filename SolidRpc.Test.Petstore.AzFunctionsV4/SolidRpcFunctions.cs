
    using Microsoft.Azure.WebJobs;
    using Microsoft.Azure.WebJobs.Extensions.Http;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    namespace SolidRpc.OpenApi.AzFunctions
    {
    

    public class Queue_SolidRpc_OpenApi_AzQueue_Services_IAzTableQueue_ProcessMessageAsync_arg0_arg1_arg2
    {
        private ILogger _logger;
        private IServiceProvider _serviceProvider;
        public Queue_SolidRpc_OpenApi_AzQueue_Services_IAzTableQueue_ProcessMessageAsync_arg0_arg1_arg2(ILogger<Queue_SolidRpc_OpenApi_AzQueue_Services_IAzTableQueue_ProcessMessageAsync_arg0_arg1_arg2> logger, IServiceProvider serviceProvider) {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }
        [FunctionName("Queue_SolidRpc_OpenApi_AzQueue_Services_IAzTableQueue_ProcessMessageAsync_arg0_arg1_arg2")]
        public Task Run(
            [QueueTrigger("s8c-o7i-a7e-s8s-iaztablequeue-processmessageasync-arg-arg-arg", Connection = "AzureWebJobsStorage")] string message,
            string id,
            CancellationToken cancellationToken)
        {
            return QueueFunction.Run(message, id, _logger, _serviceProvider, cancellationToken);
        }
    }


    public class Timer_SolidRpc_OpenApi_AzQueue_Services_IAzTableQueue_DoScheduledScanAsync_1
    {
        private ILogger _logger;
        private IServiceProvider _serviceProvider;
        public Timer_SolidRpc_OpenApi_AzQueue_Services_IAzTableQueue_DoScheduledScanAsync_1(ILogger<Timer_SolidRpc_OpenApi_AzQueue_Services_IAzTableQueue_DoScheduledScanAsync_1> logger, IServiceProvider serviceProvider) {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }
        [FunctionName("Timer_SolidRpc_OpenApi_AzQueue_Services_IAzTableQueue_DoScheduledScanAsync_1")]
        public Task Run(
            [TimerTrigger("0 * * * * *", RunOnStartup = false)] TimerInfo timerInfo,
            CancellationToken cancellationToken)
        {
            return TimerFunction.Run(timerInfo, _logger, _serviceProvider, "Timer_SolidRpc_OpenApi_AzQueue_Services_IAzTableQueue_DoScheduledScanAsync_1", cancellationToken);
        }
    }


    public class WildcardFunc
    {
        private ILogger _logger;
        private IServiceProvider _serviceProvider;
        public WildcardFunc(ILogger<WildcardFunc> logger, IServiceProvider serviceProvider) {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }
        [FunctionName("WildcardFunc")]
        public async Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "options", "post", Route = "{*restOfPath}")] HttpRequestMessage req,
            CancellationToken cancellationToken)
        {
            var res = await HttpFunction.Run(req, _logger, _serviceProvider, cancellationToken);
            return res;
        }
    }

    }
