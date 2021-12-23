using System;
using System.Collections.Generic;
using System.Linq;

namespace SolidRpc.Abstractions.OpenApi.Http
{
    /// <summary>
    /// Contains extension methods for the enums
    /// </summary>
    public static class IHttpRequestDataExtensions
    {
 
        /// <summary>
        /// Returns true if supplied type is a nullable type.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="nullableType"></param>
        /// <returns></returns>
        public static string GetStringValue(this IEnumerable<IHttpRequestData> data, string name)
        {
            if (data == null) return null;
            return data.Where(o => string.Equals(o.Name, name, StringComparison.InvariantCultureIgnoreCase))
                .Select(o => o.GetStringValue())
                .FirstOrDefault();
        }
    }
}
