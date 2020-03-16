using System.CodeDom.Compiler;
using System.Runtime.Serialization;
using System;
using System.Collections.Generic;
using SolidRpc.Test.Vitec.Types.Models.Api;
namespace SolidRpc.Test.Vitec.Types.SearchProfile.Models {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class SearchProfile {
        /// <summary>
        /// S&#246;kprofilid
        /// </summary>
        [DataMember(Name="id",EmitDefaultValue=false)]
        public string Id { get; set; }
    
        /// <summary>
        /// Kontaktid
        /// </summary>
        [DataMember(Name="contactId",EmitDefaultValue=false)]
        public string ContactId { get; set; }
    
        /// <summary>
        /// Villa
        /// </summary>
        [DataMember(Name="house",EmitDefaultValue=false)]
        public bool? House { get; set; }
    
        /// <summary>
        /// Radhus
        /// </summary>
        [DataMember(Name="housingCooperative",EmitDefaultValue=false)]
        public bool? HousingCooperative { get; set; }
    
        /// <summary>
        /// Fritidshus
        /// </summary>
        [DataMember(Name="cottage",EmitDefaultValue=false)]
        public bool? Cottage { get; set; }
    
        /// <summary>
        /// Tomt
        /// </summary>
        [DataMember(Name="plot",EmitDefaultValue=false)]
        public bool? Plot { get; set; }
    
        /// <summary>
        /// Radhus
        /// </summary>
        [DataMember(Name="rowhouse",EmitDefaultValue=false)]
        public bool? Rowhouse { get; set; }
    
        /// <summary>
        /// G&#229;rd
        /// </summary>
        [DataMember(Name="farm",EmitDefaultValue=false)]
        public bool? Farm { get; set; }
    
        /// <summary>
        /// Utland
        /// </summary>
        [DataMember(Name="foreign",EmitDefaultValue=false)]
        public bool? Foreign { get; set; }
    
        /// <summary>
        /// Lista p&#229; omr&#229;den
        /// </summary>
        [DataMember(Name="areas",EmitDefaultValue=false)]
        public IEnumerable<SolidRpc.Test.Vitec.Types.SearchProfile.Models.SearchProfileArea> Areas { get; set; }
    
        /// <summary>
        /// Lista p&#229; kommuner.
        /// </summary>
        [DataMember(Name="municips",EmitDefaultValue=false)]
        public IEnumerable<SolidRpc.Test.Vitec.Types.SearchProfile.Models.SearchProfileMunicip> Municips { get; set; }
    
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
        /// M&#229;nadsavgift
        /// </summary>
        [DataMember(Name="monthlyFee",EmitDefaultValue=false)]
        public double? MonthlyFee { get; set; }
    
        /// <summary>
        /// Tomtarea anges i ha f&#246;r g&#229;rd och i kvm f&#246;r &#246;vriga
        /// </summary>
        [DataMember(Name="plotArea",EmitDefaultValue=false)]
        public Range1_Int32 PlotArea { get; set; }
    
        /// <summary>
        /// Varav sovrum
        /// </summary>
        [DataMember(Name="bedrooms",EmitDefaultValue=false)]
        public double? Bedrooms { get; set; }
    
    }
}