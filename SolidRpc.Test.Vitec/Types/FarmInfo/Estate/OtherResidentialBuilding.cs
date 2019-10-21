using System.CodeDom.Compiler;
using System.Runtime.Serialization;
using SolidRpc.Test.Vitec.Types.Common.Estate;
using System.Collections.Generic;
namespace SolidRpc.Test.Vitec.Types.FarmInfo.Estate {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class OtherResidentialBuilding {
        /// <summary>
        /// Id
        /// </summary>
        [DataMember(Name="id",EmitDefaultValue=false)]
        public string Id { get; set; }
    
        /// <summary>
        /// Namn p&#229; byggnaden
        /// </summary>
        [DataMember(Name="name",EmitDefaultValue=false)]
        public string Name { get; set; }
    
        /// <summary>
        /// Byggnadstyp
        /// </summary>
        [DataMember(Name="buildingType",EmitDefaultValue=false)]
        public string BuildingType { get; set; }
    
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
        /// Antal rum
        /// </summary>
        [DataMember(Name="numberOfRooms",EmitDefaultValue=false)]
        public double NumberOfRooms { get; set; }
    
        /// <summary>
        /// Boarea
        /// </summary>
        [DataMember(Name="livingSpace",EmitDefaultValue=false)]
        public double LivingSpace { get; set; }
    
        /// <summary>
        /// Biarea
        /// </summary>
        [DataMember(Name="otherSpace",EmitDefaultValue=false)]
        public double OtherSpace { get; set; }
    
        /// <summary>
        /// Byggnadsyta
        /// </summary>
        [DataMember(Name="buildingSpace",EmitDefaultValue=false)]
        public double BuildingSpace { get; set; }
    
        /// <summary>
        /// Kommentar till Areauppgifter
        /// </summary>
        [DataMember(Name="commentaryOnAreaInformation",EmitDefaultValue=false)]
        public string CommentaryOnAreaInformation { get; set; }
    
        /// <summary>
        /// Drift
        /// </summary>
        [DataMember(Name="operation",EmitDefaultValue=false)]
        public Operation Operation { get; set; }
    
        /// <summary>
        /// Elf&#246;rbrukning
        /// </summary>
        [DataMember(Name="electricity",EmitDefaultValue=false)]
        public Electricity Electricity { get; set; }
    
        /// <summary>
        /// Rumsbeskrivning
        /// </summary>
        [DataMember(Name="roomDescription",EmitDefaultValue=false)]
        public string RoomDescription { get; set; }
    
        /// <summary>
        /// Tv och bredband
        /// </summary>
        [DataMember(Name="tvAndBroadband",EmitDefaultValue=false)]
        public TVAndBroadband TvAndBroadband { get; set; }
    
        /// <summary>
        /// Byggnadss&#228;tt
        /// </summary>
        [DataMember(Name="building",EmitDefaultValue=false)]
        public string Building { get; set; }
    
        /// <summary>
        /// Uppv&#228;rmning
        /// </summary>
        [DataMember(Name="heating",EmitDefaultValue=false)]
        public string Heating { get; set; }
    
        /// <summary>
        /// Vatten/avlopp
        /// </summary>
        [DataMember(Name="waterAndDrain",EmitDefaultValue=false)]
        public string WaterAndDrain { get; set; }
    
        /// <summary>
        /// Ventilation
        /// </summary>
        [DataMember(Name="ventilation",EmitDefaultValue=false)]
        public Ventilation Ventilation { get; set; }
    
        /// <summary>
        /// &#214;vrigt
        /// </summary>
        [DataMember(Name="other",EmitDefaultValue=false)]
        public string Other { get; set; }
    
        /// <summary>
        /// Tomtbeskrivning
        /// </summary>
        [DataMember(Name="plot",EmitDefaultValue=false)]
        public string Plot { get; set; }
    
        /// <summary>
        /// Uteplats
        /// </summary>
        [DataMember(Name="patio",EmitDefaultValue=false)]
        public string Patio { get; set; }
    
        /// <summary>
        /// Energideklaration
        /// </summary>
        [DataMember(Name="energyDeclaration",EmitDefaultValue=false)]
        public EnergyDeclaration EnergyDeclaration { get; set; }
    
        /// <summary>
        /// Bilplats
        /// </summary>
        [DataMember(Name="parkingSpace",EmitDefaultValue=false)]
        public string ParkingSpace { get; set; }
    
        /// <summary>
        /// Sidobyggnader
        /// </summary>
        [DataMember(Name="sideBuildings",EmitDefaultValue=false)]
        public string SideBuildings { get; set; }
    
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