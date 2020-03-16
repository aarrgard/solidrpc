using System.CodeDom.Compiler;
using System;
using System.Runtime.Serialization;
namespace SolidRpc.Test.Vitec.Types.Common.Cms {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class CmsPrice {
        /// <summary>
        /// Utg&#229;ngspris
        /// </summary>
        [DataMember(Name="startingPrice",EmitDefaultValue=false)]
        public double? StartingPrice { get; set; }
    
    }
}