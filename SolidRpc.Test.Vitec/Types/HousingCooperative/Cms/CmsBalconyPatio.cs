using System.CodeDom.Compiler;
using System.Runtime.Serialization;
namespace SolidRpc.Test.Vitec.Types.HousingCooperative.Cms {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class CmsBalconyPatio {
        /// <summary>
        /// Mark som ing&#229;r i uppl&#229;telsen
        /// </summary>
        [DataMember(Name="groundIncluded",EmitDefaultValue=false)]
        public double GroundIncluded { get; set; }
    
        /// <summary>
        /// Balkong
        /// </summary>
        [DataMember(Name="balcony",EmitDefaultValue=false)]
        public bool Balcony { get; set; }
    
        /// <summary>
        /// Uteplats
        /// </summary>
        [DataMember(Name="patio",EmitDefaultValue=false)]
        public bool Patio { get; set; }
    
        /// <summary>
        /// Bilplats
        /// </summary>
        [DataMember(Name="parking",EmitDefaultValue=false)]
        public bool Parking { get; set; }
    
        /// <summary>
        /// Beskrivning uteplats
        /// </summary>
        [DataMember(Name="patioDescription",EmitDefaultValue=false)]
        public string PatioDescription { get; set; }
    
        /// <summary>
        /// Beskrivning bilplats
        /// </summary>
        [DataMember(Name="parkingDescription",EmitDefaultValue=false)]
        public string ParkingDescription { get; set; }
    
    }
}