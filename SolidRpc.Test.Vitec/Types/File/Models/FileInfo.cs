using System.CodeDom.Compiler;
using System.Runtime.Serialization;
using System;
namespace SolidRpc.Test.Vitec.Types.File.Models {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class FileInfo {
        /// <summary>
        /// Id p� filen
        /// </summary>
        [DataMember(Name="id",EmitDefaultValue=false)]
        public string Id { get; set; }
    
        /// <summary>
        /// Namn p� filen
        /// </summary>
        [DataMember(Name="name",EmitDefaultValue=false)]
        public string Name { get; set; }
    
        /// <summary>
        /// Filtyp (fil�ndelse)
        /// </summary>
        [DataMember(Name="extension",EmitDefaultValue=false)]
        public string Extension { get; set; }
    
        /// <summary>
        /// Senast �ndrad
        /// </summary>
        [DataMember(Name="dateChangedData",EmitDefaultValue=false)]
        public DateTimeOffset DateChangedData { get; set; }
    
        /// <summary>
        /// Om filen annonseras eller inte
        /// </summary>
        [DataMember(Name="public",EmitDefaultValue=false)]
        public bool Public { get; set; }
    
    }
}