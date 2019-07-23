
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using SolidRpc.OpenApi.AzFunctions.Functions;
using SolidRpc.OpenApi.AzFunctions.Functions.Impl;
using System;
using System.IO;
using System.Linq;

namespace SolidRpc.OpenApi.AzFunctions
{
    /// <summary>
    /// The startup class
    /// </summary>
    public class Startup : FunctionsStartup
    {
        /// <summary>
        /// Configures the host
        /// </summary>
        /// <param name="builder"></param>
        public override void Configure(IFunctionsHostBuilder builder)
        {
            //
            // make sure that there is an "Initialize" function
            //
            var assemblyLocattion = new FileInfo(typeof(Startup).Assembly.Location);
            if(!assemblyLocattion.Exists)
            {
                throw new Exception("Cannot find location of assebly.");
            }
            if(assemblyLocattion.Directory.Name != "bin")
            {
                throw new Exception("Assemblies are not placed in the bin folder.");
            }
            var funcHandler = new AzFunctionHandler(assemblyLocattion.Directory.Parent);
            var initFunc = funcHandler.Functions.SingleOrDefault(o => o.Name == "Initialize");
            if (initFunc != null && false == initFunc is IAzTimerFunction)
            {
                initFunc.Delete();
                initFunc = null;
            }
            var timerFunc = (IAzTimerFunction)initFunc;
            if (timerFunc == null)
            {
                timerFunc = funcHandler.CreateTimerFunction("Initialize");
            }
            timerFunc.RunOnStartup = true;
            timerFunc.ServiceType = typeof(ISolidRpcSetup).FullName;
            timerFunc.MethodName = nameof(ISolidRpcSetup.Setup);
            timerFunc.Save();

            builder.Services.AddSingleton<IAzFunctionHandler>(funcHandler);

            builder.Services.AddSingleton<ISolidRpcSetup, SolidRpcSetup>();
        }
    }
}
