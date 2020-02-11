using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
namespace SolidRpc.Security.Types {
    /// <summary>
    /// success
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class WebContent {
        /// <summary>
        /// The content
        /// </summary>
        [DataMember(Name="content",EmitDefaultValue=false)]
        public Stream Content { get; set; }
    
        /// <summary>
        /// The content type
        /// </summary>
        [DataMember(Name="contentType",EmitDefaultValue=false)]
        public string ContentType { get; set; }
    
        /// <summary>
        /// The charset - set if content is text
        /// </summary>
        [DataMember(Name="charSet",EmitDefaultValue=false)]
        public string CharSet { get; set; }
    
        /// <summary>
        /// The location of the data.
        /// </summary>
        [DataMember(Name="location",EmitDefaultValue=false)]
        public string Location { get; set; }
    
    }
}