using System.CodeDom.Compiler;
using System.Runtime.Serialization;
using System;
using System.Collections.Generic;
using SolidRpc.Test.Vitec.Types.Common.Estate;
namespace SolidRpc.Test.Vitec.Types.Criteria.Estate {
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
        /// Id p� handl�ggare, anv�nds f�r att s�ka p� bost�der som tillh�r en specifik handl�ggare
        /// </summary>
        [DataMember(Name="userId",EmitDefaultValue=false)]
        public string UserId { get; set; }
    
        /// <summary>
        /// Typ av datum som s�kningen ska filtreras p�
        /// </summary>
        [DataMember(Name="typeOfDate",EmitDefaultValue=false)]
        public string TypeOfDate { get; set; }
    
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
        /// Objekttyp (villa, bostadsr�tt, osv)
        /// </summary>
        [DataMember(Name="type",EmitDefaultValue=false)]
        public IEnumerable<string> Type { get; set; }
    
        /// <summary>
        /// Status id (till salu, kommande, s�ld, �)
        /// </summary>
        [DataMember(Name="statuses",EmitDefaultValue=false)]
        public IEnumerable<Status> Statuses { get; set; }
    
        /// <summary>
        /// L�n och kommun kod
        /// </summary>
        [DataMember(Name="countyMunicipalityCode",EmitDefaultValue=false)]
        public string CountyMunicipalityCode { get; set; }
    
    }
}