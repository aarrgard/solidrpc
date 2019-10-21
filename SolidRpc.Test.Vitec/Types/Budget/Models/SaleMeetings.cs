using System.CodeDom.Compiler;
using System.Runtime.Serialization;
namespace SolidRpc.Test.Vitec.Types.Budget.Models {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class SaleMeetings {
        /// <summary>
        /// Totalt antal kundm&#246;ten
        /// </summary>
        [DataMember(Name="total",EmitDefaultValue=false)]
        public int Total { get; set; }
    
        /// <summary>
        /// Egenbokade kundm&#246;ten
        /// </summary>
        [DataMember(Name="ownBooked",EmitDefaultValue=false)]
        public int OwnBooked { get; set; }
    
    }
}