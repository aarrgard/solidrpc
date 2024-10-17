using System;
using System.Threading.Tasks;

namespace SolidRpc.OpenApi.AzFunctions.Services
{
    /// <summary>
    /// The function middleware
    /// </summary>
    public interface IFuncMiddleware<TInput>
    {
        /// <summary>
        /// Handles the request
        /// </summary>
        /// <param name="trigger"></param>
        /// <param name="next"></param>
        /// <returns></returns>
        Task HandleRequestAsync(TInput trigger, Func<Task> next);
    }
}
