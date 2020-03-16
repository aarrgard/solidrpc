using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System;
namespace SolidRpc.Test.Vitec.Types.File.Models {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class File {
        /// <summary>
        /// Fil data
        /// </summary>
        [DataMember(Name="data",EmitDefaultValue=false)]
        public Stream Data { get; set; }
    
        /// <summary>
        /// Namn p&#229; filen
        /// </summary>
        [DataMember(Name="name",EmitDefaultValue=false)]
        public string Name { get; set; }
    
        /// <summary>
        /// Annonseras
        /// </summary>
        [DataMember(Name="advertise",EmitDefaultValue=false)]
        public bool? Advertise { get; set; }
    
    }
}