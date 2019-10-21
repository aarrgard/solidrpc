using System.CodeDom.Compiler;
using System.Runtime.Serialization;
using SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.HouseInfo.Estate;
using System.Collections.Generic;
using System;
using SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.CustomField.Models;
namespace SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Estate.Models {
    /// <summary>
    /// OK
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class Cottage {
        /// <summary>
        /// Marknadsf&#246;ring
        /// </summary>
        [DataMember(Name="advertiseOn",EmitDefaultValue=false)]
        public SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Common.Estate.CottageAdvertiseOn AdvertiseOn { get; set; }
    
        /// <summary>
        /// Basinformation
        /// </summary>
        [DataMember(Name="baseInformation",EmitDefaultValue=false)]
        public HouseBaseInformation BaseInformation { get; set; }
    
        /// <summary>
        /// Prisuppgifter
        /// </summary>
        [DataMember(Name="price",EmitDefaultValue=false)]
        public SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Common.Estate.Price Price { get; set; }
    
        /// <summary>
        /// Tilltr&#228;desdatum
        /// </summary>
        [DataMember(Name="date",EmitDefaultValue=false)]
        public SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Common.Estate.Date Date { get; set; }
    
        /// <summary>
        /// Interi&#246;r
        /// </summary>
        [DataMember(Name="houseInterior",EmitDefaultValue=false)]
        public HouseInterior HouseInterior { get; set; }
    
        /// <summary>
        /// Byggnad
        /// </summary>
        [DataMember(Name="building",EmitDefaultValue=false)]
        public HouseBuilding Building { get; set; }
    
        /// <summary>
        /// Ventilation
        /// </summary>
        [DataMember(Name="ventilation",EmitDefaultValue=false)]
        public SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Common.Estate.Ventilation Ventilation { get; set; }
    
        /// <summary>
        /// Energideklaration
        /// </summary>
        [DataMember(Name="energyDeclaration",EmitDefaultValue=false)]
        public SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Common.Estate.EnergyDeclaration EnergyDeclaration { get; set; }
    
        /// <summary>
        /// Tomt
        /// </summary>
        [DataMember(Name="plot",EmitDefaultValue=false)]
        public SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Common.Estate.HousePlotInformation Plot { get; set; }
    
        /// <summary>
        /// Tillg&#229;ngar
        /// </summary>
        [DataMember(Name="assess",EmitDefaultValue=false)]
        public SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Common.Estate.Assess Assess { get; set; }
    
        /// <summary>
        /// Vatten och avlopp
        /// </summary>
        [DataMember(Name="waterAndDrain",EmitDefaultValue=false)]
        public SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Common.Estate.WaterAndDrain WaterAndDrain { get; set; }
    
        /// <summary>
        /// Plan
        /// </summary>
        [DataMember(Name="plan",EmitDefaultValue=false)]
        public SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Common.Estate.Plan Plan { get; set; }
    
        /// <summary>
        /// Inskrivningar
        /// </summary>
        [DataMember(Name="enrollments",EmitDefaultValue=false)]
        public SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Common.Estate.Enrollments Enrollments { get; set; }
    
        /// <summary>
        /// Drift
        /// </summary>
        [DataMember(Name="operation",EmitDefaultValue=false)]
        public SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Common.Estate.Operation Operation { get; set; }
    
        /// <summary>
        /// F&#246;rs&#228;kring
        /// </summary>
        [DataMember(Name="insurance",EmitDefaultValue=false)]
        public SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Common.Estate.Insurance Insurance { get; set; }
    
        /// <summary>
        /// Elf&#246;rbrukning
        /// </summary>
        [DataMember(Name="electricity",EmitDefaultValue=false)]
        public SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Common.Estate.Electricity Electricity { get; set; }
    
        /// <summary>
        /// Inteckningar(lista)
        /// </summary>
        [DataMember(Name="mortages",EmitDefaultValue=false)]
        public IEnumerable<SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Common.Estate.Mortage> Mortages { get; set; }
    
        /// <summary>
        /// Lista &#246;ver aktuella s&#228;ljare representerade av id:n
        /// </summary>
        [DataMember(Name="sellers",EmitDefaultValue=false)]
        public IEnumerable<string> Sellers { get; set; }
    
        /// <summary>
        /// Bud
        /// </summary>
        [DataMember(Name="bids",EmitDefaultValue=false)]
        public IEnumerable<SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Common.Estate.Bid> Bids { get; set; }
    
        /// <summary>
        /// Visningar
        /// </summary>
        [DataMember(Name="viewings",EmitDefaultValue=false)]
        public IEnumerable<SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Common.Estate.Viewing> Viewings { get; set; }
    
        /// <summary>
        /// Marknadsf&#246;rs som
        /// </summary>
        [DataMember(Name="marketedAs",EmitDefaultValue=false)]
        public string MarketedAs { get; set; }
    
        /// <summary>
        /// Uppdragstyp
        /// </summary>
        [DataMember(Name="commissionType",EmitDefaultValue=false)]
        public string CommissionType { get; set; }
    
        /// <summary>
        /// TV och bredband
        /// </summary>
        [DataMember(Name="tvAndBroadband",EmitDefaultValue=false)]
        public SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Common.Estate.TVAndBroadband TvAndBroadband { get; set; }
    
        /// <summary>
        /// N&#228;romr&#229;de
        /// </summary>
        [DataMember(Name="surrounding",EmitDefaultValue=false)]
        public SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Common.Estate.Surrounding Surrounding { get; set; }
    
        /// <summary>
        /// Besiktning
        /// </summary>
        [DataMember(Name="inspection",EmitDefaultValue=false)]
        public SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Estate.Models.HouseInspection Inspection { get; set; }
    
        /// <summary>
        /// Tomtr&#228;tt
        /// </summary>
        [DataMember(Name="leasehold",EmitDefaultValue=false)]
        public SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Common.Estate.Leasehold Leasehold { get; set; }
    
        /// <summary>
        /// Arrende
        /// </summary>
        [DataMember(Name="lease",EmitDefaultValue=false)]
        public SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Common.Estate.Lease Lease { get; set; }
    
        /// <summary>
        /// Lista &#246;ver aktuella k&#246;pare representerade av id:n
        /// </summary>
        [DataMember(Name="buyers",EmitDefaultValue=false)]
        public IEnumerable<string> Buyers { get; set; }
    
        /// <summary>
        /// Internetinst&#228;llningar
        /// </summary>
        [DataMember(Name="internetSettings",EmitDefaultValue=false)]
        public SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Common.Estate.InternetSettings InternetSettings { get; set; }
    
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
        public DateTimeOffset DateChanged { get; set; }
    
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