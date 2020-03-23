using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace SolidRpc.Abstractions.OpenApi.Invoker
{
    /// <summary>
    /// Uses the configured queues to invoke a call
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ILocalInvoker<T> : IInvoker<T> where T:class
    {
    }
}
