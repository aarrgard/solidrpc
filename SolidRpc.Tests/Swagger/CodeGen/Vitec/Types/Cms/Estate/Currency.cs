using System.CodeDom.Compiler;
using System.Runtime.Serialization;
namespace SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Cms.Estate {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class Currency {
        /// <summary>
        /// Kurs anges enligt ###.##
        /// </summary>
        [DataMember(Name="exchangeRate",EmitDefaultValue=false)]
        public string ExchangeRate { get; set; }
    
        /// <summary>
        /// Valuta enligt ISO 4217 https://sv.wikipedia.org/wiki/ISO_4217
        /// </summary>
        [DataMember(Name="currencyCode",EmitDefaultValue=false)]
        public string CurrencyCode { get; set; }
    
    }
}