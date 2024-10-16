using System.CodeDom.Compiler;
using System.Runtime.Serialization;
using System;
namespace SolidRpc.Test.Vitec.Types.CommercialPropertyInfo.Estate {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class CompilationAreaOther {
        /// <summary>
        /// Antal
        /// </summary>
        [DataMember(Name="name",EmitDefaultValue=false)]
        public string Name { get; set; }
    
        /// <summary>
        /// Antal
        /// </summary>
        [DataMember(Name="number",EmitDefaultValue=false)]
        public double? Number { get; set; }
    
        /// <summary>
        /// Area kvm
        /// </summary>
        [DataMember(Name="area",EmitDefaultValue=false)]
        public double? Area { get; set; }
    
        /// <summary>
        /// Hyresint&#228;kt kr/&#229;r
        /// </summary>
        [DataMember(Name="rentalIncome",EmitDefaultValue=false)]
        public double? RentalIncome { get; set; }
    
        /// <summary>
        /// Hyresint&#228;kt kr/kvm och &#229;r
        /// </summary>
        [DataMember(Name="pricePerSquareMeter",EmitDefaultValue=false)]
        public double? PricePerSquareMeter { get; set; }
    
        /// <summary>
        /// &#214;vriga int&#228;kter kr/&#229;r
        /// </summary>
        [DataMember(Name="otherIncome",EmitDefaultValue=false)]
        public double? OtherIncome { get; set; }
    
        /// <summary>
        /// &#214;vriga int&#228;kter kr/kvm och &#229;r
        /// </summary>
        [DataMember(Name="otherIncomePerSquareMeter",EmitDefaultValue=false)]
        public double? OtherIncomePerSquareMeter { get; set; }
    
        /// <summary>
        /// Schablon driftkostnader (kr/&#229;r)
        /// </summary>
        [DataMember(Name="flatOperatingCost",EmitDefaultValue=false)]
        public double? FlatOperatingCost { get; set; }
    
        /// <summary>
        /// Schablon kr/kvm
        /// </summary>
        [DataMember(Name="flatOperatingCostSquareMeter",EmitDefaultValue=false)]
        public double? FlatOperatingCostSquareMeter { get; set; }
    
    }
}