using System.CodeDom.Compiler;
using System.Runtime.Serialization;
namespace SolidRpc.Test.Vitec.Types.House.Cms {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class CmsHousePlotInformation {
        /// <summary>
        /// Tomttyp
        /// </summary>
        [DataMember(Name="plotType",EmitDefaultValue=false)]
        public string PlotType { get; set; }
    
        /// <summary>
        /// &#214;vrigt tomt
        /// </summary>
        [DataMember(Name="otherInfoAboutPlot",EmitDefaultValue=false)]
        public string OtherInfoAboutPlot { get; set; }
    
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
    
        /// <summary>
        /// Tomtarea
        /// </summary>
        [DataMember(Name="area",EmitDefaultValue=false)]
        public double Area { get; set; }
    
    }
}