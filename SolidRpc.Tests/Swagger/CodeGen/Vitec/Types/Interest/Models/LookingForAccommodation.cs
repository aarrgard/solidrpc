using System.CodeDom.Compiler;
using System.Runtime.Serialization;
using System.Collections.Generic;
using SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Models.Api;
namespace SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Interest.Models {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class LookingForAccommodation {
        /// <summary>
        /// S&#246;kpreferens Id
        /// </summary>
        [DataMember(Name="id",EmitDefaultValue=false)]
        public string Id { get; set; }
    
        /// <summary>
        /// Lista av omr&#229;den.Endast omr&#229;den med polygoner kan anv&#228;ndas
        /// </summary>
        [DataMember(Name="areaIds",EmitDefaultValue=false)]
        public IEnumerable<string> AreaIds { get; set; }
    
        /// <summary>
        /// L&#228;n, LK-Koder
        /// </summary>
        [DataMember(Name="countys",EmitDefaultValue=false)]
        public IEnumerable<string> Countys { get; set; }
    
        /// <summary>
        /// Landskod
        /// </summary>
        [DataMember(Name="countryCode",EmitDefaultValue=false)]
        public string CountryCode { get; set; }
    
        /// <summary>
        /// Polygon
        /// </summary>
        [DataMember(Name="polygon",EmitDefaultValue=false)]
        public IEnumerable<Coordinate> Polygon { get; set; }
    
        /// <summary>
        /// &#214;nskem&#229;let g&#228;ller utlandsbost&#228;der
        /// </summary>
        [DataMember(Name="foreignProperty",EmitDefaultValue=false)]
        public bool ForeignProperty { get; set; }
    
        /// <summary>
        /// Hus
        /// </summary>
        [DataMember(Name="house",EmitDefaultValue=false)]
        public bool House { get; set; }
    
        /// <summary>
        /// Radhus
        /// </summary>
        [DataMember(Name="rowHouse",EmitDefaultValue=false)]
        public bool RowHouse { get; set; }
    
        /// <summary>
        /// Bostadsr&#228;tt
        /// </summary>
        [DataMember(Name="housingCooperative",EmitDefaultValue=false)]
        public bool HousingCooperative { get; set; }
    
        /// <summary>
        /// Fritidshus
        /// </summary>
        [DataMember(Name="cottage",EmitDefaultValue=false)]
        public bool Cottage { get; set; }
    
        /// <summary>
        /// Lokal
        /// </summary>
        [DataMember(Name="premises",EmitDefaultValue=false)]
        public bool Premises { get; set; }
    
        /// <summary>
        /// Tomt
        /// </summary>
        [DataMember(Name="plot",EmitDefaultValue=false)]
        public bool Plot { get; set; }
    
        /// <summary>
        /// G&#229;rd
        /// </summary>
        [DataMember(Name="farm",EmitDefaultValue=false)]
        public bool Farm { get; set; }
    
        /// <summary>
        /// Hyresr&#228;tt
        /// </summary>
        [DataMember(Name="tenancy",EmitDefaultValue=false)]
        public bool Tenancy { get; set; }
    
        /// <summary>
        /// &#214;vrig boform
        /// </summary>
        [DataMember(Name="otherHousing",EmitDefaultValue=false)]
        public bool OtherHousing { get; set; }
    
        /// <summary>
        /// Boarea
        /// </summary>
        [DataMember(Name="livingSpace",EmitDefaultValue=false)]
        public SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Interest.Models.Interval LivingSpace { get; set; }
    
        /// <summary>
        /// Antal rum
        /// </summary>
        [DataMember(Name="numberOfRooms",EmitDefaultValue=false)]
        public SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Interest.Models.Interval NumberOfRooms { get; set; }
    
        /// <summary>
        /// Pris
        /// </summary>
        [DataMember(Name="price",EmitDefaultValue=false)]
        public SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Interest.Models.Interval Price { get; set; }
    
        /// <summary>
        /// Tomtarea
        /// </summary>
        [DataMember(Name="plotArea",EmitDefaultValue=false)]
        public SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Interest.Models.Interval PlotArea { get; set; }
    
        /// <summary>
        /// Special f&#246;rfr&#229;gan
        /// </summary>
        [DataMember(Name="specialRequset",EmitDefaultValue=false)]
        public string SpecialRequset { get; set; }
    
        /// <summary>
        /// Ut&#246;kade krav
        /// </summary>
        [DataMember(Name="increasedRequirementIDs",EmitDefaultValue=false)]
        public IEnumerable<string> IncreasedRequirementIDs { get; set; }
    
        /// <summary>
        /// Aktivt &#246;nskem&#229;l (ska matchas)
        /// </summary>
        [DataMember(Name="active",EmitDefaultValue=false)]
        public bool Active { get; set; }
    
    }
}