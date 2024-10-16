﻿using System;
using System.IO;
using System.Text;

namespace SolidRpc.Abstractions.OpenApi.Http
{
    /// <summary>
    /// Represents some request data.
    /// </summary>
    public interface IHttpRequestData
    {
        /// <summary>
        /// The content type - stored in the Content-Type header.
        /// </summary>
        string ContentType { get; }

        /// <summary>
        /// The encoding - stored in the Content-Type header
        /// </summary>
        Encoding Encoding { get; }

        /// <summary>
        /// The etag - stored in the ETag header
        /// </summary>
        string ETag { get; }

        /// <summary>
        /// The last modified date
        /// </summary>
        DateTimeOffset? LastModified { get; }

        /// <summary>
        /// The content of the set cookie header
        /// </summary>
        string SetCookie { get; }

        /// <summary>
        /// File filename - usually stored in the Content-Disposition header.
        /// </summary>
        string Filename { get; }

        /// <summary>
        /// The name of the data
        /// </summary>
        string Name { get; }

        /// <summary>
        /// The string representation
        /// </summary>
        /// <returns></returns>
        string GetStringValue();

        /// <summary>
        /// Returns the binary value.
        /// </summary>
        /// <returns></returns>
        Stream GetBinaryValue();

        /// <summary>
        /// Returns the data as supplied type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T As<T>();
    }
}