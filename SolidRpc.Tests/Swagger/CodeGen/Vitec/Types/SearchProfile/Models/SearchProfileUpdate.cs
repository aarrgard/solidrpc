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
        /// Lista p� omr�desid.
        /// </summary>
        [DataMember(Name="areaIds",EmitDefaultValue=false)]
        public IEnumerable<string> AreaIds { get; set; }
    
        /// <summary>
        /// Lista p� kommunid.
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
        /// Special f�rfr�gan
        /// </summary>
        [DataMember(Name="specialRequest",EmitDefaultValue=false)]
        public string SpecialRequest { get; set; }
    
        /// <summary>
        /// Ritat omr�de
        /// </summary>
        [DataMember(Name="drawnAreas",EmitDefaultValue=false)]
        public IEnumerable<Polygon> DrawnAreas { get; set; }
    
        /// <summary>
        /// Ut�kade krav
        /// </summary>
        [DataMember(Name="increasedRequirementIds",EmitDefaultValue=false)]
        public IEnumerable<string> IncreasedRequirementIds { get; set; }
    
        /// <summary>
        /// M�nadsavgift
        /// </summary>
        [DataMember(Name="monthlyFee",EmitDefaultValue=false)]
        public double MonthlyFee { get; set; }
    
        /// <summary>
        /// Tomtarea anges i ha f�r g�rd och i kvm f�r �vriga
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