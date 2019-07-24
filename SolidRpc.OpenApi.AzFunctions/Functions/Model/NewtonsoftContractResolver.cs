using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Concurrent;

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
            if(type.Assembly != GetType().Assembly)
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
