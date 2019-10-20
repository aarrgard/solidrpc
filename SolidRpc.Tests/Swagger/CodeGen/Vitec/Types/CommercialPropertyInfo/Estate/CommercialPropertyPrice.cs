using System.CodeDom.Compiler;
using System.Runtime.Serialization;
using System;
namespace SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.CommercialPropertyInfo.Estate {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class CommercialPropertyPrice {
        /// <summary>
        /// Pris
        /// </summary>
        [DataMember(Name="price",EmitDefaultValue=false)]
        public double Price { get; set; }
    
        /// <summary>
        /// Pristext
        /// </summary>
        [DataMember(Name="priceText",EmitDefaultValue=false)]
        public string PriceText { get; set; }
    
        /// <summary>
        /// Anbud
        /// </summary>
        [DataMember(Name="offer",EmitDefaultValue=false)]
        public bool Offer { get; set; }
    
        /// <summary>
        /// Anbud senast
        /// </summary>
        [DataMember(Name="offerNoLaterThen",EmitDefaultValue=false)]
        public DateTimeOffset OfferNoLaterThen { get; set; }
    
    }
}