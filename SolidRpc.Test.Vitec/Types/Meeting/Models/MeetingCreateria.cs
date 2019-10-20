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
        /// Datum fr�n
        /// </summary>
        [DataMember(Name="dateFrom",EmitDefaultValue=false)]
        public DateTimeOffset DateFrom { get; set; }
    
        /// <summary>
        /// Datum till
        /// </summary>
        [DataMember(Name="dateTo",EmitDefaultValue=false)]
        public DateTimeOffset DateTo { get; set; }
    
        /// <summary>
        /// Typ av m�tesfiltrering.
        /// True resultrerar i att filtrering blir p� bokningsdatum. False resulterar i en filtrering p� m�tesdatum.
        /// </summary>
        [DataMember(Name="search",EmitDefaultValue=false)]
        public bool Search { get; set; }
    
    }
}