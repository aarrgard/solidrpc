using System.CodeDom.Compiler;
using System.Runtime.Serialization;
namespace SolidRpc.Test.Vitec.Types.PublicAdvertisement.Models {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class PremisesSurfaces {
        /// <summary>
        /// Industriarea
        /// </summary>
        [DataMember(Name="industrial",EmitDefaultValue=false)]
        public SolidRpc.Test.Vitec.Types.PublicAdvertisement.Models.SurfaceSize Industrial { get; set; }
    
        /// <summary>
        /// Kontorsarea
        /// </summary>
        [DataMember(Name="office",EmitDefaultValue=false)]
        public SolidRpc.Test.Vitec.Types.PublicAdvertisement.Models.SurfaceSize Office { get; set; }
    
        /// <summary>
        /// Annan yta
        /// </summary>
        [DataMember(Name="other",EmitDefaultValue=false)]
        public SolidRpc.Test.Vitec.Types.PublicAdvertisement.Models.SurfaceSize Other { get; set; }
    
        /// <summary>
        /// Butiksarea
        /// </summary>
        [DataMember(Name="retail",EmitDefaultValue=false)]
        public SolidRpc.Test.Vitec.Types.PublicAdvertisement.Models.SurfaceSize Retail { get; set; }
    
        /// <summary>
        /// Lagerarea
        /// </summary>
        [DataMember(Name="storage",EmitDefaultValue=false)]
        public SolidRpc.Test.Vitec.Types.PublicAdvertisement.Models.SurfaceSize Storage { get; set; }
    
        /// <summary>
        /// Total yta
        /// </summary>
        [DataMember(Name="total",EmitDefaultValue=false)]
        public SolidRpc.Test.Vitec.Types.PublicAdvertisement.Models.SurfaceSize Total { get; set; }
    
        /// <summary>
        /// Verkstadsarea
        /// </summary>
        [DataMember(Name="workshop",EmitDefaultValue=false)]
        public SolidRpc.Test.Vitec.Types.PublicAdvertisement.Models.SurfaceSize Workshop { get; set; }
    
    }
}