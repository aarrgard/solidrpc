
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


    public class Http_SolidRpc_Node_Services_INodeService_ExecuteFileAsync_arg0_arg1_arg2
    {
        [FunctionName("Http_SolidRpc_Node_Services_INodeService_ExecuteFileAsync_arg0_arg1_arg2")]
        public static Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "SolidRpc/Node/Services/INodeService/ExecuteFileAsync/{arg0}/{arg1}/{arg2}")] HttpRequestMessage req,
            [Inject] IServiceProvider serviceProvider,
            ILogger log,
            CancellationToken cancellationToken)
        {
            return HttpFunction.Run(req, log, serviceProvider, cancellationToken);
        }
    }


    public class Http_SolidRpc_Node_Services_INodeService_ExecuteScriptAsync_arg0_arg1
    {
        [FunctionName("Http_SolidRpc_Node_Services_INodeService_ExecuteScriptAsync_arg0_arg1")]
        public static Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "SolidRpc/Node/Services/INodeService/ExecuteScriptAsync/{arg0}/{arg1}")] HttpRequestMessage req,
            [Inject] IServiceProvider serviceProvider,
            ILogger log,
            CancellationToken cancellationToken)
        {
            return HttpFunction.Run(req, log, serviceProvider, cancellationToken);
        }
    }


    public class Http_SolidRpc_Node_Services_INodeService_GetNodeVersionAsync
    {
        [FunctionName("Http_SolidRpc_Node_Services_INodeService_GetNodeVersionAsync")]
        public static Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "SolidRpc/Node/Services/INodeService/GetNodeVersionAsync")] HttpRequestMessage req,
            [Inject] IServiceProvider serviceProvider,
            ILogger log,
            CancellationToken cancellationToken)
        {
            return HttpFunction.Run(req, log, serviceProvider, cancellationToken);
        }
    }


    public class Http_SolidRpc_Node_Services_INodeService_ListNodeModulesAsync
    {
        [FunctionName("Http_SolidRpc_Node_Services_INodeService_ListNodeModulesAsync")]
        public static Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "SolidRpc/Node/Services/INodeService/ListNodeModulesAsync")] HttpRequestMessage req,
            [Inject] IServiceProvider serviceProvider,
            ILogger log,
            CancellationToken cancellationToken)
        {
            return HttpFunction.Run(req, log, serviceProvider, cancellationToken);
        }
    }


    public class Http_SolidRpc_NpmGenerator_Services_INpmGenerator_CreateCodeNamespace_arg0
    {
        [FunctionName("Http_SolidRpc_NpmGenerator_Services_INpmGenerator_CreateCodeNamespace_arg0")]
        public static Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "SolidRpc/NpmGenerator/Services/INpmGenerator/CreateCodeNamespace/{arg0}")] HttpRequestMessage req,
            [Inject] IServiceProvider serviceProvider,
            ILogger log,
            CancellationToken cancellationToken)
        {
            return HttpFunction.Run(req, log, serviceProvider, cancellationToken);
        }
    }


    public class Http_SolidRpc_NpmGenerator_Services_INpmGenerator_CreateNpm_arg0
    {
        [FunctionName("Http_SolidRpc_NpmGenerator_Services_INpmGenerator_CreateNpm_arg0")]
        public static Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "SolidRpc/NpmGenerator/Services/INpmGenerator/CreateNpm/{arg0}")] HttpRequestMessage req,
            [Inject] IServiceProvider serviceProvider,
            ILogger log,
            CancellationToken cancellationToken)
        {
            return HttpFunction.Run(req, log, serviceProvider, cancellationToken);
        }
    }


    public class Http_SolidRpc_NpmGenerator_Services_INpmGenerator_CreateNpmPackage_arg0
    {
        [FunctionName("Http_SolidRpc_NpmGenerator_Services_INpmGenerator_CreateNpmPackage_arg0")]
        public static Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "SolidRpc/NpmGenerator/Services/INpmGenerator/CreateNpmPackage/{arg0}")] HttpRequestMessage req,
            [Inject] IServiceProvider serviceProvider,
            ILogger log,
            CancellationToken cancellationToken)
        {
            return HttpFunction.Run(req, log, serviceProvider, cancellationToken);
        }
    }


    public class Http_SolidRpc_NpmGenerator_Services_INpmGenerator_CreateTypesTs_arg0
    {
        [FunctionName("Http_SolidRpc_NpmGenerator_Services_INpmGenerator_CreateTypesTs_arg0")]
        public static Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "SolidRpc/NpmGenerator/Services/INpmGenerator/CreateTypesTs/{arg0}")] HttpRequestMessage req,
            [Inject] IServiceProvider serviceProvider,
            ILogger log,
            CancellationToken cancellationToken)
        {
            return HttpFunction.Run(req, log, serviceProvider, cancellationToken);
        }
    }


    public class Http_SolidRpc_NpmGenerator_Services_INpmGenerator_RunNpm_arg0
    {
        [FunctionName("Http_SolidRpc_NpmGenerator_Services_INpmGenerator_RunNpm_arg0")]
        public static Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "SolidRpc/NpmGenerator/Services/INpmGenerator/RunNpm/{arg0}")] HttpRequestMessage req,
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


    public class Http_SolidRpc_OpenApi_AzQueue_Services_IAzTableQueue_SendTestMessageAsync
    {
        [FunctionName("Http_SolidRpc_OpenApi_AzQueue_Services_IAzTableQueue_SendTestMessageAsync")]
        public static Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "SolidRpc/OpenApi/AzQueue/Services/IAzTableQueue/SendTestMessageAsync")] HttpRequestMessage req,
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


    public class Http_SolidRpc_OpenApi_SwaggerUI_Services_ISwaggerUI_GetOauth2RedirectHtml
    {
        [FunctionName("Http_SolidRpc_OpenApi_SwaggerUI_Services_ISwaggerUI_GetOauth2RedirectHtml")]
        public static Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "SolidRpc/OpenApi/SwaggerUI/Services/ISwaggerUI/GetOauth2RedirectHtml")] HttpRequestMessage req,
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


    public class Http_SolidRpc_Security_Services_LoginPage
    {
        [FunctionName("Http_SolidRpc_Security_Services_LoginPage")]
        public static Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "SolidRpc/Security/Services/LoginPage")] HttpRequestMessage req,
            [Inject] IServiceProvider serviceProvider,
            ILogger log,
            CancellationToken cancellationToken)
        {
            return HttpFunction.Run(req, log, serviceProvider, cancellationToken);
        }
    }


    public class Http_SolidRpc_Security_Services_LoginProviders
    {
        [FunctionName("Http_SolidRpc_Security_Services_LoginProviders")]
        public static Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "SolidRpc/Security/Services/LoginProviders")] HttpRequestMessage req,
            [Inject] IServiceProvider serviceProvider,
            ILogger log,
            CancellationToken cancellationToken)
        {
            return HttpFunction.Run(req, log, serviceProvider, cancellationToken);
        }
    }


    public class Http_SolidRpc_Security_Services_LoginScript
    {
        [FunctionName("Http_SolidRpc_Security_Services_LoginScript")]
        public static Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "SolidRpc/Security/Services/LoginScript")] HttpRequestMessage req,
            [Inject] IServiceProvider serviceProvider,
            ILogger log,
            CancellationToken cancellationToken)
        {
            return HttpFunction.Run(req, log, serviceProvider, cancellationToken);
        }
    }


    public class Http_SolidRpc_Security_Services_LoginScripts
    {
        [FunctionName("Http_SolidRpc_Security_Services_LoginScripts")]
        public static Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "SolidRpc/Security/Services/LoginScripts")] HttpRequestMessage req,
            [Inject] IServiceProvider serviceProvider,
            ILogger log,
            CancellationToken cancellationToken)
        {
            return HttpFunction.Run(req, log, serviceProvider, cancellationToken);
        }
    }


    public class Http_SolidRpc_Security_Services_Oidc_authorize
    {
        [FunctionName("Http_SolidRpc_Security_Services_Oidc_authorize")]
        public static Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "SolidRpc/Security/Services/Oidc/authorize")] HttpRequestMessage req,
            [Inject] IServiceProvider serviceProvider,
            ILogger log,
            CancellationToken cancellationToken)
        {
            return HttpFunction.Run(req, log, serviceProvider, cancellationToken);
        }
    }


    public class Http_SolidRpc_Security_Services_Oidc_discovery
    {
        [FunctionName("Http_SolidRpc_Security_Services_Oidc_discovery")]
        public static Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "SolidRpc/Security/Services/Oidc/discovery")] HttpRequestMessage req,
            [Inject] IServiceProvider serviceProvider,
            ILogger log,
            CancellationToken cancellationToken)
        {
            return HttpFunction.Run(req, log, serviceProvider, cancellationToken);
        }
    }


    public class Http_SolidRpc_Security_Services_Oidc_keys
    {
        [FunctionName("Http_SolidRpc_Security_Services_Oidc_keys")]
        public static Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "SolidRpc/Security/Services/Oidc/keys")] HttpRequestMessage req,
            [Inject] IServiceProvider serviceProvider,
            ILogger log,
            CancellationToken cancellationToken)
        {
            return HttpFunction.Run(req, log, serviceProvider, cancellationToken);
        }
    }


    public class Http_SolidRpc_Security_Services_Oidc_token
    {
        [FunctionName("Http_SolidRpc_Security_Services_Oidc_token")]
        public static Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "SolidRpc/Security/Services/Oidc/token")] HttpRequestMessage req,
            [Inject] IServiceProvider serviceProvider,
            ILogger log,
            CancellationToken cancellationToken)
        {
            return HttpFunction.Run(req, log, serviceProvider, cancellationToken);
        }
    }


    public class Http_SolidRpc_Security_Services_Profile
    {
        [FunctionName("Http_SolidRpc_Security_Services_Profile")]
        public static Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "SolidRpc/Security/Services/Profile")] HttpRequestMessage req,
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


    public class Http_SolidRpc_Test_Petstore_AzFunctionsV3_ITestInterface_RunNodeService
    {
        [FunctionName("Http_SolidRpc_Test_Petstore_AzFunctionsV3_ITestInterface_RunNodeService")]
        public static Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "SolidRpc/Test/Petstore/AzFunctionsV3/ITestInterface/RunNodeService")] HttpRequestMessage req,
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


    public class Timer_SolidRpc_Abstractions_Services_ISolidRpcHost_GetHostId
    {
        [FunctionName("Timer_SolidRpc_Abstractions_Services_ISolidRpcHost_GetHostId")]
        public static Task Run(
            [TimerTrigger("0 * * * * *", RunOnStartup = false)] TimerInfo timerInfo,
            [Inject] IServiceProvider serviceProvider,
            [Constant("SolidRpc.Abstractions.Services.ISolidRpcHost")] Type serviceType,
            [Constant("GetHostId")] string methodName,
            ILogger log,
            CancellationToken cancellationToken)
        {
            return TimerFunction.Run(timerInfo, log, serviceProvider, serviceType, methodName, cancellationToken);
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
