using System.CodeDom.Compiler;
using System.Runtime.Serialization;
using SolidRpc.Test.Vitec.Types.Models.Api;
namespace SolidRpc.Test.Vitec.Types.ForeignPropertyInfo.Estate {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class Address {
        /// <summary>
        /// Omr&#229;de
        /// </summary>
        [DataMember(Name="area",EmitDefaultValue=false)]
        public string Area { get; set; }
    
        /// <summary>
        /// WGS84
        /// </summary>
        [DataMember(Name="coordinate",EmitDefaultValue=false)]
        public Coordinate Coordinate { get; set; }
    
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
        [DataMember(Name="postTown",EmitDefaultValue=false)]
        public string PostTown { get; set; }
    
        /// <summary>
        /// Landskod
        /// </summary>
        [DataMember(Name="countryCode",EmitDefaultValue=false)]
        public string CountryCode { get; set; }
    
        /// <summary>
        /// Land
        /// </summary>
        [DataMember(Name="country",EmitDefaultValue=false)]
        public string Country { get; set; }
    
        /// <summary>
        /// Provins
        /// </summary>
        [DataMember(Name="province",EmitDefaultValue=false)]
        public string Province { get; set; }
    
        /// <summary>
        /// Stad
        /// </summary>
        [DataMember(Name="city",EmitDefaultValue=false)]
        public string City { get; set; }
    
        /// <summary>
        /// Omr&#229;desid
        /// </summary>
        [DataMember(Name="areaId",EmitDefaultValue=false)]
        public string AreaId { get; set; }
    
    }
}