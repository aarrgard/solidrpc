using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace SolidRpc.NpmGenerator.Debugger.Events.Runtime
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
            [JsonProperty("id")]
            public int Id { get; set; }

            /// <summary>
            /// Execution context origin.
            /// </summary>
            [JsonProperty("origin")]
            public string Origin { get; set; }

            /// <summary>
            /// Human readable name describing given context.
            /// </summary>
            [JsonProperty("name")]
            public string Name { get; set; }

            /// <summary>
            /// Unique id of the execution context. It can be used to specify in which execution context script evaluation should be performed.
            /// </summary>
            [JsonProperty("auxData")]
            public object AuxData { get; set; }
        }

        /// <summary>
        /// A newly created execution contex.
        /// </summary>
        [JsonProperty("context")]
        public ExecutionContextDescription Context { get; set; }
    }
}
