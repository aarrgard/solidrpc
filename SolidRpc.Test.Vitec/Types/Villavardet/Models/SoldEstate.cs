using System.CodeDom.Compiler;
using System.Runtime.Serialization;
using System;
namespace SolidRpc.Test.Vitec.Types.Villavardet.Models {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class SoldEstate {
        /// <summary>
        /// K�pobjekt
        /// </summary>
        [DataMember(Name="estate",EmitDefaultValue=false)]
        public SolidRpc.Test.Vitec.Types.Villavardet.Models.Estate Estate { get; set; }
    
        /// <summary>
        /// K�pare
        /// </summary>
        [DataMember(Name="buyer",EmitDefaultValue=false)]
        public SolidRpc.Test.Vitec.Types.Villavardet.Models.Buyer Buyer { get; set; }
    
        /// <summary>
        /// Tilltr�desdag
        /// </summary>
        [DataMember(Name="accessDate",EmitDefaultValue=false)]
        public DateTimeOffset AccessDate { get; set; }
    
        /// <summary>
        /// Kontraktsdag
        /// </summary>
        [DataMember(Name="contractDate",EmitDefaultValue=false)]
        public DateTimeOffset ContractDate { get; set; }
    
        /// <summary>
        /// M�klare
        /// </summary>
        [DataMember(Name="agent",EmitDefaultValue=false)]
        public SolidRpc.Test.Vitec.Types.Villavardet.Models.Agent Agent { get; set; }
    
    }
}