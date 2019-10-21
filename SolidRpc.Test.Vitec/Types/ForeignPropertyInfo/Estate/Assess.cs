using System.CodeDom.Compiler;
using System.Runtime.Serialization;
namespace SolidRpc.Test.Vitec.Types.ForeignPropertyInfo.Estate {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class Assess {
        /// <summary>
        /// Taxerings&#229;r
        /// </summary>
        [DataMember(Name="taxAssessmentYear",EmitDefaultValue=false)]
        public int TaxAssessmentYear { get; set; }
    
        /// <summary>
        /// Prelimin&#228;rt taxeringsv&#228;rde
        /// </summary>
        [DataMember(Name="preliminaryAssessedValue",EmitDefaultValue=false)]
        public bool PreliminaryAssessedValue { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name="assessmentDirectory",EmitDefaultValue=false)]
        public string AssessmentDirectory { get; set; }
    
        /// <summary>
        /// Byggnadsv&#228;rde
        /// </summary>
        [DataMember(Name="buildingValue",EmitDefaultValue=false)]
        public double BuildingValue { get; set; }
    
        /// <summary>
        /// Markv&#228;rde
        /// </summary>
        [DataMember(Name="landValue",EmitDefaultValue=false)]
        public double LandValue { get; set; }
    
        /// <summary>
        /// Summa taxeringsv&#228;rde
        /// </summary>
        [DataMember(Name="totalAssessedValue",EmitDefaultValue=false)]
        public double TotalAssessedValue { get; set; }
    
        /// <summary>
        /// Skatt/avgift
        /// </summary>
        [DataMember(Name="taxFee",EmitDefaultValue=false)]
        public double TaxFee { get; set; }
    
    }
}