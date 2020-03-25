using System;
using System.Reflection;
using System.Threading.Tasks;

namespace SolidRpc.Abstractions.OpenApi.Transport
{
    /// <summary>
    /// The method used to transform uri:s
    /// </summary>
    /// <param name="serviceProvider"></param>
    /// <param name="originalUri"></param>
    /// <param name="methodInfo">The method to resolve the uri for(may be null)</param>
    /// <returns></returns>
    public delegate Task<Uri> MethodAddressTransformer(IServiceProvider serviceProvider, Uri originalUri, MethodInfo methodInfo);
}
