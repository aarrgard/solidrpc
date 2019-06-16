using System;
namespace SolidRpc.Tests.Swagger.CodeGen.PetstoreTest.Types {
    /// <summary>
    /// 
    /// </summary>
    public class Order {
        /// <summary>
        /// 
        /// </summary>
        public long Id { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        public long PetId { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        public int Quantity { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        public DateTime ShipDate { get; set; }
    
        /// <summary>
        /// Order Status
        /// </summary>
        public string Status { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        public bool Complete { get; set; }
    
    }
}