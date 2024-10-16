using System.CodeDom.Compiler;
using System.Runtime.Serialization;
namespace SolidRpc.Test.Vitec.Types.BusinessIntelligense.Models {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class Tip {
        /// <summary>
        /// Unik identifierare f&#246;r tipset
        /// </summary>
        [DataMember(Name="id",EmitDefaultValue=false)]
        public string Id { get; set; }
    
        /// <summary>
        /// Rubrik p&#229; tipset
        /// </summary>
        [DataMember(Name="title",EmitDefaultValue=false)]
        public string Title { get; set; }
    
        /// <summary>
        /// Aktuell status p&#229; tipset
        /// </summary>
        [DataMember(Name="state",EmitDefaultValue=false)]
        public string State { get; set; }
    
        /// <summary>
        /// Beskrivande text. Kan fyllas i av avs&#228;ndare, men kan ocks&#229; uppdateras av mottagaren.
        /// </summary>
        [DataMember(Name="message",EmitDefaultValue=false)]
        public string Message { get; set; }
    
        /// <summary>
        /// Typ av tips
        /// </summary>
        [DataMember(Name="type",EmitDefaultValue=false)]
        public string Type { get; set; }
    
    }
}