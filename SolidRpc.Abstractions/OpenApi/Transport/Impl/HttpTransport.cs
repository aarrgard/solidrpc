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

        /// <summary>
        /// Constructs a new instance
        /// </summary>
        /// <param name="invocationStrategy"></param>
        /// <param name="methodAddressTransformer"></param>
        /// <param name="methodHeadersTransformer"></param>
        public HttpTransport(InvocationStrategy invocationStrategy, MethodAddressTransformer methodAddressTransformer, MethodHeadersTransformer methodHeadersTransformer)
            : base("Http", invocationStrategy)
        {
            MethodAddressTransformer = methodAddressTransformer;
            MethodHeadersTransformer = methodHeadersTransformer;
        }
    
        /// <summary>
        /// Configures the transport
        /// </summary>
        /// <param name="methodBinding"></param>
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
            if (_operationAddress != null && _operationAddress != operationAddress)
            {
                throw new Exception("Operation address has already been configured.");
            }
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

        /// <summary>
        /// The address transformer
        /// </summary>
        public MethodAddressTransformer MethodAddressTransformer { get; }

        /// <summary>
        /// The header transformer
        /// </summary>
        public MethodHeadersTransformer MethodHeadersTransformer { get; }

        /// <summary>
        /// Returns the operation address
        /// </summary>
        public override Uri OperationAddress => _operationAddress;

        /// <summary>
        /// Returns the base address
        /// </summary>
        public Uri BaseAddress { get; private set; }

        /// <summary>
        /// Returns the path.
        /// </summary>
        public string Path { get; private set; }

        /// <summary>
        /// Sets the address transformer
        /// </summary>
        /// <param name="methodAddressTransformer"></param>
        /// <returns></returns>
        public IHttpTransport SetMethodAddressTransformer(MethodAddressTransformer methodAddressTransformer)
        {
            return new HttpTransport(InvocationStrategy, methodAddressTransformer, MethodHeadersTransformer);
        }

        /// <summary>
        /// Sets the method headers transformer
        /// </summary>
        /// <param name="methodHeadersTransformer"></param>
        /// <returns></returns>
        public IHttpTransport SetMethodHeadersTransformer(MethodHeadersTransformer methodHeadersTransformer)
        {
            return new HttpTransport(InvocationStrategy, MethodAddressTransformer, methodHeadersTransformer);
        }

        /// <summary>
        /// Sets the invocation strategy
        /// </summary>
        /// <param name="invocationStrategy"></param>
        /// <returns></returns>
        public IHttpTransport SetInvocationStrategy(InvocationStrategy invocationStrategy)
        {
            return new HttpTransport(invocationStrategy, MethodAddressTransformer, MethodHeadersTransformer);
        }
    }
}
