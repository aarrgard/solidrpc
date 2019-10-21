using System.CodeDom.Compiler;
using System.Runtime.Serialization;
namespace SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Estates.BusinessIntelligense {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class SaleReportEstateAgent {
        /// <summary>
        /// M&#228;klarens id
        /// </summary>
        [DataMember(Name="id",EmitDefaultValue=false)]
        public string Id { get; set; }
    
        /// <summary>
        /// Namn p&#229; m&#228;klaren
        /// </summary>
        [DataMember(Name="name",EmitDefaultValue=false)]
        public string Name { get; set; }
    
        /// <summary>
        /// Initialer p&#229; m&#228;klaren
        /// </summary>
        [DataMember(Name="initials",EmitDefaultValue=false)]
        public string Initials { get; set; }
    
        /// <summary>
        /// Anst&#228;llningsnummer
        /// </summary>
        [DataMember(Name="employeeId",EmitDefaultValue=false)]
        public string EmployeeId { get; set; }
    
        /// <summary>
        /// Ansvarig m&#228;klare
        /// </summary>
        [DataMember(Name="responsible",EmitDefaultValue=false)]
        public bool Responsible { get; set; }
    
        /// <summary>
        /// Kontor
        /// </summary>
        [DataMember(Name="primaryOffice",EmitDefaultValue=false)]
        public SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Estates.BusinessIntelligense.SalesReportOffice PrimaryOffice { get; set; }
    
        /// <summary>
        /// Provision
        /// </summary>
        [DataMember(Name="commission",EmitDefaultValue=false)]
        public SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Estates.BusinessIntelligense.EconomicEstateAgentCommission Commission { get; set; }
    
    }
}