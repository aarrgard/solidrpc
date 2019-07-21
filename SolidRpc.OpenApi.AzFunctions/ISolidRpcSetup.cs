using SolidRpc.OpenApi.Binder.Proxy;

namespace SolidRpc.OpenApi.AzFunctions
{
    /// <summary>
    /// Interface that is used to setup the solid rpc infrastructure
    /// </summary>
    public interface ISolidRpcSetup
    {
        /// <summary>
        /// Returns the method invoker.
        /// </summary>
        IMethodInvoker MethodInvoker { get; }
    }
}