
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using SolidRpc.OpenApi.AzFunctions.Services;
using System;
using System.IO;
using System.Threading;

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
            builder.Services.AddStartupFunction<ISolidRpcSetup, SolidRpcSetup>(o => o.Setup(CancellationToken.None));
            builder.Services.AddHttpFunction<ISolidRpcSetup, SolidRpcSetup>(o => o.Setup(CancellationToken.None));
        }

        /// <summary>
        /// Support function for logging.
        /// </summary>
        /// <param name="msg"></param>
        protected void Log(string msg)
        {
            var log = $"{typeof(Startup).Assembly.Location}.log.txt";
            using (var sw = new FileInfo(log).AppendText())
            {
                sw.WriteLine($"{DateTime.Now.ToString("HH:mm:ss.ffff")}{msg}");
            }
        }
    }
}
