using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Runtime.Serialization;
using SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Models.Api;
namespace SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.SearchProfile.Models {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class SearchProfileUpdate {
        /// <summary>
        /// Lista p&#229; omr&#229;desid.
        /// </summary>
        [DataMember(Name="areaIds",EmitDefaultValue=false)]
        public IEnumerable<string> AreaIds { get; set; }
    
        /// <summary>
        /// Lista p&#229; kommunid.
        /// </summary>
        [DataMember(Name="municipIds",EmitDefaultValue=false)]
        public IEnumerable<string> MunicipIds { get; set; }
    
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
        public double MonthlyFee { get; set; }
    
        /// <summary>
        /// Tomtarea anges i ha f&#246;r g&#229;rd och i kvm f&#246;r &#246;vriga
        /// </summary>
        [DataMember(Name="plotArea",EmitDefaultValue=false)]
        public Range1_Int32 PlotArea { get; set; }
    
        /// <summary>
        /// Varav sovrum
        /// </summary>
        [DataMember(Name="bedrooms",EmitDefaultValue=false)]
        public double Bedrooms { get; set; }
    
    }
}