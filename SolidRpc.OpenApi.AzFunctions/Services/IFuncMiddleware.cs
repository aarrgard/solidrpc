using System;
using System.Threading.Tasks;

namespace SolidRpc.OpenApi.AzFunctions.Services
{
    /// <summary>
    /// The function middleware
    /// </summary>
    public interface IFuncMiddleware
    {
        /// <summary>
        /// Handles the request
        /// </summary>
        /// <param name="next"></param>
        /// <returns></returns>
        Task HandleRequestAsync(Func<Task> next);
    }
}
