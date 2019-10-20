using System.CodeDom.Compiler;
using System.Runtime.Serialization;
namespace SolidRpc.Test.Vitec.Types.Common.Estate {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class HousePlotInformation {
        /// <summary>
        /// Sammanst�llning bilplats
        /// </summary>
        [DataMember(Name="parking",EmitDefaultValue=false)]
        public string Parking { get; set; }
    
        /// <summary>
        /// Tomtarea i kvm
        /// </summary>
        [DataMember(Name="area",EmitDefaultValue=false)]
        public double Area { get; set; }
    
        /// <summary>
        /// Tomttyp
        /// </summary>
        [DataMember(Name="plotType",EmitDefaultValue=false)]
        public string PlotType { get; set; }
    
        /// <summary>
        /// �vrigt tomt
        /// </summary>
        [DataMember(Name="otherPlot",EmitDefaultValue=false)]
        public string OtherPlot { get; set; }
    
        /// <summary>
        /// Sammanst�llning uteplats
        /// </summary>
        [DataMember(Name="patio",EmitDefaultValue=false)]
        public string Patio { get; set; }
    
        /// <summary>
        /// Byggr�tt
        /// </summary>
        [DataMember(Name="buildingPermission",EmitDefaultValue=false)]
        public string BuildingPermission { get; set; }
    
    }
}