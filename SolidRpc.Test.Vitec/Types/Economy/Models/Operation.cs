using System.CodeDom.Compiler;
using System.Runtime.Serialization;
namespace SolidRpc.Test.Vitec.Types.Economy.Models {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class Operation {
        /// <summary>
        /// Uppv&#228;rmningskostnad
        /// </summary>
        [DataMember(Name="heating",EmitDefaultValue=false)]
        public int Heating { get; set; }
    
        /// <summary>
        /// Elkostnad
        /// </summary>
        [DataMember(Name="electricity",EmitDefaultValue=false)]
        public int Electricity { get; set; }
    
        /// <summary>
        /// Kostnad f&#246;r vatten och avlopp
        /// </summary>
        [DataMember(Name="waterAndDrain",EmitDefaultValue=false)]
        public int WaterAndDrain { get; set; }
    
        /// <summary>
        /// Sotningsavgift
        /// </summary>
        [DataMember(Name="chimneySweeping",EmitDefaultValue=false)]
        public int ChimneySweeping { get; set; }
    
        /// <summary>
        /// Kostnad f&#246;r v&#228;gsamf&#228;llighet
        /// </summary>
        [DataMember(Name="roadCommunity",EmitDefaultValue=false)]
        public int RoadCommunity { get; set; }
    
        /// <summary>
        /// Renh&#229;llningsavgift
        /// </summary>
        [DataMember(Name="sanitation",EmitDefaultValue=false)]
        public int Sanitation { get; set; }
    
        /// <summary>
        /// F&#246;rs&#228;kring
        /// </summary>
        [DataMember(Name="insurance",EmitDefaultValue=false)]
        public int Insurance { get; set; }
    
        /// <summary>
        /// &#214;vrigt
        /// </summary>
        [DataMember(Name="other",EmitDefaultValue=false)]
        public int Other { get; set; }
    
        /// <summary>
        /// Antal perssoner i hush&#229;llet
        /// </summary>
        [DataMember(Name="personsInHousehold",EmitDefaultValue=false)]
        public int PersonsInHousehold { get; set; }
    
        /// <summary>
        /// Kommentar
        /// </summary>
        [DataMember(Name="comment",EmitDefaultValue=false)]
        public string Comment { get; set; }
    
    }
}