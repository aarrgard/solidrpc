using System.CodeDom.Compiler;
using System.Runtime.Serialization;
namespace SolidRpc.Test.Vitec.Types.CondominiumInfo.Estate {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class CondominiumOperation {
        /// <summary>
        /// &#197;rsavgift till samf&#228;llighet
        /// </summary>
        [DataMember(Name="annualFeeCommunity",EmitDefaultValue=false)]
        public int AnnualFeeCommunity { get; set; }
    
        /// <summary>
        /// Vad som ing&#229;r i &#229;rsavgiften
        /// </summary>
        [DataMember(Name="annualFeeCommunityIncluded",EmitDefaultValue=false)]
        public string AnnualFeeCommunityIncluded { get; set; }
    
        /// <summary>
        /// Kommentar till &#229;rsavgiften
        /// </summary>
        [DataMember(Name="commentAnnualFee",EmitDefaultValue=false)]
        public string CommentAnnualFee { get; set; }
    
        /// <summary>
        /// Uppv&#228;rmningskostnad
        /// </summary>
        [DataMember(Name="heating",EmitDefaultValue=false)]
        public double Heating { get; set; }
    
        /// <summary>
        /// Elkostnad
        /// </summary>
        [DataMember(Name="electricity",EmitDefaultValue=false)]
        public double Electricity { get; set; }
    
        /// <summary>
        /// Kostnad f&#246;r vatten och avlopp
        /// </summary>
        [DataMember(Name="waterAndDrain",EmitDefaultValue=false)]
        public double WaterAndDrain { get; set; }
    
        /// <summary>
        /// Sotningsavgift
        /// </summary>
        [DataMember(Name="chimneySweeping",EmitDefaultValue=false)]
        public double ChimneySweeping { get; set; }
    
        /// <summary>
        /// Renh&#229;llningsavgift
        /// </summary>
        [DataMember(Name="sanitation",EmitDefaultValue=false)]
        public double Sanitation { get; set; }
    
        /// <summary>
        /// F&#246;rs&#228;kring
        /// </summary>
        [DataMember(Name="insurance",EmitDefaultValue=false)]
        public double Insurance { get; set; }
    
        /// <summary>
        /// &#214;vrigt
        /// </summary>
        [DataMember(Name="other",EmitDefaultValue=false)]
        public double Other { get; set; }
    
        /// <summary>
        /// Total driftkostnad
        /// </summary>
        [DataMember(Name="sum",EmitDefaultValue=false)]
        public double Sum { get; set; }
    
        /// <summary>
        /// Antal perssoner i hush&#229;llet
        /// </summary>
        [DataMember(Name="personsInTheHousehold",EmitDefaultValue=false)]
        public int PersonsInTheHousehold { get; set; }
    
        /// <summary>
        /// Kommentar
        /// </summary>
        [DataMember(Name="commentary",EmitDefaultValue=false)]
        public string Commentary { get; set; }
    
        /// <summary>
        /// V&#228;gavg/sn&#246;
        /// </summary>
        [DataMember(Name="roadCharge",EmitDefaultValue=false)]
        public double RoadCharge { get; set; }
    
        /// <summary>
        /// Underh&#229;ll
        /// </summary>
        [DataMember(Name="maintenance",EmitDefaultValue=false)]
        public double Maintenance { get; set; }
    
    }
}