using System.CodeDom.Compiler;
using System.Runtime.Serialization;
namespace SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.CommercialPropertyInfo.Estate {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class CommercialOperation {
        /// <summary>
        /// Uppv�rmningskostnad
        /// </summary>
        [DataMember(Name="heating",EmitDefaultValue=false)]
        public double Heating { get; set; }
    
        /// <summary>
        /// F�rs�kring
        /// </summary>
        [DataMember(Name="insurance",EmitDefaultValue=false)]
        public double Insurance { get; set; }
    
        /// <summary>
        /// Kostnad f�r vatten och avlopp
        /// </summary>
        [DataMember(Name="waterAndDrain",EmitDefaultValue=false)]
        public double WaterAndDrain { get; set; }
    
        /// <summary>
        /// Renh�llningsavgift
        /// </summary>
        [DataMember(Name="sanitation",EmitDefaultValue=false)]
        public double Sanitation { get; set; }
    
        /// <summary>
        /// Sotningsavgift
        /// </summary>
        [DataMember(Name="chimneySweeping",EmitDefaultValue=false)]
        public double ChimneySweeping { get; set; }
    
        /// <summary>
        /// V�gavg/sn�
        /// </summary>
        [DataMember(Name="roadSnowCharge",EmitDefaultValue=false)]
        public int RoadSnowCharge { get; set; }
    
        /// <summary>
        /// F�rvaltning
        /// </summary>
        [DataMember(Name="management",EmitDefaultValue=false)]
        public double Management { get; set; }
    
        /// <summary>
        /// Fastighetssk�tsel
        /// </summary>
        [DataMember(Name="caretaking",EmitDefaultValue=false)]
        public double Caretaking { get; set; }
    
        /// <summary>
        /// Elkostnad
        /// </summary>
        [DataMember(Name="electricity",EmitDefaultValue=false)]
        public double Electricity { get; set; }
    
        /// <summary>
        /// TV-anl�ggning
        /// </summary>
        [DataMember(Name="tvSystem",EmitDefaultValue=false)]
        public double TvSystem { get; set; }
    
        /// <summary>
        /// Periodiskt underh�ll
        /// </summary>
        [DataMember(Name="periodicMaintenance",EmitDefaultValue=false)]
        public double PeriodicMaintenance { get; set; }
    
        /// <summary>
        /// L�pande underh�ll
        /// </summary>
        [DataMember(Name="ongoingMaintenance",EmitDefaultValue=false)]
        public double OngoingMaintenance { get; set; }
    
        /// <summary>
        /// �vrigt
        /// </summary>
        [DataMember(Name="other",EmitDefaultValue=false)]
        public double Other { get; set; }
    
        /// <summary>
        /// Egen Inmatning 1
        /// </summary>
        [DataMember(Name="ownInput1",EmitDefaultValue=false)]
        public SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.CommercialPropertyInfo.Estate.OwnInput OwnInput1 { get; set; }
    
        /// <summary>
        /// Egen Inmatning 2
        /// </summary>
        [DataMember(Name="ownInput2",EmitDefaultValue=false)]
        public SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.CommercialPropertyInfo.Estate.OwnInput OwnInput2 { get; set; }
    
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
        /// Fastigheten �r helt eller delvis momsregistrerad
        /// </summary>
        [DataMember(Name="completeOrPartialVATRegistered",EmitDefaultValue=false)]
        public bool CompleteOrPartialVATRegistered { get; set; }
    
    }
}