using System.CodeDom.Compiler;
using System.Runtime.Serialization;
using System.Collections.Generic;
using SolidRpc.Test.Vitec.Types.CommercialPropertyInfo.Estate;
using System;
using SolidRpc.Test.Vitec.Types.CustomField.Models;
namespace SolidRpc.Test.Vitec.Types.Estate.Models {
    /// <summary>
    /// OK
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class CommercialProperty {
        /// <summary>
        /// Marknadsf&#246;ring
        /// </summary>
        [DataMember(Name="advertiseOn",EmitDefaultValue=false)]
        public SolidRpc.Test.Vitec.Types.Common.Estate.AdvertiseOn AdvertiseOn { get; set; }
    
        /// <summary>
        /// Taxering (lista)
        /// </summary>
        [DataMember(Name="assessments",EmitDefaultValue=false)]
        public IEnumerable<CommercialPropertyAssessment> Assessments { get; set; }
    
        /// <summary>
        /// Grundinformation
        /// </summary>
        [DataMember(Name="baseInformation",EmitDefaultValue=false)]
        public CommercialBaseInformation BaseInformation { get; set; }
    
        /// <summary>
        /// Bud
        /// </summary>
        [DataMember(Name="bids",EmitDefaultValue=false)]
        public IEnumerable<SolidRpc.Test.Vitec.Types.Common.Estate.Bid> Bids { get; set; }
    
        /// <summary>
        /// Bostadsbyggnader(lista)
        /// </summary>
        [DataMember(Name="buildings",EmitDefaultValue=false)]
        public IEnumerable<PropertyBuilding> Buildings { get; set; }
    
        /// <summary>
        /// R&#246;relse
        /// </summary>
        [DataMember(Name="business",EmitDefaultValue=false)]
        public CommercialPropertyBusiness Business { get; set; }
    
        /// <summary>
        /// Uppdragstyp
        /// </summary>
        [DataMember(Name="commissionType",EmitDefaultValue=false)]
        public string CommissionType { get; set; }
    
        /// <summary>
        /// Sammanst&#228;llning areor
        /// </summary>
        [DataMember(Name="compilationAreas",EmitDefaultValue=false)]
        public CompilationAreas CompilationAreas { get; set; }
    
        /// <summary>
        /// Tilltr&#228;desdatum
        /// </summary>
        [DataMember(Name="date",EmitDefaultValue=false)]
        public SolidRpc.Test.Vitec.Types.Common.Estate.Date Date { get; set; }
    
        /// <summary>
        /// Ekonomi
        /// </summary>
        [DataMember(Name="price",EmitDefaultValue=false)]
        public CommercialPropertyPrice Price { get; set; }
    
        /// <summary>
        /// Elf&#246;rbrukning
        /// </summary>
        [DataMember(Name="electricity",EmitDefaultValue=false)]
        public SolidRpc.Test.Vitec.Types.Common.Estate.Electricity Electricity { get; set; }
    
        /// <summary>
        /// Renoveringsbehov
        /// </summary>
        [DataMember(Name="renovationInfo",EmitDefaultValue=false)]
        public RenovationInfo RenovationInfo { get; set; }
    
        /// <summary>
        /// Energideklaration
        /// </summary>
        [DataMember(Name="energyDeclaration",EmitDefaultValue=false)]
        public SolidRpc.Test.Vitec.Types.Common.Estate.EnergyDeclaration EnergyDeclaration { get; set; }
    
        /// <summary>
        /// Inskrivningar
        /// </summary>
        [DataMember(Name="enrollments",EmitDefaultValue=false)]
        public SolidRpc.Test.Vitec.Types.Common.Estate.Enrollments Enrollments { get; set; }
    
        /// <summary>
        /// F&#246;rs&#228;kring
        /// </summary>
        [DataMember(Name="insurance",EmitDefaultValue=false)]
        public SolidRpc.Test.Vitec.Types.Common.Estate.Insurance Insurance { get; set; }
    
        /// <summary>
        /// Marknadsf&#246;rs som
        /// </summary>
        [DataMember(Name="marketedAs",EmitDefaultValue=false)]
        public string MarketedAs { get; set; }
    
        /// <summary>
        /// Inteckningar(lista)
        /// </summary>
        [DataMember(Name="mortages",EmitDefaultValue=false)]
        public IEnumerable<SolidRpc.Test.Vitec.Types.Common.Estate.Mortage> Mortages { get; set; }
    
        /// <summary>
        /// Drift
        /// </summary>
        [DataMember(Name="operation",EmitDefaultValue=false)]
        public CommercialOperation Operation { get; set; }
    
        /// <summary>
        /// Tomtinformation
        /// </summary>
        [DataMember(Name="plotInformation",EmitDefaultValue=false)]
        public CommercialPropertyPlotInformation PlotInformation { get; set; }
    
        /// <summary>
        /// Objekttyp, beskrivningar
        /// </summary>
        [DataMember(Name="propertytypeDescription",EmitDefaultValue=false)]
        public PropertytypeDescription PropertytypeDescription { get; set; }
    
        /// <summary>
        /// Lista &#246;ver aktuella s&#228;ljare representerade av Id:n
        /// </summary>
        [DataMember(Name="sellers",EmitDefaultValue=false)]
        public IEnumerable<string> Sellers { get; set; }
    
        /// <summary>
        /// Gemensamma utrymmen
        /// </summary>
        [DataMember(Name="sharedSpaces",EmitDefaultValue=false)]
        public SharedSpaces SharedSpaces { get; set; }
    
        /// <summary>
        /// N&#228;romr&#229;de
        /// </summary>
        [DataMember(Name="surrounding",EmitDefaultValue=false)]
        public SolidRpc.Test.Vitec.Types.Common.Estate.Surrounding Surrounding { get; set; }
    
        /// <summary>
        /// Teknisk data
        /// </summary>
        [DataMember(Name="technicalData",EmitDefaultValue=false)]
        public TechnicalData TechnicalData { get; set; }
    
        /// <summary>
        /// Visningar
        /// </summary>
        [DataMember(Name="viewings",EmitDefaultValue=false)]
        public IEnumerable<SolidRpc.Test.Vitec.Types.Common.Estate.Viewing> Viewings { get; set; }
    
        /// <summary>
        /// Lista &#246;ver aktuella k&#246;pare representerade av id:n
        /// </summary>
        [DataMember(Name="buyers",EmitDefaultValue=false)]
        public IEnumerable<string> Buyers { get; set; }
    
        /// <summary>
        /// Internetinst&#228;llningar
        /// </summary>
        [DataMember(Name="internetSettings",EmitDefaultValue=false)]
        public SolidRpc.Test.Vitec.Types.Common.Estate.InternetSettings InternetSettings { get; set; }
    
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
        public SolidRpc.Test.Vitec.Types.Common.Estate.Assignment Assignment { get; set; }
    
        /// <summary>
        /// Beskrivning
        /// </summary>
        [DataMember(Name="description",EmitDefaultValue=false)]
        public SolidRpc.Test.Vitec.Types.Common.Estate.Description Description { get; set; }
    
        /// <summary>
        /// Bilder
        /// </summary>
        [DataMember(Name="images",EmitDefaultValue=false)]
        public IEnumerable<SolidRpc.Test.Vitec.Types.Common.Estate.ObjectImage> Images { get; set; }
    
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