
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

    public class Http_SolidRpc_Abstractions_Services_Code_ICodeNamespaceGenerator_CreateCodeNamespace_arg0
    {
        [FunctionName("Http_SolidRpc_Abstractions_Services_Code_ICodeNamespaceGenerator_CreateCodeNamespace_arg0")]
        public static Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "options", Route = "SolidRpc/Abstractions/Services/Code/ICodeNamespaceGenerator/CreateCodeNamespace/{arg0}")] HttpRequestMessage req,
            [Inject] IServiceProvider serviceProvider,
            ILogger log,
            CancellationToken cancellationToken)
        {
            return HttpFunction.Run(req, log, serviceProvider, cancellationToken);
        }
    }


    public class Http_SolidRpc_Abstractions_Services_Code_INpmGenerator_CreateInitialZip
    {
        [FunctionName("Http_SolidRpc_Abstractions_Services_Code_INpmGenerator_CreateInitialZip")]
        public static Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "options", Route = "SolidRpc/Abstractions/Services/Code/INpmGenerator/CreateInitialZip")] HttpRequestMessage req,
            [Inject] IServiceProvider serviceProvider,
            ILogger log,
            CancellationToken cancellationToken)
        {
            return HttpFunction.Run(req, log, serviceProvider, cancellationToken);
        }
    }


    public class Http_SolidRpc_Abstractions_Services_Code_INpmGenerator_CreateNpmPackage_arg0
    {
        [FunctionName("Http_SolidRpc_Abstractions_Services_Code_INpmGenerator_CreateNpmPackage_arg0")]
        public static Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "options", Route = "SolidRpc/Abstractions/Services/Code/INpmGenerator/CreateNpmPackage/{arg0}")] HttpRequestMessage req,
            [Inject] IServiceProvider serviceProvider,
            ILogger log,
            CancellationToken cancellationToken)
        {
            return HttpFunction.Run(req, log, serviceProvider, cancellationToken);
        }
    }


    public class Http_SolidRpc_Abstractions_Services_Code_ITypescriptGenerator_CreateTypesTsForAssemblyAsync_arg0
    {
        [FunctionName("Http_SolidRpc_Abstractions_Services_Code_ITypescriptGenerator_CreateTypesTsForAssemblyAsync_arg0")]
        public static Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "options", Route = "SolidRpc/Abstractions/Services/Code/ITypescriptGenerator/CreateTypesTsForAssemblyAsync/{arg0}")] HttpRequestMessage req,
            [Inject] IServiceProvider serviceProvider,
            ILogger log,
            CancellationToken cancellationToken)
        {
            return HttpFunction.Run(req, log, serviceProvider, cancellationToken);
        }
    }


    public class Http_SolidRpc_Abstractions_Services_Code_ITypescriptGenerator_CreateTypesTsForCodeNamespaceAsync
    {
        [FunctionName("Http_SolidRpc_Abstractions_Services_Code_ITypescriptGenerator_CreateTypesTsForCodeNamespaceAsync")]
        public static Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "options", "post", Route = "SolidRpc/Abstractions/Services/Code/ITypescriptGenerator/CreateTypesTsForCodeNamespaceAsync")] HttpRequestMessage req,
            [Inject] IServiceProvider serviceProvider,
            ILogger log,
            CancellationToken cancellationToken)
        {
            return HttpFunction.Run(req, log, serviceProvider, cancellationToken);
        }
    }


    public class Http_SolidRpc_Abstractions_Services_ISolidRpcContentHandler_GetContent
    {
        [FunctionName("Http_SolidRpc_Abstractions_Services_ISolidRpcContentHandler_GetContent")]
        public static Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "options", Route = "SolidRpc/Abstractions/Services/ISolidRpcContentHandler/GetContent")] HttpRequestMessage req,
            [Inject] IServiceProvider serviceProvider,
            ILogger log,
            CancellationToken cancellationToken)
        {
            return HttpFunction.Run(req, log, serviceProvider, cancellationToken);
        }
    }


    public class Http_SolidRpc_Abstractions_Services_ISolidRpcContentHandler_GetPathMappingsAsync_arg0
    {
        [FunctionName("Http_SolidRpc_Abstractions_Services_ISolidRpcContentHandler_GetPathMappingsAsync_arg0")]
        public static Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "options", Route = "SolidRpc/Abstractions/Services/ISolidRpcContentHandler/GetPathMappingsAsync/{arg0}")] HttpRequestMessage req,
            [Inject] IServiceProvider serviceProvider,
            ILogger log,
            CancellationToken cancellationToken)
        {
            return HttpFunction.Run(req, log, serviceProvider, cancellationToken);
        }
    }


    public class Http_SolidRpc_Abstractions_Services_ISolidRpcHost_AllowedCorsOrigins
    {
        [FunctionName("Http_SolidRpc_Abstractions_Services_ISolidRpcHost_AllowedCorsOrigins")]
        public static Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "options", Route = "SolidRpc/Abstractions/Services/ISolidRpcHost/AllowedCorsOrigins")] HttpRequestMessage req,
            [Inject] IServiceProvider serviceProvider,
            ILogger log,
            CancellationToken cancellationToken)
        {
            return HttpFunction.Run(req, log, serviceProvider, cancellationToken);
        }
    }


    public class Http_SolidRpc_Abstractions_Services_ISolidRpcHost_BaseAddress
    {
        [FunctionName("Http_SolidRpc_Abstractions_Services_ISolidRpcHost_BaseAddress")]
        public static Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "options", Route = "SolidRpc/Abstractions/Services/ISolidRpcHost/BaseAddress")] HttpRequestMessage req,
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
            [HttpTrigger(AuthorizationLevel.Anonymous, "options", "post", Route = "SolidRpc/Abstractions/Services/ISolidRpcHost/CheckHost")] HttpRequestMessage req,
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
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "options", Route = "SolidRpc/Abstractions/Services/ISolidRpcHost/GetHostConfiguration")] HttpRequestMessage req,
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
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "options", Route = "SolidRpc/Abstractions/Services/ISolidRpcHost/GetHostId")] HttpRequestMessage req,
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
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "options", Route = "SolidRpc/Abstractions/Services/ISolidRpcHost/GetHostInstance")] HttpRequestMessage req,
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
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "options", Route = "SolidRpc/Abstractions/Services/ISolidRpcHost/IsAlive")] HttpRequestMessage req,
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
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "options", Route = "SolidRpc/Abstractions/Services/ISolidRpcHost/SyncHostsFromStore")] HttpRequestMessage req,
            [Inject] IServiceProvider serviceProvider,
            ILogger log,
            CancellationToken cancellationToken)
        {
            return HttpFunction.Run(req, log, serviceProvider, cancellationToken);
        }
    }


    public class Http_SolidRpc_Abstractions_Services_ISolidRpcHostStore_AddHostInstanceAsync
    {
        [FunctionName("Http_SolidRpc_Abstractions_Services_ISolidRpcHostStore_AddHostInstanceAsync")]
        public static Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "options", "post", Route = "SolidRpc/Abstractions/Services/ISolidRpcHostStore/AddHostInstanceAsync")] HttpRequestMessage req,
            [Inject] IServiceProvider serviceProvider,
            ILogger log,
            CancellationToken cancellationToken)
        {
            return HttpFunction.Run(req, log, serviceProvider, cancellationToken);
        }
    }


    public class Http_SolidRpc_Abstractions_Services_ISolidRpcHostStore_ListHostInstancesAsync
    {
        [FunctionName("Http_SolidRpc_Abstractions_Services_ISolidRpcHostStore_ListHostInstancesAsync")]
        public static Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "options", Route = "SolidRpc/Abstractions/Services/ISolidRpcHostStore/ListHostInstancesAsync")] HttpRequestMessage req,
            [Inject] IServiceProvider serviceProvider,
            ILogger log,
            CancellationToken cancellationToken)
        {
            return HttpFunction.Run(req, log, serviceProvider, cancellationToken);
        }
    }


    public class Http_SolidRpc_Abstractions_Services_ISolidRpcHostStore_RemoveHostInstanceAsync
    {
        [FunctionName("Http_SolidRpc_Abstractions_Services_ISolidRpcHostStore_RemoveHostInstanceAsync")]
        public static Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "options", "post", Route = "SolidRpc/Abstractions/Services/ISolidRpcHostStore/RemoveHostInstanceAsync")] HttpRequestMessage req,
            [Inject] IServiceProvider serviceProvider,
            ILogger log,
            CancellationToken cancellationToken)
        {
            return HttpFunction.Run(req, log, serviceProvider, cancellationToken);
        }
    }


    public class Http_SolidRpc_Abstractions_Services_ISolidRpcOAuth2_GetAuthorizationCodeTokenAsync
    {
        [FunctionName("Http_SolidRpc_Abstractions_Services_ISolidRpcOAuth2_GetAuthorizationCodeTokenAsync")]
        public static Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "options", Route = "SolidRpc/Abstractions/Services/ISolidRpcOAuth2/GetAuthorizationCodeTokenAsync")] HttpRequestMessage req,
            [Inject] IServiceProvider serviceProvider,
            ILogger log,
            CancellationToken cancellationToken)
        {
            return HttpFunction.Run(req, log, serviceProvider, cancellationToken);
        }
    }


    public class Http_SolidRpc_Abstractions_Services_ISolidRpcOAuth2_RefreshTokenAsync_arg0
    {
        [FunctionName("Http_SolidRpc_Abstractions_Services_ISolidRpcOAuth2_RefreshTokenAsync_arg0")]
        public static Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "options", Route = "SolidRpc/Abstractions/Services/ISolidRpcOAuth2/RefreshTokenAsync/{arg0}")] HttpRequestMessage req,
            [Inject] IServiceProvider serviceProvider,
            ILogger log,
            CancellationToken cancellationToken)
        {
            return HttpFunction.Run(req, log, serviceProvider, cancellationToken);
        }
    }


    public class Http_SolidRpc_Abstractions_Services_ISolidRpcOAuth2_TokenCallbackAsync
    {
        [FunctionName("Http_SolidRpc_Abstractions_Services_ISolidRpcOAuth2_TokenCallbackAsync")]
        public static Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "options", Route = "SolidRpc/Abstractions/Services/ISolidRpcOAuth2/TokenCallbackAsync")] HttpRequestMessage req,
            [Inject] IServiceProvider serviceProvider,
            ILogger log,
            CancellationToken cancellationToken)
        {
            return HttpFunction.Run(req, log, serviceProvider, cancellationToken);
        }
    }


    public class Http_SolidRpc_Abstractions_Services_ISolidRpcOidc_AuthorizeAsync
    {
        [FunctionName("Http_SolidRpc_Abstractions_Services_ISolidRpcOidc_AuthorizeAsync")]
        public static Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "options", Route = "SolidRpc/Abstractions/Services/ISolidRpcOidc/AuthorizeAsync")] HttpRequestMessage req,
            [Inject] IServiceProvider serviceProvider,
            ILogger log,
            CancellationToken cancellationToken)
        {
            return HttpFunction.Run(req, log, serviceProvider, cancellationToken);
        }
    }


    public class Http_SolidRpc_Abstractions_Services_ISolidRpcOidc_GetKeysAsync
    {
        [FunctionName("Http_SolidRpc_Abstractions_Services_ISolidRpcOidc_GetKeysAsync")]
        public static Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "options", Route = "SolidRpc/Abstractions/Services/ISolidRpcOidc/GetKeysAsync")] HttpRequestMessage req,
            [Inject] IServiceProvider serviceProvider,
            ILogger log,
            CancellationToken cancellationToken)
        {
            return HttpFunction.Run(req, log, serviceProvider, cancellationToken);
        }
    }


    public class Http_SolidRpc_Abstractions_Services_ISolidRpcOidc_GetTokenAsync
    {
        [FunctionName("Http_SolidRpc_Abstractions_Services_ISolidRpcOidc_GetTokenAsync")]
        public static Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "options", "post", Route = "SolidRpc/Abstractions/Services/ISolidRpcOidc/GetTokenAsync")] HttpRequestMessage req,
            [Inject] IServiceProvider serviceProvider,
            ILogger log,
            CancellationToken cancellationToken)
        {
            return HttpFunction.Run(req, log, serviceProvider, cancellationToken);
        }
    }


    public class Http_SolidRpc_Abstractions_Services_RateLimit_ISolidRpcRateLimit_GetRateLimitSettingsAsync
    {
        [FunctionName("Http_SolidRpc_Abstractions_Services_RateLimit_ISolidRpcRateLimit_GetRateLimitSettingsAsync")]
        public static Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "options", Route = "SolidRpc/Abstractions/Services/RateLimit/ISolidRpcRateLimit/GetRateLimitSettingsAsync")] HttpRequestMessage req,
            [Inject] IServiceProvider serviceProvider,
            ILogger log,
            CancellationToken cancellationToken)
        {
            return HttpFunction.Run(req, log, serviceProvider, cancellationToken);
        }
    }


    public class Http_SolidRpc_Abstractions_Services_RateLimit_ISolidRpcRateLimit_GetRateLimitTokenAsync_arg0_arg1
    {
        [FunctionName("Http_SolidRpc_Abstractions_Services_RateLimit_ISolidRpcRateLimit_GetRateLimitTokenAsync_arg0_arg1")]
        public static Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "options", Route = "SolidRpc/Abstractions/Services/RateLimit/ISolidRpcRateLimit/GetRateLimitTokenAsync/{arg0}/{arg1}")] HttpRequestMessage req,
            [Inject] IServiceProvider serviceProvider,
            ILogger log,
            CancellationToken cancellationToken)
        {
            return HttpFunction.Run(req, log, serviceProvider, cancellationToken);
        }
    }


    public class Http_SolidRpc_Abstractions_Services_RateLimit_ISolidRpcRateLimit_GetSingeltonTokenAsync_arg0_arg1
    {
        [FunctionName("Http_SolidRpc_Abstractions_Services_RateLimit_ISolidRpcRateLimit_GetSingeltonTokenAsync_arg0_arg1")]
        public static Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "options", Route = "SolidRpc/Abstractions/Services/RateLimit/ISolidRpcRateLimit/GetSingeltonTokenAsync/{arg0}/{arg1}")] HttpRequestMessage req,
            [Inject] IServiceProvider serviceProvider,
            ILogger log,
            CancellationToken cancellationToken)
        {
            return HttpFunction.Run(req, log, serviceProvider, cancellationToken);
        }
    }


    public class Http_SolidRpc_Abstractions_Services_RateLimit_ISolidRpcRateLimit_ReturnRateLimitTokenAsync
    {
        [FunctionName("Http_SolidRpc_Abstractions_Services_RateLimit_ISolidRpcRateLimit_ReturnRateLimitTokenAsync")]
        public static Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "options", "post", Route = "SolidRpc/Abstractions/Services/RateLimit/ISolidRpcRateLimit/ReturnRateLimitTokenAsync")] HttpRequestMessage req,
            [Inject] IServiceProvider serviceProvider,
            ILogger log,
            CancellationToken cancellationToken)
        {
            return HttpFunction.Run(req, log, serviceProvider, cancellationToken);
        }
    }


    public class Http_SolidRpc_Abstractions_Services_RateLimit_ISolidRpcRateLimit_UpdateRateLimitSetting
    {
        [FunctionName("Http_SolidRpc_Abstractions_Services_RateLimit_ISolidRpcRateLimit_UpdateRateLimitSetting")]
        public static Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "options", "post", Route = "SolidRpc/Abstractions/Services/RateLimit/ISolidRpcRateLimit/UpdateRateLimitSetting")] HttpRequestMessage req,
            [Inject] IServiceProvider serviceProvider,
            ILogger log,
            CancellationToken cancellationToken)
        {
            return HttpFunction.Run(req, log, serviceProvider, cancellationToken);
        }
    }


    public class Http_SolidRpc_Abstractions_well_known_openid_configuration
    {
        [FunctionName("Http_SolidRpc_Abstractions_well_known_openid_configuration")]
        public static Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "options", Route = "SolidRpc/Abstractions/.well-known/openid-configuration")] HttpRequestMessage req,
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
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "options", Route = "SolidRpc/Node/Services/INodeService/ExecuteFileAsync/{arg0}/{arg1}/{arg2}")] HttpRequestMessage req,
            [Inject] IServiceProvider serviceProvider,
            ILogger log,
            CancellationToken cancellationToken)
        {
            return HttpFunction.Run(req, log, serviceProvider, cancellationToken);
        }
    }


    public class Http_SolidRpc_Node_Services_INodeService_ExecuteScriptAsync
    {
        [FunctionName("Http_SolidRpc_Node_Services_INodeService_ExecuteScriptAsync")]
        public static Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "options", "post", Route = "SolidRpc/Node/Services/INodeService/ExecuteScriptAsync")] HttpRequestMessage req,
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
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "options", Route = "SolidRpc/Node/Services/INodeService/GetNodeVersionAsync")] HttpRequestMessage req,
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
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "options", Route = "SolidRpc/Node/Services/INodeService/ListNodeModulesAsync")] HttpRequestMessage req,
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
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "options", Route = "SolidRpc/OpenApi/AzQueue/Services/IAzTableQueue/DispatchMessageAsync/{arg0}/{arg1}")] HttpRequestMessage req,
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
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "options", Route = "SolidRpc/OpenApi/AzQueue/Services/IAzTableQueue/DoScheduledScanAsync")] HttpRequestMessage req,
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
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "options", Route = "SolidRpc/OpenApi/AzQueue/Services/IAzTableQueue/FlagErrorMessagesAsPending")] HttpRequestMessage req,
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
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "options", Route = "SolidRpc/OpenApi/AzQueue/Services/IAzTableQueue/GetSettingsAsync")] HttpRequestMessage req,
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
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "options", Route = "SolidRpc/OpenApi/AzQueue/Services/IAzTableQueue/ListMessagesAsync")] HttpRequestMessage req,
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
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "options", Route = "SolidRpc/OpenApi/AzQueue/Services/IAzTableQueue/ProcessMessageAsync/{arg0}/{arg1}/{arg2}")] HttpRequestMessage req,
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
            [HttpTrigger(AuthorizationLevel.Anonymous, "options", "post", Route = "SolidRpc/OpenApi/AzQueue/Services/IAzTableQueue/ProcessTestMessage/{arg0}")] HttpRequestMessage req,
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
            [HttpTrigger(AuthorizationLevel.Anonymous, "options", "post", Route = "SolidRpc/OpenApi/AzQueue/Services/IAzTableQueue/SendTestMessageAsync")] HttpRequestMessage req,
            [Inject] IServiceProvider serviceProvider,
            ILogger log,
            CancellationToken cancellationToken)
        {
            return HttpFunction.Run(req, log, serviceProvider, cancellationToken);
        }
    }


    public class Http_SolidRpc_OpenApi_AzQueue_Services_IAzTableQueue_SendTestMessageUsingProxyAsync
    {
        [FunctionName("Http_SolidRpc_OpenApi_AzQueue_Services_IAzTableQueue_SendTestMessageUsingProxyAsync")]
        public static Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "options", "post", Route = "SolidRpc/OpenApi/AzQueue/Services/IAzTableQueue/SendTestMessageUsingProxyAsync")] HttpRequestMessage req,
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
            [HttpTrigger(AuthorizationLevel.Anonymous, "options", "post", Route = "SolidRpc/OpenApi/AzQueue/Services/IAzTableQueue/UpdateMessageAsync")] HttpRequestMessage req,
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
            [HttpTrigger(AuthorizationLevel.Anonymous, "options", "post", Route = "SolidRpc/OpenApi/AzQueue/Services/IAzTableQueue/UpdateSettings")] HttpRequestMessage req,
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
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "options", Route = "SolidRpc/OpenApi/SwaggerUI/Services/ISwaggerUI/GetIndexHtml")] HttpRequestMessage req,
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
            [HttpTrigger(AuthorizationLevel.Function, "get", "options", Route = "SolidRpc/OpenApi/SwaggerUI/Services/ISwaggerUI/GetOauth2RedirectHtml")] HttpRequestMessage req,
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
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "options", Route = "SolidRpc/OpenApi/SwaggerUI/Services/ISwaggerUI/GetOpenApiSpec/{arg0}/{arg1}")] HttpRequestMessage req,
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
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "options", Route = "SolidRpc/OpenApi/SwaggerUI/Services/ISwaggerUI/GetSwaggerUrls")] HttpRequestMessage req,
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
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "options", Route = "SolidRpc/Test/Petstore/AzFunctionsV3/ITestInterface/MyFunc/{arg0}")] HttpRequestMessage req,
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
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "options", Route = "SolidRpc/Test/Petstore/AzFunctionsV3/ITestInterface/MyFunc/{arg0}/{arg1}")] HttpRequestMessage req,
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
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "options", Route = "SolidRpc/Test/Petstore/AzFunctionsV3/ITestInterface/RunNodeService")] HttpRequestMessage req,
            [Inject] IServiceProvider serviceProvider,
            ILogger log,
            CancellationToken cancellationToken)
        {
            return HttpFunction.Run(req, log, serviceProvider, cancellationToken);
        }
    }


    public class Http_SolidRpc_Test_Petstore_AzFunctionsV3_ITestInterface_TestSetCookie
    {
        [FunctionName("Http_SolidRpc_Test_Petstore_AzFunctionsV3_ITestInterface_TestSetCookie")]
        public static Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "options", Route = "SolidRpc/Test/Petstore/AzFunctionsV3/ITestInterface/TestSetCookie")] HttpRequestMessage req,
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
            [HttpTrigger(AuthorizationLevel.Anonymous, "options", "post", Route = "SolidRpc/Test/Petstore/AzFunctionsV3/ITestInterface/UploadFile/{arg0}")] HttpRequestMessage req,
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


    public class Timer_SolidRpc_Abstractions_Services_ISolidRpcHost_GetHostId_1
    {
        [FunctionName("Timer_SolidRpc_Abstractions_Services_ISolidRpcHost_GetHostId_1")]
        public static Task Run(
            [TimerTrigger("0 * * * * *", RunOnStartup = false)] TimerInfo timerInfo,
            [Inject] IServiceProvider serviceProvider,
            [Constant("Timer_SolidRpc_Abstractions_Services_ISolidRpcHost_GetHostId_1")] string timerId,
            ILogger log,
            CancellationToken cancellationToken)
        {
            return TimerFunction.Run(timerInfo, log, serviceProvider, timerId, cancellationToken);
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


    public class Timer_SolidRpc_Test_Petstore_AzFunctionsV3_ITestInterface_RunNodeService_1
    {
        [FunctionName("Timer_SolidRpc_Test_Petstore_AzFunctionsV3_ITestInterface_RunNodeService_1")]
        public static Task Run(
            [TimerTrigger("0 * * * * *", RunOnStartup = false)] TimerInfo timerInfo,
            [Inject] IServiceProvider serviceProvider,
            [Constant("Timer_SolidRpc_Test_Petstore_AzFunctionsV3_ITestInterface_RunNodeService_1")] string timerId,
            ILogger log,
            CancellationToken cancellationToken)
        {
            return TimerFunction.Run(timerInfo, log, serviceProvider, timerId, cancellationToken);
        }
    }

}
