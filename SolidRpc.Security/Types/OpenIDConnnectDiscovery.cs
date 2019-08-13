using System;
using System.Collections.Generic;
namespace SolidRpc.Security.Types {
    /// <summary>
    /// success
    /// </summary>
    public class OpenIDConnnectDiscovery {
        /// <summary>
        /// 
        /// </summary>
        public Uri Issuer { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        public Uri Authorization_endpoint { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        public Uri Token_endpoint { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        public Uri Userinfo_endpoint { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        public Uri Jwks_uri { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<string> Scopes_supported { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<string> Response_modes_supported { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<string> Subject_types_supported { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<string> Id_token_signing_alg_values_supported { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        public string End_session_endpoint { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<string> Response_types_supported { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<string> Claims_supported { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        public bool Request_uri_parameter_supported { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        public bool Http_logout_supported { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        public bool Frontchannel_logout_supported { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        public Uri Rbac_url { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        public string Msgraph_host { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        public string Cloud_graph_host_name { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        public string Cloud_instance_name { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        public string Tenant_region_scope { get; set; }
    
    }
}