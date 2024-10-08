using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SolidRpc.OpenApi.AzFunctions.Functions.Model;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace SolidRpc.OpenApi.AzFunctions.Functions.Impl
{
    /// <summary>
    /// The function handler
    /// </summary>
    public class AzFunctionHandler : IAzFunctionHandler
    {
        private static readonly string s_defaultHttpRoutePrefix = "/api";
        private string _routePrefix = null;

        /// <summary>
        /// Constructs a new instance.
        /// </summary>
        /// <param name="baseDir"></param>
        /// <param name="functionAssembly"></param>
        public AzFunctionHandler(DirectoryInfo baseDir, Assembly functionAssembly)
        {
            BaseDir = baseDir;
            FunctionAssembly = functionAssembly;
            TriggerHandlers = new ConcurrentDictionary<string, Type>();
            FunctionCode = new Dictionary<string, string>();
        }

        /// <summary>
        /// <summary>
        /// The base dir
        /// </summary>
        public DirectoryInfo BaseDir { get; }

        /// <summary>
        /// Returns the dev dir if it exists
        /// </summary>
        public DirectoryInfo DevDir
        {
            get
            {
                var devDir = BaseDir.Parent;
                while(devDir != null)
                {
                    if (!devDir.Exists)
                    {
                        return null;
                    }
                    if(new FileInfo(Path.Combine(devDir.FullName,"host.json")).Exists)
                    {
                        //return null;
                        return devDir;
                    }
                    devDir = devDir.Parent;
                }
                return devDir;
            }
        }

        /// <summary>
        /// The code for each function
        /// </summary>
        public IDictionary<string, string> FunctionCode { get; }

        /// <summary>
        /// The base directories
        /// </summary>
        public IEnumerable<DirectoryInfo> BaseDirs => (new [] { BaseDir }).Where(o => o != null);

        /// <summary>
        /// The function assembly
        /// </summary>
        public Assembly FunctionAssembly { get; }

        /// <summary>
        /// Contains the trigger handlers
        /// </summary>
        public ConcurrentDictionary<string, Type> TriggerHandlers { get;}

        /// <summary>
        /// Create a new instance
        /// </summary>
        public IEnumerable<IAzFunction> GetFunctions()
        {
            var functions = new Dictionary<string, IAzFunction>();
            foreach(var d in BaseDirs.SelectMany(o => o.GetDirectories()))
            {
                if(GetFunction(d, out IAzFunction func))
                {
                    functions[func.Name] = func;
                }
            }
            return functions.Values;
        }

        private IAzFunction GetFunction(string functionName)
        {
            var funcDir = BaseDirs.First().GetDirectories().Where(o => o.Name == functionName).FirstOrDefault();
            if(funcDir == null)
            {
                return null;
            }
            if(GetFunction(funcDir, out IAzFunction func))
            {
                return func;
            }
            return null;
        }

        /// <summary>
        /// Returns the http trigger handler
        /// </summary>
        public Type HttpTriggerHandler => TriggerHandlers.GetOrAdd($".HttpFunction", _ => FindTriggerHandler(_));

        /// <summary>
        /// Returns the http trigger handler
        /// </summary>
        public Type TimerTriggerHandler => TriggerHandlers.GetOrAdd($".TimerFunction", _ => FindTriggerHandler(_));

        /// <summary>
        /// Returns the queue trigger handler
        /// </summary>
        public Type QueueTriggerHandler => TriggerHandlers.GetOrAdd($".QueueFunction", _ => FindTriggerHandler(_));

        /// <summary>
        /// Returns the http scheme
        /// </summary>
        public string HttpScheme
        {
            get
            {
                return "http";
            }
        }

        /// <summary>
        /// Returns the route prefix.
        /// </summary>
        public string HttpRouteBackendPrefix
        {
            get
            {
                if(_routePrefix != null)
                {
                    return _routePrefix;
                }
                var hostJson = new FileInfo(Path.Combine(BaseDir.FullName, "host.json"));
                if(!hostJson.Exists)
                {
                    return s_defaultHttpRoutePrefix;
                }
                using (var tr = hostJson.OpenText())
                {
                    var json = JObject.Parse(tr.ReadToEnd());
                    // V2 - use extensions
                    var routePrefix = json.SelectToken("extensions.http.routePrefix")?.ToObject<string>();
                    if (routePrefix == null)
                    {
                        // V1 - use http directly
                        routePrefix = json.SelectToken("http.routePrefix")?.ToObject<string>();
                    }
                    if (routePrefix == null)
                    {
                        routePrefix = s_defaultHttpRoutePrefix;
                    }
                    else if (string.IsNullOrEmpty(routePrefix))
                    {
                        // use empty prefix;
                    }
                    else
                    {
                        if (!routePrefix.StartsWith("/"))
                        {
                            routePrefix = $"/{routePrefix}";
                        }
                    }
                    _routePrefix = routePrefix;
                }
                return _routePrefix;
            }
        }

        private Type FindTriggerHandler(string typeSuffix)
        {
            var triggerHandler = FunctionAssembly.GetTypes().Where(o => o.FullName.EndsWith(typeSuffix)).FirstOrDefault();
            if (triggerHandler == null)
            {
                throw new Exception($"Cannot find type that ends with {typeSuffix} in assembly {FunctionAssembly.GetName().Name}");
            }
            return triggerHandler;
        }

        private bool GetFunction(DirectoryInfo d, out IAzFunction func)
        {
            func = null;
            var functionJson = new FileInfo(Path.Combine(d.FullName, "function.json"));
            if(!functionJson.Exists)
            {
                return false;
            }
            var strFunctionJson = ReadFileContent(functionJson);
            try
            {
                var function = JsonConvert.DeserializeObject<Function>(strFunctionJson);
                if (function.Bindings.Any(o => o.Type == "httpTrigger"))
                {
                    func = new AzHttpFunction(this, d.Name, function);
                }
                else if (function.Bindings.Any(o => o.Type == "timerTrigger"))
                {
                    func = new AzTimerFunction(this, d.Name, function);
                }
                else if (function.Bindings.Any(o => o.Type == "queueTrigger"))
                {
                    func = new AzQueueFunction(this, d.Name, function);
                }
                else if (function.Bindings.Any(o => o.Type == "serviceBusTrigger"))
                {
                    func = new AzSvcBusFunction(this, d.Name, function);
                }
                else
                {
                    return false;
                }
                return true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private string ReadFileContent(FileInfo functionJson)
        {
            using (var tr = functionJson.OpenText())
            {
                return tr.ReadToEnd();
            }
        }


        /// <summary>
        /// Creates a function
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="functionName"></param>
        /// <returns></returns>
        public T CreateFunction<T>(string functionName) where T:class,IAzFunction
        {
            if (string.IsNullOrEmpty(functionName)) throw new ArgumentNullException(nameof(functionName));
            if (typeof(T) == typeof(IAzHttpFunction))
            {
                return (T)(object)new AzHttpFunction(this, functionName);
            }
            if (typeof(T) == typeof(IAzQueueFunction))
            {
                return (T)(object)new AzQueueFunction(this, functionName);
            }
            if (typeof(T) == typeof(IAzSvcBusFunction))
            {
                return (T)(object)new AzSvcBusFunction(this, functionName);
            }
            if (typeof(T) == typeof(IAzTimerFunction))
            {
                return (T)(object)new AzTimerFunction(this, functionName);
            }
            throw new Exception("Cannot create function of type:" + typeof(T));
        }

        /// <summary>
        /// Creates a route
        /// </summary>
        /// <param name="route"></param>
        /// <returns></returns>
        public string CreateRoute(string route)
        {
            if (route.StartsWith("/"))
            {
                return route.Substring(1);
            }
            return route;
        }

        T IAzFunctionHandler.GetOrCreateFunction<T>(string functionName)
        {
            var function = GetFunction(functionName);
            T typedFunction = function as T;
            if (typedFunction == null && function != null)
            {
                function.Delete();
            }
            if (typedFunction == null)
            {
                typedFunction = CreateFunction<T>(functionName);
            }
            return typedFunction;
        }
    }
}
