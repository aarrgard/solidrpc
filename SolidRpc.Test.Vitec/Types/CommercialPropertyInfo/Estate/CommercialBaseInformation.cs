using System.CodeDom.Compiler;
using System.Runtime.Serialization;
using SolidRpc.Test.Vitec.Types.Common.Estate;
namespace SolidRpc.Test.Vitec.Types.CommercialPropertyInfo.Estate {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class CommercialBaseInformation {
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
        /// Uppl&#229;telseform
        /// </summary>
        [DataMember(Name="disposalForm",EmitDefaultValue=false)]
        public string DisposalForm { get; set; }
    
        /// <summary>
        /// Adress uppgifter
        /// </summary>
        [DataMember(Name="objectAddress",EmitDefaultValue=false)]
        public ObjectAddress ObjectAddress { get; set; }
    
        /// <summary>
        /// &#214;vrigt under allm&#228;nt
        /// </summary>
        [DataMember(Name="other",EmitDefaultValue=false)]
        public string Other { get; set; }
    
        /// <summary>
        /// Objekttyp
        /// </summary>
        [DataMember(Name="propertyType",EmitDefaultValue=false)]
        public string PropertyType { get; set; }
    
        /// <summary>
        /// Fastighetsbeteckning
        /// </summary>
        [DataMember(Name="propertyUnitDesignation",EmitDefaultValue=false)]
        public string PropertyUnitDesignation { get; set; }
    
        /// <summary>
        /// Visar om det r&#246;r en k&#228;nslig aff&#228;r
        /// </summary>
        [DataMember(Name="sensitiveBusiness",EmitDefaultValue=false)]
        public bool SensitiveBusiness { get; set; }
    
        /// <summary>
        /// Nyckelnummer
        /// </summary>
        [DataMember(Name="keyNumber",EmitDefaultValue=false)]
        public string KeyNumber { get; set; }
    
    }
}