using SolidRpc.Abstractions.OpenApi.Http;
using System;
using System.Threading.Tasks;

namespace SolidRpc.Abstractions.OpenApi.Transport
{
    /// <summary>
    /// Base definitions for a transport
    /// </summary>
    public interface ITransport
    {
        /// <summary>
        /// The operation address
        /// </summary>
        Uri OperationAddress { get; set; }

        /// <summary>
        /// The invocation strategy to use when handling calls
        /// on this transport.
        /// </summary>
        InvocationStrategy InvocationStrategy { get; set; }

        /// <summary>
        /// The message priority
        /// </summary>
        int MessagePriority { get; set; }

        /// <summary>
        /// The pre invoke callback
        /// </summary>
        Func<IHttpRequest, Task> PreInvokeCallback { get; set; }

        /// <summary>
        /// The post invoke callback
        /// </summary>
        Func<IHttpResponse, Task> PostInvokeCallback { get; set; }
    }

    /// <summary>
    /// Extension methods for the transport
    /// </summary>
    public static class ITransportExtensions
    {
        /// <summary>
        /// Returns the transport type for supplied type
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string GetTransportType(Type type)
        {
            if (type == null) return null;
            return type.Name.Substring(1, type.Name.Length - "Transport".Length - 1);
        }

        /// <summary>
        /// Returns the transport type
        /// </summary>
        /// <returns></returns>
        public static Type GetTransportInterface(this ITransport transport)
        {
            if (transport == null) return null;
            Type t = typeof(ITransport);
            foreach (var i in transport.GetType().GetInterfaces())
            {
                if (t.IsAssignableFrom(i))
                {
                    t = i;
                }
            }
            return t;
        }


        /// <summary>
        /// Returns the transport type
        /// </summary>
        /// <returns></returns>
        public static string GetTransportType(this ITransport transport)
        {
            return GetTransportType(GetTransportInterface(transport));
        }

    }
}
