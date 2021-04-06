
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


    public class Http_SolidRpc_Abstractions_Services_ISolidRpcHost_IsAlive
    {
        [FunctionName("Http_SolidRpc_Abstractions_Services_ISolidRpcHost_IsAlive")]
        public static Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "SolidRpc/Abstractions/Services/ISolidRpcHost/IsAlive")] HttpRequestMessage req,
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



    public class Queue_SolidRpc_Abstractions_Services_ISolidRpcContentHandler_GetContent
    {
        [FunctionName("Queue_SolidRpc_Abstractions_Services_ISolidRpcContentHandler_GetContent")]
        public static Task Run(
            [QueueTrigger("s8c-abstractions-services-isolidrpccontenthandler-getcontent", Connection = "SolidRpcAzQueueConnection")] string message,
            string id,
            [Inject] IServiceProvider serviceProvider,
            ILogger log,
            CancellationToken cancellationToken)
        {
            return QueueFunction.Run(message, id, log, serviceProvider, cancellationToken);
        }
    }



    public class Queue_SolidRpc_Abstractions_Services_ISolidRpcHost_IsAlive
    {
        [FunctionName("Queue_SolidRpc_Abstractions_Services_ISolidRpcHost_IsAlive")]
        public static Task Run(
            [QueueTrigger("solidrpc-abstractions-services-isolidrpchost-isalive", Connection = "SolidRpcAzQueueConnection")] string message,
            string id,
            [Inject] IServiceProvider serviceProvider,
            ILogger log,
            CancellationToken cancellationToken)
        {
            return QueueFunction.Run(message, id, log, serviceProvider, cancellationToken);
        }
    }



    public class Queue_SolidRpc_OpenApi_SwaggerUI_Services_ISwaggerUI_GetIndexHtml
    {
        [FunctionName("Queue_SolidRpc_OpenApi_SwaggerUI_Services_ISwaggerUI_GetIndexHtml")]
        public static Task Run(
            [QueueTrigger("solidrpc-openapi-swaggerui-services-iswaggerui-getindexhtml", Connection = "SolidRpcAzQueueConnection")] string message,
            string id,
            [Inject] IServiceProvider serviceProvider,
            ILogger log,
            CancellationToken cancellationToken)
        {
            return QueueFunction.Run(message, id, log, serviceProvider, cancellationToken);
        }
    }



    public class Queue_SolidRpc_OpenApi_SwaggerUI_Services_ISwaggerUI_GetOpenApiSpec_arg0_arg1
    {
        [FunctionName("Queue_SolidRpc_OpenApi_SwaggerUI_Services_ISwaggerUI_GetOpenApiSpec_arg0_arg1")]
        public static Task Run(
            [QueueTrigger("s8c-o7i-swaggerui-services-iswaggerui-getopenapispec-arg-arg", Connection = "SolidRpcAzQueueConnection")] string message,
            string id,
            [Inject] IServiceProvider serviceProvider,
            ILogger log,
            CancellationToken cancellationToken)
        {
            return QueueFunction.Run(message, id, log, serviceProvider, cancellationToken);
        }
    }



    public class Queue_SolidRpc_OpenApi_SwaggerUI_Services_ISwaggerUI_GetSwaggerUrls
    {
        [FunctionName("Queue_SolidRpc_OpenApi_SwaggerUI_Services_ISwaggerUI_GetSwaggerUrls")]
        public static Task Run(
            [QueueTrigger("solidrpc-openapi-swaggerui-services-iswaggerui-getswaggerurls", Connection = "SolidRpcAzQueueConnection")] string message,
            string id,
            [Inject] IServiceProvider serviceProvider,
            ILogger log,
            CancellationToken cancellationToken)
        {
            return QueueFunction.Run(message, id, log, serviceProvider, cancellationToken);
        }
    }
}
