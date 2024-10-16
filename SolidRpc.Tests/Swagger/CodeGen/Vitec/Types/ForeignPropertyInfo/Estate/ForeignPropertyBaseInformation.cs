using System.CodeDom.Compiler;
using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
namespace SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.ForeignPropertyInfo.Estate {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class ForeignPropertyBaseInformation {
        /// <summary>
        /// Nyproduktion
        /// </summary>
        [DataMember(Name="newConstruction",EmitDefaultValue=false)]
        public bool? NewConstruction { get; set; }
    
        /// <summary>
        /// Boarea
        /// </summary>
        [DataMember(Name="livingSpace",EmitDefaultValue=false)]
        public double? LivingSpace { get; set; }
    
        /// <summary>
        /// Byggnadsyta
        /// </summary>
        [DataMember(Name="buildningArea",EmitDefaultValue=false)]
        public double? BuildningArea { get; set; }
    
        /// <summary>
        /// M&#229;nadsavgift
        /// </summary>
        [DataMember(Name="monthlyFee",EmitDefaultValue=false)]
        public double? MonthlyFee { get; set; }
    
        /// <summary>
        /// Kommentar till m&#229;nadsavgift
        /// </summary>
        [DataMember(Name="commentary",EmitDefaultValue=false)]
        public string Commentary { get; set; }
    
        /// <summary>
        /// &#196;garregister
        /// </summary>
        [DataMember(Name="ownerRegister",EmitDefaultValue=false)]
        public string OwnerRegister { get; set; }
    
        /// <summary>
        /// Externt referensnummer
        /// </summary>
        [DataMember(Name="referenceId",EmitDefaultValue=false)]
        public string ReferenceId { get; set; }
    
        /// <summary>
        /// Uppl&#229;telseform text
        /// </summary>
        [DataMember(Name="tenure",EmitDefaultValue=false)]
        public string Tenure { get; set; }
    
        /// <summary>
        /// Uppl&#229;telseform
        /// </summary>
        [DataMember(Name="disposalForm",EmitDefaultValue=false)]
        public string DisposalForm { get; set; }
    
        /// <summary>
        /// Ut&#246;kade s&#246;kbegrepp
        /// </summary>
        [DataMember(Name="increaseSeekConception",EmitDefaultValue=false)]
        public IEnumerable<string> IncreaseSeekConception { get; set; }
    
        /// <summary>
        /// Adress uppgifter
        /// </summary>
        [DataMember(Name="address",EmitDefaultValue=false)]
        public SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.ForeignPropertyInfo.Estate.Address Address { get; set; }
    
        /// <summary>
        /// Areak&#228;lla
        /// </summary>
        [DataMember(Name="areaSource",EmitDefaultValue=false)]
        public string AreaSource { get; set; }
    
        /// <summary>
        /// &#214;vrigt under allm&#228;nt
        /// </summary>
        [DataMember(Name="other",EmitDefaultValue=false)]
        public string Other { get; set; }
    
        /// <summary>
        /// Projekt id vissas h&#228;r om objektet ing&#229;r i ett projekt
        /// </summary>
        [DataMember(Name="projectId",EmitDefaultValue=false)]
        public string ProjectId { get; set; }
    
        /// <summary>
        /// Visar om det r&#246;r en k&#228;nslig aff&#228;r
        /// </summary>
        [DataMember(Name="sensitiveBusiness",EmitDefaultValue=false)]
        public bool? SensitiveBusiness { get; set; }
    
        /// <summary>
        /// Objekttyp
        /// </summary>
        [DataMember(Name="propertyType",EmitDefaultValue=false)]
        public string PropertyType { get; set; }
    
    }
}