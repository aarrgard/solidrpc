
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

    public class Http_SolidRpc_Abstractions_Services_ISolidRpcContentHandler_GetContent
    {
        [FunctionName("Http_SolidRpc_Abstractions_Services_ISolidRpcContentHandler_GetContent")]
        public static Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "SolidRpc/Abstractions/Services/ISolidRpcContentHandler/GetContent")] HttpRequestMessage req,
            [Inject] IServiceProvider serviceProvider,
            ILogger log,
            CancellationToken cancellationToken)
        {
            return HttpFunction.Run(req, log, serviceProvider, cancellationToken);
        }
    }


    public class Http_SolidRpc_Abstractions_Services_ISolidRpcHost_CheckHost
    {
        [FunctionName("Http_SolidRpc_Abstractions_Services_ISolidRpcHost_CheckHost")]
        public static Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "SolidRpc/Abstractions/Services/ISolidRpcHost/CheckHost")] HttpRequestMessage req,
            [Inject] IServiceProvider serviceProvider,
            ILogger log,
            CancellationToken cancellationToken)
        {
            return HttpFunction.Run(req, log, serviceProvider, cancellationToken);
        }
    }


    public class Http_SolidRpc_Abstractions_Services_ISolidRpcHost_GetHostConfiguration
    {
        [FunctionName("Http_SolidRpc_Abstractions_Services_ISolidRpcHost_GetHostConfiguration")]
        public static Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "SolidRpc/Abstractions/Services/ISolidRpcHost/GetHostConfiguration")] HttpRequestMessage req,
            [Inject] IServiceProvider serviceProvider,
            ILogger log,
            CancellationToken cancellationToken)
        {
            return HttpFunction.Run(req, log, serviceProvider, cancellationToken);
        }
    }


    public class Http_SolidRpc_Abstractions_Services_ISolidRpcHost_GetHostId
    {
        [FunctionName("Http_SolidRpc_Abstractions_Services_ISolidRpcHost_GetHostId")]
        public static Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "SolidRpc/Abstractions/Services/ISolidRpcHost/GetHostId")] HttpRequestMessage req,
            [Inject] IServiceProvider serviceProvider,
            ILogger log,
            CancellationToken cancellationToken)
        {
            return HttpFunction.Run(req, log, serviceProvider, cancellationToken);
        }
    }


    public class Http_SolidRpc_Abstractions_Services_ISolidRpcHost_GetHostInstance
    {
        [FunctionName("Http_SolidRpc_Abstractions_Services_ISolidRpcHost_GetHostInstance")]
        public static Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "SolidRpc/Abstractions/Services/ISolidRpcHost/GetHostInstance")] HttpRequestMessage req,
            [Inject] IServiceProvider serviceProvider,
            ILogger log,
            CancellationToken cancellationToken)
        {
            return HttpFunction.Run(req, log, serviceProvider, cancellationToken);
        }
    }


    public class Http_SolidRpc_Abstractions_Services_ISolidRpcHost_IsAlive
    {
        [FunctionName("Http_SolidRpc_Abstractions_Services_ISolidRpcHost_IsAlive")]
        public static Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "SolidRpc/Abstractions/Services/ISolidRpcHost/IsAlive")] HttpRequestMessage req,
            [Inject] IServiceProvider serviceProvider,
            ILogger log,
            CancellationToken cancellationToken)
        {
            return HttpFunction.Run(req, log, serviceProvider, cancellationToken);
        }
    }


    public class Http_SolidRpc_Abstractions_Services_ISolidRpcHost_SyncHostsFromStore
    {
        [FunctionName("Http_SolidRpc_Abstractions_Services_ISolidRpcHost_SyncHostsFromStore")]
        public static Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "SolidRpc/Abstractions/Services/ISolidRpcHost/SyncHostsFromStore")] HttpRequestMessage req,
            [Inject] IServiceProvider serviceProvider,
            ILogger log,
            CancellationToken cancellationToken)
        {
            return HttpFunction.Run(req, log, serviceProvider, cancellationToken);
        }
    }


    public class Http_SolidRpc_OpenApi_AzQueue_Services_IAzTableQueue_DispatchMessageAsync_arg0_arg1
    {
        [FunctionName("Http_SolidRpc_OpenApi_AzQueue_Services_IAzTableQueue_DispatchMessageAsync_arg0_arg1")]
        public static Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "SolidRpc/OpenApi/AzQueue/Services/IAzTableQueue/DispatchMessageAsync/{arg0}/{arg1}")] HttpRequestMessage req,
            [Inject] IServiceProvider serviceProvider,
            ILogger log,
            CancellationToken cancellationToken)
        {
            return HttpFunction.Run(req, log, serviceProvider, cancellationToken);
        }
    }


    public class Http_SolidRpc_OpenApi_AzQueue_Services_IAzTableQueue_DoScheduledScanAsync
    {
        [FunctionName("Http_SolidRpc_OpenApi_AzQueue_Services_IAzTableQueue_DoScheduledScanAsync")]
        public static Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "SolidRpc/OpenApi/AzQueue/Services/IAzTableQueue/DoScheduledScanAsync")] HttpRequestMessage req,
            [Inject] IServiceProvider serviceProvider,
            ILogger log,
            CancellationToken cancellationToken)
        {
            return HttpFunction.Run(req, log, serviceProvider, cancellationToken);
        }
    }


    public class Http_SolidRpc_OpenApi_AzQueue_Services_IAzTableQueue_FlagErrorMessagesAsPending
    {
        [FunctionName("Http_SolidRpc_OpenApi_AzQueue_Services_IAzTableQueue_FlagErrorMessagesAsPending")]
        public static Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "SolidRpc/OpenApi/AzQueue/Services/IAzTableQueue/FlagErrorMessagesAsPending")] HttpRequestMessage req,
            [Inject] IServiceProvider serviceProvider,
            ILogger log,
            CancellationToken cancellationToken)
        {
            return HttpFunction.Run(req, log, serviceProvider, cancellationToken);
        }
    }


    public class Http_SolidRpc_OpenApi_AzQueue_Services_IAzTableQueue_GetSettingsAsync
    {
        [FunctionName("Http_SolidRpc_OpenApi_AzQueue_Services_IAzTableQueue_GetSettingsAsync")]
        public static Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "SolidRpc/OpenApi/AzQueue/Services/IAzTableQueue/GetSettingsAsync")] HttpRequestMessage req,
            [Inject] IServiceProvider serviceProvider,
            ILogger log,
            CancellationToken cancellationToken)
        {
            return HttpFunction.Run(req, log, serviceProvider, cancellationToken);
        }
    }


    public class Http_SolidRpc_OpenApi_AzQueue_Services_IAzTableQueue_ListMessagesAsync
    {
        [FunctionName("Http_SolidRpc_OpenApi_AzQueue_Services_IAzTableQueue_ListMessagesAsync")]
        public static Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "SolidRpc/OpenApi/AzQueue/Services/IAzTableQueue/ListMessagesAsync")] HttpRequestMessage req,
            [Inject] IServiceProvider serviceProvider,
            ILogger log,
            CancellationToken cancellationToken)
        {
            return HttpFunction.Run(req, log, serviceProvider, cancellationToken);
        }
    }


    public class Http_SolidRpc_OpenApi_AzQueue_Services_IAzTableQueue_ProcessMessageAsync_arg0_arg1_arg2
    {
        [FunctionName("Http_SolidRpc_OpenApi_AzQueue_Services_IAzTableQueue_ProcessMessageAsync_arg0_arg1_arg2")]
        public static Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "SolidRpc/OpenApi/AzQueue/Services/IAzTableQueue/ProcessMessageAsync/{arg0}/{arg1}/{arg2}")] HttpRequestMessage req,
            [Inject] IServiceProvider serviceProvider,
            ILogger log,
            CancellationToken cancellationToken)
        {
            return HttpFunction.Run(req, log, serviceProvider, cancellationToken);
        }
    }


    public class Http_SolidRpc_OpenApi_AzQueue_Services_IAzTableQueue_ProcessTestMessage_arg0
    {
        [FunctionName("Http_SolidRpc_OpenApi_AzQueue_Services_IAzTableQueue_ProcessTestMessage_arg0")]
        public static Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "SolidRpc/OpenApi/AzQueue/Services/IAzTableQueue/ProcessTestMessage/{arg0}")] HttpRequestMessage req,
            [Inject] IServiceProvider serviceProvider,
            ILogger log,
            CancellationToken cancellationToken)
        {
            return HttpFunction.Run(req, log, serviceProvider, cancellationToken);
        }
    }


    public class Http_SolidRpc_OpenApi_AzQueue_Services_IAzTableQueue_SendTestMessageAync
    {
        [FunctionName("Http_SolidRpc_OpenApi_AzQueue_Services_IAzTableQueue_SendTestMessageAync")]
        public static Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "SolidRpc/OpenApi/AzQueue/Services/IAzTableQueue/SendTestMessageAync")] HttpRequestMessage req,
            [Inject] IServiceProvider serviceProvider,
            ILogger log,
            CancellationToken cancellationToken)
        {
            return HttpFunction.Run(req, log, serviceProvider, cancellationToken);
        }
    }


    public class Http_SolidRpc_OpenApi_AzQueue_Services_IAzTableQueue_UpdateMessageAsync
    {
        [FunctionName("Http_SolidRpc_OpenApi_AzQueue_Services_IAzTableQueue_UpdateMessageAsync")]
        public static Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "SolidRpc/OpenApi/AzQueue/Services/IAzTableQueue/UpdateMessageAsync")] HttpRequestMessage req,
            [Inject] IServiceProvider serviceProvider,
            ILogger log,
            CancellationToken cancellationToken)
        {
            return HttpFunction.Run(req, log, serviceProvider, cancellationToken);
        }
    }


    public class Http_SolidRpc_OpenApi_AzQueue_Services_IAzTableQueue_UpdateSettings
    {
        [FunctionName("Http_SolidRpc_OpenApi_AzQueue_Services_IAzTableQueue_UpdateSettings")]
        public static Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "SolidRpc/OpenApi/AzQueue/Services/IAzTableQueue/UpdateSettings")] HttpRequestMessage req,
            [Inject] IServiceProvider serviceProvider,
            ILogger log,
            CancellationToken cancellationToken)
        {
            return HttpFunction.Run(req, log, serviceProvider, cancellationToken);
        }
    }


    public class Http_SolidRpc_OpenApi_SwaggerUI_Services_ISwaggerUI_GetIndexHtml
    {
        [FunctionName("Http_SolidRpc_OpenApi_SwaggerUI_Services_ISwaggerUI_GetIndexHtml")]
        public static Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "SolidRpc/OpenApi/SwaggerUI/Services/ISwaggerUI/GetIndexHtml")] HttpRequestMessage req,
            [Inject] IServiceProvider serviceProvider,
            ILogger log,
            CancellationToken cancellationToken)
        {
            return HttpFunction.Run(req, log, serviceProvider, cancellationToken);
        }
    }


    public class Http_SolidRpc_OpenApi_SwaggerUI_Services_ISwaggerUI_GetOpenApiSpec_arg0_arg1
    {
        [FunctionName("Http_SolidRpc_OpenApi_SwaggerUI_Services_ISwaggerUI_GetOpenApiSpec_arg0_arg1")]
        public static Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "SolidRpc/OpenApi/SwaggerUI/Services/ISwaggerUI/GetOpenApiSpec/{arg0}/{arg1}")] HttpRequestMessage req,
            [Inject] IServiceProvider serviceProvider,
            ILogger log,
            CancellationToken cancellationToken)
        {
            return HttpFunction.Run(req, log, serviceProvider, cancellationToken);
        }
    }


    public class Http_SolidRpc_OpenApi_SwaggerUI_Services_ISwaggerUI_GetSwaggerUrls
    {
        [FunctionName("Http_SolidRpc_OpenApi_SwaggerUI_Services_ISwaggerUI_GetSwaggerUrls")]
        public static Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "SolidRpc/OpenApi/SwaggerUI/Services/ISwaggerUI/GetSwaggerUrls")] HttpRequestMessage req,
            [Inject] IServiceProvider serviceProvider,
            ILogger log,
            CancellationToken cancellationToken)
        {
            return HttpFunction.Run(req, log, serviceProvider, cancellationToken);
        }
    }


    public class Http_SolidRpc_Test_Petstore_AzFunctionsV3_ITestInterface_MyFunc_arg0
    {
        [FunctionName("Http_SolidRpc_Test_Petstore_AzFunctionsV3_ITestInterface_MyFunc_arg0")]
        public static Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "SolidRpc/Test/Petstore/AzFunctionsV3/ITestInterface/MyFunc/{arg0}")] HttpRequestMessage req,
            [Inject] IServiceProvider serviceProvider,
            ILogger log,
            CancellationToken cancellationToken)
        {
            return HttpFunction.Run(req, log, serviceProvider, cancellationToken);
        }
    }


    public class Http_SolidRpc_Test_Petstore_AzFunctionsV3_ITestInterface_MyFunc_arg0_arg1
    {
        [FunctionName("Http_SolidRpc_Test_Petstore_AzFunctionsV3_ITestInterface_MyFunc_arg0_arg1")]
        public static Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "SolidRpc/Test/Petstore/AzFunctionsV3/ITestInterface/MyFunc/{arg0}/{arg1}")] HttpRequestMessage req,
            [Inject] IServiceProvider serviceProvider,
            ILogger log,
            CancellationToken cancellationToken)
        {
            return HttpFunction.Run(req, log, serviceProvider, cancellationToken);
        }
    }


    public class Http_SolidRpc_Test_Petstore_AzFunctionsV3_ITestInterface_UploadFile_arg0
    {
        [FunctionName("Http_SolidRpc_Test_Petstore_AzFunctionsV3_ITestInterface_UploadFile_arg0")]
        public static Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "SolidRpc/Test/Petstore/AzFunctionsV3/ITestInterface/UploadFile/{arg0}")] HttpRequestMessage req,
            [Inject] IServiceProvider serviceProvider,
            ILogger log,
            CancellationToken cancellationToken)
        {
            return HttpFunction.Run(req, log, serviceProvider, cancellationToken);
        }
    }



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


    public class Timer_SolidRpc_OpenApi_AzQueue_Services_IAzTableQueue_DoScheduledScanAsync
    {
        [FunctionName("Timer_SolidRpc_OpenApi_AzQueue_Services_IAzTableQueue_DoScheduledScanAsync")]
        public static Task Run(
            [TimerTrigger("0 * * * * *", RunOnStartup = false)] TimerInfo timerInfo,
            [Inject] IServiceProvider serviceProvider,
            [Constant("SolidRpc.OpenApi.AzQueue.Services.IAzTableQueue")] Type serviceType,
            [Constant("DoScheduledScanAsync")] string methodName,
            ILogger log,
            CancellationToken cancellationToken)
        {
            return TimerFunction.Run(timerInfo, log, serviceProvider, serviceType, methodName, cancellationToken);
        }
    }

}
