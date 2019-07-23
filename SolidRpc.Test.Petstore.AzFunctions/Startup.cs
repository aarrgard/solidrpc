﻿using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using SolidProxy.GeneratorCastle;
using SolidRpc.Test.Petstore.Impl;
using SolidRpc.Test.Petstore.Services;
using Microsoft.Extensions.Logging;
using System;
using System.IO;

[assembly: FunctionsStartup(typeof(MyNamespace.Startup))]

namespace MyNamespace
{
    public class Startup : SolidRpc.OpenApi.AzFunctions.Startup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            try
            {
                builder.Services.AddLogging(o => {
                    o.SetMinimumLevel(LogLevel.Trace);
                });
                builder.Services.GetSolidConfigurationBuilder().SetGenerator<SolidProxyCastleGenerator>();
                builder.Services.AddSolidRpcBindings(typeof(IPet).Assembly, typeof(PetImpl).Assembly);

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

        private void Log(string msg)
        {
            var log = $"{typeof(Startup).Assembly.Location}.log.txt";
            using (var sw = new FileInfo(log).AppendText())
            {
                sw.WriteLine($"{DateTime.Now.ToString("HH:mm:ss.ffff")}{msg}");
            }
        }
    }
}