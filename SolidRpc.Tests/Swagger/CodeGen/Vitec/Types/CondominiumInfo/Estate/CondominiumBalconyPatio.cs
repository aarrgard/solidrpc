using System.CodeDom.Compiler;
using System.Runtime.Serialization;
using System;
namespace SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.CondominiumInfo.Estate {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class CondominiumBalconyPatio {
        /// <summary>
        /// Sammanst&#228;llning uteplats
        /// </summary>
        [DataMember(Name="description",EmitDefaultValue=false)]
        public string Description { get; set; }
    
        /// <summary>
        /// Sammanst&#228;llning bilplats
        /// </summary>
        [DataMember(Name="parking",EmitDefaultValue=false)]
        public string Parking { get; set; }
    
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
        /// Bilplats
        /// </summary>
        [DataMember(Name="parkingLot",EmitDefaultValue=false)]
        public bool? ParkingLot { get; set; }
    
    }
}