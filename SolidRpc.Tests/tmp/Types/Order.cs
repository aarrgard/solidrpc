
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Services;
using Types;
namespace Types {
    public class Order {
        /// <summary>
        /// 
        /// </summary>
        public long id { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        public long petId { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        public int quantity { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        public DateTime shipDate { get; set; }
    
        /// <summary>
        /// Order Status
        /// </summary>
        public string status { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        public bool complete { get; set; }
    
    }
}