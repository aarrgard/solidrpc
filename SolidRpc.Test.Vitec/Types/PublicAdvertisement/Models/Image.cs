using System.CodeDom.Compiler;
using System.Runtime.Serialization;
using System;
using System.Collections.Generic;
using SolidRpc.Test.Vitec.Types.Api.Connect;
namespace SolidRpc.Test.Vitec.Types.PublicAdvertisement.Models {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class Image {
        /// <summary>
        /// Id
        /// </summary>
        [DataMember(Name="id",EmitDefaultValue=false)]
        public string Id { get; set; }
    
        /// <summary>
        /// N&#228;r bilddata f&#246;r&#228;ndrades senast
        /// </summary>
        [DataMember(Name="dataChangedAt",EmitDefaultValue=false)]
        public DateTimeOffset? DataChangedAt { get; set; }
    
        /// <summary>
        /// Beskrivande text
        /// </summary>
        [DataMember(Name="description",EmitDefaultValue=false)]
        public string Description { get; set; }
    
        /// <summary>
        /// Rubrik/Namn
        /// </summary>
        [DataMember(Name="name",EmitDefaultValue=false)]
        public string Name { get; set; }
    
        /// <summary>
        /// Kategori
        /// </summary>
        [DataMember(Name="category",EmitDefaultValue=false)]
        public string Category { get; set; }
    
        /// <summary>
        /// Taggningar
        /// </summary>
        [DataMember(Name="tags",EmitDefaultValue=false)]
        public string Tags { get; set; }
    
        /// <summary>
        /// Fil&#228;ndelse
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