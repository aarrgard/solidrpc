using System.CodeDom.Compiler;
using System.Runtime.Serialization;
namespace SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.CondominiumInfo.Estate {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class CondominiumOperation {
        /// <summary>
        /// �rsavgift till samf�llighet
        /// </summary>
        [DataMember(Name="annualFeeCommunity",EmitDefaultValue=false)]
        public int AnnualFeeCommunity { get; set; }
    
        /// <summary>
        /// Vad som ing�r i �rsavgiften
        /// </summary>
        [DataMember(Name="annualFeeCommunityIncluded",EmitDefaultValue=false)]
        public string AnnualFeeCommunityIncluded { get; set; }
    
        /// <summary>
        /// Kommentar till �rsavgiften
        /// </summary>
        [DataMember(Name="commentAnnualFee",EmitDefaultValue=false)]
        public string CommentAnnualFee { get; set; }
    
        /// <summary>
        /// Uppv�rmningskostnad
        /// </summary>
        [DataMember(Name="heating",EmitDefaultValue=false)]
        public double Heating { get; set; }
    
        /// <summary>
        /// Elkostnad
        /// </summary>
        [DataMember(Name="electricity",EmitDefaultValue=false)]
        public double Electricity { get; set; }
    
        /// <summary>
        /// Kostnad f�r vatten och avlopp
        /// </summary>
        [DataMember(Name="waterAndDrain",EmitDefaultValue=false)]
        public double WaterAndDrain { get; set; }
    
        /// <summary>
        /// Sotningsavgift
        /// </summary>
        [DataMember(Name="chimneySweeping",EmitDefaultValue=false)]
        public double ChimneySweeping { get; set; }
    
        /// <summary>
        /// Renh�llningsavgift
        /// </summary>
        [DataMember(Name="sanitation",EmitDefaultValue=false)]
        public double Sanitation { get; set; }
    
        /// <summary>
        /// F�rs�kring
        /// </summary>
        [DataMember(Name="insurance",EmitDefaultValue=false)]
        public double Insurance { get; set; }
    
        /// <summary>
        /// �vrigt
        /// </summary>
        [DataMember(Name="other",EmitDefaultValue=false)]
        public double Other { get; set; }
    
        /// <summary>
        /// Total driftkostnad
        /// </summary>
        [DataMember(Name="sum",EmitDefaultValue=false)]
        public double Sum { get; set; }
    
        /// <summary>
        /// Antal perssoner i hush�llet
        /// </summary>
        [DataMember(Name="personsInTheHousehold",EmitDefaultValue=false)]
        public int PersonsInTheHousehold { get; set; }
    
        /// <summary>
        /// Kommentar
        /// </summary>
        [DataMember(Name="commentary",EmitDefaultValue=false)]
        public string Commentary { get; set; }
    
        /// <summary>
        /// V�gavg/sn�
        /// </summary>
        [DataMember(Name="roadCharge",EmitDefaultValue=false)]
        public double RoadCharge { get; set; }
    
        /// <summary>
        /// Underh�ll
        /// </summary>
        [DataMember(Name="maintenance",EmitDefaultValue=false)]
        public double Maintenance { get; set; }
    
    }
}