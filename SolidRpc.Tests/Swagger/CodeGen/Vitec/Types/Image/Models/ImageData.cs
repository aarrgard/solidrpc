using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System;
namespace SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Image.Models {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class ImageData {
        /// <summary>
        /// Data (gif,jpg,tiff,bmp,png)
        /// </summary>
        [DataMember(Name="data",EmitDefaultValue=false)]
        public Stream Data { get; set; }
    
        /// <summary>
        /// Namn p&#229; bilden
        /// </summary>
        [DataMember(Name="name",EmitDefaultValue=false)]
        public string Name { get; set; }
    
        /// <summary>
        /// Bildtext
        /// </summary>
        [DataMember(Name="caption",EmitDefaultValue=false)]
        public string Caption { get; set; }
    
        /// <summary>
        /// Annoneras
        /// </summary>
        [DataMember(Name="advertise",EmitDefaultValue=false)]
        public bool? Advertise { get; set; }
    
        /// <summary>
        /// Bildkategori
        /// </summary>
        [DataMember(Name="category",EmitDefaultValue=false)]
        public string Category { get; set; }
    
    }
}