using System.CodeDom.Compiler;
using System.Runtime.Serialization;
namespace SolidRpc.Test.Vitec.Types.Common.Estate {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class BalconyPatio {
        /// <summary>
        /// Tomtarea
        /// </summary>
        [DataMember(Name="squereMeterype",EmitDefaultValue=false)]
        public int SquereMeterype { get; set; }
    
        /// <summary>
        /// �vrigt tomt
        /// </summary>
        [DataMember(Name="type",EmitDefaultValue=false)]
        public string Type { get; set; }
    
        /// <summary>
        /// Sammanst�llning uteplats
        /// </summary>
        [DataMember(Name="summary",EmitDefaultValue=false)]
        public string Summary { get; set; }
    
        /// <summary>
        /// Sammanst�llning bilplats
        /// </summary>
        [DataMember(Name="parking",EmitDefaultValue=false)]
        public string Parking { get; set; }
    
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
        [DataMember(Name="parkingLot",EmitDefaultValue=false)]
        public bool ParkingLot { get; set; }
    
    }
}