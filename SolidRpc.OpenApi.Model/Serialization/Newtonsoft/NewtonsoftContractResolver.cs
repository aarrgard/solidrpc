using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;

namespace SolidRpc.OpenApi.Model.Serialization.Newtonsoft
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
            if(type.IsNullableType(out Type nullableType))
            {
                var valueContract = CreateContractInternal(nullableType);
                var valueConverter = valueContract.Converter ?? valueContract.InternalConverter;
                if(valueConverter != null)
                {
                    var contract = new JsonObjectContract(type);
                    contract.Converter = (JsonConverter)Activator.CreateInstance(typeof(NullableConverter<>).MakeGenericType(nullableType), valueConverter);
                    return contract;
                }
            }
            if (typeof(DateTimeOffset).IsAssignableFrom(type))
            {
                var contract = new JsonObjectContract(type);
                contract.Converter = new DateTimeOffsetConverter();
                return contract;
            }
            if (typeof(Stream).IsAssignableFrom(type))
            {
                var contract = new JsonObjectContract(type);
                contract.Converter = new StreamConverter();
                return contract;
            }
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
            if (type.IsArray)
            {
                return base.CreateContract(type);
            }
            if (type.IsEnum)
            {
                return base.CreateContract(type);
            }
            if (type == typeof(StringValues))
            {
                var contract = new JsonObjectContract(type);
                contract.Converter = new StringValuesConverter();
                return contract;
            }
            {
                var converterType = typeof(NewtonsoftConverter<>).MakeGenericType(type);
                var contract = new JsonObjectContract(type);
                contract.Converter = (JsonConverter)Activator.CreateInstance(converterType);
                return contract;
            }
        }
    }
}
