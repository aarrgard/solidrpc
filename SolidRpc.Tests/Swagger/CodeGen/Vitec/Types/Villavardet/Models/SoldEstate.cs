using System.CodeDom.Compiler;
using System.Runtime.Serialization;
using System;
namespace SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Villavardet.Models {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class SoldEstate {
        /// <summary>
        /// K&#246;pobjekt
        /// </summary>
        [DataMember(Name="estate",EmitDefaultValue=false)]
        public SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Villavardet.Models.Estate Estate { get; set; }
    
        /// <summary>
        /// K&#246;pare
        /// </summary>
        [DataMember(Name="buyer",EmitDefaultValue=false)]
        public SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Villavardet.Models.Buyer Buyer { get; set; }
    
        /// <summary>
        /// Tilltr&#228;desdag
        /// </summary>
        [DataMember(Name="accessDate",EmitDefaultValue=false)]
        public DateTimeOffset AccessDate { get; set; }
    
        /// <summary>
        /// Kontraktsdag
        /// </summary>
        [DataMember(Name="contractDate",EmitDefaultValue=false)]
        public DateTimeOffset ContractDate { get; set; }
    
        /// <summary>
        /// M&#228;klare
        /// </summary>
        [DataMember(Name="agent",EmitDefaultValue=false)]
        public SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Villavardet.Models.Agent Agent { get; set; }
    
    }
}