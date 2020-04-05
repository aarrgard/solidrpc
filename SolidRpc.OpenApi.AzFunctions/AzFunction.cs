using System;
using System.Collections.Generic;
using System.Text;

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
        public static Func<Func<object>,object> Interceptor = (_) => _();

        /// <summary>
        /// Performs the invocation.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="del"></param>
        /// <returns></returns>
        public static T DoRun<T>(Func<T> del)
        {
            return (T)Interceptor(() => del());
        }
    }
}
