using System.CodeDom.Compiler;
using System.Runtime.Serialization;
using System.Collections.Generic;
namespace SolidRpc.Test.Vitec.Types.BusinessIntelligense.Models {
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
        public SolidRpc.Test.Vitec.Types.BusinessIntelligense.Models.SalesReportOffice PrimaryOffice { get; set; }
    
        /// <summary>
        /// Provision
        /// </summary>
        [DataMember(Name="commission",EmitDefaultValue=false)]
        public SolidRpc.Test.Vitec.Types.BusinessIntelligense.Models.EconomicEstateAgentCommission Commission { get; set; }
    
        /// <summary>
        /// &#196;ndringshistorik.
        /// </summary>
        [DataMember(Name="changes",EmitDefaultValue=false)]
        public IEnumerable<SolidRpc.Test.Vitec.Types.BusinessIntelligense.Models.EconomicEstateAgentChangeEntry> Changes { get; set; }
    
    }
}