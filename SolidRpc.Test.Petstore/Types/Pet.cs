
using SolidRpc.Test.Petstore.Types;
using System.Collections.Generic;
namespace SolidRpc.Test.Petstore.Types {
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
        public Category Category { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<string> PhotoUrls { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<Tag> Tags { get; set; }
    
        /// <summary>
        /// pet status in the store
        /// </summary>
        public string Status { get; set; }
    
    }
}