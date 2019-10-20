using System.CodeDom.Compiler;
using System.Runtime.Serialization;
namespace SolidRpc.Test.Vitec.Types.Category.Models {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class Category {
        /// <summary>
        /// Kategori id
        /// </summary>
        [DataMember(Name="id",EmitDefaultValue=false)]
        public string Id { get; set; }
    
        /// <summary>
        /// Kategorinamn
        /// </summary>
        [DataMember(Name="name",EmitDefaultValue=false)]
        public string Name { get; set; }
    
        /// <summary>
        /// En readonly kategori s�tts automatiskt av systemet och ska inte tilldelas manuellt till en kontakt
        /// </summary>
        [DataMember(Name="readOnly",EmitDefaultValue=false)]
        public bool ReadOnly { get; set; }
    
    }
}