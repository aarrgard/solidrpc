using System.CodeDom.Compiler;
using System.Runtime.Serialization;
namespace SolidRpc.Test.Vitec.Types.Cms.Estate {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class CmsForeignPropertyPrice {
        /// <summary>
        /// Utg&#229;ngspris i SEK
        /// </summary>
        [DataMember(Name="swedishStartingPrice",EmitDefaultValue=false)]
        public double SwedishStartingPrice { get; set; }
    
        /// <summary>
        /// Utg&#229;ngspris
        /// </summary>
        [DataMember(Name="startingPrice",EmitDefaultValue=false)]
        public double StartingPrice { get; set; }
    
    }
}