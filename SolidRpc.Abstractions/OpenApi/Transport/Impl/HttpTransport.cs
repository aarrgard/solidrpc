using SolidRpc.Abstractions.OpenApi.Binder;
using SolidRpc.Abstractions.OpenApi.Http;
using System;
using System.Reflection;
using System.Threading.Tasks;

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
        /// <param name="preInvokeCallback"></param>
        /// <param name="postInvokeCallback"></param>
        public HttpTransport(
            InvocationStrategy invocationStrategy, 
            MethodAddressTransformer methodAddressTransformer,
            Func<IHttpRequest, Task> preInvokeCallback,
            Func<IHttpResponse, Task> postInvokeCallback)
            : base("Http", invocationStrategy, preInvokeCallback, postInvokeCallback)
        {
            MethodAddressTransformer = methodAddressTransformer;
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
                throw new Exception($"Operation address({operationAddress}) has already been configured.");
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
                    address = methodAddressResolver.TransformUri(address, methodInfo);
                }
            }
            return address;
        }

        /// <summary>
        /// The address transformer
        /// </summary>
        public MethodAddressTransformer MethodAddressTransformer { get; }

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
            return new HttpTransport(InvocationStrategy, methodAddressTransformer, PreInvokeCallback, PostInvokeCallback);
        }

        /// <summary>
        /// Sets the invocation strategy
        /// </summary>
        /// <param name="invocationStrategy"></param>
        /// <returns></returns>
        public IHttpTransport SetInvocationStrategy(InvocationStrategy invocationStrategy)
        {
            return new HttpTransport(invocationStrategy, MethodAddressTransformer, PreInvokeCallback, PostInvokeCallback);
        }

        /// <summary>
        /// Adds a pre invokation callback
        /// </summary>
        /// <param name="callback"></param>
        /// <returns></returns>
        public IHttpTransport AddPreInvokeCallback(Func<IHttpRequest, Task> callback)
        {
            var oldCallback = PreInvokeCallback;
            return new HttpTransport(InvocationStrategy, MethodAddressTransformer, async (req) =>
            {
                await callback(req);
                await oldCallback(req);
            }, PostInvokeCallback);
        }

        /// <summary>
        /// Adds a pre invokation callback
        /// </summary>
        /// <param name="callback"></param>
        /// <returns></returns>
        public IHttpTransport AddPostInvokeCallback(Func<IHttpResponse, Task> callback)
        {
            var oldCallback = PostInvokeCallback;
            return new HttpTransport(InvocationStrategy, MethodAddressTransformer, PreInvokeCallback, async (resp) =>
            {
                await callback(resp);
                await oldCallback(resp);
            });
        }
    }
}
