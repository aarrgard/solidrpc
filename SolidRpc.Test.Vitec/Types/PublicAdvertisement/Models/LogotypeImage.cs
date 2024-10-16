using System.CodeDom.Compiler;
using System.Runtime.Serialization;
using System;
namespace SolidRpc.Test.Vitec.Types.PublicAdvertisement.Models {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class LogotypeImage {
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
        /// Fil&#228;ndelse
        /// </summary>
        [DataMember(Name="extension",EmitDefaultValue=false)]
        public string Extension { get; set; }
    
    }
}