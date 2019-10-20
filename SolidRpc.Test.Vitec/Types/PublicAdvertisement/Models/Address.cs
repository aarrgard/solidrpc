using System.CodeDom.Compiler;
using System.Runtime.Serialization;
namespace SolidRpc.Test.Vitec.Types.PublicAdvertisement.Models {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class Address {
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
        /// Ort
        /// </summary>
        [DataMember(Name="city",EmitDefaultValue=false)]
        public string City { get; set; }
    
        /// <summary>
        /// Omr�desnamn
        /// </summary>
        [DataMember(Name="areaName",EmitDefaultValue=false)]
        public string AreaName { get; set; }
    
        /// <summary>
        /// Landskod
        /// </summary>
        [DataMember(Name="countryCode",EmitDefaultValue=false)]
        public string CountryCode { get; set; }
    
        /// <summary>
        /// LKF kod
        /// </summary>
        [DataMember(Name="countyMunicipalityParishCode",EmitDefaultValue=false)]
        public string CountyMunicipalityParishCode { get; set; }
    
        /// <summary>
        /// Wgs84 koordinat
        /// </summary>
        [DataMember(Name="wgs84Coordinate",EmitDefaultValue=false)]
        public SolidRpc.Test.Vitec.Types.PublicAdvertisement.Models.Wgs84Coordinate Wgs84Coordinate { get; set; }
    
        /// <summary>
        /// V�gbeskrivning
        /// </summary>
        [DataMember(Name="directions",EmitDefaultValue=false)]
        public string Directions { get; set; }
    
    }
}