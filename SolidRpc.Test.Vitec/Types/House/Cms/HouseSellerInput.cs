using System.CodeDom.Compiler;
using System.Runtime.Serialization;
using SolidRpc.Test.Vitec.Types.Common.Cms;
using System.Collections.Generic;
using SolidRpc.Test.Vitec.Types.Cms.Estate;
namespace SolidRpc.Test.Vitec.Types.House.Cms {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class HouseSellerInput {
        /// <summary>
        /// Byggnad
        /// </summary>
        [DataMember(Name="building",EmitDefaultValue=false)]
        public SolidRpc.Test.Vitec.Types.House.Cms.CmsHouseBuilding Building { get; set; }
    
        /// <summary>
        /// Ventilation
        /// </summary>
        [DataMember(Name="ventilation",EmitDefaultValue=false)]
        public CmsVentilation Ventilation { get; set; }
    
        /// <summary>
        /// Energideklaration
        /// </summary>
        [DataMember(Name="energyDeclaration",EmitDefaultValue=false)]
        public CmsEnergyDeclaration EnergyDeclaration { get; set; }
    
        /// <summary>
        /// Tomt, uteplats och bilplats
        /// </summary>
        [DataMember(Name="plot",EmitDefaultValue=false)]
        public SolidRpc.Test.Vitec.Types.House.Cms.CmsHousePlotInformation Plot { get; set; }
    
        /// <summary>
        /// Vatten och avlopp
        /// </summary>
        [DataMember(Name="waterAndDrain",EmitDefaultValue=false)]
        public SolidRpc.Test.Vitec.Types.House.Cms.CmsWaterAndDrain WaterAndDrain { get; set; }
    
        /// <summary>
        /// N&#228;romr&#229;de
        /// </summary>
        [DataMember(Name="surrounding",EmitDefaultValue=false)]
        public CmsSurrounding Surrounding { get; set; }
    
        /// <summary>
        /// F&#246;rs&#228;kring
        /// </summary>
        [DataMember(Name="insurance",EmitDefaultValue=false)]
        public SolidRpc.Test.Vitec.Types.House.Cms.CmsInsurance Insurance { get; set; }
    
        /// <summary>
        /// El
        /// </summary>
        [DataMember(Name="electricity",EmitDefaultValue=false)]
        public CmsElectricity Electricity { get; set; }
    
        /// <summary>
        /// Areak&#228;lla
        /// </summary>
        [DataMember(Name="areaSource",EmitDefaultValue=false)]
        public string AreaSource { get; set; }
    
        /// <summary>
        /// Omr&#229;desid
        /// </summary>
        [DataMember(Name="areaId",EmitDefaultValue=false)]
        public string AreaId { get; set; }
    
        /// <summary>
        /// M&#246;jligt tilltr&#228;desdatum
        /// </summary>
        [DataMember(Name="possibleAccessDate",EmitDefaultValue=false)]
        public string PossibleAccessDate { get; set; }
    
        /// <summary>
        /// Basinformation
        /// </summary>
        [DataMember(Name="baseInformation",EmitDefaultValue=false)]
        public SolidRpc.Test.Vitec.Types.House.Cms.CmsHouseBaseInformation BaseInformation { get; set; }
    
        /// <summary>
        /// Interi&#246;r
        /// </summary>
        [DataMember(Name="interior",EmitDefaultValue=false)]
        public SolidRpc.Test.Vitec.Types.House.Cms.CmsHouseInterior Interior { get; set; }
    
        /// <summary>
        /// Pris information
        /// </summary>
        [DataMember(Name="price",EmitDefaultValue=false)]
        public CmsPrice Price { get; set; }
    
        /// <summary>
        /// Lista &#246;ver s&#228;ljar id&#39;n
        /// </summary>
        [DataMember(Name="sellers",EmitDefaultValue=false)]
        public IEnumerable<string> Sellers { get; set; }
    
        /// <summary>
        /// Lista &#246;ver intressenters id&#39;n
        /// </summary>
        [DataMember(Name="interests",EmitDefaultValue=false)]
        public IEnumerable<string> Interests { get; set; }
    
        /// <summary>
        /// Beskrivning
        /// </summary>
        [DataMember(Name="description",EmitDefaultValue=false)]
        public Description Description { get; set; }
    
        /// <summary>
        /// Anv&#228;ndarid
        /// </summary>
        [DataMember(Name="userId",EmitDefaultValue=false)]
        public string UserId { get; set; }
    
    }
}