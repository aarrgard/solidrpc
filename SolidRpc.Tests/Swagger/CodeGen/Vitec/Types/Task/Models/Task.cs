using System.CodeDom.Compiler;
using System.Runtime.Serialization;
namespace SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Task.Models {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class Task {
        /// <summary>
        /// Vilken typ av f&#246;rdefinierad uppgift &#228;r det som ska skapas.
        /// </summary>
        [DataMember(Name="predefinedTaskId",EmitDefaultValue=false)]
        public string PredefinedTaskId { get; set; }
    
        /// <summary>
        /// Anteckning f&#246;r uppgiften
        /// </summary>
        [DataMember(Name="note",EmitDefaultValue=false)]
        public string Note { get; set; }
    
        /// <summary>
        /// Vilket objekt den nya uppgiften ska g&#228;lla.
        /// </summary>
        [DataMember(Name="estateId",EmitDefaultValue=false)]
        public string EstateId { get; set; }
    
        /// <summary>
        /// Utf&#246;rs av
        /// </summary>
        [DataMember(Name="assignedTo",EmitDefaultValue=false)]
        public string AssignedTo { get; set; }
    
    }
}