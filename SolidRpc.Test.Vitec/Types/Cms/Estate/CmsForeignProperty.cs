using System.CodeDom.Compiler;
using SolidRpc.Test.Vitec.Types.ForeignProperty.Cms;
using System.Runtime.Serialization;
using System.Collections.Generic;
using SolidRpc.Test.Vitec.Types.Common.Cms;
using SolidRpc.Test.Vitec.Types.ForeignPropertyInfo.Estate;
namespace SolidRpc.Test.Vitec.Types.Cms.Estate {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class CmsForeignProperty {
        /// <summary>
        /// Basinformation
        /// </summary>
        [DataMember(Name="baseInformation",EmitDefaultValue=false)]
        public CmsForeignPropertyBaseInformation BaseInformation { get; set; }
    
        /// <summary>
        /// Interi�r
        /// </summary>
        [DataMember(Name="interior",EmitDefaultValue=false)]
        public CmsForeignPropertyInterior Interior { get; set; }
    
        /// <summary>
        /// Beskrivningar
        /// </summary>
        [DataMember(Name="descriptions",EmitDefaultValue=false)]
        public IEnumerable<ForeignPropertyDescription> Descriptions { get; set; }
    
        /// <summary>
        /// Energideklatation
        /// </summary>
        [DataMember(Name="energyDeclaration",EmitDefaultValue=false)]
        public EnergyDeclaration EnergyDeclaration { get; set; }
    
        /// <summary>
        /// Tomt
        /// </summary>
        [DataMember(Name="plot",EmitDefaultValue=false)]
        public CmsPlotInformation Plot { get; set; }
    
        /// <summary>
        /// Exteri�r
        /// </summary>
        [DataMember(Name="exterior",EmitDefaultValue=false)]
        public Exterior Exterior { get; set; }
    
        /// <summary>
        /// Avst�nd
        /// </summary>
        [DataMember(Name="distance",EmitDefaultValue=false)]
        public Distance Distance { get; set; }
    
        /// <summary>
        /// Valuta
        /// </summary>
        [DataMember(Name="currency",EmitDefaultValue=false)]
        public SolidRpc.Test.Vitec.Types.Cms.Estate.Currency Currency { get; set; }
    
        /// <summary>
        /// Pris information
        /// </summary>
        [DataMember(Name="price",EmitDefaultValue=false)]
        public CmsPrice Price { get; set; }
    
        /// <summary>
        /// Anv�ndarid
        /// </summary>
        [DataMember(Name="userId",EmitDefaultValue=false)]
        public string UserId { get; set; }
    
    }
}