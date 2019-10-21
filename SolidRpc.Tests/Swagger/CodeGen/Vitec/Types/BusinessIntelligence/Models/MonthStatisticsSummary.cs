using System.CodeDom.Compiler;
using System.Runtime.Serialization;
namespace SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.BusinessIntelligence.Models {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class MonthStatisticsSummary {
        /// <summary>
        /// Aktuell m&#229;nad
        /// </summary>
        [DataMember(Name="month",EmitDefaultValue=false)]
        public int Month { get; set; }
    
        /// <summary>
        /// Provision
        /// </summary>
        [DataMember(Name="commission",EmitDefaultValue=false)]
        public double Commission { get; set; }
    
        /// <summary>
        /// Summa f&#246;rs&#228;ljning
        /// </summary>
        [DataMember(Name="totalSales",EmitDefaultValue=false)]
        public double TotalSales { get; set; }
    
        /// <summary>
        /// Provision i procent av f&#246;rs&#228;ljning
        /// </summary>
        [DataMember(Name="commissionPercentage",EmitDefaultValue=false)]
        public double CommissionPercentage { get; set; }
    
        /// <summary>
        /// Medelprovision
        /// </summary>
        [DataMember(Name="averageCommission",EmitDefaultValue=false)]
        public double AverageCommission { get; set; }
    
        /// <summary>
        /// Antal intagsf&#246;rs&#246;k
        /// </summary>
        [DataMember(Name="attemptedIntakes",EmitDefaultValue=false)]
        public int AttemptedIntakes { get; set; }
    
        /// <summary>
        /// Antal s&#229;lda bost&#228;der
        /// </summary>
        [DataMember(Name="sold",EmitDefaultValue=false)]
        public int Sold { get; set; }
    
    }
}