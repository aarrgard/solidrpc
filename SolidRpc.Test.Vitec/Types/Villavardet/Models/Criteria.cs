using System.CodeDom.Compiler;
using System;
using System.Runtime.Serialization;
namespace SolidRpc.Test.Vitec.Types.Villavardet.Models {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class Criteria {
        /// <summary>
        /// Tilltr�desdatum fr�n och med
        /// </summary>
        [DataMember(Name="accessDateFrom",EmitDefaultValue=false)]
        public DateTimeOffset AccessDateFrom { get; set; }
    
        /// <summary>
        /// Tilltr�desdatum till och med
        /// </summary>
        [DataMember(Name="accessDateTo",EmitDefaultValue=false)]
        public DateTimeOffset AccessDateTo { get; set; }
    
    }
}