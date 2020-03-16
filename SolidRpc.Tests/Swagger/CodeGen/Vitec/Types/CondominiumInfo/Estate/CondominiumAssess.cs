using System.CodeDom.Compiler;
using System;
using System.Runtime.Serialization;
namespace SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.CondominiumInfo.Estate {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class CondominiumAssess {
        /// <summary>
        /// Prelimin&#228;rt taxeringsv&#228;rde
        /// </summary>
        [DataMember(Name="preliminaryAssessedValue",EmitDefaultValue=false)]
        public bool? PreliminaryAssessedValue { get; set; }
    
        /// <summary>
        /// TypeCode
        /// </summary>
        [DataMember(Name="typeCode",EmitDefaultValue=false)]
        public string TypeCode { get; set; }
    
        /// <summary>
        /// Byggnadsv&#228;rde
        /// </summary>
        [DataMember(Name="buildingValue",EmitDefaultValue=false)]
        public double? BuildingValue { get; set; }
    
        /// <summary>
        /// Markv&#228;rde
        /// </summary>
        [DataMember(Name="landValue",EmitDefaultValue=false)]
        public double? LandValue { get; set; }
    
        /// <summary>
        /// V&#228;rde&#229;r
        /// </summary>
        [DataMember(Name="valueYear",EmitDefaultValue=false)]
        public int? ValueYear { get; set; }
    
        /// <summary>
        /// Summa taxeringsv&#228;rde
        /// </summary>
        [DataMember(Name="totalAssessedValue",EmitDefaultValue=false)]
        public double? TotalAssessedValue { get; set; }
    
    }
}