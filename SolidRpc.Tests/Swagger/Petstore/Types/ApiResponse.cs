
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using SolidRpc.Tests.Swagger.Petstore.Services;
using SolidRpc.Tests.Swagger.Petstore.Types;
namespace SolidRpc.Tests.Swagger.Petstore.Types {
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