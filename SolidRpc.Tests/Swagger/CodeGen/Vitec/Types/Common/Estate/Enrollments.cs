using System.CodeDom.Compiler;
using System.Runtime.Serialization;
namespace SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Common.Estate {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class Enrollments {
        /// <summary>
        /// R�ttigheter / Gemensamhetsanl�ggningar
        /// </summary>
        [DataMember(Name="preferentialAndCommunity",EmitDefaultValue=false)]
        public string PreferentialAndCommunity { get; set; }
    
        /// <summary>
        /// Inskrivna servitut och �vriga gravationer
        /// </summary>
        [DataMember(Name="enrolledEasement",EmitDefaultValue=false)]
        public string EnrolledEasement { get; set; }
    
        /// <summary>
        /// Planbest�mmelser
        /// </summary>
        [DataMember(Name="planRegulations",EmitDefaultValue=false)]
        public string PlanRegulations { get; set; }
    
        /// <summary>
        /// R�ttigheter f�rm�n
        /// </summary>
        [DataMember(Name="easementsBenefits",EmitDefaultValue=false)]
        public string EasementsBenefits { get; set; }
    
        /// <summary>
        /// R�ttigheter last
        /// </summary>
        [DataMember(Name="easementsLoad",EmitDefaultValue=false)]
        public string EasementsLoad { get; set; }
    
        /// <summary>
        /// Gemensamhetsanl�ggningar
        /// </summary>
        [DataMember(Name="communityFacilities",EmitDefaultValue=false)]
        public string CommunityFacilities { get; set; }
    
    }
}