using System.CodeDom.Compiler;
using System.Runtime.Serialization;
using SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Models.Api;
using System;
namespace SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Common.Estate {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class ObjectAddress {
        /// <summary>
        /// Kommun
        /// </summary>
        [DataMember(Name="municipality",EmitDefaultValue=false)]
        public string Municipality { get; set; }
    
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
        /// LKF-kod
        /// </summary>
        [DataMember(Name="countyMunicipalityParishCode",EmitDefaultValue=false)]
        public string CountyMunicipalityParishCode { get; set; }
    
        /// <summary>
        /// Distrikt kod
        /// </summary>
        [DataMember(Name="districtCode",EmitDefaultValue=false)]
        public string DistrictCode { get; set; }
    
        /// <summary>
        /// Distrikt
        /// </summary>
        [DataMember(Name="district",EmitDefaultValue=false)]
        public string District { get; set; }
    
        /// <summary>
        /// Kommunalskatt
        /// </summary>
        [DataMember(Name="localIncomeTax",EmitDefaultValue=false)]
        public string LocalIncomeTax { get; set; }
    
        /// <summary>
        /// F&#246;rsamling
        /// </summary>
        [DataMember(Name="parish",EmitDefaultValue=false)]
        public string Parish { get; set; }
    
        /// <summary>
        /// L&#228;n
        /// </summary>
        [DataMember(Name="county",EmitDefaultValue=false)]
        public string County { get; set; }
    
        /// <summary>
        /// Omr&#229;desid
        /// </summary>
        [DataMember(Name="areaId",EmitDefaultValue=false)]
        public string AreaId { get; set; }
    
        /// <summary>
        /// Om omr&#229;det &#228;r arkiverat
        /// </summary>
        [DataMember(Name="areaArchived",EmitDefaultValue=false)]
        public bool? AreaArchived { get; set; }
    
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