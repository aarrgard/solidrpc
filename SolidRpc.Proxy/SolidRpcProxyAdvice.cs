using SolidProxy.Core.Proxy;
using System;
using System.Threading.Tasks;

namespace SolidRpc.Proxy
{
    public class SolidRpcProxyAdvice<TObject, TMethod, TAdvice> : ISolidProxyInvocationAdvice<TObject, TMethod, TAdvice> where TObject : class
    {
        public void Configure(ISolidRpcProxyConfig config)
        {

        }

        public Task<TAdvice> Handle(Func<Task<TAdvice>> next, ISolidProxyInvocation<TObject, TMethod, TAdvice> invocation)
        {
            throw new NotImplementedException();
        }
    }
}
