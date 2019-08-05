using System.Runtime.Serialization;

namespace SolidRpc.OpenApi.AzFunctions.Functions.Model
{
    /// <summary>
    /// Represents a proxy in the proxies class.
    /// </summary>
    public class AzProxy
    {
        /// <summary>
        /// The match condition
        /// </summary>
        [DataMember(Name = "matchCondition", EmitDefaultValue = false)]
        public AzProxyMatchCondition MatchCondition { get; set; }

        /// <summary>
        /// The backend uri
        /// </summary>
        [DataMember(Name = "backendUri", EmitDefaultValue = false)]
        public string BackendUri { get; set; }
    }
}
