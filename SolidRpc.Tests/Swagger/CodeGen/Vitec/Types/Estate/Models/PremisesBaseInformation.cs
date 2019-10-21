using System.CodeDom.Compiler;
using System.Runtime.Serialization;
using SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Common.Estate;
namespace SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Estate.Models {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class PremisesBaseInformation {
        /// <summary>
        /// Inriktning
        /// </summary>
        [DataMember(Name="alignment",EmitDefaultValue=false)]
        public string Alignment { get; set; }
    
        /// <summary>
        /// Uppl&#229;telseform
        /// </summary>
        [DataMember(Name="disposalForm",EmitDefaultValue=false)]
        public string DisposalForm { get; set; }
    
        /// <summary>
        /// Adressuppgifter
        /// </summary>
        [DataMember(Name="objectAddress",EmitDefaultValue=false)]
        public ObjectAddress ObjectAddress { get; set; }
    
        /// <summary>
        /// Objekttyp
        /// </summary>
        [DataMember(Name="propertyType",EmitDefaultValue=false)]
        public string PropertyType { get; set; }
    
        /// <summary>
        /// Visar om det r&#246;r en k&#228;nslig aff&#228;r
        /// </summary>
        [DataMember(Name="sensitiveBusiness",EmitDefaultValue=false)]
        public bool SensitiveBusiness { get; set; }
    
        /// <summary>
        /// Fastighetsbeteckning
        /// </summary>
        [DataMember(Name="propertyUnitDesignation",EmitDefaultValue=false)]
        public string PropertyUnitDesignation { get; set; }
    
    }
}