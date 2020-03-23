using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace SolidRpc.Abstractions.OpenApi.Invoker
{
    /// <summary>
    /// Uses the http stack to invoke the call
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IHttpInvoker<T> : IInvoker<T> where T:class
    {
    }
}
