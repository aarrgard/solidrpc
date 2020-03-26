using System.CodeDom.Compiler;
using System.Runtime.Serialization;
using System;
using System.Collections.Generic;
using SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Common.Estate;
namespace SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Criteria.Estate {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class EstateCriteria {
        /// <summary>
        /// Kundid
        /// </summary>
        [DataMember(Name="customerId",EmitDefaultValue=false)]
        public string CustomerId { get; set; }
    
        /// <summary>
        /// Id p&#229; handl&#228;ggare, anv&#228;nds f&#246;r att s&#246;ka p&#229; bost&#228;der som tillh&#246;r en specifik handl&#228;ggare
        /// </summary>
        [DataMember(Name="userId",EmitDefaultValue=false)]
        public string UserId { get; set; }
    
        /// <summary>
        /// Typ av datum som s&#246;kningen ska filtreras p&#229;
        /// </summary>
        [DataMember(Name="typeOfDate",EmitDefaultValue=false)]
        public string TypeOfDate { get; set; }
    
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
        /// Objekttyp (villa, bostadsr&#228;tt, osv)
        /// </summary>
        [DataMember(Name="type",EmitDefaultValue=false)]
        public IEnumerable<string> Type { get; set; }
    
        /// <summary>
        /// Status id (till salu, kommande, s&#229;ld, …)
        /// </summary>
        [DataMember(Name="statuses",EmitDefaultValue=false)]
        public IEnumerable<Status> Statuses { get; set; }
    
        /// <summary>
        /// L&#228;n och kommun kod
        /// </summary>
        [DataMember(Name="countyMunicipalityCode",EmitDefaultValue=false)]
        public string CountyMunicipalityCode { get; set; }
    
    }
}