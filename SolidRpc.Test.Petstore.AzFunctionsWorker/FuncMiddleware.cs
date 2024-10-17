using Grpc.Core;
using Microsoft.AspNetCore.Http;
using SolidRpc.OpenApi.AzFunctions.Services;

namespace SolidRpc.Test.Petstore.AzFunctionsWorker
{
    public class FuncMiddleware : IFuncMiddleware<HttpRequest>
    {
        public IServiceProvider ServiceProvider { get; }

        public FuncMiddleware(
            IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
        }

        public Task HandleRequestAsync(HttpRequest req, Func<Task> next)
        {
            return next();
        }
    }
}
