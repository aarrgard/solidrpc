using System.CodeDom.Compiler;
using System.Runtime.Serialization;
namespace SolidRpc.Test.Vitec.Types.BusinessIntelligense.Models {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class Contact {
        /// <summary>
        /// Id
        /// </summary>
        [DataMember(Name="id",EmitDefaultValue=false)]
        public string Id { get; set; }
    
        /// <summary>
        /// F&#246;rnamn
        /// </summary>
        [DataMember(Name="firstName",EmitDefaultValue=false)]
        public string FirstName { get; set; }
    
        /// <summary>
        /// Efternamn
        /// </summary>
        [DataMember(Name="lastName",EmitDefaultValue=false)]
        public string LastName { get; set; }
    
    }
}