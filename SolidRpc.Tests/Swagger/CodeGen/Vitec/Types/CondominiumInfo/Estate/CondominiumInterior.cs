using System.CodeDom.Compiler;
using System.Runtime.Serialization;
using System.Collections.Generic;
using SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Common.Estate;
namespace SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.CondominiumInfo.Estate {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class CondominiumInterior {
        /// <summary>
        /// K�kstyp
        /// </summary>
        [DataMember(Name="kitchenType",EmitDefaultValue=false)]
        public string KitchenType { get; set; }
    
        /// <summary>
        /// Antal sovrum
        /// </summary>
        [DataMember(Name="numberOfBedrooms",EmitDefaultValue=false)]
        public double NumberOfBedrooms { get; set; }
    
        /// <summary>
        /// Max antal sovrum
        /// </summary>
        [DataMember(Name="maxNumberOfBedrooms",EmitDefaultValue=false)]
        public double MaxNumberOfBedrooms { get; set; }
    
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