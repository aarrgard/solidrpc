using System.CodeDom.Compiler;
using System.Runtime.Serialization;
namespace SolidRpc.Test.Vitec.Types.CommercialPropertyInfo.Estate {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class CommercialOperation {
        /// <summary>
        /// Uppv&#228;rmningskostnad
        /// </summary>
        [DataMember(Name="heating",EmitDefaultValue=false)]
        public double Heating { get; set; }
    
        /// <summary>
        /// F&#246;rs&#228;kring
        /// </summary>
        [DataMember(Name="insurance",EmitDefaultValue=false)]
        public double Insurance { get; set; }
    
        /// <summary>
        /// Kostnad f&#246;r vatten och avlopp
        /// </summary>
        [DataMember(Name="waterAndDrain",EmitDefaultValue=false)]
        public double WaterAndDrain { get; set; }
    
        /// <summary>
        /// Renh&#229;llningsavgift
        /// </summary>
        [DataMember(Name="sanitation",EmitDefaultValue=false)]
        public double Sanitation { get; set; }
    
        /// <summary>
        /// Sotningsavgift
        /// </summary>
        [DataMember(Name="chimneySweeping",EmitDefaultValue=false)]
        public double ChimneySweeping { get; set; }
    
        /// <summary>
        /// V&#228;gavg/sn&#246;
        /// </summary>
        [DataMember(Name="roadSnowCharge",EmitDefaultValue=false)]
        public int RoadSnowCharge { get; set; }
    
        /// <summary>
        /// F&#246;rvaltning
        /// </summary>
        [DataMember(Name="management",EmitDefaultValue=false)]
        public double Management { get; set; }
    
        /// <summary>
        /// Fastighetssk&#246;tsel
        /// </summary>
        [DataMember(Name="caretaking",EmitDefaultValue=false)]
        public double Caretaking { get; set; }
    
        /// <summary>
        /// Elkostnad
        /// </summary>
        [DataMember(Name="electricity",EmitDefaultValue=false)]
        public double Electricity { get; set; }
    
        /// <summary>
        /// TV-anl&#228;ggning
        /// </summary>
        [DataMember(Name="tvSystem",EmitDefaultValue=false)]
        public double TvSystem { get; set; }
    
        /// <summary>
        /// Periodiskt underh&#229;ll
        /// </summary>
        [DataMember(Name="periodicMaintenance",EmitDefaultValue=false)]
        public double PeriodicMaintenance { get; set; }
    
        /// <summary>
        /// L&#246;pande underh&#229;ll
        /// </summary>
        [DataMember(Name="ongoingMaintenance",EmitDefaultValue=false)]
        public double OngoingMaintenance { get; set; }
    
        /// <summary>
        /// &#214;vrigt
        /// </summary>
        [DataMember(Name="other",EmitDefaultValue=false)]
        public double Other { get; set; }
    
        /// <summary>
        /// Egen Inmatning 1
        /// </summary>
        [DataMember(Name="ownInput1",EmitDefaultValue=false)]
        public SolidRpc.Test.Vitec.Types.CommercialPropertyInfo.Estate.OwnInput OwnInput1 { get; set; }
    
        /// <summary>
        /// Egen Inmatning 2
        /// </summary>
        [DataMember(Name="ownInput2",EmitDefaultValue=false)]
        public SolidRpc.Test.Vitec.Types.CommercialPropertyInfo.Estate.OwnInput OwnInput2 { get; set; }
    
        /// <summary>
        /// Total driftkostnad
        /// </summary>
        [DataMember(Name="sum",EmitDefaultValue=false)]
        public double Sum { get; set; }
    
        /// <summary>
        /// Kommentar driftskostnader
        /// </summary>
        [DataMember(Name="commentOperationalCost",EmitDefaultValue=false)]
        public string CommentOperationalCost { get; set; }
    
        /// <summary>
        /// Fastigheten &#228;r helt eller delvis momsregistrerad
        /// </summary>
        [DataMember(Name="completeOrPartialVATRegistered",EmitDefaultValue=false)]
        public bool CompleteOrPartialVATRegistered { get; set; }
    
    }
}