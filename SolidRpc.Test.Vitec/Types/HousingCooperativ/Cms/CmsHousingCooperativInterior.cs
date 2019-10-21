using System.CodeDom.Compiler;
using System.Runtime.Serialization;
using System.Collections.Generic;
namespace SolidRpc.Test.Vitec.Types.HousingCooperativ.Cms {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class CmsHousingCooperativInterior {
        /// <summary>
        /// Antal rum
        /// </summary>
        [DataMember(Name="numberOfRooms",EmitDefaultValue=false)]
        public double NumberOfRooms { get; set; }
    
        /// <summary>
        /// Allm&#228;n beskrivning av interi&#246;ren
        /// </summary>
        [DataMember(Name="description",EmitDefaultValue=false)]
        public string Description { get; set; }
    
        /// <summary>
        /// Rum
        /// </summary>
        [DataMember(Name="rooms",EmitDefaultValue=false)]
        public IEnumerable<SolidRpc.Test.Vitec.Types.HousingCooperativ.Cms.Room> Rooms { get; set; }
    
        /// <summary>
        /// K&#246;kstyp
        /// </summary>
        [DataMember(Name="kitchenType",EmitDefaultValue=false)]
        public string KitchenType { get; set; }
    
    }
}