using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SolidRpc.Abstractions.Types
{
    /// <summary>
    /// Represents a file content
    /// </summary>
    public class HttpRequestTemplate : TypeTemplate<HttpRequest>
    {
        static HttpRequestTemplate()
        {
            PropertyTypes = typeof(HttpRequest).GetProperties().ToDictionary(o => o.Name.ToLower(), o => o.PropertyType);
            RequiredProps = new[] { nameof(HttpRequest.Uri).ToLower() };
        }

        /// <summary>
        /// The properties 
        /// </summary>
        public static IDictionary<string, Type> PropertyTypes { get; }

        /// <summary>
        /// The required properties
        /// </summary>
        public static IEnumerable<string> RequiredProps { get; }

        /// <summary>
        /// Returns the template spec for supplied type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static HttpRequestTemplate GetTemplate(Type type)
        {
            return (HttpRequestTemplate)GetTemplate(type,  _ => new HttpRequestTemplate(_, RequiredProps));
        }
        /// <summary>
        /// Returns true if specified information matches a http request type.
        /// </summary>
        /// <param name="typeName"></param>
        /// <param name="propertyTypes"></param>
        /// <returns></returns>
        public static bool IsHttpRequestType(string typeName, Dictionary<string, Type> propertyTypes)
        {
            if (!typeName.Contains("HttpRequest"))
            {
                return false;
            }
            if (!RequiredProps.All(o => propertyTypes.Keys.Any(o2 => o == o2.ToLower())))
            {
                return false;
            }

            var res = propertyTypes
                .All(o => {
                    if (PropertyTypes.TryGetValue(o.Key.ToLower(), out Type t))
                    {
                        return t == o.Value;
                    }
                    return false;
                });

            return res;
        }

        /// <summary>
        /// Ensures that out type has correct name.
        /// </summary>
        /// <param name="templateType"></param>
        /// <param name="otherType"></param>
        /// <param name="isTemplateType"></param>
        /// <returns></returns>
        protected override bool HandleTemplateType(Type templateType, Type otherType, bool isTemplateType)
        {
            return isTemplateType && otherType.Name.Contains("HttpRequest");
        }

        /// <summary>
        /// Constructs an instance
        /// </summary>
        /// <param name="otherType"></param>
        /// <param name="requiredProps"></param>
        public HttpRequestTemplate(Type otherType, IEnumerable<string> requiredProps)
            : base(otherType, requiredProps)
        {
        }


        /// <summary>
        /// Sets the method on the template copy
        /// </summary>
        public Action<object, string> SetMethod { get; private set; }
        /// <summary>
        /// Gets the conmethodtent from the template copy
        /// </summary>
        public Func<object, string> GetMethod { get; private set; }

        /// <summary>
        /// Sets the uri on the template copy
        /// </summary>
        public Action<object, Uri> SetUri { get; private set; }
        /// <summary>
        /// Gets the uri from the template copy
        /// </summary>
        public Func<object, Uri> GetUri { get; private set; }

        /// <summary>
        /// Sets the headers on the template copy
        /// </summary>
        public Action<object, IDictionary<string, StringValues>> SetHeaders { get; private set; }
        /// <summary>
        /// Gets the headers from the template copy
        /// </summary>
        public Func<object, IDictionary<string, StringValues>> GetHeaders { get; private set; }

        /// <summary>
        /// Sets the query on the template copy
        /// </summary>
        public Action<object, IDictionary<string, StringValues>> SetQuery { get; private set; }
        /// <summary>
        /// Gets the query from the template copy
        /// </summary>
        public Func<object, IDictionary<string, StringValues>> GetQuery { get; private set; }

        /// <summary>
        /// Sets the query on the template copy
        /// </summary>
        public Action<object, Stream> SetBody { get; private set; }
        /// <summary>
        /// Gets the query from the template copy
        /// </summary>
        public Func<object, Stream> GetBody { get; private set; }

    }
}
