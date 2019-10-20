using System.CodeDom.Compiler;
using System.Runtime.Serialization;
namespace SolidRpc.Test.Vitec.Types.Municipality.Models {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class Municipality {
        /// <summary>
        /// Kommunid
        /// </summary>
        [DataMember(Name="id",EmitDefaultValue=false)]
        public string Id { get; set; }
    
        /// <summary>
        /// Kommunnamn
        /// </summary>
        [DataMember(Name="name",EmitDefaultValue=false)]
        public string Name { get; set; }
    
        /// <summary>
        /// L�n och kommun kod
        /// </summary>
        [DataMember(Name="countyMunicipalityCode",EmitDefaultValue=false)]
        public string CountyMunicipalityCode { get; set; }
    
    }
}