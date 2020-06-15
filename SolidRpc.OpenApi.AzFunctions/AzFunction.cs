using System;
using System.Threading.Tasks;

namespace SolidRpc.OpenApi.AzFunctions
{
    /// <summary>
    /// Base class for all the implemented method handlers
    /// </summary>
    public static class AzFunction 
    {
        /// <summary>
        /// Assign this interceptor to intercept all the invokations.
        /// </summary>
        public static Func<Func<Task<object>>, Task<object>> Interceptor = (_) => _();

        /// <summary>
        /// Performs the invocation.
        /// </summary>
        /// <param name="del"></param>
        /// <returns></returns>
        public static async Task DoRun(Func<Task> del)
        {
            await Interceptor(async () =>
            {
                await del();
                return null;
            });
        }

        /// <summary>
        /// Performs the invocation.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="del"></param>
        /// <returns></returns>
        public static async Task<T> DoRun<T>(Func<Task<T>> del)
        {
            var res = await Interceptor(async () => await del());
            return (T)res;
        }
    }
}
