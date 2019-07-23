using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Hosting;
using SolidRpc.OpenApi.AzFunctions.Bindings;

[assembly: WebJobsStartup(typeof(InjectWebJobsStartup))]

namespace SolidRpc.OpenApi.AzFunctions.Bindings
{
    public class InjectWebJobsStartup : IWebJobsStartup
    {
        public void Configure(IWebJobsBuilder builder)
        {
            builder.AddExtension<ConstantExtensionConfigProvider>();
            builder.AddExtension<InjectExtensionConfigProvider>();
        }
    }
}
