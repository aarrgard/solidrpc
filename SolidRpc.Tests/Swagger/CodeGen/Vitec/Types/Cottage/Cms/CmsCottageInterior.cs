using System.CodeDom.Compiler;
using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
namespace SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Cottage.Cms {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class CmsCottageInterior {
        /// <summary>
        /// Antal rum
        /// </summary>
        [DataMember(Name="numberOfRooms",EmitDefaultValue=false)]
        public double? NumberOfRooms { get; set; }
    
        /// <summary>
        /// Antal sovrum
        /// </summary>
        [DataMember(Name="numberOfBedrooms",EmitDefaultValue=false)]
        public int? NumberOfBedrooms { get; set; }
    
        /// <summary>
        /// Max antal sovrum
        /// </summary>
        [DataMember(Name="maxNumberOfBedrooms",EmitDefaultValue=false)]
        public int? MaxNumberOfBedrooms { get; set; }
    
        /// <summary>
        /// Allm&#228;n beskrivning av interi&#246;ren
        /// </summary>
        [DataMember(Name="description",EmitDefaultValue=false)]
        public string Description { get; set; }
    
        /// <summary>
        /// Rum
        /// </summary>
        [DataMember(Name="rooms",EmitDefaultValue=false)]
        public IEnumerable<SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Cottage.Cms.Room> Rooms { get; set; }
    
    }
}