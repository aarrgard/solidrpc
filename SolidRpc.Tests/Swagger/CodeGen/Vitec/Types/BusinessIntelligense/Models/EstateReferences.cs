using System.CodeDom.Compiler;
using System.Runtime.Serialization;
using System.Collections.Generic;
namespace SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.BusinessIntelligense.Models {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class EstateReferences {
        /// <summary>
        /// Kontorsnamn
        /// </summary>
        [DataMember(Name="name",EmitDefaultValue=false)]
        public string Name { get; set; }
    
        /// <summary>
        /// Kundnummer
        /// </summary>
        [DataMember(Name="customerId",EmitDefaultValue=false)]
        public string CustomerId { get; set; }
    
        /// <summary>
        /// Kontorets ort
        /// </summary>
        [DataMember(Name="city",EmitDefaultValue=false)]
        public string City { get; set; }
    
        /// <summary>
        /// Villor
        /// </summary>
        [DataMember(Name="houses",EmitDefaultValue=false)]
        public IEnumerable<SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.BusinessIntelligense.Models.EstateReference> Houses { get; set; }
    
        /// <summary>
        /// Bostadsr&#228;tter
        /// </summary>
        [DataMember(Name="housingCooperatives",EmitDefaultValue=false)]
        public IEnumerable<SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.BusinessIntelligense.Models.EstateReference> HousingCooperatives { get; set; }
    
        /// <summary>
        /// Fritidshus
        /// </summary>
        [DataMember(Name="cottages",EmitDefaultValue=false)]
        public IEnumerable<SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.BusinessIntelligense.Models.EstateReference> Cottages { get; set; }
    
        /// <summary>
        /// Tomter
        /// </summary>
        [DataMember(Name="plots",EmitDefaultValue=false)]
        public IEnumerable<SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.BusinessIntelligense.Models.EstateReference> Plots { get; set; }
    
        /// <summary>
        /// G&#229;rdar
        /// </summary>
        [DataMember(Name="farms",EmitDefaultValue=false)]
        public IEnumerable<SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.BusinessIntelligense.Models.EstateReference> Farms { get; set; }
    
        /// <summary>
        /// &#196;garl&#228;genheter
        /// </summary>
        [DataMember(Name="condominiums",EmitDefaultValue=false)]
        public IEnumerable<SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.BusinessIntelligense.Models.EstateReference> Condominiums { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name="commercialProperties",EmitDefaultValue=false)]
        public IEnumerable<SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.BusinessIntelligense.Models.EstateReference> CommercialProperties { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name="foreignProperties",EmitDefaultValue=false)]
        public IEnumerable<SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.BusinessIntelligense.Models.EstateReference> ForeignProperties { get; set; }
    
    }
}