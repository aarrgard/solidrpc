using SolidRpc.OpenApi.AzFunctions.Functions.Model;
using System;
using System.IO;
using System.Linq;

namespace SolidRpc.OpenApi.AzFunctions.Functions.Impl
{
    /// <summary>
    /// Represents a timer function
    /// </summary>
    public class AzTimerFunction : AzFunction, IAzTimerFunction
    {

        private static Function DefaultFunction(IAzFunctionHandler functionHandler)
        {
            return new Function()
            {
                GeneratedBy = $"{typeof(AzTimerFunction).Assembly.GetName().Name}-{typeof(AzTimerFunction).Assembly.GetName().Version}",
                //ConfigurationSource = "attributes",
                Disabled = false,
                Bindings = new Binding[]
                {
                    new Binding()
                    {
                        Name ="myTimer",
                        Type =  "timerTrigger",
                        Direction = "in",
                        Schedule = "0 0 0 1 1 0"
                    }, new Binding()
                    {
                        Name ="serviceProvider",
                        Type =  "inject"
                    }, new Binding()
                    {
                        Name ="serviceType",
                        Type =  "constant",
                        Value = ""
                    }, new Binding()
                    {
                        Name ="methodName",
                        Type =  "constant",
                        Value = ""
                    }
                },
                ScriptFile = $"../bin/{functionHandler.TimerTriggerHandler.Assembly.GetName().Name}.dll",
                EntryPoint = $"{functionHandler.TimerTriggerHandler.FullName}.Run"
            };
        }

        protected override string WriteFunctionClass()
        {
            return $@"
    public class {Name}
    {{
        [FunctionName(""{Name}"")]
        public static Task Run(
            [TimerTrigger(""{Schedule}"", RunOnStartup = {RunOnStartup.ToString().ToLower()})] TimerInfo timerInfo,
            [Inject] IServiceProvider serviceProvider,
            [Constant(""{ServiceType}"")] Type serviceType,
            [Constant(""{MethodName}"")] string methodName,
            ILogger log,
            CancellationToken cancellationToken)
        {{
            return TimerFunction.Run(timerInfo, log, serviceProvider, serviceType, methodName, cancellationToken);
        }}
    }}
";
        }


        /// <summary>
        /// Constructs a new timer function.
        /// </summary>
        /// <param name="functionHandler"></param>
        /// <param name="subDir"></param>
        public AzTimerFunction(IAzFunctionHandler functionHandler, string subDir) : base(functionHandler, subDir, DefaultFunction(functionHandler))
        {
        }

        /// <summary>
        /// Constructs a new timer function
        /// </summary>
        /// <param name="functionHandler"></param>
        /// <param name="subDir"></param>
        /// <param name="functionJson"></param>
        public AzTimerFunction(IAzFunctionHandler functionHandler, string subDir, Function functionJson) : base(functionHandler, subDir, functionJson)
        {
        }

        /// <summary>
        /// The trigger type
        /// </summary>
        public override string TriggerType => "timerTrigger";

        /// <summary>
        /// The schedule
        /// </summary>
        public string Schedule
        {
            get => TriggerBinding.Schedule;
            set => TriggerBinding.Schedule = value;
        }

        /// <summary>
        /// Should the trigger run on startup
        /// </summary>
        public bool RunOnStartup
        {
            get => TriggerBinding.RunOnStartup;
            set => TriggerBinding.RunOnStartup = value;
        }

        /// <summary>
        /// The service type
        /// </summary>
        public string ServiceType
        {
            get => Function.Bindings.Where(o => o.Name == "serviceType").First().Value;
            set => Function.Bindings.Where(o => o.Name == "serviceType").First().Value = value;
        }

        /// <summary>
        /// The method
        /// </summary>
        public string MethodName
        {
            get => Function.Bindings.Where(o => o.Name == "methodName").First().Value;
            set => Function.Bindings.Where(o => o.Name == "methodName").First().Value = value;
        }

    }
}