using System.CodeDom.Compiler;
using System;
using System.Runtime.Serialization;
namespace SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Common.Estate {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class Price {
        /// <summary>
        /// Utg&#229;ngspris (SEK)
        /// </summary>
        [DataMember(Name="startingPrice",EmitDefaultValue=false)]
        public double? StartingPrice { get; set; }
    
        /// <summary>
        /// Slutpris (SEK)
        /// </summary>
        [DataMember(Name="finalPrice",EmitDefaultValue=false)]
        public double? FinalPrice { get; set; }
    
        /// <summary>
        /// Pris i en annan valuta
        /// </summary>
        [DataMember(Name="priceInOtherCurrencie",EmitDefaultValue=false)]
        public double? PriceInOtherCurrencie { get; set; }
    
        /// <summary>
        /// Annan valuta
        /// </summary>
        [DataMember(Name="otherCurrency",EmitDefaultValue=false)]
        public string OtherCurrency { get; set; }
    
        /// <summary>
        /// Text
        /// </summary>
        [DataMember(Name="text",EmitDefaultValue=false)]
        public string Text { get; set; }
    
    }
}