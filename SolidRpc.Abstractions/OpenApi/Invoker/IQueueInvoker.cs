using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace SolidRpc.Abstractions.OpenApi.Invoker
{
    /// <summary>
    /// Represents a queue invoker
    /// </summary>
    public interface IQueueInvoker : IInvoker
    {

    }

    /// <summary>
    /// Uses the configured queues to invoke a call
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IQueueInvoker<T> : IInvoker<T>, IQueueInvoker where T:class
    {
    }
}
