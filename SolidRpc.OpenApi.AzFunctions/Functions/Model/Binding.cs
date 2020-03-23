using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SolidRpc.OpenApi.AzFunctions.Functions.Model
{
    /// <summary>
    /// Represents the bindings.
    /// </summary>
    public class Binding
    {
        /// <summary>
        /// The name
        /// </summary>
        [DataMember(Name = "name", EmitDefaultValue = false)]
        public string Name { get; set; }

        /// <summary>
        /// The direction
        /// </summary>
        [DataMember(Name = "direction", EmitDefaultValue = false)]
        public string Direction { get; set; }

        /// <summary>
        /// The binding type
        /// </summary>
        [DataMember(Name = "type", EmitDefaultValue = false)]
        public string Type { get; set; }

        #region HttpTrigger

        /// <summary>
        /// The authorization level
        /// </summary>
        [DataMember(Name = "authLevel", EmitDefaultValue = false)]
        public string AuthLevel { get; set; }

        /// <summary>
        /// The methods
        /// </summary>
        [DataMember(Name = "methods", EmitDefaultValue=false)]
        public IEnumerable<string> Methods { get; set; }

        /// <summary>
        /// The methods
        /// </summary>
        [DataMember(Name = "route", EmitDefaultValue = false)]
        public string Route { get; set; }

        #endregion
        #region TimerTrigger
        /// <summary>
        /// The timer shedule
        /// </summary>
        [DataMember(Name = "schedule", EmitDefaultValue = false)]
        public string Schedule { get; set; }

        /// <summary>
        /// Should the trigger run on startup.
        /// </summary>
        [DataMember(Name = "runOnStartup", EmitDefaultValue = false)]
        public bool RunOnStartup { get; set; }
        #endregion
        #region QueueTrigger
        /// <summary>
        /// The queue name
        /// </summary>
        [DataMember(Name = "queueName", EmitDefaultValue = false)]
        public string QueueName { get; set; }

        /// <summary>
        /// The connection name
        /// </summary>
        [DataMember(Name = "connection", EmitDefaultValue = false)]
        public string Connection { get; set; }
#endregion
        #region ConstantBinder
        /// <summary>
        /// The value for the constant
        /// </summary>
        [DataMember(Name = "value", EmitDefaultValue = false)]
        public string Value { get; set; }
        #endregion
    }
}