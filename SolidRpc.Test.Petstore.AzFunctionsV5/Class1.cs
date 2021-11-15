using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;


namespace SolidRpc.Test.Petstore.AzFunctionsV5
{
    class Program
    {
        readonly static string _storageConnectionString = Environment.GetEnvironmentVariable("AzureWebJobsStorage") ?? string.Empty;

        static Task Main(string[] args)
        {
            var host = new HostBuilder()
                .ConfigureAppConfiguration(configurationBuilder =>
                {
                    configurationBuilder.AddCommandLine(args);
                })
                .ConfigureFunctionsWorkerDefaults()
                .ConfigureServices(services =>
                {
                    // Add Logging
                    services.AddLogging();

                    // Add HttpClient
                    services.AddHttpClient();

                    // Add Custom Services
                })
                .Build();

            return host.RunAsync();
        }
    }
}