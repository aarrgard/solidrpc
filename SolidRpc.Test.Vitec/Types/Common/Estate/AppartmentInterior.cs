using System.CodeDom.Compiler;
using System.Runtime.Serialization;
using System.Collections.Generic;
namespace SolidRpc.Test.Vitec.Types.Common.Estate {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class AppartmentInterior {
        /// <summary>
        /// Typ av k�k
        /// </summary>
        [DataMember(Name="kitchenType",EmitDefaultValue=false)]
        public string KitchenType { get; set; }
    
        /// <summary>
        /// Antal sovrum
        /// </summary>
        [DataMember(Name="numberOfBedroom",EmitDefaultValue=false)]
        public double NumberOfBedroom { get; set; }
    
        /// <summary>
        /// Max antal sovrum
        /// </summary>
        [DataMember(Name="maxNumberOfBedroom",EmitDefaultValue=false)]
        public double MaxNumberOfBedroom { get; set; }
    
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
        public IEnumerable<SolidRpc.Test.Vitec.Types.Common.Estate.Room> Rooms { get; set; }
    
    }
}