using System.CodeDom.Compiler;
using System.Runtime.Serialization;
namespace SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.ForeignPropertyInfo.Estate {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class PlotInformation {
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
    
    }
}