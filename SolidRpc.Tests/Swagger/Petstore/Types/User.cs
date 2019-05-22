
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using SolidRpc.Tests.Swagger.Petstore.Services;
using SolidRpc.Tests.Swagger.Petstore.Types;
namespace SolidRpc.Tests.Swagger.Petstore.Types {
    public class User {
        /// <summary>
        /// 
        /// </summary>
        public long id { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        public string username { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        public string firstName { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        public string lastName { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        public string email { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        public string password { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        public string phone { get; set; }
    
        /// <summary>
        /// User Status
        /// </summary>
        public int userStatus { get; set; }
    
    }
}