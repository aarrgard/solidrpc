﻿using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SolidRpc.Abstractions.Services;
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
        private static readonly string DefaultHttpRoutePrefix = "/api";
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
                    return DefaultHttpRoutePrefix;
                }
                using (var tr = hostJson.OpenText())
                {
                    var json = JObject.Parse(tr.ReadToEnd());
                    var routePrefix = json.SelectToken("extensions.http.routePrefix")?.ToObject<string>();
                    _routePrefix = routePrefix ?? DefaultHttpRoutePrefix;
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
                    func = new AzHttpFunction(d, function);
                }
                else if (function.Bindings.Any(o => o.Type == "timerTrigger"))
                {
                    func = new AzTimerFunction(d, function);
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
            var functionDir = new DirectoryInfo(Path.Combine(BaseDir.FullName, functionName));
            var timerFunction = new AzTimerFunction(this, functionDir);
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
            var functionDir = new DirectoryInfo(Path.Combine(BaseDir.FullName, functionName));
            var httpFunction = new AzHttpFunction(this, functionDir);
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
            AzProxies proxies = null;
            var fileInfo = new FileInfo(Path.Combine(BaseDir.FullName, "proxies.json"));
            if(fileInfo.Exists)
            {
                using (var tr = fileInfo.OpenText())
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
            if(proxies == null)
            {
                proxies = new AzProxies();
            }

            var routes = Functions.OfType<IAzHttpFunction>()
                .SelectMany(o => o.Methods.Select(o2 => new { o.Route, Method = o2.ToUpper() }))
                .Distinct()
                .GroupBy(o => o.Route)
                .ToList();

            bool modified = false;
            var staticContentFunctionPath = "/SolidRpc/Abstractions/Services/ISolidRpcStaticContent/GetStaticContent";

            // remove the routes that are not available
            foreach (var proxy in proxies.Proxies.ToList())
            {
                if(proxy.Key == staticContentFunctionPath)
                {
                    continue;
                }
                if(!routes.Any(o => $"/{o.Key}" == proxy.Value.MatchCondition.Route))
                {
                    modified = true;
                    proxies.Proxies.Remove(proxy);
                }
            }

            // add new routes
            routes.ForEach(o =>
                {
                    var route = $"/{o.Key}";
                    var methods = o.Select(o2 => o2.Method).ToList();
                    var proxyModified = CreateProxy(proxies.Proxies, route, methods);
                    if(proxyModified && !modified && !route.Equals(staticContentFunctionPath))
                    {
                        modified = true;
                    }
                });

            // convert the path "/SolidRpc/Abstractions/Services/ISolidRpcStaticContent/GetStaticContent"
            // to match /{*path}
            if (proxies.Proxies.TryGetValue(staticContentFunctionPath, out AzProxy staticContentProxy))
            {
                staticContentProxy.MatchCondition.Route = $"/{{*path}}";
                staticContentProxy.BackendUri = $"http://%WEBSITE_HOSTNAME%{HttpRoutePrefix}{staticContentFunctionPath}?path=/{{path}}";

                proxies.Proxies.Remove(staticContentFunctionPath);
                proxies.Proxies[staticContentFunctionPath] = staticContentProxy;
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
                using (var fs = fileInfo.CreateText())
                {
                    fs.Write(sw.ToString());
                }
            }
        }

        private bool CreateProxy(IDictionary<string, AzProxy> proxies, string route, IEnumerable<string> methods)
        {
            AzProxy proxy;
            if (!proxies.TryGetValue(route, out proxy))
            {
                proxies[route] = proxy = new AzProxy();
            }
            bool modified = false;
            proxy.MatchCondition = proxy.MatchCondition ?? new AzProxyMatchCondition();
            proxy.MatchCondition.Route = SetValue(ref modified, proxy.MatchCondition.Route, route);
            proxy.MatchCondition.Methods = SetValue(ref modified, proxy.MatchCondition.Methods, methods);
            proxy.BackendUri = SetValue(ref modified, proxy.BackendUri, $"http://%WEBSITE_HOSTNAME%{HttpRoutePrefix}{route}");
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
