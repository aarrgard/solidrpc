using SolidRpc.OpenApi.AzFunctions.Functions.Model;

namespace SolidRpc.OpenApi.AzFunctions.Functions.Impl
{
    /// <summary>
    /// Represents a timer function
    /// </summary>
    public class AzQueueFunction : AzFunction, IAzQueueFunction
    {

        private static Function DefaultFunction(IAzFunctionHandler functionHandler)
        {
            return new Function()
            {
                GeneratedBy = $"{typeof(AzQueueFunction).Assembly.GetName().Name}-{typeof(AzQueueFunction).Assembly.GetName().Version}",
                //ConfigurationSource = "attributes",
                Disabled = false,
                Bindings = new Binding[]
                {
                    new Binding()
                    {
                        Name ="message",
                        Type =  "queueTrigger",
                        Direction = "in",
                        QueueName = "myQueue",
                        Connection = "myConnection"
                    }, new Binding()
                    {
                        Name ="serviceProvider",
                        Type =  "inject"
                    }
                },
                ScriptFile = $"../bin/{functionHandler.QueueTriggerHandler.Assembly.GetName().Name}.dll",
                EntryPoint = $"{functionHandler.QueueTriggerHandler.FullName}.Run"
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
            [QueueTrigger(""{QueueName}"", Connection = ""{Connection}"")] string message,
            string id,
            CancellationToken cancellationToken)
        {{
            return QueueFunction.Run(message, id, _logger, _serviceProvider, cancellationToken);
        }}
    }}
";
        }

        /// <summary>
        /// Constructs a new quue function.
        /// </summary>
        /// <param name="functionHandler"></param>
        /// <param name="subDir"></param>
        public AzQueueFunction(IAzFunctionHandler functionHandler, string subDir) : base(functionHandler, subDir, DefaultFunction(functionHandler))
        {
        }

        /// <summary>
        /// Constructs a new timer function
        /// </summary>
        /// <param name="functionHandler"></param>
        /// <param name="subDir"></param>
        /// <param name="functionJson"></param>
        public AzQueueFunction(IAzFunctionHandler functionHandler, string subDir, Function functionJson) : base(functionHandler, subDir, functionJson)
        {
        }

        /// <summary>
        /// The trigger type
        /// </summary>
        public override string TriggerType => "queueTrigger";


        /// <summary>
        /// The queue name
        /// </summary>
        public string QueueName
        {
            get => TriggerBinding.QueueName;
            set => TriggerBinding.QueueName = value;
        }

        /// <summary>
        /// The connection
        /// </summary>
        public string Connection
        {
            get => TriggerBinding.Connection;
            set => TriggerBinding.Connection = value;
        }

    }
}