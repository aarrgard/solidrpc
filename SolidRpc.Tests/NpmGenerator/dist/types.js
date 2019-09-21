"use strict";
var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
        return extendStatics(d, b);
    };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
Object.defineProperty(exports, "__esModule", { value: true });
var solidrpc_1 = require("solidrpc");
var Security;
(function (Security) {
    var Types;
    (function (Types) {
        var OpenIDConnnectDiscovery = /** @class */ (function () {
            function OpenIDConnnectDiscovery(obj) {
                for (var prop in obj) {
                    switch (prop) {
                        case "issuer":
                            if (obj.issuer) {
                                this.Issuer = obj.issuer.toString();
                            }
                            break;
                        case "authorization_endpoint":
                            if (obj.authorization_endpoint) {
                                this.AuthorizationEndpoint = obj.authorization_endpoint.toString();
                            }
                            break;
                        case "token_endpoint":
                            if (obj.token_endpoint) {
                                this.TokenEndpoint = obj.token_endpoint.toString();
                            }
                            break;
                        case "userinfo_endpoint":
                            if (obj.userinfo_endpoint) {
                                this.UserinfoEndpoint = obj.userinfo_endpoint.toString();
                            }
                            break;
                        case "revocation_endpoint":
                            if (obj.revocation_endpoint) {
                                this.RevocationEndpoint = obj.revocation_endpoint.toString();
                            }
                            break;
                        case "jwks_uri":
                            if (obj.jwks_uri) {
                                this.JwksUri = obj.jwks_uri.toString();
                            }
                            break;
                        case "scopes_supported":
                            if (obj.scopes_supported) {
                                this.ScopesSupported = Array.from(obj.scopes_supported).map(function (o) { return o.toString(); });
                            }
                            break;
                        case "response_modes_supported":
                            if (obj.response_modes_supported) {
                                this.ResponseModesSupported = Array.from(obj.response_modes_supported).map(function (o) { return o.toString(); });
                            }
                            break;
                        case "subject_types_supported":
                            if (obj.subject_types_supported) {
                                this.SubjectTypesSupported = Array.from(obj.subject_types_supported).map(function (o) { return o.toString(); });
                            }
                            break;
                        case "id_token_signing_alg_values_supported":
                            if (obj.id_token_signing_alg_values_supported) {
                                this.IdTokenSigningAlgValuesSupported = Array.from(obj.id_token_signing_alg_values_supported).map(function (o) { return o.toString(); });
                            }
                            break;
                        case "end_session_endpoint":
                            if (obj.end_session_endpoint) {
                                this.EndSessionEndpoint = obj.end_session_endpoint.toString();
                            }
                            break;
                        case "response_types_supported":
                            if (obj.response_types_supported) {
                                this.ResponseTypesSupported = Array.from(obj.response_types_supported).map(function (o) { return o.toString(); });
                            }
                            break;
                        case "claims_supported":
                            if (obj.claims_supported) {
                                this.ClaimsSupported = Array.from(obj.claims_supported).map(function (o) { return o.toString(); });
                            }
                            break;
                        case "token_endpoint_auth_methods_supported":
                            if (obj.token_endpoint_auth_methods_supported) {
                                this.TokenEndpointAuthMethodsSupported = Array.from(obj.token_endpoint_auth_methods_supported).map(function (o) { return o.toString(); });
                            }
                            break;
                        case "code_challenge_methods_supported":
                            if (obj.code_challenge_methods_supported) {
                                this.CodeChallengeMethodsSupported = Array.from(obj.code_challenge_methods_supported).map(function (o) { return o.toString(); });
                            }
                            break;
                        case "request_uri_parameter_supported":
                            if (obj.request_uri_parameter_supported) {
                                this.RequestUriParameterSupported = [true, 'true', 1].some(function (o) { return o === obj.request_uri_parameter_supported; });
                            }
                            break;
                        case "http_logout_supported":
                            if (obj.http_logout_supported) {
                                this.HttpLogoutSupported = [true, 'true', 1].some(function (o) { return o === obj.http_logout_supported; });
                            }
                            break;
                        case "frontchannel_logout_supported":
                            if (obj.frontchannel_logout_supported) {
                                this.FrontchannelLogoutSupported = [true, 'true', 1].some(function (o) { return o === obj.frontchannel_logout_supported; });
                            }
                            break;
                        case "rbac_url":
                            if (obj.rbac_url) {
                                this.RbacUrl = obj.rbac_url.toString();
                            }
                            break;
                        case "msgraph_host":
                            if (obj.msgraph_host) {
                                this.MsgraphHost = obj.msgraph_host.toString();
                            }
                            break;
                        case "cloud_graph_host_name":
                            if (obj.cloud_graph_host_name) {
                                this.CloudGraphHostName = obj.cloud_graph_host_name.toString();
                            }
                            break;
                        case "cloud_instance_name":
                            if (obj.cloud_instance_name) {
                                this.CloudInstanceName = obj.cloud_instance_name.toString();
                            }
                            break;
                        case "tenant_region_scope":
                            if (obj.tenant_region_scope) {
                                this.TenantRegionScope = obj.tenant_region_scope.toString();
                            }
                            break;
                    }
                }
            }
            return OpenIDConnnectDiscovery;
        }());
        Types.OpenIDConnnectDiscovery = OpenIDConnnectDiscovery;
        var OpenIDKeys = /** @class */ (function () {
            function OpenIDKeys(obj) {
                for (var prop in obj) {
                    switch (prop) {
                        case "Keys":
                            if (obj.Keys) {
                                this.Keys = Array.from(obj.Keys).map(function (o) { return new Security.Types.OpenIDKey(o); });
                            }
                            break;
                    }
                }
            }
            return OpenIDKeys;
        }());
        Types.OpenIDKeys = OpenIDKeys;
        var OpenIDKey = /** @class */ (function () {
            function OpenIDKey(obj) {
                for (var prop in obj) {
                    switch (prop) {
                        case "alg":
                            if (obj.alg) {
                                this.Alg = obj.alg.toString();
                            }
                            break;
                        case "kty":
                            if (obj.kty) {
                                this.Kty = obj.kty.toString();
                            }
                            break;
                        case "use":
                            if (obj.use) {
                                this.Use = obj.use.toString();
                            }
                            break;
                        case "kid":
                            if (obj.kid) {
                                this.Kid = obj.kid.toString();
                            }
                            break;
                        case "x5u":
                            if (obj.x5u) {
                                this.X5u = obj.x5u.toString();
                            }
                            break;
                        case "x5t":
                            if (obj.x5t) {
                                this.X5t = obj.x5t.toString();
                            }
                            break;
                        case "x5c":
                            if (obj.x5c) {
                                this.X5c = Array.from(obj.x5c).map(function (o) { return o.toString(); });
                            }
                            break;
                        case "n":
                            if (obj.n) {
                                this.N = obj.n.toString();
                            }
                            break;
                        case "e":
                            if (obj.e) {
                                this.E = obj.e.toString();
                            }
                            break;
                        case "issuer":
                            if (obj.issuer) {
                                this.Issuer = obj.issuer.toString();
                            }
                            break;
                    }
                }
            }
            return OpenIDKey;
        }());
        Types.OpenIDKey = OpenIDKey;
        var LoginProvider = /** @class */ (function () {
            function LoginProvider(obj) {
                for (var prop in obj) {
                    switch (prop) {
                        case "name":
                            if (obj.name) {
                                this.Name = obj.name.toString();
                            }
                            break;
                        case "status":
                            if (obj.status) {
                                this.Status = obj.status.toString();
                            }
                            break;
                        case "script":
                            if (obj.script) {
                                this.Script = Array.from(obj.script).map(function (o) { return o.toString(); });
                            }
                            break;
                        case "meta":
                            if (obj.meta) {
                                this.Meta = Array.from(obj.meta).map(function (o) { return new Security.Types.LoginProviderMeta(o); });
                            }
                            break;
                        case "buttonHtml":
                            if (obj.buttonHtml) {
                                this.ButtonHtml = obj.buttonHtml.toString();
                            }
                            break;
                    }
                }
            }
            return LoginProvider;
        }());
        Types.LoginProvider = LoginProvider;
        var LoginProviderMeta = /** @class */ (function () {
            function LoginProviderMeta(obj) {
                for (var prop in obj) {
                    switch (prop) {
                        case "name":
                            if (obj.name) {
                                this.Name = obj.name.toString();
                            }
                            break;
                        case "content":
                            if (obj.content) {
                                this.Content = obj.content.toString();
                            }
                            break;
                    }
                }
            }
            return LoginProviderMeta;
        }());
        Types.LoginProviderMeta = LoginProviderMeta;
        var WebContent = /** @class */ (function () {
            function WebContent(obj) {
                for (var prop in obj) {
                    switch (prop) {
                        case "content":
                            if (obj.content) {
                                this.Content = new Uint8Array(obj.content);
                            }
                            break;
                        case "contentType":
                            if (obj.contentType) {
                                this.ContentType = obj.contentType.toString();
                            }
                            break;
                        case "charSet":
                            if (obj.charSet) {
                                this.CharSet = obj.charSet.toString();
                            }
                            break;
                    }
                }
            }
            return WebContent;
        }());
        Types.WebContent = WebContent;
        var FacebookAccessToken = /** @class */ (function () {
            function FacebookAccessToken(obj) {
                for (var prop in obj) {
                    switch (prop) {
                        case "access_token":
                            if (obj.access_token) {
                                this.AccessToken = obj.access_token.toString();
                            }
                            break;
                        case "token_type":
                            if (obj.token_type) {
                                this.TokenType = obj.token_type.toString();
                            }
                            break;
                    }
                }
            }
            return FacebookAccessToken;
        }());
        Types.FacebookAccessToken = FacebookAccessToken;
        var FacebookDebugToken = /** @class */ (function () {
            function FacebookDebugToken(obj) {
                for (var prop in obj) {
                    switch (prop) {
                        case "data":
                            if (obj.data) {
                                this.Data = new Security.Types.FacebookDebugTokenData(obj.data);
                            }
                            break;
                    }
                }
            }
            return FacebookDebugToken;
        }());
        Types.FacebookDebugToken = FacebookDebugToken;
        var FacebookDebugTokenData = /** @class */ (function () {
            function FacebookDebugTokenData(obj) {
                for (var prop in obj) {
                    switch (prop) {
                        case "app_id":
                            if (obj.app_id) {
                                this.AppId = Number(obj.app_id);
                            }
                            break;
                        case "type":
                            if (obj.type) {
                                this.Type = obj.type.toString();
                            }
                            break;
                        case "application":
                            if (obj.application) {
                                this.Application = obj.application.toString();
                            }
                            break;
                        case "data_access_expires_at":
                            if (obj.data_access_expires_at) {
                                this.DataAccessExpiresAt = Number(obj.data_access_expires_at);
                            }
                            break;
                        case "error":
                            if (obj.error) {
                                this.Error = new Security.Types.FacebookDebugTokenDataError(obj.error);
                            }
                            break;
                        case "expires_at":
                            if (obj.expires_at) {
                                this.ExpiresAt = Number(obj.expires_at);
                            }
                            break;
                        case "is_valid":
                            if (obj.is_valid) {
                                this.IsValid = [true, 'true', 1].some(function (o) { return o === obj.is_valid; });
                            }
                            break;
                        case "scopes":
                            if (obj.scopes) {
                                this.Scopes = Array.from(obj.scopes).map(function (o) { return o.toString(); });
                            }
                            break;
                        case "user_id":
                            if (obj.user_id) {
                                this.UserId = Number(obj.user_id);
                            }
                            break;
                    }
                }
            }
            return FacebookDebugTokenData;
        }());
        Types.FacebookDebugTokenData = FacebookDebugTokenData;
        var FacebookDebugTokenDataError = /** @class */ (function () {
            function FacebookDebugTokenDataError(obj) {
                for (var prop in obj) {
                    switch (prop) {
                        case "code":
                            if (obj.code) {
                                this.Code = Number(obj.code);
                            }
                            break;
                        case "message":
                            if (obj.message) {
                                this.Message = obj.message.toString();
                            }
                            break;
                        case "subcode":
                            if (obj.subcode) {
                                this.Subcode = Number(obj.subcode);
                            }
                            break;
                    }
                }
            }
            return FacebookDebugTokenDataError;
        }());
        Types.FacebookDebugTokenDataError = FacebookDebugTokenDataError;
    })(Types = Security.Types || (Security.Types = {}));
    var Services;
    (function (Services) {
        var Microsoft;
        (function (Microsoft) {
            var MicrosoftRemoteImpl = /** @class */ (function (_super) {
                __extends(MicrosoftRemoteImpl, _super);
                function MicrosoftRemoteImpl() {
                    return _super !== null && _super.apply(this, arguments) || this;
                }
                MicrosoftRemoteImpl.prototype.Authorize = function (tenant, clientId, responseType, redirectUri, scope, nounce, responseMode, state, resource, prompt, loginHint, domainHint, cancellationToken) {
                    var uri = 'https://login.microsoftonline.com/{tenant}/oauth2/authorize';
                    uri = uri.replace('{tenant}', encodeURI(tenant));
                    return this.request('get', uri, {
                        'client_id': clientId,
                        'response_type': responseType,
                        'redirect_uri': redirectUri,
                        'scope': scope,
                        'nounce': nounce,
                        'response_mode': responseMode,
                        'state': state,
                        'resource': resource,
                        'prompt': prompt,
                        'login_hint': loginHint,
                        'domain_hint': domainHint,
                    }, null, null, cancellationToken);
                };
                MicrosoftRemoteImpl.prototype.OpenIdConfiguration = function (tenant, cancellationToken) {
                    var uri = 'https://login.microsoftonline.com/{tenant}/v2.0/.well-known/openid-configuration';
                    uri = uri.replace('{tenant}', encodeURI(tenant));
                    return this.request('get', uri, null, null, null, cancellationToken);
                };
                MicrosoftRemoteImpl.prototype.OpenIdKeys = function (tenant, cancellationToken) {
                    var uri = 'https://login.microsoftonline.com/{tenant}/discovery/v2.0/keys';
                    uri = uri.replace('{tenant}', encodeURI(tenant));
                    return this.request('get', uri, null, null, null, cancellationToken);
                };
                return MicrosoftRemoteImpl;
            }(solidrpc_1.SolidRpc.RpcServiceImpl));
            Microsoft.MicrosoftRemoteImpl = MicrosoftRemoteImpl;
            var MicrosoftLocalImpl = /** @class */ (function (_super) {
                __extends(MicrosoftLocalImpl, _super);
                function MicrosoftLocalImpl() {
                    return _super !== null && _super.apply(this, arguments) || this;
                }
                MicrosoftLocalImpl.prototype.LoginProvider = function (cancellationToken) {
                    var uri = 'https://localhost/SolidRpc/Security/Services/Microsoft/LoginProvider';
                    return this.request('get', uri, null, null, null, cancellationToken);
                };
                MicrosoftLocalImpl.prototype.LoginScript = function (cancellationToken) {
                    var uri = 'https://localhost/SolidRpc/Security/Services/Microsoft/LoginScript';
                    return this.request('get', uri, null, null, null, cancellationToken);
                };
                MicrosoftLocalImpl.prototype.LoggedIn = function (cancellationToken) {
                    var uri = 'https://localhost/SolidRpc/Security/Services/Microsoft/LoggedIn';
                    return this.request('post', uri, null, null, null, cancellationToken);
                };
                MicrosoftLocalImpl.prototype.LoggedOut = function (cancellationToken) {
                    var uri = 'https://localhost/SolidRpc/Security/Services/Microsoft/LoggedOut';
                    return this.request('post', uri, null, null, null, cancellationToken);
                };
                return MicrosoftLocalImpl;
            }(solidrpc_1.SolidRpc.RpcServiceImpl));
            Microsoft.MicrosoftLocalImpl = MicrosoftLocalImpl;
        })(Microsoft = Services.Microsoft || (Services.Microsoft = {}));
        var Google;
        (function (Google) {
            var GoogleRemoteImpl = /** @class */ (function (_super) {
                __extends(GoogleRemoteImpl, _super);
                function GoogleRemoteImpl() {
                    return _super !== null && _super.apply(this, arguments) || this;
                }
                GoogleRemoteImpl.prototype.Authorize = function (clientId, responseType, scope, nounce, redirectUri, state, prompt, display, loginHint, includeGrantedScopes, accessType, openidRealm, hd, cancellationToken) {
                    var uri = 'https://accounts.google.com/o/oauth2/v2/auth';
                    return this.request('get', uri, {
                        'client_id': clientId,
                        'response_type': responseType,
                        'scope': scope,
                        'nounce': nounce,
                        'redirect_uri': redirectUri,
                        'state': state,
                        'prompt': prompt,
                        'display': display,
                        'login_hint': loginHint,
                        'include_granted_scopes': includeGrantedScopes,
                        'access_type': accessType,
                        'openid.realm': openidRealm,
                        'hd': hd,
                    }, null, null, cancellationToken);
                };
                GoogleRemoteImpl.prototype.OpenIdConfiguration = function (cancellationToken) {
                    var uri = 'https://accounts.google.com/.well-known/openid-configuration';
                    return this.request('get', uri, null, null, null, cancellationToken);
                };
                GoogleRemoteImpl.prototype.OpenIdKeys = function (cancellationToken) {
                    var uri = 'https://accounts.google.com/.well-known/openid-keys';
                    return this.request('get', uri, null, null, null, cancellationToken);
                };
                return GoogleRemoteImpl;
            }(solidrpc_1.SolidRpc.RpcServiceImpl));
            Google.GoogleRemoteImpl = GoogleRemoteImpl;
            var GoogleLocalImpl = /** @class */ (function (_super) {
                __extends(GoogleLocalImpl, _super);
                function GoogleLocalImpl() {
                    return _super !== null && _super.apply(this, arguments) || this;
                }
                GoogleLocalImpl.prototype.LoginProvider = function (cancellationToken) {
                    var uri = 'https://localhost/SolidRpc/Security/Services/Google/LoginProvider';
                    return this.request('get', uri, null, null, null, cancellationToken);
                };
                GoogleLocalImpl.prototype.LoginScript = function (cancellationToken) {
                    var uri = 'https://localhost/SolidRpc/Security/Services/Google/LoginScript';
                    return this.request('get', uri, null, null, null, cancellationToken);
                };
                GoogleLocalImpl.prototype.LoggedIn = function (cancellationToken) {
                    var uri = 'https://localhost/SolidRpc/Security/Services/Google/LoggedIn';
                    return this.request('post', uri, null, null, null, cancellationToken);
                };
                GoogleLocalImpl.prototype.LoggedOut = function (cancellationToken) {
                    var uri = 'https://localhost/SolidRpc/Security/Services/Google/LoggedOut';
                    return this.request('post', uri, null, null, null, cancellationToken);
                };
                return GoogleLocalImpl;
            }(solidrpc_1.SolidRpc.RpcServiceImpl));
            Google.GoogleLocalImpl = GoogleLocalImpl;
        })(Google = Services.Google || (Services.Google = {}));
        var Facebook;
        (function (Facebook) {
            var FacebookLocalImpl = /** @class */ (function (_super) {
                __extends(FacebookLocalImpl, _super);
                function FacebookLocalImpl() {
                    return _super !== null && _super.apply(this, arguments) || this;
                }
                FacebookLocalImpl.prototype.LoginProvider = function (cancellationToken) {
                    var uri = 'https://localhost/SolidRpc/Security/Services/Facebook/LoginProvider';
                    return this.request('get', uri, null, null, null, cancellationToken);
                };
                FacebookLocalImpl.prototype.LoginScript = function (cancellationToken) {
                    var uri = 'https://localhost/SolidRpc/Security/Services/Facebook/LoginScript';
                    return this.request('get', uri, null, null, null, cancellationToken);
                };
                FacebookLocalImpl.prototype.LoggedIn = function (cancellationToken) {
                    var uri = 'https://localhost/SolidRpc/Security/Services/Facebook/LoggedIn';
                    return this.request('post', uri, null, null, null, cancellationToken);
                };
                FacebookLocalImpl.prototype.LoggedOut = function (cancellationToken) {
                    var uri = 'https://localhost/SolidRpc/Security/Services/Facebook/LoggedOut';
                    return this.request('post', uri, null, null, null, cancellationToken);
                };
                return FacebookLocalImpl;
            }(solidrpc_1.SolidRpc.RpcServiceImpl));
            Facebook.FacebookLocalImpl = FacebookLocalImpl;
            var FacebookRemoteImpl = /** @class */ (function (_super) {
                __extends(FacebookRemoteImpl, _super);
                function FacebookRemoteImpl() {
                    return _super !== null && _super.apply(this, arguments) || this;
                }
                FacebookRemoteImpl.prototype.GetAccessToken = function (clientId, clientSecret, grantType, cancellationToken) {
                    var uri = 'https://graph.facebook.com/oauth/access_token';
                    return this.request('get', uri, {
                        'client_id': clientId,
                        'client_secret': clientSecret,
                        'grant_type': grantType,
                    }, null, null, cancellationToken);
                };
                FacebookRemoteImpl.prototype.GetDebugToken = function (inputToken, accessToken, cancellationToken) {
                    var uri = 'https://graph.facebook.com/v4.0/debug_token';
                    return this.request('get', uri, {
                        'input_token': inputToken,
                        'access_token': accessToken,
                    }, null, null, cancellationToken);
                };
                return FacebookRemoteImpl;
            }(solidrpc_1.SolidRpc.RpcServiceImpl));
            Facebook.FacebookRemoteImpl = FacebookRemoteImpl;
        })(Facebook = Services.Facebook || (Services.Facebook = {}));
        var SolidRpcSecurityImpl = /** @class */ (function (_super) {
            __extends(SolidRpcSecurityImpl, _super);
            function SolidRpcSecurityImpl() {
                return _super !== null && _super.apply(this, arguments) || this;
            }
            SolidRpcSecurityImpl.prototype.LoginPage = function (cancellationToken) {
                var uri = 'https://localhost/SolidRpc/Security/Services/LoginPage';
                return this.request('get', uri, null, null, null, cancellationToken);
            };
            SolidRpcSecurityImpl.prototype.LoginScripts = function (cancellationToken) {
                var uri = 'https://localhost/SolidRpc/Security/Services/LoginScripts';
                return this.request('get', uri, null, null, null, cancellationToken);
            };
            SolidRpcSecurityImpl.prototype.LoginProviders = function (cancellationToken) {
                var uri = 'https://localhost/SolidRpc/Security/Services/LoginProviders';
                return this.request('get', uri, null, null, null, cancellationToken);
            };
            return SolidRpcSecurityImpl;
        }(solidrpc_1.SolidRpc.RpcServiceImpl));
        Services.SolidRpcSecurityImpl = SolidRpcSecurityImpl;
    })(Services = Security.Services || (Security.Services = {}));
})(Security = exports.Security || (exports.Security = {}));
//# sourceMappingURL=types.js.map