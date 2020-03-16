using System.CodeDom.Compiler;
using System.Runtime.Serialization;
using System;
namespace SolidRpc.Test.Vitec.Types.Meeting.Models {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class MeetingCreateria {
        /// <summary>
        /// Kund id
        /// </summary>
        [DataMember(Name="customerId",EmitDefaultValue=false)]
        public string CustomerId { get; set; }
    
        /// <summary>
        /// Datum fr&#229;n
        /// </summary>
        [DataMember(Name="dateFrom",EmitDefaultValue=false)]
        public DateTimeOffset? DateFrom { get; set; }
    
        /// <summary>
        /// Datum till
        /// </summary>
        [DataMember(Name="dateTo",EmitDefaultValue=false)]
        public DateTimeOffset? DateTo { get; set; }
    
        /// <summary>
        /// Typ av m&#246;tesfiltrering.
        /// True resultrerar i att filtrering blir p&#229; bokningsdatum. False resulterar i en filtrering p&#229; m&#246;tesdatum.
        /// </summary>
        [DataMember(Name="search",EmitDefaultValue=false)]
        public bool? Search { get; set; }
    
    }
}