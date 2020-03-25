using System;
using System.Collections.Generic;
using System.Text;

namespace SolidRpc.Abstractions.OpenApi.Transport.Impl
{
    /// <summary>
    /// Contains the settings for the http transport.
    /// </summary>
    public class HttpTransport : Transport, IHttpTransport
    {
        public HttpTransport(MethodAddressTransformer methodAddressTransformer, MethodHeadersTransformer methodHeadersTransformer)
        {
            MethodAddressTransformer = methodAddressTransformer;
            MethodHeadersTransformer = methodHeadersTransformer;
        }
        public MethodAddressTransformer MethodAddressTransformer { get; }

        public IHttpTransport SetMethodAddressTransformer(MethodAddressTransformer methodAddressTransformer)
        {
            return new HttpTransport(methodAddressTransformer, MethodHeadersTransformer);
        }

        public MethodHeadersTransformer MethodHeadersTransformer { get; }

        public IHttpTransport SetMethodHeadersTransformer(MethodHeadersTransformer methodHeadersTransformer)
        {
            return new HttpTransport(MethodAddressTransformer, methodHeadersTransformer);
        }
    }
}
