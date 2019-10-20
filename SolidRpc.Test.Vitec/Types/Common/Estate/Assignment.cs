using System.CodeDom.Compiler;
using System.Runtime.Serialization;
using System.Collections.Generic;
namespace SolidRpc.Test.Vitec.Types.Common.Estate {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class Assignment {
        /// <summary>
        /// Statusen
        /// </summary>
        [DataMember(Name="status",EmitDefaultValue=false)]
        public SolidRpc.Test.Vitec.Types.Common.Estate.Status Status { get; set; }
    
        /// <summary>
        /// F�rs�ljningss�tt
        /// </summary>
        [DataMember(Name="marketingMethods",EmitDefaultValue=false)]
        public IEnumerable<string> MarketingMethods { get; set; }
    
        /// <summary>
        /// Ansvarig m�klare (id)
        /// </summary>
        [DataMember(Name="responsibleBroker",EmitDefaultValue=false)]
        public string ResponsibleBroker { get; set; }
    
        /// <summary>
        /// Extra kontakt (id)
        /// </summary>
        [DataMember(Name="additionalContact",EmitDefaultValue=false)]
        public string AdditionalContact { get; set; }
    
        /// <summary>
        /// Objektnummer
        /// </summary>
        [DataMember(Name="estatenumber",EmitDefaultValue=false)]
        public string Estatenumber { get; set; }
    
        /// <summary>
        /// Producent
        /// </summary>
        [DataMember(Name="producer",EmitDefaultValue=false)]
        public string Producer { get; set; }
    
    }
}