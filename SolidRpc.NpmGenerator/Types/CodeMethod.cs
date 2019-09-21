using System.Runtime.Serialization;
using System.Collections.Generic;
using SolidRpc.NpmGenerator.Types;
using System;
namespace SolidRpc.NpmGenerator.Types {
    /// <summary>
    /// 
    /// </summary>
    public class CodeMethod {
        /// <summary>
        /// A description of the the method
        /// </summary>
        [DataMember(Name="description")]
        public string Description { get; set; }
    
        /// <summary>
        /// The name of this method
        /// </summary>
        [DataMember(Name="name")]
        public string Name { get; set; }
    
        /// <summary>
        /// The method arguments
        /// </summary>
        [DataMember(Name="arguments")]
        public IEnumerable<CodeMethodArg> Arguments { get; set; }
    
        /// <summary>
        /// The return type of the method(fully qualified)
        /// </summary>
        [DataMember(Name="returnType")]
        public IEnumerable<string> ReturnType { get; set; }
    
        /// <summary>
        /// The http method(GET,POST,etc.)
        /// </summary>
        [DataMember(Name="httpMethod")]
        public string HttpMethod { get; set; }
    
        /// <summary>
        /// The base address to this method
        /// </summary>
        [DataMember(Name="httpBaseAddress")]
        public Uri HttpBaseAddress { get; set; }
    
        /// <summary>
        /// The http path relative to the base address
        /// </summary>
        [DataMember(Name="httpPath")]
        public string HttpPath { get; set; }
    
    }
}