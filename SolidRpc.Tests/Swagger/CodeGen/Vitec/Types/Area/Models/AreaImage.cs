using System.CodeDom.Compiler;
using System.Runtime.Serialization;
using System;
namespace SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Area.Models {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class AreaImage {
        /// <summary>
        /// Bild id
        /// </summary>
        [DataMember(Name="imageId",EmitDefaultValue=false)]
        public string ImageId { get; set; }
    
        /// <summary>
        /// &#196;ndringsdatum
        /// </summary>
        [DataMember(Name="dateChanged",EmitDefaultValue=false)]
        public DateTimeOffset DateChanged { get; set; }
    
        /// <summary>
        /// Bildurl
        /// </summary>
        [DataMember(Name="url",EmitDefaultValue=false)]
        public string Url { get; set; }
    
        /// <summary>
        /// Bildformat
        /// </summary>
        [DataMember(Name="extension",EmitDefaultValue=false)]
        public string Extension { get; set; }
    
    }
}