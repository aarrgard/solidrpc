using System.CodeDom.Compiler;
using System.Runtime.Serialization;
namespace SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Common.Estate {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class Enrollments {
        /// <summary>
        /// R&#228;ttigheter / Gemensamhetsanl&#228;ggningar
        /// </summary>
        [DataMember(Name="preferentialAndCommunity",EmitDefaultValue=false)]
        public string PreferentialAndCommunity { get; set; }
    
        /// <summary>
        /// Inskrivna servitut och &#246;vriga gravationer
        /// </summary>
        [DataMember(Name="enrolledEasement",EmitDefaultValue=false)]
        public string EnrolledEasement { get; set; }
    
        /// <summary>
        /// Planbest&#228;mmelser
        /// </summary>
        [DataMember(Name="planRegulations",EmitDefaultValue=false)]
        public string PlanRegulations { get; set; }
    
        /// <summary>
        /// R&#228;ttigheter f&#246;rm&#229;n
        /// </summary>
        [DataMember(Name="easementsBenefits",EmitDefaultValue=false)]
        public string EasementsBenefits { get; set; }
    
        /// <summary>
        /// R&#228;ttigheter last
        /// </summary>
        [DataMember(Name="easementsLoad",EmitDefaultValue=false)]
        public string EasementsLoad { get; set; }
    
        /// <summary>
        /// Gemensamhetsanl&#228;ggningar
        /// </summary>
        [DataMember(Name="communityFacilities",EmitDefaultValue=false)]
        public string CommunityFacilities { get; set; }
    
    }
}