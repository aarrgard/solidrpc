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
        /// S�kpreferens Id
        /// </summary>
        [DataMember(Name="id",EmitDefaultValue=false)]
        public string Id { get; set; }
    
        /// <summary>
        /// Lista av omr�den.Endast omr�den med polygoner kan anv�ndas
        /// </summary>
        [DataMember(Name="areaIds",EmitDefaultValue=false)]
        public IEnumerable<string> AreaIds { get; set; }
    
        /// <summary>
        /// L�n, LK-Koder
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
        /// �nskem�let g�ller utlandsbost�der
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
        /// Bostadsr�tt
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
        /// G�rd
        /// </summary>
        [DataMember(Name="farm",EmitDefaultValue=false)]
        public bool Farm { get; set; }
    
        /// <summary>
        /// Hyresr�tt
        /// </summary>
        [DataMember(Name="tenancy",EmitDefaultValue=false)]
        public bool Tenancy { get; set; }
    
        /// <summary>
        /// �vrig boform
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
        /// Special f�rfr�gan
        /// </summary>
        [DataMember(Name="specialRequset",EmitDefaultValue=false)]
        public string SpecialRequset { get; set; }
    
        /// <summary>
        /// Ut�kade krav
        /// </summary>
        [DataMember(Name="increasedRequirementIDs",EmitDefaultValue=false)]
        public IEnumerable<string> IncreasedRequirementIDs { get; set; }
    
        /// <summary>
        /// Aktivt �nskem�l (ska matchas)
        /// </summary>
        [DataMember(Name="active",EmitDefaultValue=false)]
        public bool Active { get; set; }
    
    }
}