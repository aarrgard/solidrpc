using System.CodeDom.Compiler;
using System;
using System.Runtime.Serialization;
namespace SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Meeting.Models {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class AssignmentSource {
        /// <summary>
        /// Intagsk&#228;lla id
        /// </summary>
        [DataMember(Name="id",EmitDefaultValue=false)]
        public int? Id { get; set; }
    
        /// <summary>
        /// Namn p&#229; intagsk&#228;llan
        /// </summary>
        [DataMember(Name="name",EmitDefaultValue=false)]
        public string Name { get; set; }
    
    }
}