using System.CodeDom.Compiler;
using System.Runtime.Serialization;
using System;
using SolidRpc.Test.Vitec.Types.Common.Estate;
using SolidRpc.Test.Vitec.Types.Models.Api;
using System.Collections.Generic;
namespace SolidRpc.Test.Vitec.Types.CommercialPropertyInfo.Estate {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class PropertyBuilding {
        /// <summary>
        /// Byggnad/Hus
        /// </summary>
        [DataMember(Name="name",EmitDefaultValue=false)]
        public string Name { get; set; }
    
        /// <summary>
        /// Id
        /// </summary>
        [DataMember(Name="id",EmitDefaultValue=false)]
        public string Id { get; set; }
    
        /// <summary>
        /// Huvudbyggnad
        /// </summary>
        [DataMember(Name="mainBuilding",EmitDefaultValue=false)]
        public bool? MainBuilding { get; set; }
    
        /// <summary>
        /// Byggnadstyp
        /// </summary>
        [DataMember(Name="buildingType",EmitDefaultValue=false)]
        public string BuildingType { get; set; }
    
        /// <summary>
        /// Bygg&#229;rs kommentar
        /// </summary>
        [DataMember(Name="buildingYearComment",EmitDefaultValue=false)]
        public string BuildingYearComment { get; set; }
    
        /// <summary>
        /// V&#229;ning/hiss
        /// </summary>
        [DataMember(Name="floorAndElevator",EmitDefaultValue=false)]
        public SolidRpc.Test.Vitec.Types.CommercialPropertyInfo.Estate.CommercialFloorAndElevator FloorAndElevator { get; set; }
    
        /// <summary>
        /// Byggnads&#229;r
        /// </summary>
        [DataMember(Name="yearBuilt",EmitDefaultValue=false)]
        public string YearBuilt { get; set; }
    
        /// <summary>
        /// Renoveringar
        /// </summary>
        [DataMember(Name="renovations",EmitDefaultValue=false)]
        public string Renovations { get; set; }
    
        /// <summary>
        /// Beskrivning
        /// </summary>
        [DataMember(Name="description",EmitDefaultValue=false)]
        public string Description { get; set; }
    
        /// <summary>
        /// Energideklaration
        /// </summary>
        [DataMember(Name="energyDeclaration",EmitDefaultValue=false)]
        public EnergyDeclaration EnergyDeclaration { get; set; }
    
        /// <summary>
        /// Byggnadss&#228;tt
        /// </summary>
        [DataMember(Name="building",EmitDefaultValue=false)]
        public SolidRpc.Test.Vitec.Types.CommercialPropertyInfo.Estate.CommercialPropertyBuilding Building { get; set; }
    
        /// <summary>
        /// Tv och bredband
        /// </summary>
        [DataMember(Name="tvAndBroadband",EmitDefaultValue=false)]
        public TVAndBroadband TvAndBroadband { get; set; }
    
        /// <summary>
        /// Antal rum
        /// </summary>
        [DataMember(Name="numberOfRooms",EmitDefaultValue=false)]
        public double? NumberOfRooms { get; set; }
    
        /// <summary>
        /// Varav sovrum
        /// </summary>
        [DataMember(Name="numberOfBedRooms",EmitDefaultValue=false)]
        public Range1_Int32 NumberOfBedRooms { get; set; }
    
        /// <summary>
        /// Boarea
        /// </summary>
        [DataMember(Name="livingSpace",EmitDefaultValue=false)]
        public double? LivingSpace { get; set; }
    
        /// <summary>
        /// Byggnadsyta
        /// </summary>
        [DataMember(Name="buildingSpace",EmitDefaultValue=false)]
        public double? BuildingSpace { get; set; }
    
        /// <summary>
        /// Biarea
        /// </summary>
        [DataMember(Name="biArea",EmitDefaultValue=false)]
        public double? BiArea { get; set; }
    
        /// <summary>
        /// Allm&#228;n beskrivning av interi&#246;ren
        /// </summary>
        [DataMember(Name="generalDescription",EmitDefaultValue=false)]
        public string GeneralDescription { get; set; }
    
        /// <summary>
        /// Rumsbeskrivningar
        /// </summary>
        [DataMember(Name="rooms",EmitDefaultValue=false)]
        public IEnumerable<Room> Rooms { get; set; }
    
    }
}