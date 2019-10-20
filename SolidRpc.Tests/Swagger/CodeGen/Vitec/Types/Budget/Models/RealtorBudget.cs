using System.CodeDom.Compiler;
using System.Runtime.Serialization;
namespace SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Budget.Models {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class RealtorBudget {
        /// <summary>
        /// Provision (exkl.moms) max 10 000 000kr
        /// </summary>
        [DataMember(Name="commission",EmitDefaultValue=false)]
        public int Commission { get; set; }
    
        /// <summary>
        /// Bost�der
        /// </summary>
        [DataMember(Name="estates",EmitDefaultValue=false)]
        public SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Budget.Models.Estates Estates { get; set; }
    
        /// <summary>
        /// M�ten
        /// </summary>
        [DataMember(Name="saleMeetings",EmitDefaultValue=false)]
        public SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Budget.Models.SaleMeetings SaleMeetings { get; set; }
    
    }
}