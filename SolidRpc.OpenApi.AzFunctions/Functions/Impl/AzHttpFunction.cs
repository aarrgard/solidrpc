using System.Collections.Generic;
using System.IO;
using SolidRpc.OpenApi.AzFunctions.Functions.Model;

namespace SolidRpc.OpenApi.AzFunctions.Functions.Impl
{

    /// <summary>
    /// Implements an azure function.
    /// </summary>
    public class AzHttpFunction : AzFunction, IAzHttpFunction
    {

        private static Function DefaultFunction()
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
                        Name ="methodInvoker",
                        Type =  "inject"
                    }, new Binding()
                    {
                        Name ="$return",
                        Type =  "http",
                        Direction = "out"
                    }
                },
                ScriptFile = $"../bin/{typeof(HttpFunction).Assembly.GetName().Name}.dll",
                EntryPoint = $"{typeof(HttpFunction).FullName}.Run"
            };
        }

        /// <summary>
        /// Constructs a new instance
        /// </summary>
        /// <param name="dir"></param>
        public AzHttpFunction(DirectoryInfo dir) : base(dir, DefaultFunction())
        {
        }

        /// <summary>
        /// Constructs a new instance
        /// </summary>
        /// <param name="dir"></param>
        /// <param name="functionJson"></param>
        public AzHttpFunction(DirectoryInfo dir, Function functionJson) : base(dir, functionJson)
        {
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
            get => TriggerBinding.Route;
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

    }
}