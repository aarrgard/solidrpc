using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SolidRpc.OpenApi.AzFunctions.Functions.Model;
using System;
using System.Collections;
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
        private static readonly string s_staticContentFunctionRoute = "SolidRpc/Abstractions/Services/ISolidRpcStaticContent/GetStaticContent";
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
        public IEnumerable<IAzFunction> Functions
        {
            get
            {
                var functions = new List<IAzFunction>();
                foreach(var d in BaseDir.GetDirectories())
                {
                    if(GetFunction(d, out IAzFunction func))
                    {
                        functions.Add(func);
                    }
                }
                return functions;
            }
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
        public string HttpRoutePrefix
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

        /// <summary>
        /// Creates a timer function
        /// </summary>
        /// <returns></returns>
        public IAzTimerFunction CreateTimerFunction(string functionName)
        {
            var timerFunction = new AzTimerFunction(this, functionName);
            return timerFunction;
        }

        private string ReadFileContent(FileInfo functionJson)
        {
            using (var tr = functionJson.OpenText())
            {
                return tr.ReadToEnd();
            }
        }

        /// <summary>
        /// Creates a http function
        /// </summary>
        /// <param name="functionName"></param>
        /// <returns></returns>
        public IAzHttpFunction CreateHttpFunction(string functionName)
        {
            var httpFunction = new AzHttpFunction(this, functionName);
            return httpFunction;
        }

        /// <summary>
        /// triggers a restart of the application
        /// </summary>
        public void TriggerRestart()
        {
            var hostJson = new FileInfo(Path.Combine(BaseDir.FullName, "host.json"));
            if(hostJson.Exists)
            {
                using (var sw = hostJson.AppendText())
                {
                    sw.WriteLine($"//{DateTime.Now.ToString("yyyyMMddHHmmssfffff")}");
                }
            }
        }

        /// <summary>
        /// Syncronizes the proxies file with the functions.
        /// </summary>
        public void SyncProxiesFile()
        {
            SyncProxiesFile(new FileInfo(Path.Combine(BaseDir.FullName, "proxies.json")));
            if(DevDir != null)
            {
                SyncProxiesFile(new FileInfo(Path.Combine(DevDir.FullName, "proxies.json")));
            }
        }
        private void SyncProxiesFile(FileInfo proxiesFile)
        {
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

            //proxies.SetSortedProxies();

            var routes = Functions.OfType<IAzHttpFunction>()
                .SelectMany(o => o.Methods.Select(o2 => new { o.Route, Method = o2.ToUpper() }))
                .Distinct()
                .GroupBy(o => o.Route)
                .ToList();

            bool modified = false;

            // remove the routes that are not available
            foreach (var proxy in proxies.Proxies.ToList())
            {
                // keep static content route
                if (proxy.Key == s_staticContentFunctionRoute)
                {
                    continue;
                }
                // remove proxy if route prefix is null
                if (string.IsNullOrEmpty(HttpRoutePrefix))
                {
                    modified = true;
                    proxies.Proxies.Remove(proxy);
                }
                // remove route if no function
                if (!routes.Any(o => $"/{o.Key}" == proxy.Value.MatchCondition.Route))
                {
                    modified = true;
                    proxies.Proxies.Remove(proxy);
                }
            }

            // add new routes
            routes.ForEach(o =>
            {
                if (o.Key == s_staticContentFunctionRoute)
                {
                    // use this route
                }
                else if (string.IsNullOrEmpty(HttpRoutePrefix))
                {
                    return;
                }

                var methods = o.Select(o2 => o2.Method).ToList();
                var proxyModified = CreateProxy(proxies.Proxies, o.Key, methods);
                if (proxyModified && !modified)
                {
                    modified = true;
                }
            });

            //
            // put static route last
            //
            var staticRoute = proxies.Proxies.FirstOrDefault(o => o.Key == s_staticContentFunctionRoute);
            if(!string.IsNullOrEmpty(staticRoute.Key))
            {
                proxies.Proxies.Remove(staticRoute);
                proxies.Proxies.Add(staticRoute);
            }

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


        public string CreateRoute(string route)
        {
            if (route.StartsWith("/"))
            {
                return route.Substring(1);
            }
            return route;
        }

        private bool CreateProxy(IList<KeyValuePair<string, AzProxy>> proxies, string route, IEnumerable<string> methods)
        {
            var proxyKV = proxies.FirstOrDefault(o => o.Key == route);
            if (string.IsNullOrEmpty(proxyKV.Key))
            {
                proxyKV = new KeyValuePair<string, AzProxy>(route, new AzProxy());
                proxies.Add(proxyKV);
            }

            var backendUri = $"http://%WEBSITE_HOSTNAME%{HttpRoutePrefix}/{route}";
            if (route == s_staticContentFunctionRoute)
            {
                route = $"{{*path}}";
                backendUri = $"{backendUri}?path=/{{path}}";
            }

            var proxy = proxyKV.Value;
            bool modified = false;
            proxy.MatchCondition = proxy.MatchCondition ?? new AzProxyMatchCondition();
            proxy.MatchCondition.Route = SetValue(ref modified, proxy.MatchCondition.Route, $"/{route}");
            proxy.MatchCondition.Methods = SetValue(ref modified, proxy.MatchCondition.Methods, methods);
            proxy.BackendUri = SetValue(ref modified, proxy.BackendUri, backendUri);
            if(modified)
            {
                return true;
            }
            return false;
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
            if (oldValue.GetType() == newValue.GetType())
            {
                if(typeof(IEnumerable).IsAssignableFrom(oldValue.GetType()))
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
    }
}
