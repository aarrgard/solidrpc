using System;
using System.Collections.Generic;
using System.Text;

namespace SolidRpc.Abstractions.OpenApi.Model
{
    /// <summary>
    /// Interface used to handle open api specs
    /// </summary>
    public interface IOpenApiParser
    {
        /// <summary>
        /// Parses upplied json to an open api spec.
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        IOpenApiSpec ParseSpec(string json);
    }
}
