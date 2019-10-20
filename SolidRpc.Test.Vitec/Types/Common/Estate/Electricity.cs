using System.CodeDom.Compiler;
using System.Runtime.Serialization;
namespace SolidRpc.Test.Vitec.Types.Common.Estate {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class Electricity {
        /// <summary>
        /// F�retag
        /// </summary>
        [DataMember(Name="companie",EmitDefaultValue=false)]
        public string Companie { get; set; }
    
        /// <summary>
        /// Ellevrant�r
        /// </summary>
        [DataMember(Name="distributor",EmitDefaultValue=false)]
        public string Distributor { get; set; }
    
        /// <summary>
        /// Elf�rbrukning KWH /�r
        /// </summary>
        [DataMember(Name="powerConsumptionKWH",EmitDefaultValue=false)]
        public double PowerConsumptionKWH { get; set; }
    
    }
}