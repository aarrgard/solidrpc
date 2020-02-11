using System.CodeDom.Compiler;
using System.Runtime.Serialization;
using System.Collections.Generic;
using SolidRpc.Test.Vitec.Types.Common.Estate;
namespace SolidRpc.Test.Vitec.Types.HousingCooperativeInfo.Estate {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class HousingCooperativeBaseInformation {
        /// <summary>
        /// Nyproduktion
        /// </summary>
        [DataMember(Name="newConstruction",EmitDefaultValue=false)]
        public bool NewConstruction { get; set; }
    
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
        /// L&#228;genhetsnummer
        /// </summary>
        [DataMember(Name="apartmentNumber",EmitDefaultValue=false)]
        public string ApartmentNumber { get; set; }
    
        /// <summary>
        /// L&#228;genhetsnummerRegister
        /// </summary>
        [DataMember(Name="apartmentNumberRegistration",EmitDefaultValue=false)]
        public string ApartmentNumberRegistration { get; set; }
    
        /// <summary>
        /// M&#229;nadsavgift
        /// </summary>
        [DataMember(Name="monthlyFee",EmitDefaultValue=false)]
        public double MonthlyFee { get; set; }
    
        /// <summary>
        /// Ing&#229;r i m&#229;nadsavgift
        /// </summary>
        [DataMember(Name="includedInFee",EmitDefaultValue=false)]
        public SolidRpc.Test.Vitec.Types.IncludedInFee IncludedInFee { get; set; }
    
        /// <summary>
        /// Kommentar till m&#229;nadsavgift
        /// </summary>
        [DataMember(Name="commentary",EmitDefaultValue=false)]
        public string Commentary { get; set; }
    
        /// <summary>
        /// Uppl&#229;telseform
        /// </summary>
        [DataMember(Name="disposalForm",EmitDefaultValue=false)]
        public string DisposalForm { get; set; }
    
        /// <summary>
        /// Uppl&#229;telseform text
        /// </summary>
        [DataMember(Name="tenure",EmitDefaultValue=false)]
        public string Tenure { get; set; }
    
        /// <summary>
        /// Ut&#246;kade s&#246;kbegrepp
        /// </summary>
        [DataMember(Name="increaseSeekConception",EmitDefaultValue=false)]
        public IEnumerable<string> IncreaseSeekConception { get; set; }
    
        /// <summary>
        /// Adress uppgifter
        /// </summary>
        [DataMember(Name="objectAddress",EmitDefaultValue=false)]
        public ObjectAddress ObjectAddress { get; set; }
    
        /// <summary>
        /// Areak&#228;lla
        /// </summary>
        [DataMember(Name="areaSource",EmitDefaultValue=false)]
        public string AreaSource { get; set; }
    
        /// <summary>
        /// Kommentar f&#246;r areak&#228;lla
        /// </summary>
        [DataMember(Name="areaSourceComment",EmitDefaultValue=false)]
        public string AreaSourceComment { get; set; }
    
        /// <summary>
        /// &#214;vrigt under allm&#228;nt
        /// </summary>
        [DataMember(Name="other",EmitDefaultValue=false)]
        public string Other { get; set; }
    
        /// <summary>
        /// Projekt id visas h&#228;r om objektet ing&#229;r i ett projekt
        /// </summary>
        [DataMember(Name="projectId",EmitDefaultValue=false)]
        public string ProjectId { get; set; }
    
        /// <summary>
        /// Visar om det r&#246;r en k&#228;nslig aff&#228;r
        /// </summary>
        [DataMember(Name="sensitiveBusiness",EmitDefaultValue=false)]
        public bool SensitiveBusiness { get; set; }
    
        /// <summary>
        /// Bostadstyp
        /// </summary>
        [DataMember(Name="propertyType",EmitDefaultValue=false)]
        public string PropertyType { get; set; }
    
    }
}