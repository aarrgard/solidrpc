using System.CodeDom.Compiler;
using System.Runtime.Serialization;
namespace SolidRpc.Test.Vitec.Types.PublicAdvertisement.Models {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class FarmSurfaces {
        /// <summary>
        /// Tomtarea
        /// </summary>
        [DataMember(Name="plot",EmitDefaultValue=false)]
        public SolidRpc.Test.Vitec.Types.PublicAdvertisement.Models.SurfaceSize Plot { get; set; }
    
        /// <summary>
        /// Skog
        /// </summary>
        [DataMember(Name="forest",EmitDefaultValue=false)]
        public SolidRpc.Test.Vitec.Types.PublicAdvertisement.Models.SurfaceSize Forest { get; set; }
    
        /// <summary>
        /// Impediment
        /// </summary>
        [DataMember(Name="impediment",EmitDefaultValue=false)]
        public SolidRpc.Test.Vitec.Types.PublicAdvertisement.Models.SurfaceSize Impediment { get; set; }
    
        /// <summary>
        /// �ker
        /// </summary>
        [DataMember(Name="field",EmitDefaultValue=false)]
        public SolidRpc.Test.Vitec.Types.PublicAdvertisement.Models.SurfaceSize Field { get; set; }
    
        /// <summary>
        /// In�gomark
        /// </summary>
        [DataMember(Name="infield",EmitDefaultValue=false)]
        public SolidRpc.Test.Vitec.Types.PublicAdvertisement.Models.SurfaceSize Infield { get; set; }
    
        /// <summary>
        /// Betesmark
        /// </summary>
        [DataMember(Name="pasture",EmitDefaultValue=false)]
        public SolidRpc.Test.Vitec.Types.PublicAdvertisement.Models.SurfaceSize Pasture { get; set; }
    
        /// <summary>
        /// Vatten
        /// </summary>
        [DataMember(Name="water",EmitDefaultValue=false)]
        public SolidRpc.Test.Vitec.Types.PublicAdvertisement.Models.SurfaceSize Water { get; set; }
    
        /// <summary>
        /// �vrigt
        /// </summary>
        [DataMember(Name="other",EmitDefaultValue=false)]
        public SolidRpc.Test.Vitec.Types.PublicAdvertisement.Models.SurfaceSize Other { get; set; }
    
    }
}