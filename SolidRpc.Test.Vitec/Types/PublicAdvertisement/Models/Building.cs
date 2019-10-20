using System.CodeDom.Compiler;
using System.Runtime.Serialization;
namespace SolidRpc.Test.Vitec.Types.PublicAdvertisement.Models {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class Building {
        /// <summary>
        /// Antal rum
        /// </summary>
        [DataMember(Name="numberOfRooms",EmitDefaultValue=false)]
        public double NumberOfRooms { get; set; }
    
        /// <summary>
        /// Rumsbeskrivning
        /// </summary>
        [DataMember(Name="roomDescription",EmitDefaultValue=false)]
        public string RoomDescription { get; set; }
    
        /// <summary>
        /// Bygg�r
        /// </summary>
        [DataMember(Name="yearBuilt",EmitDefaultValue=false)]
        public int YearBuilt { get; set; }
    
        /// <summary>
        /// Biarea (m�)
        /// </summary>
        [DataMember(Name="grossFloorArea",EmitDefaultValue=false)]
        public double GrossFloorArea { get; set; }
    
        /// <summary>
        /// Boarea (m�)
        /// </summary>
        [DataMember(Name="livingSpace",EmitDefaultValue=false)]
        public double LivingSpace { get; set; }
    
    }
}