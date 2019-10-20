using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Runtime.Serialization;
using SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.ForeignPropertyInfo.Estate;
using System;
using SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.CustomField.Models;
namespace SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Estate.Models {
    /// <summary>
    /// OK
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class ForeignProperty {
        /// <summary>
        /// Lista av beskrivningar
        /// </summary>
        [DataMember(Name="descriptions",EmitDefaultValue=false)]
        public IEnumerable<SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Common.Estate.Description> Descriptions { get; set; }
    
        /// <summary>
        /// Marknadsf�ring
        /// </summary>
        [DataMember(Name="advertiseOn",EmitDefaultValue=false)]
        public SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Common.Estate.AdvertiseOn AdvertiseOn { get; set; }
    
        /// <summary>
        /// Basinformation
        /// </summary>
        [DataMember(Name="baseInformation",EmitDefaultValue=false)]
        public ForeignPropertyBaseInformation BaseInformation { get; set; }
    
        /// <summary>
        /// Prisuppgifter
        /// </summary>
        [DataMember(Name="price",EmitDefaultValue=false)]
        public Price Price { get; set; }
    
        /// <summary>
        /// Datum
        /// </summary>
        [DataMember(Name="dates",EmitDefaultValue=false)]
        public Date Dates { get; set; }
    
        /// <summary>
        /// Interi�r
        /// </summary>
        [DataMember(Name="interior",EmitDefaultValue=false)]
        public ForeignPropertyInterior Interior { get; set; }
    
        /// <summary>
        /// Byggnad
        /// </summary>
        [DataMember(Name="building",EmitDefaultValue=false)]
        public ForeignPropertyBuilding Building { get; set; }
    
        /// <summary>
        /// V�ning/hiss
        /// </summary>
        [DataMember(Name="floorAndElevator",EmitDefaultValue=false)]
        public FloorAndElevator FloorAndElevator { get; set; }
    
        /// <summary>
        /// Energideklaration
        /// </summary>
        [DataMember(Name="energyDeclaration",EmitDefaultValue=false)]
        public SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Common.Estate.EnergyDeclaration EnergyDeclaration { get; set; }
    
        /// <summary>
        /// Tomt
        /// </summary>
        [DataMember(Name="plot",EmitDefaultValue=false)]
        public PlotInformation Plot { get; set; }
    
        /// <summary>
        /// Tillg�ngar
        /// </summary>
        [DataMember(Name="assess",EmitDefaultValue=false)]
        public Assess Assess { get; set; }
    
        /// <summary>
        /// Distans
        /// </summary>
        [DataMember(Name="distance",EmitDefaultValue=false)]
        public Distance Distance { get; set; }
    
        /// <summary>
        /// Vatten och avlopp
        /// </summary>
        [DataMember(Name="waterAndDrain",EmitDefaultValue=false)]
        public SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Common.Estate.WaterAndDrain WaterAndDrain { get; set; }
    
        /// <summary>
        /// Drift
        /// </summary>
        [DataMember(Name="operation",EmitDefaultValue=false)]
        public Operation Operation { get; set; }
    
        /// <summary>
        /// Elf�rbrukning
        /// </summary>
        [DataMember(Name="electricity",EmitDefaultValue=false)]
        public SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Common.Estate.Electricity Electricity { get; set; }
    
        /// <summary>
        /// Lista �ver aktuella s�ljare representerade av Id:n
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
        /// Marknadsf�rs som
        /// </summary>
        [DataMember(Name="marketedAs",EmitDefaultValue=false)]
        public string MarketedAs { get; set; }
    
        /// <summary>
        /// Uppdragstyp
        /// </summary>
        [DataMember(Name="commissionType",EmitDefaultValue=false)]
        public string CommissionType { get; set; }
    
        /// <summary>
        /// N�romr�de
        /// </summary>
        [DataMember(Name="surrounding",EmitDefaultValue=false)]
        public IEnumerable<Surrounding> Surrounding { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name="exterior",EmitDefaultValue=false)]
        public Exterior Exterior { get; set; }
    
        /// <summary>
        /// Lista �ver aktuella k�pare representerade av id:n
        /// </summary>
        [DataMember(Name="buyers",EmitDefaultValue=false)]
        public IEnumerable<string> Buyers { get; set; }
    
        /// <summary>
        /// Internetinst�llningar
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
        /// �ndringsdatum
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
        /// Lista �ver aktuella intressenter representerade av id:n
        /// </summary>
        [DataMember(Name="interests",EmitDefaultValue=false)]
        public IEnumerable<string> Interests { get; set; }
    
        /// <summary>
        /// Egendefinierade f�lt
        /// </summary>
        [DataMember(Name="customFields",EmitDefaultValue=false)]
        public IEnumerable<FieldValue> CustomFields { get; set; }
    
    }
}