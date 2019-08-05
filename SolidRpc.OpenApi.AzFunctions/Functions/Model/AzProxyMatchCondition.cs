using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SolidRpc.OpenApi.AzFunctions.Functions.Model
{
    /// <summary>
    /// A proxy match condition
    /// </summary>
    public class AzProxyMatchCondition
    {
        /// <summary>
        /// The route
        /// </summary>
        [DataMember(Name = "route", EmitDefaultValue = false)]
        public string Route { get; set; }

        /// <summary>
        /// The route
        /// </summary>
        [DataMember(Name = "methods", EmitDefaultValue = false)]
        public IEnumerable<string> Methods { get; set; }
    }
}