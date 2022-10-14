﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using SolidRpc.OpenApi.AzFunctions.Functions.Model;

namespace SolidRpc.OpenApi.AzFunctions.Functions.Impl
{

    /// <summary>
    /// Implements an azure function.
    /// </summary>
    public class AzHttpFunction : AzFunction, IAzHttpFunction
    {

        private static Function DefaultFunction(IAzFunctionHandler functionHandler)
        {
            return new Function()
            {
                GeneratedBy = $"{typeof(AzTimerFunction).Assembly.GetName().Name}-{typeof(AzTimerFunction).Assembly.GetName().Version}",
                Disabled = false,
                Bindings = new Binding[]
                {
                    new Binding()
                    {
                        AuthLevel = "function",
                        Name ="req",
                        Type =  "httpTrigger",
                        Direction = "in",
                        Methods = new [] { "get" }
                    }, new Binding()
                    {
                        Name ="serviceProvider",
                        Type =  "inject",
                        Direction = "in"
                    }, new Binding()
                    {
                        Name ="$return",
                        Type =  "http",
                        Direction = "out"
                    }
                },
                ScriptFile = $"../bin/{functionHandler.HttpTriggerHandler.Assembly.GetName().Name}.dll",
                EntryPoint = $"{functionHandler.HttpTriggerHandler.FullName}.Run"
            };
        }

        /// <summary>
        /// Constructs a new instance
        /// </summary>
        /// <param name="functionHandler"></param>
        /// <param name="subDir"></param>
        public AzHttpFunction(IAzFunctionHandler functionHandler, string subDir) : base(functionHandler, subDir, DefaultFunction(functionHandler))
        {
        }

        /// <summary>
        /// Constructs a new instance
        /// </summary>
        /// <param name="functionHandler"></param>
        /// <param name="subDir"></param>
        /// <param name="functionJson"></param>
        public AzHttpFunction(IAzFunctionHandler functionHandler, string subDir, Function functionJson) : base(functionHandler, subDir, functionJson)
        {
        }

        /// <summary>
        /// Writes the function class
        /// </summary>
        /// <returns></returns>
        protected override string WriteFunctionClass()
        {
            return $@"
    public class {Name}
    {{
        private ILogger _logger;
        private IServiceProvider _serviceProvider;
        public {Name}(ILogger<{Name}> logger, IServiceProvider serviceProvider) {{
            _logger = logger;
            _serviceProvider = serviceProvider;
        }}
        [FunctionName(""{Name}"")]
        public async Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.{Char.ToUpper(AuthLevel[0]) + AuthLevel.Substring(1)}, {string.Join(", ", Methods.Select(o => $"\"{o}\""))}, Route = ""{Route}"")] HttpRequestMessage req,
            CancellationToken cancellationToken)
        {{
            var res = await HttpFunction.Run(req, _logger, _serviceProvider, cancellationToken);
            return res;
        }}
    }}
";
        }

        /// <summary>
        /// The trigger type.
        /// </summary>
        public override string TriggerType => "httpTrigger";

        /// <summary>
        /// The route
        /// </summary>
        public string Route
        {
            get => TriggerBinding.Route ?? Name;
            set => TriggerBinding.Route = value;
        }

        /// <summary>
        /// The methods
        /// </summary>
        public IEnumerable<string> Methods
        {
            get => TriggerBinding.Methods;
            set => TriggerBinding.Methods = value;
        }

        /// <summary>
        /// The AuthLevel
        /// </summary>
        public string AuthLevel
        {
            get => TriggerBinding.AuthLevel;
            set => TriggerBinding.AuthLevel = value;
        }

    }
}