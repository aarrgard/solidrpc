using System.CodeDom.Compiler;
using System.Runtime.Serialization;
using System;
using System.Collections.Generic;
using SolidRpc.Test.Vitec.Types.Api.Connect;
namespace SolidRpc.Test.Vitec.Types.Common.Estate {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class ObjectImage {
        /// <summary>
        /// Kategori
        /// </summary>
        [DataMember(Name="category",EmitDefaultValue=false)]
        public string Category { get; set; }
    
        /// <summary>
        /// Leverant&#246;r
        /// </summary>
        [DataMember(Name="supplier",EmitDefaultValue=false)]
        public string Supplier { get; set; }
    
        /// <summary>
        /// Leverant&#246;rsId
        /// </summary>
        [DataMember(Name="supplierId",EmitDefaultValue=false)]
        public string SupplierId { get; set; }
    
        /// <summary>
        /// Bildrubrik
        /// </summary>
        [DataMember(Name="text",EmitDefaultValue=false)]
        public string Text { get; set; }
    
        /// <summary>
        /// Filnamn
        /// </summary>
        [DataMember(Name="name",EmitDefaultValue=false)]
        public string Name { get; set; }
    
        /// <summary>
        /// Bildtext
        /// </summary>
        [DataMember(Name="textExtended",EmitDefaultValue=false)]
        public string TextExtended { get; set; }
    
        /// <summary>
        /// Sorteringsnummer
        /// </summary>
        [DataMember(Name="orderNumber",EmitDefaultValue=false)]
        public int? OrderNumber { get; set; }
    
        /// <summary>
        /// Sorteringsnummer p&#229; hemnet
        /// </summary>
        [DataMember(Name="hemnetOrderNumber",EmitDefaultValue=false)]
        public int? HemnetOrderNumber { get; set; }
    
        /// <summary>
        /// Bildid
        /// </summary>
        [DataMember(Name="imageId",EmitDefaultValue=false)]
        public string ImageId { get; set; }
    
        /// <summary>
        /// Senast &#228;ndrad
        /// </summary>
        [DataMember(Name="dateChanged",EmitDefaultValue=false)]
        public DateTimeOffset? DateChanged { get; set; }
    
        /// <summary>
        /// Senast bilden &#228;ndrades
        /// </summary>
        [DataMember(Name="dateChangedImageData",EmitDefaultValue=false)]
        public DateTimeOffset? DateChangedImageData { get; set; }
    
        /// <summary>
        /// Bildurl
        /// </summary>
        [DataMember(Name="url",EmitDefaultValue=false)]
        public string Url { get; set; }
    
        /// <summary>
        /// Till&#229;t visning p&#229; internet
        /// </summary>
        [DataMember(Name="showImageOnInternet",EmitDefaultValue=false)]
        public bool? ShowImageOnInternet { get; set; }
    
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