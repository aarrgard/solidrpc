
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Services;
using Types;
namespace Types {
    public class Pet {
        /// <summary>
        /// 
        /// </summary>
        public long id { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        public Category category { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        public string name { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<string> photoUrls { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<Tag> tags { get; set; }
    
        /// <summary>
        /// pet status in the store
        /// </summary>
        public string status { get; set; }
    
    }
}