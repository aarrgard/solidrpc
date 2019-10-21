using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System;
namespace SolidRpc.Test.Vitec.Types.Documents.Models {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class Document {
        /// <summary>
        /// Data
        /// </summary>
        [DataMember(Name="data",EmitDefaultValue=false)]
        public Stream Data { get; set; }
    
        /// <summary>
        /// Url till dokument.
        /// </summary>
        [DataMember(Name="url",EmitDefaultValue=false)]
        public string Url { get; set; }
    
        /// <summary>
        /// Namn
        /// </summary>
        [DataMember(Name="name",EmitDefaultValue=false)]
        public string Name { get; set; }
    
        /// <summary>
        /// Leverant&#246;r
        /// </summary>
        [DataMember(Name="supplier",EmitDefaultValue=false)]
        public string Supplier { get; set; }
    
        /// <summary>
        /// Malltyp
        /// </summary>
        [DataMember(Name="templateType",EmitDefaultValue=false)]
        public string TemplateType { get; set; }
    
        /// <summary>
        /// Externt id
        /// </summary>
        [DataMember(Name="externalId",EmitDefaultValue=false)]
        public string ExternalId { get; set; }
    
        /// <summary>
        /// &#196;ndringsdatum
        /// </summary>
        [DataMember(Name="changeDate",EmitDefaultValue=false)]
        public DateTimeOffset ChangeDate { get; set; }
    
        /// <summary>
        /// Filtyp
        /// </summary>
        [DataMember(Name="extension",EmitDefaultValue=false)]
        public string Extension { get; set; }
    
    }
}