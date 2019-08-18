using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace SolidRpc.OpenApi.Model
{
    /// <summary>
    /// Implements logic to handle the serialization/deseralization using newtonsoft.
    /// </summary>
    public class NewtonsoftContractResolver : DefaultContractResolver
    {
        private static ConcurrentDictionary<Type, JsonContract> s_converters = new ConcurrentDictionary<Type, JsonContract>();

        /// <summary>
        /// The default contract resolver
        /// </summary>
        public static NewtonsoftContractResolver Instance = new NewtonsoftContractResolver();

        /// <summary>
        /// Constructs a new instance
        /// </summary>
        public NewtonsoftContractResolver()
        {

        }

        /// <summary>
        /// Cretes a contract for supplied type
        /// </summary>
        /// <param name="objectType"></param>
        /// <returns></returns>
        protected override JsonContract CreateContract(Type objectType)
        {
            return s_converters.GetOrAdd(objectType, CreateContractInternal);
        }

        private JsonContract CreateContractInternal(Type type)
        {
            if (type.Assembly == typeof(string).Assembly)
            {
                return base.CreateContract(type);
            }
            if (type.Assembly == typeof(Uri).Assembly)
            {
                return base.CreateContract(type);
            }
            if (type.Assembly == typeof(IEnumerable<>).Assembly)
            {
                return base.CreateContract(type);
            }
            if(type.IsArray)
            {
                return base.CreateContract(type);
            }
            var converterType = typeof(NewtonsoftConverter<>).MakeGenericType(type);
            var contract = new JsonObjectContract(type);
            contract.Converter = (JsonConverter) Activator.CreateInstance(converterType);
            return contract;
        }
    }
}
