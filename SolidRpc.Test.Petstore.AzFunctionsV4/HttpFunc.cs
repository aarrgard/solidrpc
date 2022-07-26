﻿using SolidRpc.Abstractions.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SolidRpc.Test.Petstore.AzFunctionsV4
{
    /// <summary>
    /// Exposes some of the http functions
    /// </summary>
    public interface IHttpFunc
    {
        /// <summary>
        /// Returns the data from an https call
        /// </summary>
        /// <param name="ops"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<FileContent> Https(
            string ops,
            CancellationToken cancellationToken = default);
    }
    public class HttpFuncImpl : IHttpFunc   
    {
        public HttpFuncImpl()
        {
        }

        public async Task<FileContent> Https(string ops, CancellationToken cancellationToken = default)
        {
            return new FileContent();
        }
    }
}