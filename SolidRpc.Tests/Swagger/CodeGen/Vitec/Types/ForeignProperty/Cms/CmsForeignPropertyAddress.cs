using System.CodeDom.Compiler;
using System.Runtime.Serialization;
using SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Models.Api;
namespace SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.ForeignProperty.Cms {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class CmsForeignPropertyAddress {
        /// <summary>
        /// Provins
        /// </summary>
        [DataMember(Name="province",EmitDefaultValue=false)]
        public string Province { get; set; }
    
        /// <summary>
        /// WGS84
        /// </summary>
        [DataMember(Name="coordinate",EmitDefaultValue=false)]
        public Coordinate Coordinate { get; set; }
    
        /// <summary>
        /// Stad
        /// </summary>
        [DataMember(Name="town",EmitDefaultValue=false)]
        public string Town { get; set; }
    
        /// <summary>
        /// Omr�desid
        /// </summary>
        [DataMember(Name="areaId",EmitDefaultValue=false)]
        public string AreaId { get; set; }
    
        /// <summary>
        /// Gatuadress
        /// </summary>
        [DataMember(Name="streetAddress",EmitDefaultValue=false)]
        public string StreetAddress { get; set; }
    
        /// <summary>
        /// Postnummer
        /// </summary>
        [DataMember(Name="zipCode",EmitDefaultValue=false)]
        public string ZipCode { get; set; }
    
        /// <summary>
        /// Postort
        /// </summary>
        [DataMember(Name="city",EmitDefaultValue=false)]
        public string City { get; set; }
    
        /// <summary>
        /// Landskod
        /// </summary>
        [DataMember(Name="countryCode",EmitDefaultValue=false)]
        public string CountryCode { get; set; }
    
    }
}