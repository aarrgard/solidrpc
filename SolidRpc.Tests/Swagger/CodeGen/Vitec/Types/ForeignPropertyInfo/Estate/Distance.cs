using System.CodeDom.Compiler;
using System.Runtime.Serialization;
namespace SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.ForeignPropertyInfo.Estate {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class Distance {
        /// <summary>
        /// Aff&#228;rscentrum
        /// </summary>
        [DataMember(Name="shoppingCentre",EmitDefaultValue=false)]
        public int ShoppingCentre { get; set; }
    
        /// <summary>
        /// Centrum
        /// </summary>
        [DataMember(Name="centre",EmitDefaultValue=false)]
        public int Centre { get; set; }
    
        /// <summary>
        /// Flyplats
        /// </summary>
        [DataMember(Name="airport",EmitDefaultValue=false)]
        public int Airport { get; set; }
    
        /// <summary>
        /// Golfbana
        /// </summary>
        [DataMember(Name="golfCource",EmitDefaultValue=false)]
        public int GolfCource { get; set; }
    
        /// <summary>
        /// Hav
        /// </summary>
        [DataMember(Name="sea",EmitDefaultValue=false)]
        public int Sea { get; set; }
    
        /// <summary>
        /// Mataff&#228;r
        /// </summary>
        [DataMember(Name="supermarket",EmitDefaultValue=false)]
        public int Supermarket { get; set; }
    
        /// <summary>
        /// Pool
        /// </summary>
        [DataMember(Name="pool",EmitDefaultValue=false)]
        public int Pool { get; set; }
    
        /// <summary>
        /// Sjukhus
        /// </summary>
        [DataMember(Name="hospital",EmitDefaultValue=false)]
        public int Hospital { get; set; }
    
        /// <summary>
        /// Strand
        /// </summary>
        [DataMember(Name="beach",EmitDefaultValue=false)]
        public int Beach { get; set; }
    
    }
}