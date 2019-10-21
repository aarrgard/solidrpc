using System.CodeDom.Compiler;
using System.Runtime.Serialization;
using SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Common.Estate;
namespace SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Estate.Models {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class ProjectBaseInformation {
        /// <summary>
        /// Namn p&#229; projektet
        /// </summary>
        [DataMember(Name="projectName",EmitDefaultValue=false)]
        public string ProjectName { get; set; }
    
        /// <summary>
        /// Address f&#246;r projektet
        /// </summary>
        [DataMember(Name="address",EmitDefaultValue=false)]
        public ObjectAddress Address { get; set; }
    
    }
}