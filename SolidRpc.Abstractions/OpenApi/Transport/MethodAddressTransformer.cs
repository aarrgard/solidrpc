using System;
using System.Reflection;

namespace SolidRpc.Abstractions.OpenApi.Transport
{
    /// <summary>
    /// The method used to transform uri:s
    /// </summary>
    /// <param name="serviceProvider"></param>
    /// <param name="originalUri"></param>
    /// <param name="methodInfo">The method to resolve the uri for(may be null)</param>
    /// <returns></returns>
    public delegate Uri MethodAddressTransformer(IServiceProvider serviceProvider, Uri originalUri, MethodInfo methodInfo);
}
