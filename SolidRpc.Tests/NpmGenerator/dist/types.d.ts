import { default as CancellationToken } from 'cancellationtoken';
import { Observable } from 'rxjs';
import { SolidRpc } from 'solidrpc';
export declare namespace Security {
    namespace Types {
        class OpenIDConnnectDiscovery {
            constructor(obj?: any);
            Issuer: string;
            AuthorizationEndpoint: string;
            TokenEndpoint: string;
            UserinfoEndpoint: string;
            RevocationEndpoint: string;
            JwksUri: string;
            ScopesSupported: string[];
            ResponseModesSupported: string[];
            SubjectTypesSupported: string[];
            IdTokenSigningAlgValuesSupported: string[];
            EndSessionEndpoint: string;
            ResponseTypesSupported: string[];
            ClaimsSupported: string[];
            TokenEndpointAuthMethodsSupported: string[];
            CodeChallengeMethodsSupported: string[];
            RequestUriParameterSupported: boolean;
            HttpLogoutSupported: boolean;
            FrontchannelLogoutSupported: boolean;
            RbacUrl: string;
            MsgraphHost: string;
            CloudGraphHostName: string;
            CloudInstanceName: string;
            TenantRegionScope: string;
        }
        class OpenIDKeys {
            constructor(obj?: any);
            Keys: Security.Types.OpenIDKey[];
        }
        class OpenIDKey {
            constructor(obj?: any);
            Alg: string;
            Kty: string;
            Use: string;
            Kid: string;
            X5u: string;
            X5t: string;
            X5c: string[];
            N: string;
            E: string;
            Issuer: string;
        }
        class LoginProvider {
            constructor(obj?: any);
            Name: string;
            Status: string;
            Script: string[];
            Meta: Security.Types.LoginProviderMeta[];
            ButtonHtml: string;
        }
        class LoginProviderMeta {
            constructor(obj?: any);
            Name: string;
            Content: string;
        }
        class WebContent {
            constructor(obj?: any);
            Content: Uint8Array;
            ContentType: string;
            CharSet: string;
        }
        class FacebookAccessToken {
            constructor(obj?: any);
            AccessToken: string;
            TokenType: string;
        }
        class FacebookDebugToken {
            constructor(obj?: any);
            Data: Security.Types.FacebookDebugTokenData;
        }
        class FacebookDebugTokenData {
            constructor(obj?: any);
            AppId: number;
            Type: string;
            Application: string;
            DataAccessExpiresAt: number;
            Error: Security.Types.FacebookDebugTokenDataError;
            ExpiresAt: number;
            IsValid: boolean;
            Scopes: string[];
            UserId: number;
        }
        class FacebookDebugTokenDataError {
            constructor(obj?: any);
            Code: number;
            Message: string;
            Subcode: number;
        }
    }
    namespace Services {
        namespace Microsoft {
            interface IMicrosoftRemote {
                Authorize(tenant: string, clientId: string, responseType: string[], redirectUri?: string, scope?: string[], nounce?: string, responseMode?: string, state?: string, resource?: string, prompt?: string, loginHint?: string, domainHint?: string, cancellationToken?: CancellationToken): Observable<void>;
                OpenIdConfiguration(tenant: string, cancellationToken?: CancellationToken): Observable<Security.Types.OpenIDConnnectDiscovery>;
                OpenIdKeys(tenant: string, cancellationToken?: CancellationToken): Observable<Security.Types.OpenIDKeys>;
            }
            class MicrosoftRemoteImpl extends SolidRpc.RpcServiceImpl implements IMicrosoftRemote {
                Authorize(tenant: string, clientId: string, responseType: string[], redirectUri?: string, scope?: string[], nounce?: string, responseMode?: string, state?: string, resource?: string, prompt?: string, loginHint?: string, domainHint?: string, cancellationToken?: CancellationToken): Observable<void>;
                OpenIdConfiguration(tenant: string, cancellationToken?: CancellationToken): Observable<Security.Types.OpenIDConnnectDiscovery>;
                OpenIdKeys(tenant: string, cancellationToken?: CancellationToken): Observable<Security.Types.OpenIDKeys>;
            }
            interface IMicrosoftLocal {
                LoginProvider(cancellationToken?: CancellationToken): Observable<Security.Types.LoginProvider>;
                LoginScript(cancellationToken?: CancellationToken): Observable<Security.Types.WebContent>;
                LoggedIn(cancellationToken?: CancellationToken): Observable<void>;
                LoggedOut(cancellationToken?: CancellationToken): Observable<void>;
            }
            class MicrosoftLocalImpl extends SolidRpc.RpcServiceImpl implements IMicrosoftLocal {
                LoginProvider(cancellationToken?: CancellationToken): Observable<Security.Types.LoginProvider>;
                LoginScript(cancellationToken?: CancellationToken): Observable<Security.Types.WebContent>;
                LoggedIn(cancellationToken?: CancellationToken): Observable<void>;
                LoggedOut(cancellationToken?: CancellationToken): Observable<void>;
            }
        }
        namespace Google {
            interface IGoogleRemote {
                Authorize(clientId: string, responseType: string[], scope: string[], nounce: string, redirectUri: string, state?: string, prompt?: string, display?: string, loginHint?: string, includeGrantedScopes?: boolean, accessType?: string, openidRealm?: string, hd?: string, cancellationToken?: CancellationToken): Observable<void>;
                OpenIdConfiguration(cancellationToken?: CancellationToken): Observable<Security.Types.OpenIDConnnectDiscovery>;
                OpenIdKeys(cancellationToken?: CancellationToken): Observable<Security.Types.OpenIDKeys>;
            }
            class GoogleRemoteImpl extends SolidRpc.RpcServiceImpl implements IGoogleRemote {
                Authorize(clientId: string, responseType: string[], scope: string[], nounce: string, redirectUri: string, state?: string, prompt?: string, display?: string, loginHint?: string, includeGrantedScopes?: boolean, accessType?: string, openidRealm?: string, hd?: string, cancellationToken?: CancellationToken): Observable<void>;
                OpenIdConfiguration(cancellationToken?: CancellationToken): Observable<Security.Types.OpenIDConnnectDiscovery>;
                OpenIdKeys(cancellationToken?: CancellationToken): Observable<Security.Types.OpenIDKeys>;
            }
            interface IGoogleLocal {
                LoginProvider(cancellationToken?: CancellationToken): Observable<Security.Types.LoginProvider>;
                LoginScript(cancellationToken?: CancellationToken): Observable<Security.Types.WebContent>;
                LoggedIn(cancellationToken?: CancellationToken): Observable<void>;
                LoggedOut(cancellationToken?: CancellationToken): Observable<void>;
            }
            class GoogleLocalImpl extends SolidRpc.RpcServiceImpl implements IGoogleLocal {
                LoginProvider(cancellationToken?: CancellationToken): Observable<Security.Types.LoginProvider>;
                LoginScript(cancellationToken?: CancellationToken): Observable<Security.Types.WebContent>;
                LoggedIn(cancellationToken?: CancellationToken): Observable<void>;
                LoggedOut(cancellationToken?: CancellationToken): Observable<void>;
            }
        }
        namespace Facebook {
            interface IFacebookLocal {
                LoginProvider(cancellationToken?: CancellationToken): Observable<Security.Types.LoginProvider>;
                LoginScript(cancellationToken?: CancellationToken): Observable<Security.Types.WebContent>;
                LoggedIn(cancellationToken?: CancellationToken): Observable<void>;
                LoggedOut(cancellationToken?: CancellationToken): Observable<void>;
            }
            class FacebookLocalImpl extends SolidRpc.RpcServiceImpl implements IFacebookLocal {
                LoginProvider(cancellationToken?: CancellationToken): Observable<Security.Types.LoginProvider>;
                LoginScript(cancellationToken?: CancellationToken): Observable<Security.Types.WebContent>;
                LoggedIn(cancellationToken?: CancellationToken): Observable<void>;
                LoggedOut(cancellationToken?: CancellationToken): Observable<void>;
            }
            interface IFacebookRemote {
                GetAccessToken(clientId: string, clientSecret: string, grantType: string, cancellationToken?: CancellationToken): Observable<Security.Types.FacebookAccessToken>;
                GetDebugToken(inputToken: string, accessToken: string, cancellationToken?: CancellationToken): Observable<Security.Types.FacebookDebugToken>;
            }
            class FacebookRemoteImpl extends SolidRpc.RpcServiceImpl implements IFacebookRemote {
                GetAccessToken(clientId: string, clientSecret: string, grantType: string, cancellationToken?: CancellationToken): Observable<Security.Types.FacebookAccessToken>;
                GetDebugToken(inputToken: string, accessToken: string, cancellationToken?: CancellationToken): Observable<Security.Types.FacebookDebugToken>;
            }
        }
        interface ISolidRpcSecurity {
            LoginPage(cancellationToken?: CancellationToken): Observable<Security.Types.WebContent>;
            LoginScripts(cancellationToken?: CancellationToken): Observable<string[]>;
            LoginProviders(cancellationToken?: CancellationToken): Observable<Security.Types.LoginProvider[]>;
        }
        class SolidRpcSecurityImpl extends SolidRpc.RpcServiceImpl implements ISolidRpcSecurity {
            LoginPage(cancellationToken?: CancellationToken): Observable<Security.Types.WebContent>;
            LoginScripts(cancellationToken?: CancellationToken): Observable<string[]>;
            LoginProviders(cancellationToken?: CancellationToken): Observable<Security.Types.LoginProvider[]>;
        }
    }
}
