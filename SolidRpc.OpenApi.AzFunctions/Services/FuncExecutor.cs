using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolidRpc.OpenApi.AzFunctions.Services
{
    /// <summary>
    /// Executes a function
    /// </summary>
    public class FuncExecutor
    {
        /// <summary>
        /// Executes a function
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serviceProvider"></param>
        /// <param name="logger"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static async Task ExecuteFunction(IServiceProvider serviceProvider, ILogger logger, Func<Task> action)
        {
            await ExecuteFunction<object>(serviceProvider, logger, () => { action(); return null; }, () => { return null; });
        }

        /// <summary>
        /// Executes a function
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serviceProvider"></param>
        /// <param name="logger"></param>
        /// <param name="action"></param>
        /// <param name="errorAction"></param>
        /// <returns></returns>
        public static async Task<T> ExecuteFunction<T>(IServiceProvider serviceProvider, ILogger logger, Func<Task<T>> action, Func<Task<T>> errorAction)
        {
            try
            {
                var middlewares = serviceProvider.GetRequiredService<IEnumerable<IFuncMiddleware>>();
                return await ExecuteMiddlewareChain<T>(middlewares, action);
            }
            catch (Exception e)
            {
                logger.LogError(e, "Failed to invoke request");
                return await errorAction();
            }
        }

        private static async Task<T> ExecuteMiddlewareChain<T>(IEnumerable<IFuncMiddleware> middlewares, Func<Task<T>> action)
        {
            if(middlewares.Any())
            {
                var middleware = middlewares.FirstOrDefault();
                T res = default;
                await middleware.HandleRequestAsync(async () =>
                {
                    res = await ExecuteMiddlewareChain(middlewares.Skip(1), action);
                });
                return res;
            }
            else
            {
                return await action();
            }
        }
    }
}
