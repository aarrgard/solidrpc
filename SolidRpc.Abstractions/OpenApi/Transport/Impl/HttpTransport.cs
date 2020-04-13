using SolidRpc.Abstractions.OpenApi.Binder;
using SolidRpc.Abstractions.OpenApi.Model;
using System;
using System.Reflection;

namespace SolidRpc.Abstractions.OpenApi.Transport.Impl
{
    /// <summary>
    /// Contains the settings for the http transport.
    /// </summary>
    public class HttpTransport : Transport, IHttpTransport
    {
        private Uri _operationAddress;

        public HttpTransport(MethodAddressTransformer methodAddressTransformer, MethodHeadersTransformer methodHeadersTransformer)
            : base("Http")
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

        public override void Configure(IMethodBinding methodBinding)
        {
            base.Configure(methodBinding);
            Path = methodBinding.RelativePath;

            if(methodBinding.IsLocal)
            {
                BaseAddress = methodBinding.MethodBinder.HostedAddress;
            }
            else
            {
                BaseAddress = TransformAddress(methodBinding, methodBinding.MethodBinder.OpenApiSpec.BaseAddress, null);
            }
            var operationAddress = new Uri(BaseAddress, Path);
            operationAddress = TransformAddress(methodBinding, operationAddress, methodBinding.MethodInfo);
            _operationAddress = operationAddress;
        }

        private Uri TransformAddress(IMethodBinding methodBinding, Uri address, MethodInfo methodInfo)
        {
            var serviceProvider = methodBinding.MethodBinder.ServiceProvider;
            if (MethodAddressTransformer != null)
            {
                address = MethodAddressTransformer(serviceProvider, address, methodInfo);
            }
            else if (methodBinding.IsLocal)
            {
                var methodAddressResolver = (IMethodAddressTransformer)serviceProvider.GetService(typeof(IMethodAddressTransformer));
                if (methodAddressResolver != null)
                {
                    address = methodAddressResolver.TransformUriAsync(address, methodInfo);
                }
            }
            return address;
        }

        public override Uri OperationAddress => _operationAddress;

        public Uri BaseAddress { get; private set; }

        public string Path { get; private set; }

        public IHttpTransport SetMethodHeadersTransformer(MethodHeadersTransformer methodHeadersTransformer)
        {
            return new HttpTransport(MethodAddressTransformer, methodHeadersTransformer);
        }
    }
}
