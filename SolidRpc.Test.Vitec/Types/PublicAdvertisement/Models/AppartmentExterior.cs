using System.CodeDom.Compiler;
using System;
using System.Runtime.Serialization;
namespace SolidRpc.Test.Vitec.Types.PublicAdvertisement.Models {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class AppartmentExterior {
        /// <summary>
        /// Balkong
        /// </summary>
        [DataMember(Name="balcony",EmitDefaultValue=false)]
        public bool? Balcony { get; set; }
    
        /// <summary>
        /// Uteplats
        /// </summary>
        [DataMember(Name="patio",EmitDefaultValue=false)]
        public bool? Patio { get; set; }
    
        /// <summary>
        /// Pool
        /// </summary>
        [DataMember(Name="pool",EmitDefaultValue=false)]
        public bool? Pool { get; set; }
    
    }
}