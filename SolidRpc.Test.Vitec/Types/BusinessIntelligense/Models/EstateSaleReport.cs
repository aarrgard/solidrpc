using System.CodeDom.Compiler;
using System.Runtime.Serialization;
using SolidRpc.Test.Vitec.Types.Models.Api;
using System.Collections.Generic;
using System;
namespace SolidRpc.Test.Vitec.Types.BusinessIntelligense.Models {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class EstateSaleReport {
        /// <summary>
        /// Objektets id
        /// </summary>
        [DataMember(Name="id",EmitDefaultValue=false)]
        public string Id { get; set; }
    
        /// <summary>
        /// Objektnummer
        /// </summary>
        [DataMember(Name="estateNumber",EmitDefaultValue=false)]
        public string EstateNumber { get; set; }
    
        /// <summary>
        /// Status
        /// </summary>
        [DataMember(Name="status",EmitDefaultValue=false)]
        public DescriptiveId1_Int16 Status { get; set; }
    
        /// <summary>
        /// Typ av objekt
        /// </summary>
        [DataMember(Name="type",EmitDefaultValue=false)]
        public DescriptiveId1_Int16 Type { get; set; }
    
        /// <summary>
        /// Uppdragstyp
        /// </summary>
        [DataMember(Name="assignment",EmitDefaultValue=false)]
        public DescriptiveId1_Int16 Assignment { get; set; }
    
        /// <summary>
        /// Adressinformation
        /// </summary>
        [DataMember(Name="address",EmitDefaultValue=false)]
        public SolidRpc.Test.Vitec.Types.BusinessIntelligense.Models.EconomicAddress Address { get; set; }
    
        /// <summary>
        /// M&#228;klarna som ska ha provision
        /// </summary>
        [DataMember(Name="agents",EmitDefaultValue=false)]
        public IEnumerable<SolidRpc.Test.Vitec.Types.BusinessIntelligense.Models.SaleReportEstateAgent> Agents { get; set; }
    
        /// <summary>
        /// Kontraktsdag
        /// </summary>
        [DataMember(Name="contractDay",EmitDefaultValue=false)]
        public DateTimeOffset? ContractDay { get; set; }
    
        /// <summary>
        /// Tilltr&#228;desdag
        /// </summary>
        [DataMember(Name="accessDay",EmitDefaultValue=false)]
        public DateTimeOffset? AccessDay { get; set; }
    
        /// <summary>
        /// Handpenning redovisningsdatum
        /// </summary>
        [DataMember(Name="depositReportedAt",EmitDefaultValue=false)]
        public DateTimeOffset? DepositReportedAt { get; set; }
    
        /// <summary>
        /// Totala provision
        /// </summary>
        [DataMember(Name="commission",EmitDefaultValue=false)]
        public double? Commission { get; set; }
    
        /// <summary>
        /// Totala provision i utl&#228;ndsk valuta, g&#228;ller endast utlandsbost&#228;der.
        /// </summary>
        [DataMember(Name="commissionForeign",EmitDefaultValue=false)]
        public double? CommissionForeign { get; set; }
    
        /// <summary>
        /// Fritext f&#246;r fast arvode
        /// </summary>
        [DataMember(Name="commissionText",EmitDefaultValue=false)]
        public string CommissionText { get; set; }
    
        /// <summary>
        /// Provisionsdatum
        /// </summary>
        [DataMember(Name="commissionReceivedAt",EmitDefaultValue=false)]
        public DateTimeOffset? CommissionReceivedAt { get; set; }
    
        /// <summary>
        /// Nyproduktion (ja/nej)
        /// </summary>
        [DataMember(Name="isNewHome",EmitDefaultValue=false)]
        public bool? IsNewHome { get; set; }
    
        /// <summary>
        /// Koordinat WGS84.
        /// </summary>
        [DataMember(Name="wgs84Coordinate",EmitDefaultValue=false)]
        public Coordinate Wgs84Coordinate { get; set; }
    
        /// <summary>
        /// Intagsk&#228;lla
        /// </summary>
        [DataMember(Name="intakeSource",EmitDefaultValue=false)]
        public DescriptiveId1_Int32 IntakeSource { get; set; }
    
        /// <summary>
        /// N&#228;r objektinformationen uppdaterades senast
        /// </summary>
        [DataMember(Name="changedAt",EmitDefaultValue=false)]
        public DateTimeOffset? ChangedAt { get; set; }
    
        /// <summary>
        /// &#196;ndringshistorik. Detta &#228;r en lista p&#229; uppdateringar och vad f&#228;lten var satta till vid dessa uppdateringar
        /// </summary>
        [DataMember(Name="changes",EmitDefaultValue=false)]
        public IEnumerable<SolidRpc.Test.Vitec.Types.BusinessIntelligense.Models.EconomicEstateChangeEntry> Changes { get; set; }
    
        /// <summary>
        /// Startpris
        /// </summary>
        [DataMember(Name="startingPrice",EmitDefaultValue=false)]
        public long? StartingPrice { get; set; }
    
        /// <summary>
        /// Slutpris
        /// </summary>
        [DataMember(Name="finalPrice",EmitDefaultValue=false)]
        public long? FinalPrice { get; set; }
    
        /// <summary>
        /// Startpris i utl&#228;ndsk valuta, g&#228;ller endast utlandsbost&#228;der.
        /// </summary>
        [DataMember(Name="startingPriceForeign",EmitDefaultValue=false)]
        public long? StartingPriceForeign { get; set; }
    
        /// <summary>
        /// Slutpris i utl&#228;ndsk valuta, g&#228;ller endast utlandsbost&#228;der.
        /// </summary>
        [DataMember(Name="finalPriceForeign",EmitDefaultValue=false)]
        public long? FinalPriceForeign { get; set; }
    
        /// <summary>
        /// Valutakod (SEK, EUR, USD etc), g&#228;ller endast utlandsbost&#228;der.
        /// </summary>
        [DataMember(Name="currencyCode",EmitDefaultValue=false)]
        public string CurrencyCode { get; set; }
    
        /// <summary>
        /// V&#228;xlingskurs, g&#228;ller endast utlandsbost&#228;der.
        /// </summary>
        [DataMember(Name="currencyExchangeRate",EmitDefaultValue=false)]
        public double? CurrencyExchangeRate { get; set; }
    
    }
}