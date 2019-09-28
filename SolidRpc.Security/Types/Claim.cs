using System.Runtime.Serialization;
namespace SolidRpc.Security.Types {
    /// <summary>
    /// 
    /// </summary>
    public class Claim {
        /// <summary>
        /// The name of the claim
        /// </summary>
        [DataMember(Name="name")]
        public string Name { get; set; }
    
        /// <summary>
        /// The value of the claim
        /// </summary>
        [DataMember(Name="value")]
        public string Value { get; set; }
    
    }
}