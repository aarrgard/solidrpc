using System.CodeDom.Compiler;
using System.Runtime.Serialization;
using System.Collections.Generic;
namespace SolidRpc.Test.Vitec.Types.Common.Estate {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class AdvertiseOn {
        /// <summary>
        /// Visar om objektet ska annonserars p� hemsida
        /// </summary>
        [DataMember(Name="homepage",EmitDefaultValue=false)]
        public bool Homepage { get; set; }
    
        /// <summary>
        /// Visa som kommande
        /// </summary>
        [DataMember(Name="showAsComming",EmitDefaultValue=false)]
        public bool ShowAsComming { get; set; }
    
        /// <summary>
        /// Visa som dagens objekt
        /// </summary>
        [DataMember(Name="showTodaysHousing",EmitDefaultValue=false)]
        public bool ShowTodaysHousing { get; set; }
    
        /// <summary>
        /// D�lj pris
        /// </summary>
        [DataMember(Name="hidePrice",EmitDefaultValue=false)]
        public bool HidePrice { get; set; }
    
        /// <summary>
        /// D�lj l�ge
        /// </summary>
        [DataMember(Name="hideAddress",EmitDefaultValue=false)]
        public bool HideAddress { get; set; }
    
        /// <summary>
        /// Visa in bildslinga
        /// </summary>
        [DataMember(Name="showInImageLoop",EmitDefaultValue=false)]
        public bool ShowInImageLoop { get; set; }
    
        /// <summary>
        /// Visa som referensobjekt
        /// </summary>
        [DataMember(Name="showAsReferenceHousing",EmitDefaultValue=false)]
        public bool ShowAsReferenceHousing { get; set; }
    
        /// <summary>
        /// Byteskravtext
        /// </summary>
        [DataMember(Name="changingRequirementsText",EmitDefaultValue=false)]
        public string ChangingRequirementsText { get; set; }
    
        /// <summary>
        /// Lista av bildid&#39;n som ska visas p� egen hemsida
        /// </summary>
        [DataMember(Name="imageIds",EmitDefaultValue=false)]
        public IEnumerable<string> ImageIds { get; set; }
    
        /// <summary>
        /// Publika dokument
        /// </summary>
        [DataMember(Name="documents",EmitDefaultValue=false)]
        public IEnumerable<SolidRpc.Test.Vitec.Types.Common.Estate.Document> Documents { get; set; }
    
        /// <summary>
        /// L�nkar
        /// </summary>
        [DataMember(Name="links",EmitDefaultValue=false)]
        public IEnumerable<SolidRpc.Test.Vitec.Types.Common.Estate.Link> Links { get; set; }
    
        /// <summary>
        /// Marknadsplatser som objektet annonseras p�
        /// </summary>
        [DataMember(Name="marketplaces",EmitDefaultValue=false)]
        public IEnumerable<string> Marketplaces { get; set; }
    
        /// <summary>
        /// Visa som f�rhandsgranskning
        /// </summary>
        [DataMember(Name="showAsPreview",EmitDefaultValue=false)]
        public bool ShowAsPreview { get; set; }
    
    }
}