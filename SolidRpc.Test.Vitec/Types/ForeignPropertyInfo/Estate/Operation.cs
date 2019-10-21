using System.CodeDom.Compiler;
using System.Runtime.Serialization;
namespace SolidRpc.Test.Vitec.Types.ForeignPropertyInfo.Estate {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class Operation {
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
        /// Kostnad f&#246;r v&#228;gsamf&#228;llighet
        /// </summary>
        [DataMember(Name="roadCommunity",EmitDefaultValue=false)]
        public double RoadCommunity { get; set; }
    
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
        public int RoadCharge { get; set; }
    
        /// <summary>
        /// Underh&#229;ll
        /// </summary>
        [DataMember(Name="maintenance",EmitDefaultValue=false)]
        public int Maintenance { get; set; }
    
    }
}