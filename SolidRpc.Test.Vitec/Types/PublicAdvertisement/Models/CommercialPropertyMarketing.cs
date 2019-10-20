using System.CodeDom.Compiler;
using System.Runtime.Serialization;
namespace SolidRpc.Test.Vitec.Types.PublicAdvertisement.Models {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class CommercialPropertyMarketing {
        /// <summary>
        /// Bostadsfastighet
        /// </summary>
        [DataMember(Name="residental",EmitDefaultValue=false)]
        public bool Residental { get; set; }
    
        /// <summary>
        /// Butiksfastighet
        /// </summary>
        [DataMember(Name="retail",EmitDefaultValue=false)]
        public bool Retail { get; set; }
    
        /// <summary>
        /// Industrifastighet
        /// </summary>
        [DataMember(Name="industrial",EmitDefaultValue=false)]
        public bool Industrial { get; set; }
    
        /// <summary>
        /// Kontorsfastighet
        /// </summary>
        [DataMember(Name="office",EmitDefaultValue=false)]
        public bool Office { get; set; }
    
        /// <summary>
        /// Lagerfastighet
        /// </summary>
        [DataMember(Name="warehouse",EmitDefaultValue=false)]
        public bool Warehouse { get; set; }
    
        /// <summary>
        /// Lokalfastighet
        /// </summary>
        [DataMember(Name="premises",EmitDefaultValue=false)]
        public bool Premises { get; set; }
    
        /// <summary>
        /// Tomt
        /// </summary>
        [DataMember(Name="plot",EmitDefaultValue=false)]
        public bool Plot { get; set; }
    
        /// <summary>
        /// R�relse
        /// </summary>
        [DataMember(Name="business",EmitDefaultValue=false)]
        public bool Business { get; set; }
    
        /// <summary>
        /// Kommande
        /// </summary>
        [DataMember(Name="isFutureSale",EmitDefaultValue=false)]
        public bool IsFutureSale { get; set; }
    
        /// <summary>
        /// Snart till salu
        /// </summary>
        [DataMember(Name="isSoonForSale",EmitDefaultValue=false)]
        public bool IsSoonForSale { get; set; }
    
    }
}