using System.CodeDom.Compiler;
using System.Runtime.Serialization;
using System.Collections.Generic;
using SolidRpc.Test.Vitec.Types.Common.Estate;
namespace SolidRpc.Test.Vitec.Types.HouseInfo.Estate {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class HouseInterior {
        /// <summary>
        /// Antal sovrum
        /// </summary>
        [DataMember(Name="numberOffBedroom",EmitDefaultValue=false)]
        public double NumberOffBedroom { get; set; }
    
        /// <summary>
        /// Max antal sovrum
        /// </summary>
        [DataMember(Name="maxNumberOffBedroom",EmitDefaultValue=false)]
        public double MaxNumberOffBedroom { get; set; }
    
        /// <summary>
        /// Antal rum
        /// </summary>
        [DataMember(Name="numberOfRooms",EmitDefaultValue=false)]
        public double NumberOfRooms { get; set; }
    
        /// <summary>
        /// Allm�n beskrivning
        /// </summary>
        [DataMember(Name="generealDescription",EmitDefaultValue=false)]
        public string GenerealDescription { get; set; }
    
        /// <summary>
        /// Sammansatt beskrivning
        /// </summary>
        [DataMember(Name="compiledDescription",EmitDefaultValue=false)]
        public bool CompiledDescription { get; set; }
    
        /// <summary>
        /// Rumbeskrivningar
        /// </summary>
        [DataMember(Name="rooms",EmitDefaultValue=false)]
        public IEnumerable<Room> Rooms { get; set; }
    
    }
}