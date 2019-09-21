using System.Runtime.Serialization;
namespace SolidRpc.Security.Types {
    /// <summary>
    /// 
    /// </summary>
    public class Claim {
        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name="name")]
        public string Name { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name="value")]
        public string Value { get; set; }
    
    }
}