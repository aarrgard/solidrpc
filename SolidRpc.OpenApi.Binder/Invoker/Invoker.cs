using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SolidRpc.Abstractions;
using SolidRpc.Abstractions.InternalServices;
using SolidRpc.Abstractions.OpenApi.Binder;
using SolidRpc.Abstractions.OpenApi.Http;
using SolidRpc.Abstractions.OpenApi.Invoker;
using SolidRpc.Abstractions.OpenApi.Proxy;
using SolidRpc.Abstractions.OpenApi.Transport;
using SolidRpc.OpenApi.Binder.Http;
using SolidRpc.OpenApi.Binder.Invoker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

[assembly: SolidRpcService(typeof(IInvoker<>), typeof(Invoker<>), SolidRpcServiceLifetime.Transient)]
namespace SolidRpc.OpenApi.Binder.Invoker
{
    public class Invoker<TObj> : IInvoker<TObj> where TObj:class
    {
        private class DummyMethodBinding : IMethodBinding
        {
            public DummyMethodBinding(MethodInfo mi)
            {
                MethodInfo = mi;
            }

            public IMethodBinder MethodBinder => throw new NotImplementedException();

            public MethodInfo MethodInfo { get; }

            public bool IsLocal => throw new NotImplementedException();

            public bool IsEnabled => throw new NotImplementedException();

            public IEnumerable<ITransport> Transports => new ITransport[0];

            public string OperationId => throw new NotImplementedException();

            public string Method => throw new NotImplementedException();

            public string LocalPath => throw new NotImplementedException();

            public string RelativePath => throw new NotImplementedException();

            public IEnumerable<IMethodArgument> Arguments => throw new NotImplementedException();

            public Task BindArgumentsAsync(IHttpRequest request, object[] args, Uri addressOverride = null)
            {
                throw new NotImplementedException();
            }

            public Task BindResponseAsync(IHttpResponse response, object obj, Type objType)
            {
                throw new NotImplementedException();
            }

            public Uri BindUri(IHttpRequest request, Uri addressOverride = null)
            {
                throw new NotImplementedException();
            }

            public Task<object[]> ExtractArgumentsAsync(IHttpRequest request)
            {
                throw new NotImplementedException();
            }

            public T ExtractResponse<T>(IHttpResponse response)
            {
                throw new NotImplementedException();
            }

            public object ExtractResponse(Type responseType, IHttpResponse response)
            {
                throw new NotImplementedException();
            }

            T IMethodBinding.GetSolidProxyConfig<T>()
            {
                throw new NotImplementedException();
            }
        }

        public Invoker(
            ILogger<Invoker<TObj>> logger, 
            Invokers invokers,
            IServiceProvider serviceProvider)
        {
            Invokers = invokers;
            ServiceProvider = serviceProvider;
        }
        private Invokers Invokers { get; }
        private IServiceProvider ServiceProvider { get; }

        public IMethodBinding GetMethodBinding(MethodInfo mi)
        {
            return Invokers.MethodBinderStore.GetMethodBinding(mi);
        }

        public IMethodBinding GetMethodBinding(Expression<Action<TObj>> action)
        {
            var (mi, _) = GetMethodInfo(action);
            return GetMethodBinding(mi);
        }

        public IMethodBinding GetMethodBinding<TResult>(Expression<Func<TObj, TResult>> func)
        {
            var (mi, _) = GetMethodInfo(func);
            return GetMethodBinding(mi);
        }

        public Task<Uri> GetUriAsync(Expression<Action<TObj>> action, bool includeQueryString = true)
        {
            var (mi, args) = GetMethodInfo(action);
            return GetUriAsync(mi, args, includeQueryString);
        }

        public Task<Uri> GetUriAsync<TRes>(Expression<Func<TObj, TRes>> func, bool includeQueryString = true)
        {
            var (mi, args) = GetMethodInfo(func);
            return GetUriAsync(mi, args, includeQueryString);
        }

        private async Task<Uri> GetUriAsync(MethodInfo mi, object[] args, bool includeQueryString)
        {
            var methodBinding = GetMethodBinding(mi);
            if (methodBinding == null)
            {
                throw new Exception($"Cannot find openapi method binding for method {mi.DeclaringType.FullName}.{mi.Name}");
            }
            var operationAddress = methodBinding.Transports
                .OfType<IHttpTransport>()
                .Select(o => o.OperationAddress)
                .FirstOrDefault();

            var req = new SolidHttpRequest();
            await methodBinding.BindArgumentsAsync(req, args, operationAddress);

            //
            // if we have an authorized user and the url requires authentication - add it.
            //
            var currentPrincipal = ServiceProvider.GetRequiredService<ISolidRpcAuthorization>().CurrentPrincipal;
            if((currentPrincipal?.Identity?.IsAuthenticated) ?? false)
            {
                var accessToken = currentPrincipal.Claims.Where(o => o.Type == "accesstoken").Select(o => o.Value).FirstOrDefault();
                if(accessToken != null)
                {
                    // add access_token if resource is protected
                    var oauthConf = methodBinding.GetSolidProxyConfig<ISecurityOAuth2Config>();
                    if (oauthConf.RedirectUnauthorizedIdentity)
                    {
                        req.Query = req.Query.Union(new[] { new SolidHttpRequestDataString("text/plain", "access_token", accessToken) }).ToList();
                    }
                }
            }

            return req.CreateUri(includeQueryString);
        }

        public Task InvokeAsync(Expression<Action<TObj>> action, Func<InvocationOptions, InvocationOptions> invocationOptions)
        {
            var (mi, args) = GetMethodInfo(action);
            return InvokeAsync(mi, args, invocationOptions);
        }

        public TResult InvokeAsync<TResult>(Expression<Func<TObj, TResult>> func, Func<InvocationOptions, InvocationOptions> invocationOptions)
        {
            var (mi, args) = GetMethodInfo(func);
            var mb = Invokers.MethodBinderStore.GetMethodBinding(mi) ?? new DummyMethodBinding(mi);
            var res = Invokers.GetCachedInvoker<TResult>()(ServiceProvider, mb, args, invocationOptions);
            return (TResult)res;
        }

        protected static (MethodInfo, object[]) GetMethodInfo(LambdaExpression expr)
        {
            if (expr.Body is MethodCallExpression mce)
            {
                var args = new List<object>();
                foreach (var argument in mce.Arguments)
                {
                    var le = Expression.Lambda(argument);
                    args.Add(le.Compile().DynamicInvoke());
                }

                return (mce.Method, args.ToArray());
            }
            throw new Exception("expression should be a method call.");
        }

        public Task<object> InvokeAsync(MethodInfo mi, IEnumerable<object> args, Func<InvocationOptions, InvocationOptions> invocationOptions)
        {
            var mb = GetMethodBinding(mi);
            return Invokers.InvokeMethodAsync(ServiceProvider, mb, args.ToArray(), invocationOptions);
        }
    }
}
