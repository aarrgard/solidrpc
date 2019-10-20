using System.CodeDom.Compiler;
using System.Runtime.Serialization;
namespace SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Estates.BusinessIntelligense {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class SalesReportOffice {
        /// <summary>
        /// Kund-id f�r detta kontor
        /// </summary>
        [DataMember(Name="customerId",EmitDefaultValue=false)]
        public string CustomerId { get; set; }
    
        /// <summary>
        /// Namn p� kontoret
        /// </summary>
        [DataMember(Name="name",EmitDefaultValue=false)]
        public string Name { get; set; }
    
        /// <summary>
        /// Ort
        /// </summary>
        [DataMember(Name="city",EmitDefaultValue=false)]
        public string City { get; set; }
    
    }
}