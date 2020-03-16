using System.CodeDom.Compiler;
using System;
using System.Runtime.Serialization;
using SolidRpc.Test.Vitec.Types.Models.Api;
namespace SolidRpc.Test.Vitec.Types.BusinessIntelligense.Models {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class EconomicEstateChangeEntry {
        /// <summary>
        /// Objektet &#228;ndrades vid detta datum
        /// </summary>
        [DataMember(Name="changedAt",EmitDefaultValue=false)]
        public DateTimeOffset? ChangedAt { get; set; }
    
        /// <summary>
        /// Status
        /// </summary>
        [DataMember(Name="status",EmitDefaultValue=false)]
        public DescriptiveId1_Int16 Status { get; set; }
    
        /// <summary>
        /// Provision
        /// </summary>
        [DataMember(Name="commission",EmitDefaultValue=false)]
        public double? Commission { get; set; }
    
        /// <summary>
        /// Provision i utl&#228;ndsk valuta, g&#228;ller endast utlandsbost&#228;der.
        /// </summary>
        [DataMember(Name="commissionForeign",EmitDefaultValue=false)]
        public double? CommissionForeign { get; set; }
    
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