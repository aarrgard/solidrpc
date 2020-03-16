using System.CodeDom.Compiler;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System;
using SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.CustomField.Models;
namespace SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Estate.Models {
    /// <summary>
    /// OK
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class Premises {
        /// <summary>
        /// Marknadsf&#246;ring
        /// </summary>
        [DataMember(Name="advertiseOn",EmitDefaultValue=false)]
        public SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Common.Estate.AdvertiseOn AdvertiseOn { get; set; }
    
        /// <summary>
        /// Grundinformation
        /// </summary>
        [DataMember(Name="baseInformation",EmitDefaultValue=false)]
        public SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Estate.Models.PremisesBaseInformation BaseInformation { get; set; }
    
        /// <summary>
        /// Sammanst&#228;llning areor
        /// </summary>
        [DataMember(Name="compilationAreas",EmitDefaultValue=false)]
        public SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Estate.Models.CompilationAreas CompilationAreas { get; set; }
    
        /// <summary>
        /// Tilltr&#228;desdatum
        /// </summary>
        [DataMember(Name="date",EmitDefaultValue=false)]
        public SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Common.Estate.Date Date { get; set; }
    
        /// <summary>
        /// Ekonomi
        /// </summary>
        [DataMember(Name="price",EmitDefaultValue=false)]
        public SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Common.Estate.Price Price { get; set; }
    
        /// <summary>
        /// Tomt, uteplats,bilplats
        /// </summary>
        [DataMember(Name="balconyPatio",EmitDefaultValue=false)]
        public SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Common.Estate.BalconyPatio BalconyPatio { get; set; }
    
        /// <summary>
        /// V&#229;ning/Hiss
        /// </summary>
        [DataMember(Name="floorAndElevator",EmitDefaultValue=false)]
        public SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Estate.Models.PremisesFloorAndElevator FloorAndElevator { get; set; }
    
        /// <summary>
        /// Omgivningar
        /// </summary>
        [DataMember(Name="surrounding",EmitDefaultValue=false)]
        public SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Estate.Models.PremisesSurrounding Surrounding { get; set; }
    
        /// <summary>
        /// Teknisk data
        /// </summary>
        [DataMember(Name="technicalData",EmitDefaultValue=false)]
        public SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Estate.Models.TechnicalData TechnicalData { get; set; }
    
        /// <summary>
        /// Visningar
        /// </summary>
        [DataMember(Name="viewings",EmitDefaultValue=false)]
        public IEnumerable<SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Common.Estate.Viewing> Viewings { get; set; }
    
        /// <summary>
        /// &#196;gare/uthyrare
        /// </summary>
        [DataMember(Name="owners",EmitDefaultValue=false)]
        public IEnumerable<SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Estate.Models.Owner> Owners { get; set; }
    
        /// <summary>
        /// Hyresg&#228;ster
        /// </summary>
        [DataMember(Name="tenants",EmitDefaultValue=false)]
        public IEnumerable<SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Estate.Models.Tenant> Tenants { get; set; }
    
        /// <summary>
        /// Byggnad
        /// </summary>
        [DataMember(Name="building",EmitDefaultValue=false)]
        public SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Estate.Models.PremisesBuilding Building { get; set; }
    
        /// <summary>
        /// Kort bostadsid.
        /// </summary>
        [DataMember(Name="webbId",EmitDefaultValue=false)]
        public string WebbId { get; set; }
    
        /// <summary>
        /// Bostadsid
        /// </summary>
        [DataMember(Name="estateId",EmitDefaultValue=false)]
        public string EstateId { get; set; }
    
        /// <summary>
        /// Kundid
        /// </summary>
        [DataMember(Name="customerId",EmitDefaultValue=false)]
        public string CustomerId { get; set; }
    
        /// <summary>
        /// Kontorsid
        /// </summary>
        [DataMember(Name="officeId",EmitDefaultValue=false)]
        public string OfficeId { get; set; }
    
        /// <summary>
        /// &#196;ndringsdatum
        /// </summary>
        [DataMember(Name="dateChanged",EmitDefaultValue=false)]
        public System.DateTimeOffset? DateChanged { get; set; }
    
        /// <summary>
        /// Uppdraget
        /// </summary>
        [DataMember(Name="assignment",EmitDefaultValue=false)]
        public SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Common.Estate.Assignment Assignment { get; set; }
    
        /// <summary>
        /// Beskrivning
        /// </summary>
        [DataMember(Name="description",EmitDefaultValue=false)]
        public SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Common.Estate.Description Description { get; set; }
    
        /// <summary>
        /// Bilder
        /// </summary>
        [DataMember(Name="images",EmitDefaultValue=false)]
        public IEnumerable<SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Common.Estate.ObjectImage> Images { get; set; }
    
        /// <summary>
        /// Lista &#246;ver aktuella intressenter representerade av id:n
        /// </summary>
        [DataMember(Name="interests",EmitDefaultValue=false)]
        public IEnumerable<string> Interests { get; set; }
    
        /// <summary>
        /// Egendefinierade f&#228;lt
        /// </summary>
        [DataMember(Name="customFields",EmitDefaultValue=false)]
        public IEnumerable<FieldValue> CustomFields { get; set; }
    
    }
}