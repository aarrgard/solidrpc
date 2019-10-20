using System.CodeDom.Compiler;
using System.Runtime.Serialization;
using SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Estate.Models;
namespace SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.ForeignProperty.Cms {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class CmsForeignPropertyInterior {
        /// <summary>
        /// Boarea
        /// </summary>
        [DataMember(Name="livingarea",EmitDefaultValue=false)]
        public double Livingarea { get; set; }
    
        /// <summary>
        /// Byggnadsarea
        /// </summary>
        [DataMember(Name="buildingArea",EmitDefaultValue=false)]
        public double BuildingArea { get; set; }
    
        /// <summary>
        /// Antal rum
        /// </summary>
        [DataMember(Name="numberOfRooms",EmitDefaultValue=false)]
        public double NumberOfRooms { get; set; }
    
        /// <summary>
        /// Antal sovrum
        /// </summary>
        [DataMember(Name="numberOfBedrooms",EmitDefaultValue=false)]
        public Interval NumberOfBedrooms { get; set; }
    
        /// <summary>
        /// Antal badrum
        /// </summary>
        [DataMember(Name="numberOfBathrooms",EmitDefaultValue=false)]
        public int NumberOfBathrooms { get; set; }
    
    }
}