using System.CodeDom.Compiler;
using System.Runtime.Serialization;
namespace SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Estate.Models {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class AreaInfo {
        /// <summary>
        /// Typ
        /// </summary>
        [DataMember(Name="type",EmitDefaultValue=false)]
        public string Type { get; set; }
    
        /// <summary>
        /// Yta
        /// </summary>
        [DataMember(Name="area",EmitDefaultValue=false)]
        public double Area { get; set; }
    
        /// <summary>
        /// Hyra kr/�r
        /// </summary>
        [DataMember(Name="rentalIncome",EmitDefaultValue=false)]
        public double RentalIncome { get; set; }
    
        /// <summary>
        /// Driftkostnad
        /// </summary>
        [DataMember(Name="operatingCost",EmitDefaultValue=false)]
        public string OperatingCost { get; set; }
    
        /// <summary>
        /// Hyra kr/kvm/�r
        /// </summary>
        [DataMember(Name="rentalIncomePerSqm",EmitDefaultValue=false)]
        public double RentalIncomePerSqm { get; set; }
    
    }
}