using System.CodeDom.Compiler;
using System.Runtime.Serialization;
namespace SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Budget.Models {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class Estates {
        /// <summary>
        /// Antal s&#229;lda bost&#228;der
        /// </summary>
        [DataMember(Name="sold",EmitDefaultValue=false)]
        public int Sold { get; set; }
    
        /// <summary>
        /// Antal nyregistrerade bost&#228;der
        /// </summary>
        [DataMember(Name="registered",EmitDefaultValue=false)]
        public int Registered { get; set; }
    
    }
}