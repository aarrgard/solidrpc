using System.CodeDom.Compiler;
using System;
using System.Runtime.Serialization;
namespace SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.ForeignPropertyInfo.Estate {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class Exterior {
        /// <summary>
        /// Pool
        /// </summary>
        [DataMember(Name="pool",EmitDefaultValue=false)]
        public bool? Pool { get; set; }
    
        /// <summary>
        /// Beskrivning av pool
        /// </summary>
        [DataMember(Name="descriptionOfPool",EmitDefaultValue=false)]
        public string DescriptionOfPool { get; set; }
    
        /// <summary>
        /// Terrass
        /// </summary>
        [DataMember(Name="terrace",EmitDefaultValue=false)]
        public bool? Terrace { get; set; }
    
        /// <summary>
        /// Terrasyta (kvm)
        /// </summary>
        [DataMember(Name="terraceSurface",EmitDefaultValue=false)]
        public int? TerraceSurface { get; set; }
    
        /// <summary>
        /// Balkong
        /// </summary>
        [DataMember(Name="balcony",EmitDefaultValue=false)]
        public bool? Balcony { get; set; }
    
        /// <summary>
        /// Balkongyta (kvm)
        /// </summary>
        [DataMember(Name="balconySurface",EmitDefaultValue=false)]
        public int? BalconySurface { get; set; }
    
        /// <summary>
        /// Beskrivning terrass och balkong
        /// </summary>
        [DataMember(Name="descriptionTerraceAndBalcony",EmitDefaultValue=false)]
        public string DescriptionTerraceAndBalcony { get; set; }
    
    }
}