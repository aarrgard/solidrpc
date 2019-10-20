using System.CodeDom.Compiler;
using System.Runtime.Serialization;
namespace SolidRpc.Test.Vitec.Types.CondominiumInfo.Estate {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class CondominiumAssess {
        /// <summary>
        /// Prelimin�rt taxeringsv�rde
        /// </summary>
        [DataMember(Name="preliminaryAssessedValue",EmitDefaultValue=false)]
        public bool PreliminaryAssessedValue { get; set; }
    
        /// <summary>
        /// TypeCode
        /// </summary>
        [DataMember(Name="typeCode",EmitDefaultValue=false)]
        public string TypeCode { get; set; }
    
        /// <summary>
        /// Byggnadsv�rde
        /// </summary>
        [DataMember(Name="buildingValue",EmitDefaultValue=false)]
        public double BuildingValue { get; set; }
    
        /// <summary>
        /// Markv�rde
        /// </summary>
        [DataMember(Name="landValue",EmitDefaultValue=false)]
        public double LandValue { get; set; }
    
        /// <summary>
        /// V�rde�r
        /// </summary>
        [DataMember(Name="valueYear",EmitDefaultValue=false)]
        public int ValueYear { get; set; }
    
        /// <summary>
        /// Summa taxeringsv�rde
        /// </summary>
        [DataMember(Name="totalAssessedValue",EmitDefaultValue=false)]
        public double TotalAssessedValue { get; set; }
    
    }
}