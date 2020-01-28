using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace SolidRpc.Abstractions.OpenApi.Binder
{
    /// <summary>
    /// The method used to transform headers
    /// </summary>
    /// <param name="serviceProvider"></param>
    /// <param name="headers"></param>
    /// <param name="methodInfo">The method to transform the headers for</param>
    /// <returns></returns>
    public delegate Task MethodHeadersTransformer(IServiceProvider serviceProvider, IDictionary<string, IEnumerable<string>> headers, MethodInfo methodInfo);
}
