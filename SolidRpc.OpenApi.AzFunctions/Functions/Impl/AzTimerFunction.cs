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
                        Name ="timerId",
                        Type =  "constant",
                        Value = ""
                    }
                },
                ScriptFile = $"../bin/{functionHandler.TimerTriggerHandler.Assembly.GetName().Name}.dll",
                EntryPoint = $"{functionHandler.TimerTriggerHandler.FullName}.Run"
            };
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
        public Task Run(
            [TimerTrigger(""{Schedule}"", RunOnStartup = {RunOnStartup.ToString().ToLower()})] TimerInfo timerInfo,
            CancellationToken cancellationToken)
        {{
            return TimerFunction.Run(timerInfo, _logger, _serviceProvider, ""{ Name }"", cancellationToken);
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
        /// The method
        /// </summary>
        public string TimerId
        {
            get => Function.Bindings.Where(o => o.Name == "timerId").First().Value;
            set => Function.Bindings.Where(o => o.Name == "timerId").First().Value = value;
        }

    }
}