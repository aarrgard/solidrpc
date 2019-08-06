using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace SolidRpc.OpenApi.AzFunctions.Functions.Model
{
    /// <summary>
    /// 
    /// </summary>
    public partial class NewtonsoftContractResolver : DefaultContractResolver
    {
        private static ConcurrentDictionary<Type, JsonContract> s_converters = new ConcurrentDictionary<Type, JsonContract>();

        /// <summary>
        /// 
        /// </summary>
        public static NewtonsoftContractResolver Instance = new NewtonsoftContractResolver();

        /// <summary>
        /// 
        /// </summary>
        public NewtonsoftContractResolver()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objectType"></param>
        /// <returns></returns>
        protected override JsonContract CreateContract(Type objectType)
        {
            return s_converters.GetOrAdd(objectType, CreateContractInternal);
        }

        private JsonContract CreateContractInternal(Type type)
        {
            var enumType = GetKeyValuePairEnumerableType(type);
            if(enumType != null)
            {
                var t = enumType.GetGenericArguments()[1];
                var converterType = typeof(NewtonsoftKVEConverter<>).MakeGenericType(t);
                var contract = new JsonObjectContract(type);
                contract.Converter = (JsonConverter)Activator.CreateInstance(converterType, type);
                return contract;
            }

            if (type.Assembly != GetType().Assembly)
            {
                return base.CreateContract(type);
            }
            if(type.IsArray)
            {
                return base.CreateContract(type);
            }
            {
                var converterType = typeof(NewtonsoftConverter<>).MakeGenericType(type);
                var contract = new JsonObjectContract(type);
                contract.Converter = (JsonConverter)Activator.CreateInstance(converterType);
                return contract;
            }
        }

        private Type GetKeyValuePairEnumerableType(Type type)
        {
            var interfaces = type.GetInterfaces();

            var enumTypes = interfaces
                .Where(o => o.IsGenericType)
                .Where(o => o.GetGenericTypeDefinition() == typeof(IEnumerable<>))
                .Select(o => o.GetGenericArguments()[0]);

            if (!enumTypes.Any()) return null;
            enumTypes = enumTypes.Where(o => o.IsGenericType);
            if (!enumTypes.Any()) return null;
            enumTypes = enumTypes.Where(o => o.GetGenericTypeDefinition() == typeof(KeyValuePair<,>));
            if (!enumTypes.Any()) return null;

            return enumTypes.Where(o => o.GetGenericArguments()[0] == typeof(string))
                .FirstOrDefault();
        }
    }
}
