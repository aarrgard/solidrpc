using System.CodeDom.Compiler;
using System.Runtime.Serialization;
using System;
namespace SolidRpc.Test.Vitec.Types.Common.Estate {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class HousePlotInformation {
        /// <summary>
        /// Sammanst&#228;llning bilplats
        /// </summary>
        [DataMember(Name="parking",EmitDefaultValue=false)]
        public string Parking { get; set; }
    
        /// <summary>
        /// Tomtarea i kvm
        /// </summary>
        [DataMember(Name="area",EmitDefaultValue=false)]
        public double? Area { get; set; }
    
        /// <summary>
        /// Tomttyp
        /// </summary>
        [DataMember(Name="plotType",EmitDefaultValue=false)]
        public string PlotType { get; set; }
    
        /// <summary>
        /// &#214;vrigt tomt
        /// </summary>
        [DataMember(Name="otherPlot",EmitDefaultValue=false)]
        public string OtherPlot { get; set; }
    
        /// <summary>
        /// Sammanst&#228;llning uteplats
        /// </summary>
        [DataMember(Name="patio",EmitDefaultValue=false)]
        public string Patio { get; set; }
    
        /// <summary>
        /// Byggr&#228;tt
        /// </summary>
        [DataMember(Name="buildingPermission",EmitDefaultValue=false)]
        public string BuildingPermission { get; set; }
    
    }
}