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
        public static async Task ExecuteFunction<TInput>(IServiceProvider serviceProvider, ILogger logger, TInput input, Func<Task> action)
        {
            await ExecuteFunction<TInput, object>(serviceProvider, logger, input, () => { action(); return null; }, () => { return null; });
        }

        /// <summary>
        /// Executes a function
        /// </summary>
        /// <typeparam name="TInput"></typeparam>
        /// <typeparam name="TRes"></typeparam>
        /// <param name="serviceProvider"></param>
        /// <param name="logger"></param>
        /// <param name="action"></param>
        /// <param name="errorAction"></param>
        /// <returns></returns>
        public static async Task<TRes> ExecuteFunction<TInput, TRes>(IServiceProvider serviceProvider, ILogger logger, TInput input, Func<Task<TRes>> action, Func<Task<TRes>> errorAction)
        {
            try
            {
                var middlewares = serviceProvider.GetRequiredService<IEnumerable<IFuncMiddleware<TInput>>>();
                return await ExecuteMiddlewareChain(middlewares, input, action);
            }
            catch (Exception e)
            {
                logger.LogError(e, "Failed to invoke request");
                return await errorAction();
            }
        }

        private static async Task<TRes> ExecuteMiddlewareChain<TInput, TRes>(IEnumerable<IFuncMiddleware<TInput>> middlewares, TInput input, Func<Task<TRes>> action)
        {
            if(middlewares.Any())
            {
                var middleware = middlewares.FirstOrDefault();
                TRes res = default;
                await middleware.HandleRequestAsync(input, async () =>
                {
                    res = await ExecuteMiddlewareChain(middlewares.Skip(1), input, action);
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
