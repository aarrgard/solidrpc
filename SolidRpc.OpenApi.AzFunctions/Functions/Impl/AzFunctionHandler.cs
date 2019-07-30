using Newtonsoft.Json;
using SolidRpc.Abstractions.Services;
using SolidRpc.OpenApi.AzFunctions.Functions.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SolidRpc.OpenApi.AzFunctions.Functions.Impl
{
    /// <summary>
    /// The function handler
    /// </summary>
    public class AzFunctionHandler : IAzFunctionHandler
    {
        /// <summary>
        /// Constructs a new instance.
        /// </summary>
        /// <param name="baseDir"></param>
        public AzFunctionHandler(DirectoryInfo baseDir)
        {
            BaseDir = baseDir;
        }

        /// <summary>
        /// The base dir
        /// </summary>
        public DirectoryInfo BaseDir { get; }

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
            var timerFunction = new AzTimerFunction(functionDir);
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
            var httpFunction = new AzHttpFunction(functionDir);
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
            AzProxies proxies;
            var fileInfo = new FileInfo(Path.Combine(BaseDir.FullName, "proxies.json"));
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

            bool modified = false;
            Functions.OfType<IAzHttpFunction>()
                .SelectMany(o => o.Methods.Select(o2 => new { o.Route, Method = o2.ToUpper() }))
                .Distinct()
                .GroupBy(o => o.Route)
                .ToList().ForEach(o =>
                {
                    var route = o.Key;
                    var methods = o.Select(o2 => o2.Method).ToList();

                    modified = CreateProxy(proxies.Proxies, route, methods) || modified;
                });

            proxies.Proxies.Remove("StaticContent");
            proxies.Proxies["StaticContent"] = new AzProxy()
            {
                MatchCondition = new AzProxyMatchCondition()
                {
                    Route = "/{*path}",
                    Methods = new[] { "GET", "HEAD" }
                },
                BackendUri = $"http://%WEBSITE_HOSTNAME%/{typeof(ISolidRpcStaticContent).FullName.Replace('.', '/')}/{nameof(ISolidRpcStaticContent.GetStaticContent)}?path=/{{path}}"
            };

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
            proxy.BackendUri = SetValue(ref modified, proxy.BackendUri, $"http://%WEBSITE_HOSTNAME%{route}");
            return modified;
        }

        private T SetValue<T>(ref bool modified, T oldValue, T newValue)
        {
            if(!newValue.Equals(oldValue))
            {
                modified = true;
            }
            return newValue;
        }
    }
}
