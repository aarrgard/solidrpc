using System.CodeDom.Compiler;
using System.Runtime.Serialization;
using System;
using SolidRpc.Test.Vitec.Types.Models.Api;
namespace SolidRpc.Test.Vitec.Types.Crm.Contact {
    /// <summary>
    /// OK
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class CrmPresentAccomodation {
        /// <summary>
        /// Bostadstyp
        /// </summary>
        [DataMember(Name="estateType",EmitDefaultValue=false)]
        public string EstateType { get; set; }
    
        /// <summary>
        /// Boarea
        /// </summary>
        [DataMember(Name="livingSpace",EmitDefaultValue=false)]
        public double? LivingSpace { get; set; }
    
        /// <summary>
        /// Antal rum
        /// </summary>
        [DataMember(Name="numberOfRooms",EmitDefaultValue=false)]
        public double? NumberOfRooms { get; set; }
    
        /// <summary>
        /// Pris
        /// </summary>
        [DataMember(Name="price",EmitDefaultValue=false)]
        public double? Price { get; set; }
    
        /// <summary>
        /// &#214;vrig information
        /// </summary>
        [DataMember(Name="other",EmitDefaultValue=false)]
        public string Other { get; set; }
    
        /// <summary>
        /// Koordinater i koordinatsystemet WGS84
        /// </summary>
        [DataMember(Name="coordinate",EmitDefaultValue=false)]
        public Wgs84Coordinate Coordinate { get; set; }
    
    }
}