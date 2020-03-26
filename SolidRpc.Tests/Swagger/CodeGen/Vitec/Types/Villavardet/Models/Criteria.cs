using System.CodeDom.Compiler;
using System;
using System.Runtime.Serialization;
namespace SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Villavardet.Models {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class Criteria {
        /// <summary>
        /// Tilltr&#228;desdatum fr&#229;n och med
        /// </summary>
        [DataMember(Name="accessDateFrom",EmitDefaultValue=false)]
        public DateTimeOffset? AccessDateFrom { get; set; }
    
        /// <summary>
        /// Tilltr&#228;desdatum till och med
        /// </summary>
        [DataMember(Name="accessDateTo",EmitDefaultValue=false)]
        public DateTimeOffset? AccessDateTo { get; set; }
    
    }
}