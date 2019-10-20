using System.CodeDom.Compiler;
using System.Runtime.Serialization;
namespace SolidRpc.Test.Vitec.Types.PublicAdvertisement.Models {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class PremisesMarketing {
        /// <summary>
        /// Byteskrav
        /// </summary>
        [DataMember(Name="swapDemanded",EmitDefaultValue=false)]
        public bool SwapDemanded { get; set; }
    
        /// <summary>
        /// Butik
        /// </summary>
        [DataMember(Name="retail",EmitDefaultValue=false)]
        public bool Retail { get; set; }
    
        /// <summary>
        /// Kontorslokal
        /// </summary>
        [DataMember(Name="office",EmitDefaultValue=false)]
        public bool Office { get; set; }
    
        /// <summary>
        /// Industrilokal
        /// </summary>
        [DataMember(Name="industrial",EmitDefaultValue=false)]
        public bool Industrial { get; set; }
    
        /// <summary>
        /// Verkstadslokal
        /// </summary>
        [DataMember(Name="workshop",EmitDefaultValue=false)]
        public bool Workshop { get; set; }
    
        /// <summary>
        /// Kontorshotell
        /// </summary>
        [DataMember(Name="officeHotel",EmitDefaultValue=false)]
        public bool OfficeHotel { get; set; }
    
        /// <summary>
        /// Lager/F�rr�dslokal
        /// </summary>
        [DataMember(Name="storage",EmitDefaultValue=false)]
        public bool Storage { get; set; }
    
    }
}