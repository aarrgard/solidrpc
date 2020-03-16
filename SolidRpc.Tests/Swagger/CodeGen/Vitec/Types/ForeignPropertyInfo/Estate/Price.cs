using System.CodeDom.Compiler;
using System;
using System.Runtime.Serialization;
namespace SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.ForeignPropertyInfo.Estate {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class Price {
        /// <summary>
        /// Utg&#229;ngspris
        /// </summary>
        [DataMember(Name="startingPrice",EmitDefaultValue=false)]
        public double? StartingPrice { get; set; }
    
        /// <summary>
        /// Slutpris
        /// </summary>
        [DataMember(Name="finalPrice",EmitDefaultValue=false)]
        public double? FinalPrice { get; set; }
    
        /// <summary>
        /// Valuta
        /// </summary>
        [DataMember(Name="exchangeRate",EmitDefaultValue=false)]
        public double? ExchangeRate { get; set; }
    
        /// <summary>
        /// Valuta
        /// </summary>
        [DataMember(Name="currency",EmitDefaultValue=false)]
        public string Currency { get; set; }
    
        /// <summary>
        /// Text
        /// </summary>
        [DataMember(Name="text",EmitDefaultValue=false)]
        public string Text { get; set; }
    
    }
}