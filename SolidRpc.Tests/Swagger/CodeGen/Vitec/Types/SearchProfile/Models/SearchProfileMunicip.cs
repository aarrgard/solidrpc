using System.CodeDom.Compiler;
using System.Runtime.Serialization;
namespace SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.SearchProfile.Models {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class SearchProfileMunicip {
        /// <summary>
        /// L&#228;n
        /// </summary>
        [DataMember(Name="county",EmitDefaultValue=false)]
        public string County { get; set; }
    
        /// <summary>
        /// Namn
        /// </summary>
        [DataMember(Name="name",EmitDefaultValue=false)]
        public string Name { get; set; }
    
        /// <summary>
        /// Id
        /// </summary>
        [DataMember(Name="id",EmitDefaultValue=false)]
        public string Id { get; set; }
    
        /// <summary>
        /// L&#228;n och kommunkod
        /// </summary>
        [DataMember(Name="countyMunicipalityCode",EmitDefaultValue=false)]
        public string CountyMunicipalityCode { get; set; }
    
    }
}