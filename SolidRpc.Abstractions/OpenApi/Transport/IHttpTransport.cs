using SolidRpc.Abstractions.OpenApi.Http;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SolidRpc.Abstractions.OpenApi.Transport
{
    /// <summary>
    /// Represents the settings for the http transport
    /// </summary>
    public interface IHttpTransport : ITransport
    {
        /// <summary>
        /// The base address of the method.
        /// </summary>
        Uri BaseAddress { get; set; }

        /// <summary>
        /// The path relative to the base method.
        /// </summary>
        string Path { get; set; }

        /// <summary>
        /// The path relative to the base method.
        /// </summary>
        IDictionary<int, Func<TimeSpan>> RetryPolicy { get; set; }

        /// <summary>
        /// The method to transform the Uri. This delegate is invoked to determine
        /// the base Uri for the service. Supplied uri is the one obtained from
        /// the openapi config.
        /// </summary>
        MethodAddressTransformer MethodAddressTransformer { get; set; }
    }

    /// <summary>
    /// Extension methods for the transport
    /// </summary>
    public static class IHttpTransportExtensions
    {
        /// <summary>
        /// Sets the method address transformer on the transports
        /// </summary>
        /// <param name="t"></param>
        /// <param name="methodAddressTransformer"></param>
        public static IHttpTransport SetMethodAddressTransformer(this IHttpTransport t, MethodAddressTransformer methodAddressTransformer)
        {
            t.MethodAddressTransformer = methodAddressTransformer ?? t.MethodAddressTransformer;
            return t;
        }

        /// <summary>
        /// Sets the method address transformer on the transports
        /// </summary>
        /// <param name="t"></param>
        /// <param name="statusCode"></param>
        /// <param name="retryTimeout"></param>
        public static IHttpTransport AddRetryPolicy(this IHttpTransport t, int statusCode, Func<TimeSpan> retryTimeout)
        {
            var retryPolicy = t.RetryPolicy;
            if (retryPolicy == null)
            {
                t.RetryPolicy = retryPolicy = new Dictionary<int, Func<TimeSpan>>();
            }
            retryPolicy[statusCode] = retryTimeout;
            return t;
        }
    }
}