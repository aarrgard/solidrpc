using SolidRpc.Abstractions.OpenApi.Http;
using System;
using System.Threading.Tasks;

namespace SolidRpc.Abstractions.OpenApi.Transport
{
    /// <summary>
    /// Base definition for a transport
    /// </summary>
    public interface ITransport
    {
        /// <summary>
        /// The transport that we use when the invoker receives
        /// a call on this transport.
        /// </summary>
        string InvokerTransport { get; set; }

        /// <summary>
        /// The operation address
        /// </summary>
        Uri OperationAddress { get; set; }

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

        /// <summary>
        /// Returns the ordinal for supplied transport
        /// </summary>
        /// <param name="transport"></param>
        /// <returns></returns>
        public static int GetInvocationOrdinal(this ITransport transport)
        {
            if(!string.IsNullOrEmpty(transport.InvokerTransport))
            {
                return 1000;
            }
            if (typeof(ILocalTransport).IsAssignableFrom(transport.GetType()))
            {
                return 0;
            }
            if (typeof(IHttpTransport).IsAssignableFrom(transport.GetType()))
            {
                return 1;
            }
            return 2;
        }

        /// <summary>
        /// Sets the pre invoke callback on the http transport
        /// </summary>
        /// <param name="t"></param>
        /// <param name="priority"></param>
        public static TTransport SetMessagePriority<TTransport>(this TTransport t, int priority) where TTransport : ITransport
        {
            t.MessagePriority = priority;
            return t;
        }

        /// <summary>
        /// Sets the pre invoke callback on the http transport
        /// </summary>
        /// <param name="t"></param>
        /// <param name="preInvokeCallback"></param>
        public static TTransport AddHttpTransportPreInvokeCallback<TTransport>(this TTransport t, Func<IHttpRequest, Task> preInvokeCallback) where TTransport : ITransport
        {
            t.PreInvokeCallback = preInvokeCallback ?? t.PreInvokeCallback;
            return t;
        }

        /// <summary>
        /// Sets the pre invoke callback on the http transport
        /// </summary>
        /// <param name="t"></param>
        /// <param name="postInvokeCallback"></param>
        public static TTransport AddHttpTransportPostInvokeCallback<TTransport>(this TTransport t, Func<IHttpResponse, Task> postInvokeCallback) where TTransport:ITransport
        {
            t.PostInvokeCallback = postInvokeCallback ?? t.PostInvokeCallback;
            return t;
        }
    }
}
