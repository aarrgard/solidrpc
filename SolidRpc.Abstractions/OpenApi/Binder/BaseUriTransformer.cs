using System;

namespace SolidRpc.Abstractions.OpenApi.Binder
{
    /// <summary>
    /// The method used to transform uri:s
    /// </summary>
    /// <param name="serviceProvider"></param>
    /// <param name="baseUri"></param>
    /// <returns></returns>
    public delegate Uri BaseUriTransformer(IServiceProvider serviceProvider, Uri baseUri);
}
