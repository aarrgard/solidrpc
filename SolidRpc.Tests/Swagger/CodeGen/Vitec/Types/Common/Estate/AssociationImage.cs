using System.CodeDom.Compiler;
using System.Runtime.Serialization;
using System;
namespace SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Common.Estate {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class AssociationImage {
        /// <summary>
        /// Kategori
        /// </summary>
        [DataMember(Name="category",EmitDefaultValue=false)]
        public string Category { get; set; }
    
        /// <summary>
        /// Bildrubrik
        /// </summary>
        [DataMember(Name="headning",EmitDefaultValue=false)]
        public string Headning { get; set; }
    
        /// <summary>
        /// Bildtext
        /// </summary>
        [DataMember(Name="description",EmitDefaultValue=false)]
        public string Description { get; set; }
    
        /// <summary>
        /// Sorteringsnummer
        /// </summary>
        [DataMember(Name="orderNumber",EmitDefaultValue=false)]
        public int? OrderNumber { get; set; }
    
        /// <summary>
        /// Bildid
        /// </summary>
        [DataMember(Name="imageId",EmitDefaultValue=false)]
        public string ImageId { get; set; }
    
        /// <summary>
        /// Senast &#228;ndrad
        /// </summary>
        [DataMember(Name="dateChanged",EmitDefaultValue=false)]
        public System.DateTimeOffset? DateChanged { get; set; }
    
        /// <summary>
        /// Senast bilden &#228;ndrades
        /// </summary>
        [DataMember(Name="dateChangedImageData",EmitDefaultValue=false)]
        public System.DateTimeOffset? DateChangedImageData { get; set; }
    
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
    
    }
}