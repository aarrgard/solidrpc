
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Services;
using Types;
namespace Types {
    public class ApiResponse {
        /// <summary>
        /// 
        /// </summary>
        public int code { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        public string type { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        public string message { get; set; }
    
    }
}