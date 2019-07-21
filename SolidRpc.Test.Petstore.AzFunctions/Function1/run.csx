#r "Newtonsoft.Json"
#r "Microsoft.Extensions.DependencyInjection.Abstractions"
#r "../bin/SolidRpc.OpenApi.AzFunctions.dll"
#r "../bin/SolidRpc.OpenApi.AspNetCore.dll"
#r "../bin/SolidRpc.OpenApi.Binder.dll"

using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using SolidRpc.OpenApi.Binder.Http;
using SolidRpc.OpenApi.AzFunctions;

public static async Task<IActionResult> Run(HttpRequest req, ILogger log)
{
    var rpcSetup = req.HttpContext.RequestServices.GetRequiredService<ISolidRpcSetup>();
    var solidReq = new SolidHttpRequest();
    await solidReq.CopyFromAsync(req);

    var res = await rpcSetup.MethodInvoker.InvokeAsync(solidReq, req.HttpContext.RequestAborted);

    log.LogInformation($"C# HTTP trigger function processed a request - {res.StatusCode}");

    return await res.CreateActionResult();
}
