using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System;
namespace SolidRpc.Test.Vitec.Types.Estate.Models {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class CompilationAreas {
        /// <summary>
        /// Areor
        /// </summary>
        [DataMember(Name="areas",EmitDefaultValue=false)]
        public IEnumerable<SolidRpc.Test.Vitec.Types.Estate.Models.AreaInfo> Areas { get; set; }
    
        /// <summary>
        /// Summa area kvm
        /// </summary>
        [DataMember(Name="area",EmitDefaultValue=false)]
        public double? Area { get; set; }
    
        /// <summary>
        /// Summa hyra kr/&#229;r
        /// </summary>
        [DataMember(Name="rent",EmitDefaultValue=false)]
        public double? Rent { get; set; }
    
        /// <summary>
        /// Summa driftkostnad kr/&#229;r
        /// </summary>
        [DataMember(Name="operatingCosts",EmitDefaultValue=false)]
        public double? OperatingCosts { get; set; }
    
    }
}