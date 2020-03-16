using System.CodeDom.Compiler;
using System.Runtime.Serialization;
using System;
namespace SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.User.Models {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class CustomerIdWithOrderNumber {
        /// <summary>
        /// Kontorsid
        /// </summary>
        [DataMember(Name="customerId",EmitDefaultValue=false)]
        public string CustomerId { get; set; }
    
        /// <summary>
        /// Sorteringsnummer
        /// </summary>
        [DataMember(Name="orderNumber",EmitDefaultValue=false)]
        public int? OrderNumber { get; set; }
    
        /// <summary>
        /// Huvudf&#246;retag
        /// </summary>
        [DataMember(Name="mainBusiness",EmitDefaultValue=false)]
        public bool? MainBusiness { get; set; }
    
    }
}