using System.CodeDom.Compiler;
using System.Runtime.Serialization;
namespace SolidRpc.Test.Vitec.Types.Contact.Models {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class IntrestOnObject {
        /// <summary>
        /// Objektid
        /// </summary>
        [DataMember(Name="estateId",EmitDefaultValue=false)]
        public string EstateId { get; set; }
    
        /// <summary>
        /// Objekttyp
        /// </summary>
        [DataMember(Name="type",EmitDefaultValue=false)]
        public string Type { get; set; }
    
        /// <summary>
        /// Objekttypskriterium
        /// </summary>
        [DataMember(Name="estateType",EmitDefaultValue=false)]
        public string EstateType { get; set; }
    
        /// <summary>
        /// Visningsintressent
        /// </summary>
        [DataMember(Name="viewing",EmitDefaultValue=false)]
        public bool Viewing { get; set; }
    
        /// <summary>
        /// Budgivare
        /// </summary>
        [DataMember(Name="bidder",EmitDefaultValue=false)]
        public bool Bidder { get; set; }
    
        /// <summary>
        /// Vilken niv&#229; av intresse som spekulanten har p&#229; bostaden
        /// </summary>
        [DataMember(Name="interestLevel",EmitDefaultValue=false)]
        public string InterestLevel { get; set; }
    
    }
}