using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Hosting;
using SolidRpc.OpenApi.AzFunctions.Bindings;

[assembly: WebJobsStartup(typeof(InjectWebJobsStartup))]

namespace SolidRpc.OpenApi.AzFunctions.Bindings
{
    /// <summary>
    /// 
    /// </summary>
    public class InjectWebJobsStartup : IWebJobsStartup
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        public void Configure(IWebJobsBuilder builder)
        {
            builder.AddExtension<ConstantExtensionConfigProvider>();
            builder.AddExtension<InjectExtensionConfigProvider>();
        }
    }
}
