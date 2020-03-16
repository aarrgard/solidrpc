using System.CodeDom.Compiler;
using System.Runtime.Serialization;
using SolidRpc.Test.Vitec.Types.Common.Cms;
using System;
using SolidRpc.Test.Vitec.Types.Cms.Estate;
using System.Collections.Generic;
namespace SolidRpc.Test.Vitec.Types.HousingCooperative.Cms {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class HousingCooperativeSellerInput {
        /// <summary>
        /// Basinformation
        /// </summary>
        [DataMember(Name="baseInformation",EmitDefaultValue=false)]
        public SolidRpc.Test.Vitec.Types.HousingCooperative.Cms.CmsHousingCooperativeBaseInformation BaseInformation { get; set; }
    
        /// <summary>
        /// Interi&#246;r
        /// </summary>
        [DataMember(Name="interior",EmitDefaultValue=false)]
        public SolidRpc.Test.Vitec.Types.HousingCooperative.Cms.CmsHousingCooperativeInterior Interior { get; set; }
    
        /// <summary>
        /// Byggnad
        /// </summary>
        [DataMember(Name="building",EmitDefaultValue=false)]
        public SolidRpc.Test.Vitec.Types.HousingCooperative.Cms.CmsHousingCooperativeBuilding Building { get; set; }
    
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
        /// Balkong, uteplats pch bilplats
        /// </summary>
        [DataMember(Name="balconyPatio",EmitDefaultValue=false)]
        public SolidRpc.Test.Vitec.Types.HousingCooperative.Cms.CmsBalconyPatio BalconyPatio { get; set; }
    
        /// <summary>
        /// N&#228;romr&#229;de
        /// </summary>
        [DataMember(Name="surrounding",EmitDefaultValue=false)]
        public CmsSurrounding Surrounding { get; set; }
    
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
        /// Andel i f&#246;rening (%)
        /// </summary>
        [DataMember(Name="shareInAssociation",EmitDefaultValue=false)]
        public double? ShareInAssociation { get; set; }
    
        /// <summary>
        /// Andel av &#229;rsavgiften (%)
        /// </summary>
        [DataMember(Name="shareOfAnnualFee",EmitDefaultValue=false)]
        public double? ShareOfAnnualFee { get; set; }
    
        /// <summary>
        /// V&#229;ning/hiss
        /// </summary>
        [DataMember(Name="floorAndElevator",EmitDefaultValue=false)]
        public FloorAndElevator FloorAndElevator { get; set; }
    
        /// <summary>
        /// Pris information
        /// </summary>
        [DataMember(Name="price",EmitDefaultValue=false)]
        public CmsPrice Price { get; set; }
    
        /// <summary>
        /// Lista &#246;ver s&#228;ljarid&#39;n
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