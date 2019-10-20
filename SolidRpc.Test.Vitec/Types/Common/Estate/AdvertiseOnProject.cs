using System.CodeDom.Compiler;
using System.Runtime.Serialization;
using System.Collections.Generic;
namespace SolidRpc.Test.Vitec.Types.Common.Estate {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class AdvertiseOnProject {
        /// <summary>
        /// Visar om objektet ska annonserars p� hemsida
        /// </summary>
        [DataMember(Name="homepage",EmitDefaultValue=false)]
        public bool Homepage { get; set; }
    
        /// <summary>
        /// Visa som kommande
        /// </summary>
        [DataMember(Name="showAsComming",EmitDefaultValue=false)]
        public bool ShowAsComming { get; set; }
    
        /// <summary>
        /// Lista av bildid&#39;n som ska visas p� egen hemsida
        /// </summary>
        [DataMember(Name="imageIds",EmitDefaultValue=false)]
        public IEnumerable<string> ImageIds { get; set; }
    
        /// <summary>
        /// Dokument
        /// </summary>
        [DataMember(Name="documents",EmitDefaultValue=false)]
        public IEnumerable<SolidRpc.Test.Vitec.Types.Common.Estate.Document> Documents { get; set; }
    
        /// <summary>
        /// L�nkar
        /// </summary>
        [DataMember(Name="links",EmitDefaultValue=false)]
        public IEnumerable<SolidRpc.Test.Vitec.Types.Common.Estate.Link> Links { get; set; }
    
        /// <summary>
        /// Visa som f�rhandsgranskning
        /// </summary>
        [DataMember(Name="showAsPreview",EmitDefaultValue=false)]
        public bool ShowAsPreview { get; set; }
    
    }
}