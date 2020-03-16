using System.CodeDom.Compiler;
using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
namespace SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.ForeignPropertyInfo.Estate {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class ForeignPropertyInterior {
        /// <summary>
        /// Antal sovrum
        /// </summary>
        [DataMember(Name="numberOfBedrooms",EmitDefaultValue=false)]
        public double? NumberOfBedrooms { get; set; }
    
        /// <summary>
        /// Antal rum
        /// </summary>
        [DataMember(Name="numberOfRooms",EmitDefaultValue=false)]
        public double? NumberOfRooms { get; set; }
    
        /// <summary>
        /// Max antal sovrum
        /// </summary>
        [DataMember(Name="maxNumberOfBedrooms",EmitDefaultValue=false)]
        public double? MaxNumberOfBedrooms { get; set; }
    
        /// <summary>
        /// Antal badrum
        /// </summary>
        [DataMember(Name="numberOfBathrooms",EmitDefaultValue=false)]
        public double? NumberOfBathrooms { get; set; }
    
        /// <summary>
        /// Allm&#228;n beskrivning
        /// </summary>
        [DataMember(Name="generealDescriptionInterior",EmitDefaultValue=false)]
        public IEnumerable<SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.ForeignPropertyInfo.Estate.GeneralDescription> GenerealDescriptionInterior { get; set; }
    
    }
}