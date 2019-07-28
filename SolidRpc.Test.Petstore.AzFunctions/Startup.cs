using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using SolidProxy.GeneratorCastle;
using SolidRpc.OpenApi.SwaggerUI.Services;
using SolidRpc.Test.Petstore.Impl;
using SolidRpc.Test.Petstore.Services;
using System;

[assembly: FunctionsStartup(typeof(MyNamespace.Startup))]

namespace MyNamespace
{
    public class Startup : SolidRpc.OpenApi.AzFunctions.Startup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            try
            {
                var preloaded = typeof(SolidRpc.OpenApi.Binder.Proxy.MethodInvoker).Assembly;
                preloaded = typeof(SolidRpc.OpenApi.Binder.MethodBinderStore).Assembly;
                preloaded = typeof(SolidRpc.OpenApi.AspNetCore.Services.SolidRpcStaticContent).Assembly;

                builder.Services.GetSolidConfigurationBuilder().SetGenerator<SolidProxyCastleGenerator>();

                builder.Services.AddSolidRpcBindings(typeof(IPet).Assembly, typeof(PetImpl).Assembly);
                builder.Services.GetSolidRpcStaticContent().AddContent(typeof(PetImpl).Assembly, "www", "/");
                builder.Services.AddSolidRpcBindings(typeof(SwaggerUI).Assembly);
                builder.Services.GetSolidRpcStaticContent().AddContent(typeof(SwaggerUI).Assembly, "www", "/swagger");

                base.Configure(builder);
            }
            catch (Exception e)
            {
                Log("Exception caught:" + e);
            }
            finally
            {
                Log("Configured");
            }
        }
    }
}