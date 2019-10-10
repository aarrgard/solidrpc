using System;
using System.Collections.Concurrent;
using System.Reflection;

namespace SolidRpc.OpenApi.Model.Json.Impl
{
    public class ObjectProxy<T> : DispatchProxy where T : IJsonObject
    {
        private static ConcurrentDictionary<MethodInfo, Func<object[], JsonObject, object>> _invokers = new ConcurrentDictionary<MethodInfo, Func<object[], JsonObject, object>>();

        public ObjectProxy()
        {
        }

        internal JsonObject JsonObject { get; set; }

        protected override object Invoke(MethodInfo targetMethod, object[] args)
        {
            return _invokers.GetOrAdd(targetMethod, CreateInvoker).Invoke(args, JsonObject);
        }

        private Func<object[], JsonObject, object> CreateInvoker(MethodInfo targetMethod)
        {
            if (targetMethod.DeclaringType == typeof(IJsonObject))
            {
                if (targetMethod.Name == nameof(IJsonObject.Parent))
                {
                    return (args, jsonObject) => jsonObject.Parent;
                }
                if (targetMethod.Name == "get_Item")
                {
                    return (args, jsonObject) => jsonObject[(string)args[0]];
                }
                if (targetMethod.Name == "set_Item")
                {
                    return (args, jsonObject) => jsonObject[(string)args[0]] = (IJsonStruct)args[1];
                }
            }
            throw new Exception("Cannot handle method:");
        }
    }
}
