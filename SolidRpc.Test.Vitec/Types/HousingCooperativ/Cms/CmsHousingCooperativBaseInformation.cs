using System.CodeDom.Compiler;
using System.Runtime.Serialization;
using SolidRpc.Test.Vitec.Types.Common.Cms;
namespace SolidRpc.Test.Vitec.Types.HousingCooperativ.Cms {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class CmsHousingCooperativBaseInformation {
        /// <summary>
        /// L&#228;genhetsnummer
        /// </summary>
        [DataMember(Name="apartmentNumber",EmitDefaultValue=false)]
        public string ApartmentNumber { get; set; }
    
        /// <summary>
        /// M&#229;nadsavgift
        /// </summary>
        [DataMember(Name="monthlyFee",EmitDefaultValue=false)]
        public double MonthlyFee { get; set; }
    
        /// <summary>
        /// Boarea
        /// </summary>
        [DataMember(Name="livingSpace",EmitDefaultValue=false)]
        public double LivingSpace { get; set; }
    
        /// <summary>
        /// Biarea
        /// </summary>
        [DataMember(Name="otherSpace",EmitDefaultValue=false)]
        public double OtherSpace { get; set; }
    
        /// <summary>
        /// Portkod
        /// </summary>
        [DataMember(Name="portCode",EmitDefaultValue=false)]
        public string PortCode { get; set; }
    
        /// <summary>
        /// Status
        /// </summary>
        [DataMember(Name="status",EmitDefaultValue=false)]
        public string Status { get; set; }
    
        /// <summary>
        /// Objektets adress
        /// </summary>
        [DataMember(Name="address",EmitDefaultValue=false)]
        public CmsEstateAddress Address { get; set; }
    
    }
}