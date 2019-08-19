using System.IO;
using System.Runtime.Serialization;
namespace SolidRpc.Security.Types {
    /// <summary>
    /// success
    /// </summary>
    public class WebContent {
        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name="content")]
        public Stream Content { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name="contentType")]
        public string ContentType { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name="charSet")]
        public string CharSet { get; set; }
    
    }
}