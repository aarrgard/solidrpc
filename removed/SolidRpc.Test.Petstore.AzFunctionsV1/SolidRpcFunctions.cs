
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
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
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "SolidRpc/Abstractions/Services/ISolidRpcContentHandler/GetContent")] HttpRequestMessage req,
            [Inject] IServiceProvider serviceProvider,
            TraceWriter log,
            CancellationToken cancellationToken)
        {
            return HttpFunction.Run(req, log, serviceProvider, cancellationToken);
        }
    }


    public class Http_SolidRpc_Abstractions_Services_ISolidRpcHost_IsAlive
    {
        [FunctionName("Http_SolidRpc_Abstractions_Services_ISolidRpcHost_IsAlive")]
        public static Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "SolidRpc/Abstractions/Services/ISolidRpcHost/IsAlive")] HttpRequestMessage req,
            [Inject] IServiceProvider serviceProvider,
            TraceWriter log,
            CancellationToken cancellationToken)
        {
            return HttpFunction.Run(req, log, serviceProvider, cancellationToken);
        }
    }


    public class Http_SolidRpc_OpenApi_SwaggerUI_Services_ISwaggerUI_GetIndexHtml
    {
        [FunctionName("Http_SolidRpc_OpenApi_SwaggerUI_Services_ISwaggerUI_GetIndexHtml")]
        public static Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "SolidRpc/OpenApi/SwaggerUI/Services/ISwaggerUI/GetIndexHtml")] HttpRequestMessage req,
            [Inject] IServiceProvider serviceProvider,
            TraceWriter log,
            CancellationToken cancellationToken)
        {
            return HttpFunction.Run(req, log, serviceProvider, cancellationToken);
        }
    }


    public class Http_SolidRpc_OpenApi_SwaggerUI_Services_ISwaggerUI_GetOpenApiSpec_arg0_arg1
    {
        [FunctionName("Http_SolidRpc_OpenApi_SwaggerUI_Services_ISwaggerUI_GetOpenApiSpec_arg0_arg1")]
        public static Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "SolidRpc/OpenApi/SwaggerUI/Services/ISwaggerUI/GetOpenApiSpec/{arg0}/{arg1}")] HttpRequestMessage req,
            [Inject] IServiceProvider serviceProvider,
            TraceWriter log,
            CancellationToken cancellationToken)
        {
            return HttpFunction.Run(req, log, serviceProvider, cancellationToken);
        }
    }


    public class Http_SolidRpc_OpenApi_SwaggerUI_Services_ISwaggerUI_GetSwaggerUrls
    {
        [FunctionName("Http_SolidRpc_OpenApi_SwaggerUI_Services_ISwaggerUI_GetSwaggerUrls")]
        public static Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "SolidRpc/OpenApi/SwaggerUI/Services/ISwaggerUI/GetSwaggerUrls")] HttpRequestMessage req,
            [Inject] IServiceProvider serviceProvider,
            TraceWriter log,
            CancellationToken cancellationToken)
        {
            return HttpFunction.Run(req, log, serviceProvider, cancellationToken);
        }
    }


    public class Timer_SolidRpc_Abstractions_Services_ISolidRpcHost_GetHostId
    {
        [FunctionName("Timer_SolidRpc_Abstractions_Services_ISolidRpcHost_GetHostId")]
        public static Task Run(
            [TimerTrigger("0 * * * * *", RunOnStartup = false)] TimerInfo timerInfo,
            [Inject] IServiceProvider serviceProvider,
            [Constant("SolidRpc.Abstractions.Services.ISolidRpcHost")] Type serviceType,
            [Constant("GetHostId")] string methodName,
            TraceWriter log,
            CancellationToken cancellationToken)
        {
            return TimerFunction.Run(timerInfo, log, serviceProvider, serviceType, methodName, cancellationToken);
        }
    }

}
