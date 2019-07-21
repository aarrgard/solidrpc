using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using SolidProxy.GeneratorCastle;
using SolidRpc.Test.Petstore.Impl;
using SolidRpc.Test.Petstore.Services;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using SolidRpc.OpenApi.Binder.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SolidRpc.OpenApi.AzFunctions;

[assembly: FunctionsStartup(typeof(MyNamespace.Startup))]

namespace MyNamespace
{
    public class Startup : SolidRpc.OpenApi.AzFunctions.Startup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddLogging(o => {
                o.SetMinimumLevel(LogLevel.Trace);
            });
            builder.Services.GetSolidConfigurationBuilder().SetGenerator<SolidProxyCastleGenerator>();
            builder.Services.AddSolidRpcBindings(typeof(IPet).Assembly, typeof(PetImpl).Assembly);

            base.Configure(builder);
        }
        private async Task<IActionResult> Dummy(HttpRequest req, ILogger log)
        {
            var rpcSetup = req.HttpContext.RequestServices.GetRequiredService<ISolidRpcSetup>();
            var solidReq = new SolidHttpRequest();
            await solidReq.CopyFromAsync(req);

            var res = await rpcSetup.MethodInvoker.InvokeAsync(solidReq, req.HttpContext.RequestAborted);

            log.LogInformation($"C# HTTP trigger function processed a request - {res.StatusCode}");

            return await res.CreateActionResult();
        }
    }
}