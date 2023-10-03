using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using SolidRpc.Abstractions.OpenApi.Http;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace SolidRpc.Abstractions.OpenApi.Invoker
{
    /// <summary>
    /// Contains additional invocation options.
    /// </summary>
    public class InvocationOptions
    {
        public const string RequestHeaderInboundPrefix = "http_in_req_";
        public const string RequestHeaderOutboundPrefix = "http_out_req_";
        public const string ResponseHeaderPrefix = "http_resp_";

        public const string RequestHeaderPriority = "X-SolidRpc-Priority";
        public const string RequestHeaderContinuationToken = "X-SolidRpc-ContinuationToken";
        public const string RequestHeaderMethodUri = "X-SolidRpc-MethodUri";

        private struct KV
        {
            public KV(string key, object value)
            {
                Key = key;
                Value = value;
            }
            public string Key { get;}
            public object Value { get; }
        }

        private static readonly ReadOnlyDictionary<string, KV> EMPTY_KVS = new ReadOnlyDictionary<string, KV>(new Dictionary<string, KV>());

        /// <summary>
        /// Returns the current invocation options
        /// </summary>
        public static InvocationOptions Current
        {
            get
            {
                return InvocationOptionsLocal.Current;
            }
        }

        /// <summary>
        /// Returns the invocation options for a new invocation
        /// </summary>
        public static InvocationOptions New
        {
            get
            {
                var curr = InvocationOptionsLocal.Current;
                return new InvocationOptions(
                    null,
                    null,
                    curr.Priority,
                    null,
                    curr.KeyValues,
                    null,
                    null);
            }
        }

        /// <summary>
        /// Returns the invocation options for specified method
        /// </summary>
        /// <param name="methodInfo"></param>
        /// <returns></returns>
        public static InvocationOptions GetOptions(MethodInfo methodInfo)
        {
            var options = Current;
            if (options.MethodInfo == methodInfo)
            {
                return options;
            }
            if (options.MethodInfo == null)
            {
                return options.SetMethodInfo(methodInfo);
            }
            return new InvocationOptions(
                methodInfo, 
                null, 
                MessagePriorityNormal,
                null,
                options.KeyValues,
                null,
                null);
        }

        /// <summary>
        /// Returns the default invocation options
        /// </summary>
        public static InvocationOptions Default = new InvocationOptions(null, null, MessagePriorityNormal);

        /// <summary>
        /// The default pre invoke callback(Does nothing)
        /// </summary>
        /// <param name="httpReq"></param>
        /// <returns></returns>
        private static Task DefaultPreInvokeCallback(IHttpRequest httpReq)
        {
            return Task.CompletedTask;
        }

        /// <summary>
        /// The default post invoke callback(Does nothing)
        /// </summary>
        /// <param name="httpResp"></param>
        /// <returns></returns>
        private static Task DefaultPostInvokeCallback(IHttpResponse httpResp)
        {
            return Task.CompletedTask;
        }

        /// <summary>
        /// The high message prio
        /// </summary>
        public const int MessagePriorityHigh = 3;
        /// <summary>
        /// The normal message prio
        /// </summary>
        public const int MessagePriorityNormal = 5;
        /// <summary>
        /// The low message prio
        /// </summary>
        public const int MessagePriorityLow = 7;

        /// <summary>
        /// Constructs a new instance
        /// </summary>
        /// <param name="methodInfo"></param>
        /// <param name="transportType"></param>
        /// <param name="priority"></param>
        /// <param name="continuationToken"></param>
        /// <param name="kvs"></param>
        /// <param name="preInvokeCallback"></param>
        /// <param name="postInvokeCallback"></param>
        private InvocationOptions(
            MethodInfo methodInfo,
            string transportType, 
            int priority, 
            string continuationToken = null,
            IDictionary<string, KV> kvs = null,
            Func<IHttpRequest, Task> preInvokeCallback = null, 
            Func<IHttpResponse, Task> postInvokeCallback = null)
        {
            if (priority <= 0)
            {
                priority = MessagePriorityNormal;
            }
            MethodInfo = methodInfo;
            TransportType = transportType;
            Priority = priority;
            ContinuationToken = continuationToken;
            KeyValues = kvs ?? EMPTY_KVS;
            PreInvokeCallback = preInvokeCallback ?? DefaultPreInvokeCallback;
            PostInvokeCallback = postInvokeCallback ?? DefaultPostInvokeCallback;
        }

        /// <summary>
        /// Attaches these options to the task local
        /// </summary>
        /// <returns></returns>
        public IDisposable Attach()
        {
            if(ReferenceEquals(this, InvocationOptionsLocal.Current))
            {
                return InvocationOptionsLocal.DummyDisposable;
            }
            else
            {
                return new InvocationOptionsLocal(this);
            }
        }

        /// <summary>
        /// The method info that this option is associated with
        /// </summary>
        public MethodInfo MethodInfo { get; }

        /// <summary>
        /// The preferred transport type. Defaults to "Http"
        /// </summary>
        public string TransportType { get; }

        /// <summary>
        /// The invocation priority.
        /// </summary>
        public int Priority { get; }

        /// <summary>
        /// The continuation token.
        /// </summary>
        public string ContinuationToken { get; }

        /// <summary>
        /// The key values.
        /// </summary>
        private IDictionary<string, KV> KeyValues { get; }

        /// <summary>
        /// The pre invoke callback
        /// </summary>
        public Func<IHttpRequest, Task> PreInvokeCallback { get; }

        /// <summary>
        /// The post invoke callback
        /// </summary>
        public Func<IHttpResponse, Task> PostInvokeCallback { get; }

        /// <summary>
        /// The keys
        /// </summary>
        public IEnumerable<string> Keys => KeyValues.Select(o => o.Value.Key);


        /// <summary>
        /// Returns a copy of this instance with another method info.
        /// </summary>
        /// <param name="methodInfo"></param>
        /// <returns></returns>
        public InvocationOptions SetMethodInfo(MethodInfo methodInfo)
        {
            if (MethodInfo == methodInfo)
            {
                return this;
            }
            return new InvocationOptions(
                methodInfo,
                TransportType,
                Priority,
                ContinuationToken,
                KeyValues,
                PreInvokeCallback,
                PostInvokeCallback);
        }

        /// <summary>
        /// Returns a copy of this instance with another priority.
        /// </summary>
        /// <returns></returns>
        public InvocationOptions LowerPriority()
        {
            return new InvocationOptions(
                MethodInfo,
                TransportType,
                Priority + 1,
                ContinuationToken,
                KeyValues,
                PreInvokeCallback,
                PostInvokeCallback);
        }

        /// <summary>
        /// Returns a copy of this instance with another priority.
        /// </summary>
        /// <param name="priority"></param>
        /// <returns></returns>
        public InvocationOptions SetPriority(int priority)
        {
            if (priority == Priority)
            {
                return this;
            }
            return new InvocationOptions(
                MethodInfo,
                TransportType,
                priority,
                ContinuationToken,
                KeyValues,
                PreInvokeCallback,
                PostInvokeCallback);
        }


        /// <summary>
        /// Adds a value to the key value set
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="prefix"></param>
        /// <param name="key"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public InvocationOptions SetKeyValue<T>(string prefix, string key, T val)
        {
            return SetKeyValue($"{prefix}{key}", val);
        }

        /// <summary>
        /// Adds a value to the key value set
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public InvocationOptions SetKeyValue<T>(string key, T val)
        {
            var newKvs = new Dictionary<string, KV>(KeyValues);
            newKvs[key.ToLower()] = new KV(key, val);
            return new InvocationOptions(
                MethodInfo,
                TransportType,
                Priority,
                ContinuationToken,
                newKvs,
                PreInvokeCallback,
                PostInvokeCallback);
        }

        /// <summary>
        /// Returns a copy of this instance with another key value set added.
        /// </summary>
        /// <param name="kvs"></param>
        /// <returns></returns>
        public InvocationOptions SetKeyValues(IDictionary<string, object> kvs)
        {
            return new InvocationOptions(
                MethodInfo,
                TransportType, 
                Priority, 
                ContinuationToken, 
                MergeKeyValues(KeyValues, kvs), 
                PreInvokeCallback, 
                PostInvokeCallback);
        }

        private IDictionary<string, KV> MergeKeyValues(IDictionary<string, KV> oldValues, IDictionary<string, object> newValues)
        {
            var modifiedValues = newValues.Where(o =>
            {
                if (oldValues.TryGetValue(o.Key.ToLower(), out KV oldValue))
                {
                    if (ReferenceEquals(o.Value, oldValue.Value)) return false;
                    if (ReferenceEquals(o.Value, null)) return true;
                    if (ReferenceEquals(oldValue.Value, null)) return true;
                    return o.Value.Equals(oldValue.Value);
                }
                else
                {
                    return true;
                }
            });
            if(!modifiedValues.Any())
            {
                return oldValues;
            }
            var retVal = new Dictionary<string, KV>(oldValues);
            modifiedValues.ToList().ForEach(o => retVal[o.Key.ToLower()] = new KV(o.Key, o.Value));
            return retVal;
        }

        /// <summary>
        /// Returns the value
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public bool TryGetValue<T>(string prefix, string key, out T value)
        {
            return TryGetValue($"{prefix}{key}", out value);
        }

        /// <summary>
        /// Returns the value
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public bool TryGetValue<T>(string key, out T value)
        {
            if (KeyValues.TryGetValue(key.ToLower(), out KV oVal))
            {
                if (oVal.Value == null)
                {
                    value = default(T);
                    return false;
                }
                if (typeof(T).IsAssignableFrom(oVal.GetType()))
                {
                    value = (T)oVal.Value;
                    return false;
                }
                if (typeof(T) == typeof(StringValues))
                {
                    if (oVal.Value.GetType() == typeof(string))
                    {
                        value = (T)(object)new StringValues((string)oVal.Value);
                        return true;
                    }
                    if (oVal.Value.GetType() == typeof(Uri))
                    {
                        value = (T)(object)new StringValues(((Uri)oVal.Value).ToString());
                        return true;
                    }
                }
                if (typeof(T) == typeof(string))
                {
                    if (oVal.Value.GetType() == typeof(StringValues))
                    {
                        value = (T)(object)oVal.Value.ToString();
                        return true;
                    }
                }
                value = (T)oVal.Value;
                return true;
            }
            value = default(T);
            return false;
        }


        /// <summary>
        /// Returns a copy of this instance with another continuation token.
        /// </summary>
        /// <param name="continuationToken"></param>
        /// <returns></returns>
        public InvocationOptions SetContinuationToken(string continuationToken)
        {
            return new InvocationOptions(
                MethodInfo,
                TransportType, 
                Priority, 
                continuationToken,
                KeyValues, 
                PreInvokeCallback, 
                PostInvokeCallback);
        }

        /// <summary>
        /// Sets the transport to use.
        /// </summary>
        /// <param name="transportType"></param>
        /// <returns></returns>
        public InvocationOptions SetTransport(string transportType)
        {
            if (transportType == TransportType)
            {
                return this;
            }
            return new InvocationOptions(
                MethodInfo,
                transportType, 
                Priority,
                ContinuationToken, 
                KeyValues, 
                PreInvokeCallback,
                PostInvokeCallback);
        }

        /// <summary>
        /// Adds a pre invokation callback
        /// </summary>
        /// <param name="callback"></param>
        /// <returns></returns>
        public InvocationOptions AddPreInvokeCallback(Func<IHttpRequest, Task> callback)
        {
            if(callback == null)
            {
                return this;
            }
            var oldCallback = PreInvokeCallback;
            return new InvocationOptions(
                MethodInfo,
                TransportType,
                Priority, 
                ContinuationToken, 
                KeyValues, 
                async (req) =>
                {
                    await callback(req);
                    await oldCallback(req);
                }, 
                PostInvokeCallback);
        }

        /// <summary>
        /// Adds a pre invokation callback
        /// </summary>
        /// <param name="callback"></param>
        /// <returns></returns>
        public InvocationOptions AddPostInvokeCallback(Func<IHttpResponse, Task> callback)
        {
            if (callback == null)
            {
                return this;
            }
            var oldCallback = PostInvokeCallback;
            return new InvocationOptions(
                MethodInfo,
                TransportType, 
                Priority, 
                ContinuationToken,
                KeyValues, 
                PreInvokeCallback, 
                async (resp) =>
                {
                    await callback(resp);
                    await oldCallback(resp);
                });
        }
    }
}
