using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System;
using SolidRpc.Test.Vitec.Types.Models.Api;
using SolidRpc.Test.Vitec.Types.ForeignPropertyInfo.Estate;
namespace SolidRpc.Test.Vitec.Types.SearchProfile.Models {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class ForeignSearchProfileCreate {
        /// <summary>
        /// Lista p&#229; omr&#229;desid.
        /// </summary>
        [DataMember(Name="areaIds",EmitDefaultValue=false)]
        public IEnumerable<string> AreaIds { get; set; }
    
        /// <summary>
        /// L&#228;genhet
        /// </summary>
        [DataMember(Name="apartment",EmitDefaultValue=false)]
        public bool? Apartment { get; set; }
    
        /// <summary>
        /// Villa
        /// </summary>
        [DataMember(Name="house",EmitDefaultValue=false)]
        public bool? House { get; set; }
    
        /// <summary>
        /// Radhus
        /// </summary>
        [DataMember(Name="terraceHouse",EmitDefaultValue=false)]
        public bool? TerraceHouse { get; set; }
    
        /// <summary>
        /// Tomt
        /// </summary>
        [DataMember(Name="plot",EmitDefaultValue=false)]
        public bool? Plot { get; set; }
    
        /// <summary>
        /// Boarea
        /// </summary>
        [DataMember(Name="livingSpace",EmitDefaultValue=false)]
        public Range1_Int16 LivingSpace { get; set; }
    
        /// <summary>
        /// Antal rum
        /// </summary>
        [DataMember(Name="numberOfRooms",EmitDefaultValue=false)]
        public Range1_Double NumberOfRooms { get; set; }
    
        /// <summary>
        /// Pris
        /// </summary>
        [DataMember(Name="price",EmitDefaultValue=false)]
        public Range1_Int32 Price { get; set; }
    
        /// <summary>
        /// Valuta
        /// </summary>
        [DataMember(Name="exchangeRate",EmitDefaultValue=false)]
        public double? ExchangeRate { get; set; }
    
        /// <summary>
        /// Valuta
        /// </summary>
        [DataMember(Name="currency",EmitDefaultValue=false)]
        public string Currency { get; set; }
    
        /// <summary>
        /// Special f&#246;rfr&#229;gan
        /// </summary>
        [DataMember(Name="specialRequest",EmitDefaultValue=false)]
        public string SpecialRequest { get; set; }
    
        /// <summary>
        /// Ritat omr&#229;de
        /// </summary>
        [DataMember(Name="drawnAreas",EmitDefaultValue=false)]
        public IEnumerable<Polygon> DrawnAreas { get; set; }
    
        /// <summary>
        /// Ut&#246;kade krav
        /// </summary>
        [DataMember(Name="increasedRequirementIds",EmitDefaultValue=false)]
        public IEnumerable<string> IncreasedRequirementIds { get; set; }
    
        /// <summary>
        /// Avst&#229;nd
        /// </summary>
        [DataMember(Name="distance",EmitDefaultValue=false)]
        public Distance Distance { get; set; }
    
        /// <summary>
        /// Pool
        /// </summary>
        [DataMember(Name="swimmingPool",EmitDefaultValue=false)]
        public bool? SwimmingPool { get; set; }
    
        /// <summary>
        /// Balkong
        /// </summary>
        [DataMember(Name="balcony",EmitDefaultValue=false)]
        public bool? Balcony { get; set; }
    
        /// <summary>
        /// Terrass
        /// </summary>
        [DataMember(Name="terrace",EmitDefaultValue=false)]
        public bool? Terrace { get; set; }
    
    }
}