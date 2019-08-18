using System.Runtime.Serialization;
using System;
namespace SolidRpc.Tests.Swagger.CodeGen.Petstore.Types {
    /// <summary>
    /// successful operation
    /// </summary>
    public class Order {
        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name="id")]
        public long Id { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name="petId")]
        public long PetId { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name="quantity")]
        public int Quantity { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name="shipDate")]
        public DateTime ShipDate { get; set; }
    
        /// <summary>
        /// Order Status
        /// </summary>
        [DataMember(Name="status")]
        public string Status { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name="complete")]
        public bool Complete { get; set; }
    
    }
}