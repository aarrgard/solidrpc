using System.CodeDom.Compiler;
using System.Runtime.Serialization;
using SolidRpc.Test.Vitec.Types.Common.Estate;
namespace SolidRpc.Test.Vitec.Types.Estate.Models {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class ProjectBaseInformation {
        /// <summary>
        /// Namn p� projektet
        /// </summary>
        [DataMember(Name="projectName",EmitDefaultValue=false)]
        public string ProjectName { get; set; }
    
        /// <summary>
        /// Address f�r projektet
        /// </summary>
        [DataMember(Name="address",EmitDefaultValue=false)]
        public ObjectAddress Address { get; set; }
    
    }
}