using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
namespace SolidRpc.Security.Types {
    /// <summary>
    /// success
    /// </summary>
    public class OpenIDConnnectDiscovery {
        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name="issuer")]
        public Uri Issuer { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name="authorization_endpoint")]
        public Uri AuthorizationEndpoint { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name="token_endpoint")]
        public Uri TokenEndpoint { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name="userinfo_endpoint")]
        public Uri UserinfoEndpoint { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name="revocation_endpoint")]
        public Uri RevocationEndpoint { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name="jwks_uri")]
        public Uri JwksUri { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name="scopes_supported")]
        public IEnumerable<string> ScopesSupported { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name="response_modes_supported")]
        public IEnumerable<string> ResponseModesSupported { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name="subject_types_supported")]
        public IEnumerable<string> SubjectTypesSupported { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name="id_token_signing_alg_values_supported")]
        public IEnumerable<string> IdTokenSigningAlgValuesSupported { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name="end_session_endpoint")]
        public string EndSessionEndpoint { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name="response_types_supported")]
        public IEnumerable<string> ResponseTypesSupported { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name="claims_supported")]
        public IEnumerable<string> ClaimsSupported { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name="token_endpoint_auth_methods_supported")]
        public IEnumerable<string> TokenEndpointAuthMethodsSupported { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name="code_challenge_methods_supported")]
        public IEnumerable<string> CodeChallengeMethodsSupported { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name="request_uri_parameter_supported")]
        public bool RequestUriParameterSupported { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name="http_logout_supported")]
        public bool HttpLogoutSupported { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name="frontchannel_logout_supported")]
        public bool FrontchannelLogoutSupported { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name="rbac_url")]
        public Uri RbacUrl { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name="msgraph_host")]
        public string MsgraphHost { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name="cloud_graph_host_name")]
        public string CloudGraphHostName { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name="cloud_instance_name")]
        public string CloudInstanceName { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name="tenant_region_scope")]
        public string TenantRegionScope { get; set; }
    
    }
}