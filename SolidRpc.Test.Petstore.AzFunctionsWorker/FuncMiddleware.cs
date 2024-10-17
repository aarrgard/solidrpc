using SolidRpc.OpenApi.AzFunctions.Services;

namespace SolidRpc.Test.Petstore.AzFunctionsWorker
{
    public class FuncMiddleware : IFuncMiddleware
    {
        public FuncMiddleware() { }

        public Task HandleRequestAsync(Func<Task> next)
        {
            return next();
        }
    }
}
