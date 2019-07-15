using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using SolidProxy.Core.Proxy;

namespace SolidRpc.OpenApi.Binder.Proxy
{
    /// <summary>
    /// Configures the bindings for the rpc proxy.
    /// </summary>
    public class SolidRpcOpenApiAdvice<TObject, TMethod, TAdvice> : ISolidProxyInvocationAdvice<TObject, TMethod, TAdvice> where TObject : class
    {
        /// <summary>
        /// Configures this advice. By returning false this advice will not be added to the 
        /// adcvice pipeline.
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public bool Configure(ISolidRpcOpenApiConfig config)
        {
            if (config.OpenApiConfiguration == null)
            {
                // locate config base on assembly name
                var assembly = typeof(TObject).Assembly;
                var assemblyName = assembly.GetName().Name;
                var resourceName = assembly.GetManifestResourceNames().FirstOrDefault(o => o.EndsWith($".{assemblyName}.json"));
                if (resourceName == null)
                {
                    throw new Exception($"Solid proxy advice config does not contain a swagger spec for {typeof(TObject).FullName}.");
                }
                using (var s = assembly.GetManifestResourceStream(resourceName))
                {
                    using (var sr = new StreamReader(s))
                    {
                        config.OpenApiConfiguration = sr.ReadToEnd();
                    }
                }
            }
            return false;
        }
        public virtual Task<TAdvice> Handle(Func<Task<TAdvice>> next, ISolidProxyInvocation<TObject, TMethod, TAdvice> invocation)
        {
            throw new NotImplementedException("This advice should never be invoked.");
        }
    }
}
