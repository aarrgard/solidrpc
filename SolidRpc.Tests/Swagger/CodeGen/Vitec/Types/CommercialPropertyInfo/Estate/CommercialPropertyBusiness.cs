using System.CodeDom.Compiler;
using System.Runtime.Serialization;
using System;
namespace SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.CommercialPropertyInfo.Estate {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class CommercialPropertyBusiness {
        /// <summary>
        /// Verksamhet
        /// </summary>
        [DataMember(Name="business",EmitDefaultValue=false)]
        public string Business { get; set; }
    
        /// <summary>
        /// Antal anst&#228;llda
        /// </summary>
        [DataMember(Name="employees",EmitDefaultValue=false)]
        public int? Employees { get; set; }
    
        /// <summary>
        /// Utrustning som medf&#246;ljer
        /// </summary>
        [DataMember(Name="equipment",EmitDefaultValue=false)]
        public string Equipment { get; set; }
    
        /// <summary>
        /// Ospecificerad ort
        /// </summary>
        [DataMember(Name="hiddenLocation",EmitDefaultValue=false)]
        public bool? HiddenLocation { get; set; }
    
        /// <summary>
        /// Bransch
        /// </summary>
        [DataMember(Name="lineOfBusiness",EmitDefaultValue=false)]
        public string LineOfBusiness { get; set; }
    
        /// <summary>
        /// Resultat
        /// </summary>
        [DataMember(Name="profit",EmitDefaultValue=false)]
        public string Profit { get; set; }
    
        /// <summary>
        /// Oms&#228;ttning
        /// </summary>
        [DataMember(Name="revenue",EmitDefaultValue=false)]
        public double? Revenue { get; set; }
    
        /// <summary>
        /// Start&#229;r
        /// </summary>
        [DataMember(Name="startingYear",EmitDefaultValue=false)]
        public int? StartingYear { get; set; }
    
    }
}