using System.CodeDom.Compiler;
using System.Runtime.Serialization;
using SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.BusinessIntelligense.Models;
namespace SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Tips.BusinessIntelligense {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class Receiver {
        /// <summary>
        /// Id
        /// </summary>
        [DataMember(Name="id",EmitDefaultValue=false)]
        public string Id { get; set; }
    
        /// <summary>
        /// Namn
        /// </summary>
        [DataMember(Name="name",EmitDefaultValue=false)]
        public string Name { get; set; }
    
        /// <summary>
        /// Anst&#228;llningsnummer
        /// </summary>
        [DataMember(Name="employeeNumber",EmitDefaultValue=false)]
        public string EmployeeNumber { get; set; }
    
        /// <summary>
        /// Typ av anv&#228;ndare
        /// </summary>
        [DataMember(Name="type",EmitDefaultValue=false)]
        public string Type { get; set; }
    
        /// <summary>
        /// F&#246;retag
        /// </summary>
        [DataMember(Name="company",EmitDefaultValue=false)]
        public Company Company { get; set; }
    
    }
}