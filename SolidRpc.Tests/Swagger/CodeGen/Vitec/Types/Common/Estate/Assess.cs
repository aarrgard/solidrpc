using System.CodeDom.Compiler;
using System.Runtime.Serialization;
namespace SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Common.Estate {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class Assess {
        /// <summary>
        /// Taxerings�r
        /// </summary>
        [DataMember(Name="taxAssessmentYear",EmitDefaultValue=false)]
        public int TaxAssessmentYear { get; set; }
    
        /// <summary>
        /// Prelimin�rt taxeringsv�rde
        /// </summary>
        [DataMember(Name="preliminaryAssessedValue",EmitDefaultValue=false)]
        public bool PreliminaryAssessedValue { get; set; }
    
        /// <summary>
        /// Typkod
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
        public double ValueYear { get; set; }
    
        /// <summary>
        /// Summa taxeringsv�rde
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