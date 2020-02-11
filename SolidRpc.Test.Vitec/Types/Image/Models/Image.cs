using System.CodeDom.Compiler;
using System.Runtime.Serialization;
using System;
using System.Collections.Generic;
using SolidRpc.Test.Vitec.Types.Api.Connect;
namespace SolidRpc.Test.Vitec.Types.Image.Models {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class Image {
        /// <summary>
        /// Bildid
        /// </summary>
        [DataMember(Name="imageId",EmitDefaultValue=false)]
        public string ImageId { get; set; }
    
        /// <summary>
        /// Senast &#228;ndrad
        /// </summary>
        [DataMember(Name="dateChanged",EmitDefaultValue=false)]
        public DateTimeOffset DateChanged { get; set; }
    
        /// <summary>
        /// Senast bilden &#228;ndrades
        /// </summary>
        [DataMember(Name="dateChangedImageData",EmitDefaultValue=false)]
        public DateTimeOffset DateChangedImageData { get; set; }
    
        /// <summary>
        /// Bildurl
        /// </summary>
        [DataMember(Name="url",EmitDefaultValue=false)]
        public string Url { get; set; }
    
        /// <summary>
        /// Till&#229;t visning p&#229; internet
        /// </summary>
        [DataMember(Name="showImageOnInternet",EmitDefaultValue=false)]
        public bool ShowImageOnInternet { get; set; }
    
        /// <summary>
        /// Bildformat
        /// </summary>
        [DataMember(Name="extension",EmitDefaultValue=false)]
        public string Extension { get; set; }
    
        /// <summary>
        /// CDN referenser
        /// </summary>
        [DataMember(Name="cdnReferences",EmitDefaultValue=false)]
        public IEnumerable<CdnImageReference> CdnReferences { get; set; }
    
    }
}