using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Logging;
using SolidProxy.Core.Configuration.Runtime;
using SolidRpc.OpenApi.Binder;
using SolidRpc.OpenApi.Binder.Proxy;

namespace SolidRpc.OpenApi.AzFunctions
{
    public class SolidRpcSetup : ISolidRpcSetup
    {
        private object _mutex = new object();
        private IMethodInvoker _methodInvoker;
        private bool _initialized;
        public SolidRpcSetup(ILogger<SolidRpcSetup> logger, ISolidProxyConfigurationStore configStore, IMethodInvoker methodInvoker)
        {
            Logger = logger;
            ConfigStore = configStore;
            _methodInvoker = methodInvoker;
        }

        private ILogger Logger { get; }

        public ISolidProxyConfigurationStore ConfigStore { get; }

        public IMethodInvoker MethodInvoker
        {
            get
            {
                if(!_initialized)
                {
                    lock(_mutex)
                    {
                        if (!_initialized)
                        {
                            _methodInvoker.MethodBinderStore.MethodBinders
                                .SelectMany(o => o.MethodInfos)
                                .ToList()
                                .ForEach(o => WriteFunctions(o));
                        }
                        _initialized = true;
                    }
                }
                return _methodInvoker;
            }
        }

        private void WriteFunctions(IMethodInfo o)
        {
            Logger.LogInformation($"Mapping method:{o.Path}");
            var d = new DirectoryInfo(".");
            var funcName = CreateFunctionName(o);
            var funcFolder = new DirectoryInfo(Path.Combine(d.FullName, funcName));
            if(!funcFolder.Exists)
            {
                funcFolder.Create();
            }

            WriteRunCsx(funcFolder);
            WriteFunctionJson(funcFolder, new[] { o.Method }, o.Path);
        }

        private string CreateFunctionName(IMethodInfo o)
        {
            return $"{o.MethodBinder.Assembly.GetName().Name}.{o.OperationId}"
                .Replace(".", "");
        }

        private void WriteRunCsx(DirectoryInfo funcFolder)
        {
            var fi = new FileInfo(Path.Combine(funcFolder.FullName, "run.csx"));
            WriteFile(fi, @"#r ""Newtonsoft.Json""
#r ""Microsoft.Extensions.DependencyInjection.Abstractions""
#r ""../bin/SolidRpc.OpenApi.AzFunctions.dll""
#r ""../bin/SolidRpc.OpenApi.AspNetCore.dll""
#r ""../bin/SolidRpc.OpenApi.Binder.dll""

using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using SolidRpc.OpenApi.Binder.Http;
using SolidRpc.OpenApi.AzFunctions;

public static async Task<IActionResult> Run(HttpRequest req, ILogger log)
{
    var rpcSetup = req.HttpContext.RequestServices.GetRequiredService<ISolidRpcSetup>();
    var solidReq = new SolidHttpRequest();
    await solidReq.CopyFromAsync(req);
    var res = await rpcSetup.MethodInvoker.InvokeAsync(solidReq, req.HttpContext.RequestAborted);
    return await res.CreateActionResult();
}
");
        }

        private void WriteFunctionJson(DirectoryInfo funcFolder, IEnumerable<string> methods, string route)
        {
            var fi = new FileInfo(Path.Combine(funcFolder.FullName, "function.json"));
            WriteFile(fi, $@"{{
  ""bindings"": [
    {{
      ""authLevel"": ""function"",
      ""name"": ""req"",
      ""type"": ""httpTrigger"",
      ""direction"": ""in"",
      ""methods"": [{string.Join(",", methods.Select(o => $"\"{o.ToLower()}\""))}],
      ""route"": ""{route}""
    }},
    {{
      ""name"": ""$return"",
      ""type"": ""http"",
      ""direction"": ""out""
    }}
  ]
}}");
        }

        private void WriteFile(FileInfo fi, string newContent)
        {
            if(fi.Exists)
            {
                using (var tr = fi.OpenText())
                {
                    var existingContent = tr.ReadToEnd();
                    if (existingContent.Equals(newContent))
                    {
                        return;
                    }
                }
            }
            using (var tw = fi.CreateText())
            {
                tw.Write(newContent);
            }
        }
    }
}
