using SolidRpc.OpenApi.AspNetCore.Services;
using System.Linq;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Extension methods fro the http request
    /// </summary>
    public static class IServiceCollectionExtensions
    {
        /// <summary>
        /// Returns the static content provider.
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static ISolidRpcStaticContent GetSolidRpcStaticContent(this IServiceCollection services)
        {
            var service = services.SingleOrDefault(o => o.ServiceType == typeof(ISolidRpcStaticContent));
            if(service == null)
            {
                service = new ServiceDescriptor(typeof(ISolidRpcStaticContent), new SolidRpcStaticContent());
                services.Add(service);
            }
            return (ISolidRpcStaticContent) service.ImplementationInstance;
        }
     }
}
