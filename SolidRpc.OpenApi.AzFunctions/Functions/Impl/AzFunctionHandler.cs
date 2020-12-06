using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SolidRpc.Abstractions.Services;
using SolidRpc.OpenApi.AzFunctions.Functions.Model;
using SolidRpc.OpenApi.Binder;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace SolidRpc.OpenApi.AzFunctions.Functions.Impl
{
    /// <summary>
    /// The function handler
    /// </summary>
    public class AzFunctionHandler : IAzFunctionHandler
    {
        private static Task s_restartJob = Task.CompletedTask;
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
                    if (string.IsNullOrEmpty(routePrefix))
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

        /// <summary>
        /// Returns the frontend prefix
        /// </summary>
        public string HttpRouteFrontendPrefix
        {
            get
            {
                return "/front";
            }
        }

        /// <summary>
        /// Returns the prefix mappings
        /// </summary>
        /// <returns></returns>
        public IDictionary<string, string> GetPrefixMappings()
        {
            return new Dictionary<string, string>()
            {
                { HttpRouteBackendPrefix, "" },
                { HttpRouteFrontendPrefix, "" },
            };
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
        /// Syncronizes the proxies file with the functions.
        /// </summary>
        public void SyncProxiesFile(
            List<FunctionDef> functionDefs,
            IDictionary<string, string> staticRoutes,
            IDictionary<string, string> redirects)
        {
            if(DevDir != null)
            {
                SyncProxiesFile(DevDir, functionDefs, staticRoutes, redirects);
            }
        }
        private void SyncProxiesFile(
            DirectoryInfo baseDir,
            List<FunctionDef> functionDefs,
            IDictionary<string, string> staticRoutes,
            IDictionary<string, string> redirects)
        {
            var scheme = Environment.GetEnvironmentVariable(ConfigurationMethodAddressTransformer.ConfigScheme) ?? "http";
            // add the content fetcher 
            var staticRoute = $"{HttpRouteFrontendPrefix}/{{*path}}";
            staticRoutes[staticRoute] = $"{scheme}://%WEBSITE_HOSTNAME%{HttpRouteBackendPrefix}/{typeof(ISolidRpcContentHandler).FullName.Replace('.', '/')}/{nameof(ISolidRpcContentHandler.GetContent)}?path={HttpRouteFrontendPrefix}/{{path}}";
            if (staticRoutes.ToList().Any(o => !o.Key.StartsWith("/")))
            {
                throw new Exception("Found strange entry");
            }
            staticRoutes = staticRoutes.ToList().ToDictionary(o => o.Key.Substring(1), o => o.Value);

            var proxiesFile = new FileInfo(Path.Combine(baseDir.FullName, "proxies.json"));
            AzProxies proxies = null;
            if (proxiesFile.Exists)
            {
                using (var tr = proxiesFile.OpenText())
                {
                    using (JsonReader reader = new JsonTextReader(tr))
                    {
                        var settings = new JsonSerializerSettings()
                        {
                            ContractResolver = NewtonsoftContractResolver.Instance
                        };
                        var serializer = JsonSerializer.Create(settings);
                        proxies = serializer.Deserialize<AzProxies>(reader);
                    }
                }
            }
            if (proxies == null)
            {
                proxies = new AzProxies();
            }

            var routes = functionDefs.OfType<HttpFunctionDef>()
                .GroupBy(o => o.PathWithArgNames)
                .ToList();

            bool modified = false;

            // remove the routes that are not available
            foreach (var proxy in proxies.Proxies.ToList())
            {
                // keep static content route
                if (staticRoutes.Any(o => FixRouteName(o.Key) == proxy.Key))
                {
                    continue;
                }
                if (redirects.Any(o => FixRouteName(o.Key) == proxy.Key))
                {
                    continue;
                }
                // remove proxy if route prefix is null
                if (string.IsNullOrEmpty(HttpRouteBackendPrefix))
                {
                    modified = true;
                    proxies.Proxies.Remove(proxy);
                }
                // remove route if no function
                if (!routes.Any(o => CreateFrontendRoute(o.Key) == proxy.Value.MatchCondition.Route))
                {
                    modified = true;
                    proxies.Proxies.Remove(proxy);
                }
            }

            // add new routes
            routes.ForEach(o =>
            {
                if (string.IsNullOrEmpty(HttpRouteBackendPrefix))
                {
                    // if the backend prefix is empty - dont proxy - causes circular calls
                    return;
                }

                var methods = o.Select(o2 => o2.Method.ToUpper()).ToList();
                var backendUri = $"{scheme}://%WEBSITE_HOSTNAME%{HttpRouteBackendPrefix}/{o.Key}";
                var frontEndRoute = CreateFrontendRoute(o.Key);
                var proxyModified = CreateProxy(proxies.Proxies, o.Key, methods, frontEndRoute, backendUri, false);
                if (proxyModified && !modified)
                {
                    modified = true;
                }
            });

            //
            // write dynamic content paths
            //
            staticRoutes.ToList().ForEach(o =>
            {
                var proxyModified = CreateProxy(proxies.Proxies, o.Key, new[] { "GET" }, $"/{o.Key}", o.Value, false);
                if (proxyModified && !modified)
                {
                    modified = true;
                }
            });

            //
            // write redirects
            //
            redirects.ToList().ForEach(o =>
            {
                var proxyModified = CreateProxy(proxies.Proxies, o.Key, new[] { "GET" }, $"/{o.Key}", o.Value, true);
                if (proxyModified && !modified)
                {
                    modified = true;
                }
            });

            //
            // put static route last
            //
            var staticRouteProxy = proxies.Proxies.Single(o => o.Key == FixRouteName(staticRoute.Substring(1)));
            proxies.Proxies.Remove(staticRouteProxy);
            proxies.Proxies.Add(staticRouteProxy);


            var sw = new StringWriter();
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                var settings = new JsonSerializerSettings()
                {
                    Formatting = Formatting.Indented,
                    ContractResolver = NewtonsoftContractResolver.Instance
                };
                var serializer = JsonSerializer.Create(settings);
                serializer.Serialize(writer, proxies);
            }

            if(modified)
            {
                using (var fs = proxiesFile.CreateText())
                {
                    fs.Write(sw.ToString());
                }
            }
        }

        private string FixRouteName(string key)
        {
            if (key.EndsWith("/"))
            {
                key = key.Substring(0, key.Length - 1);
            }
            key = key.Replace("*", "");
            if (key == "") key = "EmPtY";
            return key;
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

        private bool CreateProxy(IList<KeyValuePair<string, AzProxy>> proxies, string routeName, IEnumerable<string> methods, string frontendRoute, string backendUri, bool redirect)
        {
            routeName = FixRouteName(routeName);
            if(frontendRoute.EndsWith("/"))
            {
                frontendRoute = frontendRoute.Substring(0, frontendRoute.Length-1);
            }
            var proxyKV = proxies.FirstOrDefault(o => o.Key == routeName);
            if (string.IsNullOrEmpty(proxyKV.Key))
            {
                proxyKV = new KeyValuePair<string, AzProxy>(routeName, new AzProxy());
                proxies.Add(proxyKV);
            }


            var proxy = proxyKV.Value;
            bool modified = false;
            proxy.MatchCondition = proxy.MatchCondition ?? new AzProxyMatchCondition();
            proxy.MatchCondition.Route = SetValue(ref modified, proxy.MatchCondition.Route, frontendRoute);
            proxy.MatchCondition.Methods = SetValue(ref modified, proxy.MatchCondition.Methods, methods);
            if(redirect)
            {
                proxy.ResponseOverrides = SetValue(ref modified, proxy.ResponseOverrides, new Dictionary<string, string>
                {
                    { "response.statusCode", "302" },
                    { "response.headers.Location", backendUri }
                });
            }
            else
            {
                proxy.BackendUri = SetValue(ref modified, proxy.BackendUri, backendUri);
            }
            if (modified)
            {
                return true;
            }
            return false;
        }

        private string CreateFrontendRoute(string route)
        {
            return $"{HttpRouteFrontendPrefix}/{route}";
        }

        private T SetValue<T>(ref bool modified, T oldValue, T newValue)
        {
            if (ReferenceEquals(newValue, oldValue))
            {
                return newValue;
            }
            if(oldValue == null || newValue == null)
            {
                modified = true;
                return newValue;
            }
            if (typeof(IEnumerable).IsAssignableFrom(oldValue.GetType()))
            {
                if (oldValue.GetType() == newValue.GetType() || (oldValue.GetType().IsArray || newValue.GetType().IsArray))
                {
                    return SetValueEnum<T>(ref modified, (IEnumerable)oldValue, (IEnumerable)newValue);
                }
            }
            if(!newValue.Equals(oldValue))
            {
                modified = true;
            }
            return newValue;
        }
        private T SetValueEnum<T>(ref bool modified, IEnumerable oldValue, IEnumerable newValue)
        {
            var o = oldValue.GetEnumerator();
            var n = newValue.GetEnumerator();
            while(o.MoveNext())
            {
                if(!n.MoveNext())
                {
                    modified = true;
                    return (T)newValue;
                }
                if(false == (o?.Current?.Equals(n.Current) ?? false))
                {
                    modified = true;
                    return (T)newValue;
                }
            }
            if (n.MoveNext())
            {
                modified = true;
                return (T)newValue;
            }
            return (T)oldValue;
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
