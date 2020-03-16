using System.CodeDom.Compiler;
using System.Runtime.Serialization;
using System;
namespace SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.FarmInfo.Estate {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class AreaData {
        /// <summary>
        /// K&#228;lla
        /// </summary>
        [DataMember(Name="areaSource",EmitDefaultValue=false)]
        public string AreaSource { get; set; }
    
        /// <summary>
        /// Skog
        /// </summary>
        [DataMember(Name="forest",EmitDefaultValue=false)]
        public double? Forest { get; set; }
    
        /// <summary>
        /// Impediment
        /// </summary>
        [DataMember(Name="wasteLand",EmitDefaultValue=false)]
        public double? WasteLand { get; set; }
    
        /// <summary>
        /// &#197;ker
        /// </summary>
        [DataMember(Name="arabel",EmitDefaultValue=false)]
        public double? Arabel { get; set; }
    
        /// <summary>
        /// In&#228;gomark
        /// </summary>
        [DataMember(Name="infield",EmitDefaultValue=false)]
        public double? Infield { get; set; }
    
        /// <summary>
        /// Bete
        /// </summary>
        [DataMember(Name="pasture",EmitDefaultValue=false)]
        public double? Pasture { get; set; }
    
        /// <summary>
        /// Vatten
        /// </summary>
        [DataMember(Name="water",EmitDefaultValue=false)]
        public double? Water { get; set; }
    
        /// <summary>
        /// &#214;vrigt
        /// </summary>
        [DataMember(Name="other",EmitDefaultValue=false)]
        public double? Other { get; set; }
    
        /// <summary>
        /// Totalareal
        /// </summary>
        [DataMember(Name="totalArea",EmitDefaultValue=false)]
        public double? TotalArea { get; set; }
    
    }
}