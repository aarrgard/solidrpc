using System.CodeDom.Compiler;
using System;
using System.Runtime.Serialization;
namespace SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Maklarstatistik.Models {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class Criteria {
        /// <summary>
        /// &#196;ndringsdatum fr&#229;n och med
        /// </summary>
        [DataMember(Name="changeAtFrom",EmitDefaultValue=false)]
        public DateTimeOffset ChangeAtFrom { get; set; }
    
        /// <summary>
        /// &#196;ndringsdatum till och med
        /// </summary>
        [DataMember(Name="changeAtTo",EmitDefaultValue=false)]
        public DateTimeOffset ChangeAtTo { get; set; }
    
        /// <summary>
        /// Kontraktsdatum fr&#229;n och med, max tre m&#229;nader tidigare &#228;n ContractDateTo
        /// </summary>
        [DataMember(Name="contractDateFrom",EmitDefaultValue=false)]
        public DateTimeOffset ContractDateFrom { get; set; }
    
        /// <summary>
        /// Kontraktsdatum till och med, max tre m&#229;nader senare &#228;n ContractDateFrom
        /// </summary>
        [DataMember(Name="contractDateTo",EmitDefaultValue=false)]
        public DateTimeOffset ContractDateTo { get; set; }
    
    }
}