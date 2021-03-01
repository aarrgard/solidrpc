using System.Runtime.Serialization;

namespace SolidRpc.Node.Debugger.Events.Runtime
{
    /// <summary>
    /// Represents an execution context created event
    /// </summary>
    public class ExecutionContextCreated
    {
        public class ExecutionContextDescription
        {
            /// <summary>
            /// Unique id of the execution context. It can be used to specify in which execution context script evaluation should be performed.
            /// </summary>
            [DataMember(Name = "id")]
            public int Id { get; set; }

            /// <summary>
            /// Execution context origin.
            /// </summary>
            [DataMember(Name = "origin")]
            public string Origin { get; set; }

            /// <summary>
            /// Human readable name describing given context.
            /// </summary>
            [DataMember(Name = "name")]
            public string Name { get; set; }

            /// <summary>
            /// Unique id of the execution context. It can be used to specify in which execution context script evaluation should be performed.
            /// </summary>
            [DataMember(Name = "auxData")]
            public object AuxData { get; set; }
        }

        /// <summary>
        /// A newly created execution contex.
        /// </summary>
        [DataMember(Name = "context")]
        public ExecutionContextDescription Context { get; set; }
    }
}
