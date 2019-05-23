
namespace SolidRpc.Tests.Generated.Petstore.Types {
    /// <summary>
    /// 
    /// </summary>
    public class Pet {
        /// <summary>
        /// 
        /// </summary>
        public long Id { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        public SolidRpc.Tests.Generated.Petstore.Types.Category Category { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        public System.Collections.Generic.IEnumerable<string> PhotoUrls { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        public System.Collections.Generic.IEnumerable<SolidRpc.Tests.Generated.Petstore.Types.Tag> Tags { get; set; }
    
        /// <summary>
        /// pet status in the store
        /// </summary>
        public string Status { get; set; }
    
    }
}