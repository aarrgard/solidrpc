using System.CodeDom.Compiler;
using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
namespace SolidRpc.Security.Types {
    /// <summary>
    /// success
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class OpenIDConnnectDiscovery {
        /// <summary>
        /// REQUIRED. URL using the https scheme with no query or fragment component that the OP asserts as its Issuer Identifier. If Issuer discovery is supported (see Section 2), this value MUST be identical to the issuer value returned by WebFinger. This also MUST be identical to the iss Claim value in ID Tokens issued from this Issuer.
        /// </summary>
        [DataMember(Name="issuer",EmitDefaultValue=false)]
        public Uri Issuer { get; set; }
    
        /// <summary>
        /// REQUIRED. URL of the OP&#39;s OAuth 2.0 Authorization Endpoint [OpenID.Core].
        /// </summary>
        [DataMember(Name="authorization_endpoint",EmitDefaultValue=false)]
        public Uri AuthorizationEndpoint { get; set; }
    
        /// <summary>
        /// URL of the OP&#39;s OAuth 2.0 Token Endpoint [OpenID.Core]. This is REQUIRED unless only the Implicit Flow is used.
        /// </summary>
        [DataMember(Name="token_endpoint",EmitDefaultValue=false)]
        public Uri TokenEndpoint { get; set; }
    
        /// <summary>
        /// RECOMMENDED. URL of the OP&#39;s UserInfo Endpoint [OpenID.Core]. This URL MUST use the https scheme and MAY contain port, path, and query parameter components.
        /// </summary>
        [DataMember(Name="userinfo_endpoint",EmitDefaultValue=false)]
        public Uri UserinfoEndpoint { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name="revocation_endpoint",EmitDefaultValue=false)]
        public Uri RevocationEndpoint { get; set; }
    
        /// <summary>
        /// OPTIONAL. URL of the authorization server&#39;s device authorization endpoint defined in Section 3.1.
        /// </summary>
        [DataMember(Name="device_authorization_endpoint",EmitDefaultValue=false)]
        public Uri DeviceAuthorizationEndpoint { get; set; }
    
        /// <summary>
        /// REQUIRED. URL of the OP&#39;s JSON Web Key Set [JWK] document. This contains the signing key(s) the RP uses to validate signatures from the OP. The JWK Set MAY also contain the Server&#39;s encryption key(s), which are used by RPs to encrypt requests to the Server. When both signing and encryption keys are made available, a use (Key Use) parameter value is REQUIRED for all keys in the referenced JWK Set to indicate each key&#39;s intended usage. Although some algorithms allow the same key to be used for both signatures and encryption, doing so is NOT RECOMMENDED, as it is less secure. The JWK x5c parameter MAY be used to provide X.509 representations of keys provided. When used, the bare key values MUST still be present and MUST match those in the certificate.
        /// </summary>
        [DataMember(Name="jwks_uri",EmitDefaultValue=false)]
        public Uri JwksUri { get; set; }
    
        /// <summary>
        /// RECOMMENDED. JSON array containing a list of the OAuth 2.0 [RFC6749] scope values that this server supports. The server MUST support the openid scope value. Servers MAY choose not to advertise some supported scope values even when this parameter is used, although those defined in [OpenID.Core] SHOULD be listed, if supported.
        /// </summary>
        [DataMember(Name="scopes_supported",EmitDefaultValue=false)]
        public IEnumerable<string> ScopesSupported { get; set; }
    
        /// <summary>
        /// OPTIONAL. JSON array containing a list of the OAuth 2.0 Grant Type values that this OP supports. Dynamic OpenID Providers MUST support the authorization_code and implicit Grant Type values and MAY support other Grant Types. If omitted, the default value is [&quot;authorization_code&quot;, &quot;implicit&quot;]&quot;
        /// </summary>
        [DataMember(Name="grant_types_supported",EmitDefaultValue=false)]
        public IEnumerable<string> GrantTypesSupported { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name="response_modes_supported",EmitDefaultValue=false)]
        public IEnumerable<string> ResponseModesSupported { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name="subject_types_supported",EmitDefaultValue=false)]
        public IEnumerable<string> SubjectTypesSupported { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name="id_token_signing_alg_values_supported",EmitDefaultValue=false)]
        public IEnumerable<string> IdTokenSigningAlgValuesSupported { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name="end_session_endpoint",EmitDefaultValue=false)]
        public string EndSessionEndpoint { get; set; }
    
        /// <summary>
        /// REQUIRED. JSON array containing a list of the OAuth 2.0 response_type values that this OP supports. Dynamic OpenID Providers MUST support the code, id_token, and the token id_token Response Type values.
        /// </summary>
        [DataMember(Name="response_types_supported",EmitDefaultValue=false)]
        public IEnumerable<string> ResponseTypesSupported { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name="claims_supported",EmitDefaultValue=false)]
        public IEnumerable<string> ClaimsSupported { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name="token_endpoint_auth_methods_supported",EmitDefaultValue=false)]
        public IEnumerable<string> TokenEndpointAuthMethodsSupported { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name="code_challenge_methods_supported",EmitDefaultValue=false)]
        public IEnumerable<string> CodeChallengeMethodsSupported { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name="request_uri_parameter_supported",EmitDefaultValue=false)]
        public bool RequestUriParameterSupported { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name="http_logout_supported",EmitDefaultValue=false)]
        public bool HttpLogoutSupported { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name="frontchannel_logout_supported",EmitDefaultValue=false)]
        public bool FrontchannelLogoutSupported { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name="rbac_url",EmitDefaultValue=false)]
        public Uri RbacUrl { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name="msgraph_host",EmitDefaultValue=false)]
        public string MsgraphHost { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name="cloud_graph_host_name",EmitDefaultValue=false)]
        public string CloudGraphHostName { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name="cloud_instance_name",EmitDefaultValue=false)]
        public string CloudInstanceName { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name="tenant_region_scope",EmitDefaultValue=false)]
        public string TenantRegionScope { get; set; }
    
    }
}