using System.CodeDom.Compiler;
using System.Runtime.Serialization;
namespace SolidRpc.Test.Vitec.Types.Meeting.Models {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class AssignmentSource {
        /// <summary>
        /// Intagsk�lla id
        /// </summary>
        [DataMember(Name="id",EmitDefaultValue=false)]
        public int Id { get; set; }
    
        /// <summary>
        /// Namn p� intagsk�llan
        /// </summary>
        [DataMember(Name="name",EmitDefaultValue=false)]
        public string Name { get; set; }
    
    }
}