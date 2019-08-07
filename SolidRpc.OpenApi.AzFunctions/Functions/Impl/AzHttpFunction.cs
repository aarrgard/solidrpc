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
        /// <param name="functionName"></param>
        public AzHttpFunction(IAzFunctionHandler functionHandler, string functionName) : base(functionHandler, functionName, DefaultFunction(functionHandler))
        {
        }

        /// <summary>
        /// Constructs a new instance
        /// </summary>
        /// <param name="functionHandler"></param>
        /// <param name="functionName"></param>
        /// <param name="functionJson"></param>
        public AzHttpFunction(IAzFunctionHandler functionHandler, string functionName, Function functionJson) : base(functionHandler, functionName, functionJson)
        {
        }

        private string SetRouteArgs(string route)
        {
            var args = GetRouteArgs(ref route);
            return route;
        }

        private IEnumerable<string> GetRouteArgs(ref string route)
        {
            var sbRoute = new StringBuilder();
            var sbArg = new StringBuilder();
            var args = new List<string>();
           
            foreach(var c in route)
            {
                switch(c)
                {
                    case '{':
                        sbArg.Clear();
                        break;
                    case '}':
                        var arg = sbArg.ToString();
                        if(string.IsNullOrEmpty(arg))
                        {
                            arg = $"arg{args.Count}";
                            sbRoute.Append(arg);
                        }
                        args.Add(arg);
                        break;
                    default:
                        sbArg.Append(c);
                        break;

                }
                sbRoute.Append(c);
            }

            route = sbRoute.ToString();
            return args;
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
            set => TriggerBinding.Route = SetRouteArgs(value);
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