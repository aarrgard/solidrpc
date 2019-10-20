using System.CodeDom.Compiler;
using System.Runtime.Serialization;
namespace SolidRpc.Test.Vitec.Types.BusinessIntelligence.Models {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class MonthStatisticsSummary {
        /// <summary>
        /// Aktuell m�nad
        /// </summary>
        [DataMember(Name="month",EmitDefaultValue=false)]
        public int Month { get; set; }
    
        /// <summary>
        /// Provision
        /// </summary>
        [DataMember(Name="commission",EmitDefaultValue=false)]
        public double Commission { get; set; }
    
        /// <summary>
        /// Summa f�rs�ljning
        /// </summary>
        [DataMember(Name="totalSales",EmitDefaultValue=false)]
        public double TotalSales { get; set; }
    
        /// <summary>
        /// Provision i procent av f�rs�ljning
        /// </summary>
        [DataMember(Name="commissionPercentage",EmitDefaultValue=false)]
        public double CommissionPercentage { get; set; }
    
        /// <summary>
        /// Medelprovision
        /// </summary>
        [DataMember(Name="averageCommission",EmitDefaultValue=false)]
        public double AverageCommission { get; set; }
    
        /// <summary>
        /// Antal intagsf�rs�k
        /// </summary>
        [DataMember(Name="attemptedIntakes",EmitDefaultValue=false)]
        public int AttemptedIntakes { get; set; }
    
        /// <summary>
        /// Antal s�lda bost�der
        /// </summary>
        [DataMember(Name="sold",EmitDefaultValue=false)]
        public int Sold { get; set; }
    
    }
}