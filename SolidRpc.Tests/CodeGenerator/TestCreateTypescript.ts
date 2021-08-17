﻿import { default as CancellationToken } from 'cancellationtoken';
import { Observable, Subject } from 'rxjs';
import { share } from 'rxjs/operators'
import { SolidRpc } from 'solidrpc';
export namespace Security {
    export namespace Services {
        export namespace Facebook {
            /**
             * Defines logic for the callback from facebook
             */
            export interface IFacebookLocal {
                /**
                 * Returns the login provider information
                 * @param cancellationToken 
                 */
                LoginProvider(
                    cancellationToken? : CancellationToken
                ): Observable<Security.Types.LoginProvider>;
                /**
                 * This observable is hot and monitors all the responses from the LoginProvider invocations.
                 */
                LoginProviderObservable : Observable<Security.Types.LoginProvider>;
                /**
                 * Returns the script to embedd to enable login
                 * @param cancellationToken 
                 */
                LoginScript(
                    cancellationToken? : CancellationToken
                ): Observable<Security.Types.WebContent>;
                /**
                 * This observable is hot and monitors all the responses from the LoginScript invocations.
                 */
                LoginScriptObservable : Observable<Security.Types.WebContent>;
                /**
                 * Callback when a user has logged in successfully.
                 * @param accessToken The the access token for the logged in user
                 * @param cancellationToken 
                 */
                LoggedIn(
                    accessToken : string,
                    cancellationToken? : CancellationToken
                ): Observable<string>;
                /**
                 * This observable is hot and monitors all the responses from the LoggedIn invocations.
                 */
                LoggedInObservable : Observable<string>;
                /**
                 * Callback when a user has logged out successfully.
                 * @param accessToken The the access token for the logged out in user
                 * @param cancellationToken 
                 */
                LoggedOut(
                    accessToken : string,
                    cancellationToken? : CancellationToken
                ): Observable<string>;
                /**
                 * This observable is hot and monitors all the responses from the LoggedOut invocations.
                 */
                LoggedOutObservable : Observable<string>;
            }
            /**
             * Defines logic for the callback from facebook
             */
            export class FacebookLocalImpl  extends SolidRpc.RpcServiceImpl implements IFacebookLocal {
                constructor() {
                    super();
                    this.LoginProviderSubject = new Subject<Security.Types.LoginProvider>();
                    this.LoginProviderObservable = this.LoginProviderSubject.asObservable().pipe(share());
                    this.LoginScriptSubject = new Subject<Security.Types.WebContent>();
                    this.LoginScriptObservable = this.LoginScriptSubject.asObservable().pipe(share());
                    this.LoggedInSubject = new Subject<string>();
                    this.LoggedInObservable = this.LoggedInSubject.asObservable().pipe(share());
                    this.LoggedOutSubject = new Subject<string>();
                    this.LoggedOutObservable = this.LoggedOutSubject.asObservable().pipe(share());
                }
                /**
                 * Returns the login provider information
                 * @param cancellationToken 
                 */
                LoginProvider(
                    cancellationToken? : CancellationToken
                ): Observable<Security.Types.LoginProvider> {
                    let uri = 'https://localhost/SolidRpc/Security/Services/Facebook/LoginProvider';
                    return this.request<Security.Types.LoginProvider>('get', uri, null, null, null, cancellationToken, function(code : number, data : any) {
                        if(code == 200) {
                            return new Security.Types.LoginProvider(data);
                        } else {
                            throw 'Response code != 200('+code+')';
                        }
                    }, this.LoginProviderSubject);
                }
                /**
                 * This observable is hot and monitors all the responses from the LoginProvider invocations.
                 */
                LoginProviderObservable : Observable<Security.Types.LoginProvider>;
                private LoginProviderSubject : Subject<Security.Types.LoginProvider>;
                /**
                 * Returns the script to embedd to enable login
                 * @param cancellationToken 
                 */
                LoginScript(
                    cancellationToken? : CancellationToken
                ): Observable<Security.Types.WebContent> {
                    let uri = 'https://localhost/SolidRpc/Security/Services/Facebook/LoginScript';
                    return this.request<Security.Types.WebContent>('get', uri, null, null, null, cancellationToken, function(code : number, data : any) {
                        if(code == 200) {
                            return new Security.Types.WebContent(data);
                        } else {
                            throw 'Response code != 200('+code+')';
                        }
                    }, this.LoginScriptSubject);
                }
                /**
                 * This observable is hot and monitors all the responses from the LoginScript invocations.
                 */
                LoginScriptObservable : Observable<Security.Types.WebContent>;
                private LoginScriptSubject : Subject<Security.Types.WebContent>;
                /**
                 * Callback when a user has logged in successfully.
                 * @param accessToken The the access token for the logged in user
                 * @param cancellationToken 
                 */
                LoggedIn(
                    accessToken : string,
                    cancellationToken? : CancellationToken
                ): Observable<string> {
                    let uri = 'https://localhost/SolidRpc/Security/Services/Facebook/LoggedIn/{accessToken}';
                    uri = uri.replace('{accessToken}', this.enocodeUriValue(accessToken.toString()));
                    return this.request<string>('get', uri, null, null, null, cancellationToken, function(code : number, data : any) {
                        if(code == 200) {
                            return data.toString();
                        } else {
                            throw 'Response code != 200('+code+')';
                        }
                    }, this.LoggedInSubject);
                }
                /**
                 * This observable is hot and monitors all the responses from the LoggedIn invocations.
                 */
                LoggedInObservable : Observable<string>;
                private LoggedInSubject : Subject<string>;
                /**
                 * Callback when a user has logged out successfully.
                 * @param accessToken The the access token for the logged out in user
                 * @param cancellationToken 
                 */
                LoggedOut(
                    accessToken : string,
                    cancellationToken? : CancellationToken
                ): Observable<string> {
                    let uri = 'https://localhost/SolidRpc/Security/Services/Facebook/LoggedOut/{accessToken}';
                    uri = uri.replace('{accessToken}', this.enocodeUriValue(accessToken.toString()));
                    return this.request<string>('get', uri, null, null, null, cancellationToken, function(code : number, data : any) {
                        if(code == 200) {
                            return data.toString();
                        } else {
                            throw 'Response code != 200('+code+')';
                        }
                    }, this.LoggedOutSubject);
                }
                /**
                 * This observable is hot and monitors all the responses from the LoggedOut invocations.
                 */
                LoggedOutObservable : Observable<string>;
                private LoggedOutSubject : Subject<string>;
            }
            /**
             * Instance for the IFacebookLocal type. Implemented by the FacebookLocalImpl
             */
            export var FacebookLocalInstance : IFacebookLocal = new FacebookLocalImpl();
            /**
             * Defines logic @ facebook
             */
            export interface IFacebookRemote {
                /**
                 * Obtains an access token for the application
                 * @param clientId 
                 * @param clientSecret 
                 * @param grantType 
                 * @param cancellationToken 
                 */
                GetAccessToken(
                    clientId : string,
                    clientSecret : string,
                    grantType : string,
                    cancellationToken? : CancellationToken
                ): Observable<Security.Types.FacebookAccessToken>;
                /**
                 * This observable is hot and monitors all the responses from the GetAccessToken invocations.
                 */
                GetAccessTokenObservable : Observable<Security.Types.FacebookAccessToken>;
                /**
                 * Returns information about supplied access token
                 * @param inputToken 
                 * @param accessToken 
                 * @param cancellationToken 
                 */
                GetDebugToken(
                    inputToken : string,
                    accessToken : string,
                    cancellationToken? : CancellationToken
                ): Observable<Security.Types.FacebookDebugToken>;
                /**
                 * This observable is hot and monitors all the responses from the GetDebugToken invocations.
                 */
                GetDebugTokenObservable : Observable<Security.Types.FacebookDebugToken>;
            }
            /**
             * Defines logic @ facebook
             */
            export class FacebookRemoteImpl  extends SolidRpc.RpcServiceImpl implements IFacebookRemote {
                constructor() {
                    super();
                    this.GetAccessTokenSubject = new Subject<Security.Types.FacebookAccessToken>();
                    this.GetAccessTokenObservable = this.GetAccessTokenSubject.asObservable().pipe(share());
                    this.GetDebugTokenSubject = new Subject<Security.Types.FacebookDebugToken>();
                    this.GetDebugTokenObservable = this.GetDebugTokenSubject.asObservable().pipe(share());
                }
                /**
                 * Obtains an access token for the application
                 * @param clientId 
                 * @param clientSecret 
                 * @param grantType 
                 * @param cancellationToken 
                 */
                GetAccessToken(
                    clientId : string,
                    clientSecret : string,
                    grantType : string,
                    cancellationToken? : CancellationToken
                ): Observable<Security.Types.FacebookAccessToken> {
                    let uri = 'https://graph.facebook.com/oauth/access_token';
                    return this.request<Security.Types.FacebookAccessToken>('get', uri, {
                        'client_id': clientId,
                        'client_secret': clientSecret,
                        'grant_type': grantType,
}, null, null, cancellationToken, function(code : number, data : any) {
                        if(code == 200) {
                            return new Security.Types.FacebookAccessToken(data);
                        } else {
                            throw 'Response code != 200('+code+')';
                        }
                    }, this.GetAccessTokenSubject);
                }
                /**
                 * This observable is hot and monitors all the responses from the GetAccessToken invocations.
                 */
                GetAccessTokenObservable : Observable<Security.Types.FacebookAccessToken>;
                private GetAccessTokenSubject : Subject<Security.Types.FacebookAccessToken>;
                /**
                 * Returns information about supplied access token
                 * @param inputToken 
                 * @param accessToken 
                 * @param cancellationToken 
                 */
                GetDebugToken(
                    inputToken : string,
                    accessToken : string,
                    cancellationToken? : CancellationToken
                ): Observable<Security.Types.FacebookDebugToken> {
                    let uri = 'https://graph.facebook.com/v4.0/debug_token';
                    return this.request<Security.Types.FacebookDebugToken>('get', uri, {
                        'input_token': inputToken,
                        'access_token': accessToken,
}, null, null, cancellationToken, function(code : number, data : any) {
                        if(code == 200) {
                            return new Security.Types.FacebookDebugToken(data);
                        } else {
                            throw 'Response code != 200('+code+')';
                        }
                    }, this.GetDebugTokenSubject);
                }
                /**
                 * This observable is hot and monitors all the responses from the GetDebugToken invocations.
                 */
                GetDebugTokenObservable : Observable<Security.Types.FacebookDebugToken>;
                private GetDebugTokenSubject : Subject<Security.Types.FacebookDebugToken>;
            }
            /**
             * Instance for the IFacebookRemote type. Implemented by the FacebookRemoteImpl
             */
            export var FacebookRemoteInstance : IFacebookRemote = new FacebookRemoteImpl();
        }
        export namespace Google {
            /**
             * Defines logic for the callback from google
             */
            export interface IGoogleLocal {
                /**
                 * Returns the login provider information
                 * @param cancellationToken 
                 */
                LoginProvider(
                    cancellationToken? : CancellationToken
                ): Observable<Security.Types.LoginProvider>;
                /**
                 * This observable is hot and monitors all the responses from the LoginProvider invocations.
                 */
                LoginProviderObservable : Observable<Security.Types.LoginProvider>;
                /**
                 * Returns the script to embed to enable login
                 * @param cancellationToken 
                 */
                LoginScript(
                    cancellationToken? : CancellationToken
                ): Observable<Security.Types.WebContent>;
                /**
                 * This observable is hot and monitors all the responses from the LoginScript invocations.
                 */
                LoginScriptObservable : Observable<Security.Types.WebContent>;
                /**
                 * Callback when a user has logged in successfully.
                 * @param accessToken The the access token for the logged in user
                 * @param cancellationToken 
                 */
                LoggedIn(
                    accessToken : string,
                    cancellationToken? : CancellationToken
                ): Observable<string>;
                /**
                 * This observable is hot and monitors all the responses from the LoggedIn invocations.
                 */
                LoggedInObservable : Observable<string>;
                /**
                 * Callback when a user has logged out successfully.
                 * @param accessToken The the access token for the logged out in user
                 * @param cancellationToken 
                 */
                LoggedOut(
                    accessToken : string,
                    cancellationToken? : CancellationToken
                ): Observable<string>;
                /**
                 * This observable is hot and monitors all the responses from the LoggedOut invocations.
                 */
                LoggedOutObservable : Observable<string>;
            }
            /**
             * Defines logic for the callback from google
             */
            export class GoogleLocalImpl  extends SolidRpc.RpcServiceImpl implements IGoogleLocal {
                constructor() {
                    super();
                    this.LoginProviderSubject = new Subject<Security.Types.LoginProvider>();
                    this.LoginProviderObservable = this.LoginProviderSubject.asObservable().pipe(share());
                    this.LoginScriptSubject = new Subject<Security.Types.WebContent>();
                    this.LoginScriptObservable = this.LoginScriptSubject.asObservable().pipe(share());
                    this.LoggedInSubject = new Subject<string>();
                    this.LoggedInObservable = this.LoggedInSubject.asObservable().pipe(share());
                    this.LoggedOutSubject = new Subject<string>();
                    this.LoggedOutObservable = this.LoggedOutSubject.asObservable().pipe(share());
                }
                /**
                 * Returns the login provider information
                 * @param cancellationToken 
                 */
                LoginProvider(
                    cancellationToken? : CancellationToken
                ): Observable<Security.Types.LoginProvider> {
                    let uri = 'https://localhost/SolidRpc/Security/Services/Google/LoginProvider';
                    return this.request<Security.Types.LoginProvider>('get', uri, null, null, null, cancellationToken, function(code : number, data : any) {
                        if(code == 200) {
                            return new Security.Types.LoginProvider(data);
                        } else {
                            throw 'Response code != 200('+code+')';
                        }
                    }, this.LoginProviderSubject);
                }
                /**
                 * This observable is hot and monitors all the responses from the LoginProvider invocations.
                 */
                LoginProviderObservable : Observable<Security.Types.LoginProvider>;
                private LoginProviderSubject : Subject<Security.Types.LoginProvider>;
                /**
                 * Returns the script to embed to enable login
                 * @param cancellationToken 
                 */
                LoginScript(
                    cancellationToken? : CancellationToken
                ): Observable<Security.Types.WebContent> {
                    let uri = 'https://localhost/SolidRpc/Security/Services/Google/LoginScript';
                    return this.request<Security.Types.WebContent>('get', uri, null, null, null, cancellationToken, function(code : number, data : any) {
                        if(code == 200) {
                            return new Security.Types.WebContent(data);
                        } else {
                            throw 'Response code != 200('+code+')';
                        }
                    }, this.LoginScriptSubject);
                }
                /**
                 * This observable is hot and monitors all the responses from the LoginScript invocations.
                 */
                LoginScriptObservable : Observable<Security.Types.WebContent>;
                private LoginScriptSubject : Subject<Security.Types.WebContent>;
                /**
                 * Callback when a user has logged in successfully.
                 * @param accessToken The the access token for the logged in user
                 * @param cancellationToken 
                 */
                LoggedIn(
                    accessToken : string,
                    cancellationToken? : CancellationToken
                ): Observable<string> {
                    let uri = 'https://localhost/SolidRpc/Security/Services/Google/LoggedIn/{accessToken}';
                    uri = uri.replace('{accessToken}', this.enocodeUriValue(accessToken.toString()));
                    return this.request<string>('get', uri, null, null, null, cancellationToken, function(code : number, data : any) {
                        if(code == 200) {
                            return data.toString();
                        } else {
                            throw 'Response code != 200('+code+')';
                        }
                    }, this.LoggedInSubject);
                }
                /**
                 * This observable is hot and monitors all the responses from the LoggedIn invocations.
                 */
                LoggedInObservable : Observable<string>;
                private LoggedInSubject : Subject<string>;
                /**
                 * Callback when a user has logged out successfully.
                 * @param accessToken The the access token for the logged out in user
                 * @param cancellationToken 
                 */
                LoggedOut(
                    accessToken : string,
                    cancellationToken? : CancellationToken
                ): Observable<string> {
                    let uri = 'https://localhost/SolidRpc/Security/Services/Google/LoggedOut/{accessToken}';
                    uri = uri.replace('{accessToken}', this.enocodeUriValue(accessToken.toString()));
                    return this.request<string>('get', uri, null, null, null, cancellationToken, function(code : number, data : any) {
                        if(code == 200) {
                            return data.toString();
                        } else {
                            throw 'Response code != 200('+code+')';
                        }
                    }, this.LoggedOutSubject);
                }
                /**
                 * This observable is hot and monitors all the responses from the LoggedOut invocations.
                 */
                LoggedOutObservable : Observable<string>;
                private LoggedOutSubject : Subject<string>;
            }
            /**
             * Instance for the IGoogleLocal type. Implemented by the GoogleLocalImpl
             */
            export var GoogleLocalInstance : IGoogleLocal = new GoogleLocalImpl();
            /**
             * Defines access to the google oauth implementation
             */
            export interface IGoogleRemote {
                /**
                 * Authorizes a user @ google
                 * @param clientId 
                 * @param responseType 
                 * @param scope The client ID string that you obtain from the API Console, as described in Obtain OAuth 2.0 credentials.
                 * @param nounce A random value generated by your app that enables replay protection.
                 * @param redirectUri 
                 * @param state An opaque string that is round-tripped in the protocol; that is to say, it is returned as a URI parameter in the Basic flow, and in the URI #fragment in the Implicit flow. The state can be useful for correlating requests and responses. Because your redirect_uri can be guessed, using a state value can increase your assurance that an incoming connection is the result of an authentication request. If you generate a random string or encode the hash of some client state (e.g., a cookie) in this state variable, you can validate the response to additionally ensure that the request and response originated in the same browser. This provides protection against attacks such as cross-site request forgery.
                 * @param prompt 
                 * @param display An ASCII string value for specifying how the authorization server displays the authentication and consent user interface pages. The following values are specified, and accepted by the Google servers, but do not have any effect on its behavior: page, popup, touch, and wap.
                 * @param loginHint 
                 * @param includeGrantedScopes 
                 * @param accessType 
                 * @param openidRealm 
                 * @param hd The hd (hosted domain) parameter streamlines the login process for G Suite hosted accounts. By including the domain of the G Suite user (for example, mycollege.edu), you can indicate that the account selection UI should be optimized for accounts at that domain. To optimize for G Suite accounts generally instead of just one domain, use an asterisk: hd=*. Don't rely on this UI optimization to control who can access your app, as client-side requests can be modified. Be sure to validate that the returned ID token has an hd claim value that matches what you expect (e.g. mycolledge.edu). Unlike the request parameter, the ID token claim is contained within a security token from Google, so the value can be trusted.
                 * @param cancellationToken 
                 */
                Authorize(
                    clientId : string,
                    responseType : string[],
                    scope : string[],
                    nounce : string,
                    redirectUri : string,
                    state? : string,
                    prompt? : string,
                    display? : string,
                    loginHint? : string,
                    includeGrantedScopes? : boolean,
                    accessType? : string,
                    openidRealm? : string,
                    hd? : string,
                    cancellationToken? : CancellationToken
                ): Observable<void>;
                /**
                 * This observable is hot and monitors all the responses from the Authorize invocations.
                 */
                AuthorizeObservable : Observable<void>;
                /**
                 * Returns the openid configuration
                 * @param cancellationToken 
                 */
                OpenIdConfiguration(
                    cancellationToken? : CancellationToken
                ): Observable<Security.Types.OpenIDConnnectDiscovery>;
                /**
                 * This observable is hot and monitors all the responses from the OpenIdConfiguration invocations.
                 */
                OpenIdConfigurationObservable : Observable<Security.Types.OpenIDConnnectDiscovery>;
                /**
                 * Returns the openid keys used for signing.
                 * @param cancellationToken 
                 */
                OpenIdKeys(
                    cancellationToken? : CancellationToken
                ): Observable<Security.Types.OpenIDKeys>;
                /**
                 * This observable is hot and monitors all the responses from the OpenIdKeys invocations.
                 */
                OpenIdKeysObservable : Observable<Security.Types.OpenIDKeys>;
            }
            /**
             * Defines access to the google oauth implementation
             */
            export class GoogleRemoteImpl  extends SolidRpc.RpcServiceImpl implements IGoogleRemote {
                constructor() {
                    super();
                    this.AuthorizeSubject = new Subject<void>();
                    this.AuthorizeObservable = this.AuthorizeSubject.asObservable().pipe(share());
                    this.OpenIdConfigurationSubject = new Subject<Security.Types.OpenIDConnnectDiscovery>();
                    this.OpenIdConfigurationObservable = this.OpenIdConfigurationSubject.asObservable().pipe(share());
                    this.OpenIdKeysSubject = new Subject<Security.Types.OpenIDKeys>();
                    this.OpenIdKeysObservable = this.OpenIdKeysSubject.asObservable().pipe(share());
                }
                /**
                 * Authorizes a user @ google
                 * @param clientId 
                 * @param responseType 
                 * @param scope The client ID string that you obtain from the API Console, as described in Obtain OAuth 2.0 credentials.
                 * @param nounce A random value generated by your app that enables replay protection.
                 * @param redirectUri 
                 * @param state An opaque string that is round-tripped in the protocol; that is to say, it is returned as a URI parameter in the Basic flow, and in the URI #fragment in the Implicit flow. The state can be useful for correlating requests and responses. Because your redirect_uri can be guessed, using a state value can increase your assurance that an incoming connection is the result of an authentication request. If you generate a random string or encode the hash of some client state (e.g., a cookie) in this state variable, you can validate the response to additionally ensure that the request and response originated in the same browser. This provides protection against attacks such as cross-site request forgery.
                 * @param prompt 
                 * @param display An ASCII string value for specifying how the authorization server displays the authentication and consent user interface pages. The following values are specified, and accepted by the Google servers, but do not have any effect on its behavior: page, popup, touch, and wap.
                 * @param loginHint 
                 * @param includeGrantedScopes 
                 * @param accessType 
                 * @param openidRealm 
                 * @param hd The hd (hosted domain) parameter streamlines the login process for G Suite hosted accounts. By including the domain of the G Suite user (for example, mycollege.edu), you can indicate that the account selection UI should be optimized for accounts at that domain. To optimize for G Suite accounts generally instead of just one domain, use an asterisk: hd=*. Don't rely on this UI optimization to control who can access your app, as client-side requests can be modified. Be sure to validate that the returned ID token has an hd claim value that matches what you expect (e.g. mycolledge.edu). Unlike the request parameter, the ID token claim is contained within a security token from Google, so the value can be trusted.
                 * @param cancellationToken 
                 */
                Authorize(
                    clientId : string,
                    responseType : string[],
                    scope : string[],
                    nounce : string,
                    redirectUri : string,
                    state? : string,
                    prompt? : string,
                    display? : string,
                    loginHint? : string,
                    includeGrantedScopes? : boolean,
                    accessType? : string,
                    openidRealm? : string,
                    hd? : string,
                    cancellationToken? : CancellationToken
                ): Observable<void> {
                    let uri = 'https://accounts.google.com/o/oauth2/v2/auth';
                    return this.request<void>('get', uri, {
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
}, null, null, cancellationToken, function(code : number, data : any) {
                        if(code == 200) {
                            return null;
                        } else {
                            throw 'Response code != 200('+code+')';
                        }
                    }, this.AuthorizeSubject);
                }
                /**
                 * This observable is hot and monitors all the responses from the Authorize invocations.
                 */
                AuthorizeObservable : Observable<void>;
                private AuthorizeSubject : Subject<void>;
                /**
                 * Returns the openid configuration
                 * @param cancellationToken 
                 */
                OpenIdConfiguration(
                    cancellationToken? : CancellationToken
                ): Observable<Security.Types.OpenIDConnnectDiscovery> {
                    let uri = 'https://accounts.google.com/.well-known/openid-configuration';
                    return this.request<Security.Types.OpenIDConnnectDiscovery>('get', uri, null, null, null, cancellationToken, function(code : number, data : any) {
                        if(code == 200) {
                            return new Security.Types.OpenIDConnnectDiscovery(data);
                        } else {
                            throw 'Response code != 200('+code+')';
                        }
                    }, this.OpenIdConfigurationSubject);
                }
                /**
                 * This observable is hot and monitors all the responses from the OpenIdConfiguration invocations.
                 */
                OpenIdConfigurationObservable : Observable<Security.Types.OpenIDConnnectDiscovery>;
                private OpenIdConfigurationSubject : Subject<Security.Types.OpenIDConnnectDiscovery>;
                /**
                 * Returns the openid keys used for signing.
                 * @param cancellationToken 
                 */
                OpenIdKeys(
                    cancellationToken? : CancellationToken
                ): Observable<Security.Types.OpenIDKeys> {
                    let uri = 'https://accounts.google.com/.well-known/openid-keys';
                    return this.request<Security.Types.OpenIDKeys>('get', uri, null, null, null, cancellationToken, function(code : number, data : any) {
                        if(code == 200) {
                            return new Security.Types.OpenIDKeys(data);
                        } else {
                            throw 'Response code != 200('+code+')';
                        }
                    }, this.OpenIdKeysSubject);
                }
                /**
                 * This observable is hot and monitors all the responses from the OpenIdKeys invocations.
                 */
                OpenIdKeysObservable : Observable<Security.Types.OpenIDKeys>;
                private OpenIdKeysSubject : Subject<Security.Types.OpenIDKeys>;
            }
            /**
             * Instance for the IGoogleRemote type. Implemented by the GoogleRemoteImpl
             */
            export var GoogleRemoteInstance : IGoogleRemote = new GoogleRemoteImpl();
        }
        export namespace Microsoft {
            /**
             * Defines logic for the callback from microsoft
             */
            export interface IMicrosoftLocal {
                /**
                 * Returns the login provider information
                 * @param cancellationToken 
                 */
                LoginProvider(
                    cancellationToken? : CancellationToken
                ): Observable<Security.Types.LoginProvider>;
                /**
                 * This observable is hot and monitors all the responses from the LoginProvider invocations.
                 */
                LoginProviderObservable : Observable<Security.Types.LoginProvider>;
                /**
                 * Returns the script to embedd to enable login
                 * @param cancellationToken 
                 */
                LoginScript(
                    cancellationToken? : CancellationToken
                ): Observable<Security.Types.WebContent>;
                /**
                 * This observable is hot and monitors all the responses from the LoginScript invocations.
                 */
                LoginScriptObservable : Observable<Security.Types.WebContent>;
                /**
                 * Callback when a user has logged in successfully.
                 * @param accessToken The the access token for the logged in user
                 * @param cancellationToken 
                 */
                LoggedIn(
                    accessToken : string,
                    cancellationToken? : CancellationToken
                ): Observable<string>;
                /**
                 * This observable is hot and monitors all the responses from the LoggedIn invocations.
                 */
                LoggedInObservable : Observable<string>;
                /**
                 * Callback when a user has logged out successfully.
                 * @param accessToken The the access token for the logged out user
                 * @param cancellationToken 
                 */
                LoggedOut(
                    accessToken : string,
                    cancellationToken? : CancellationToken
                ): Observable<string>;
                /**
                 * This observable is hot and monitors all the responses from the LoggedOut invocations.
                 */
                LoggedOutObservable : Observable<string>;
            }
            /**
             * Defines logic for the callback from microsoft
             */
            export class MicrosoftLocalImpl  extends SolidRpc.RpcServiceImpl implements IMicrosoftLocal {
                constructor() {
                    super();
                    this.LoginProviderSubject = new Subject<Security.Types.LoginProvider>();
                    this.LoginProviderObservable = this.LoginProviderSubject.asObservable().pipe(share());
                    this.LoginScriptSubject = new Subject<Security.Types.WebContent>();
                    this.LoginScriptObservable = this.LoginScriptSubject.asObservable().pipe(share());
                    this.LoggedInSubject = new Subject<string>();
                    this.LoggedInObservable = this.LoggedInSubject.asObservable().pipe(share());
                    this.LoggedOutSubject = new Subject<string>();
                    this.LoggedOutObservable = this.LoggedOutSubject.asObservable().pipe(share());
                }
                /**
                 * Returns the login provider information
                 * @param cancellationToken 
                 */
                LoginProvider(
                    cancellationToken? : CancellationToken
                ): Observable<Security.Types.LoginProvider> {
                    let uri = 'https://localhost/SolidRpc/Security/Services/Microsoft/LoginProvider';
                    return this.request<Security.Types.LoginProvider>('get', uri, null, null, null, cancellationToken, function(code : number, data : any) {
                        if(code == 200) {
                            return new Security.Types.LoginProvider(data);
                        } else {
                            throw 'Response code != 200('+code+')';
                        }
                    }, this.LoginProviderSubject);
                }
                /**
                 * This observable is hot and monitors all the responses from the LoginProvider invocations.
                 */
                LoginProviderObservable : Observable<Security.Types.LoginProvider>;
                private LoginProviderSubject : Subject<Security.Types.LoginProvider>;
                /**
                 * Returns the script to embedd to enable login
                 * @param cancellationToken 
                 */
                LoginScript(
                    cancellationToken? : CancellationToken
                ): Observable<Security.Types.WebContent> {
                    let uri = 'https://localhost/SolidRpc/Security/Services/Microsoft/LoginScript';
                    return this.request<Security.Types.WebContent>('get', uri, null, null, null, cancellationToken, function(code : number, data : any) {
                        if(code == 200) {
                            return new Security.Types.WebContent(data);
                        } else {
                            throw 'Response code != 200('+code+')';
                        }
                    }, this.LoginScriptSubject);
                }
                /**
                 * This observable is hot and monitors all the responses from the LoginScript invocations.
                 */
                LoginScriptObservable : Observable<Security.Types.WebContent>;
                private LoginScriptSubject : Subject<Security.Types.WebContent>;
                /**
                 * Callback when a user has logged in successfully.
                 * @param accessToken The the access token for the logged in user
                 * @param cancellationToken 
                 */
                LoggedIn(
                    accessToken : string,
                    cancellationToken? : CancellationToken
                ): Observable<string> {
                    let uri = 'https://localhost/SolidRpc/Security/Services/Microsoft/LoggedIn';
                    return this.request<string>('get', uri, {
                        'accessToken': accessToken,
}, null, null, cancellationToken, function(code : number, data : any) {
                        if(code == 200) {
                            return data.toString();
                        } else {
                            throw 'Response code != 200('+code+')';
                        }
                    }, this.LoggedInSubject);
                }
                /**
                 * This observable is hot and monitors all the responses from the LoggedIn invocations.
                 */
                LoggedInObservable : Observable<string>;
                private LoggedInSubject : Subject<string>;
                /**
                 * Callback when a user has logged out successfully.
                 * @param accessToken The the access token for the logged out user
                 * @param cancellationToken 
                 */
                LoggedOut(
                    accessToken : string,
                    cancellationToken? : CancellationToken
                ): Observable<string> {
                    let uri = 'https://localhost/SolidRpc/Security/Services/Microsoft/LoggedOut/{accessToken}';
                    uri = uri.replace('{accessToken}', this.enocodeUriValue(accessToken.toString()));
                    return this.request<string>('get', uri, null, null, null, cancellationToken, function(code : number, data : any) {
                        if(code == 200) {
                            return data.toString();
                        } else {
                            throw 'Response code != 200('+code+')';
                        }
                    }, this.LoggedOutSubject);
                }
                /**
                 * This observable is hot and monitors all the responses from the LoggedOut invocations.
                 */
                LoggedOutObservable : Observable<string>;
                private LoggedOutSubject : Subject<string>;
            }
            /**
             * Instance for the IMicrosoftLocal type. Implemented by the MicrosoftLocalImpl
             */
            export var MicrosoftLocalInstance : IMicrosoftLocal = new MicrosoftLocalImpl();
            /**
             * Defines access to the microsoft oauth implementation
             */
            export interface IMicrosoftRemote {
                /**
                 * When your web app needs to authenticate the user, it can direct the user to the /authorize endpoint. This request is similar to the first leg of the OAuth 2.0 authorization code flow, with these important distinctions:
                 * @param tenant You can use the {tenant} value in the path of the request to control who can sign in to the application. The allowed values are common, organizations, consumers, and tenant identifiers
                 * @param clientId 
                 * @param responseType 
                 * @param redirectUri 
                 * @param responseMode 
                 * @param scope The redirect URI of your app, where authentication responses can be sent and received by your app. It must exactly match one of the redirect URIs you registered in the portal, except that it must be URL encoded. If not present, the endpoint will pick one registered redirect_uri at random to send the user back to.
                 * @param state A value included in the request, generated by the app, that will be included in the resulting id_token value as a claim. The app can verify this value to mitigate token replay attacks. The value typically is a randomized, unique string that can be used to identify the origin o
                 * @param nounce A value included in the request, generated by the app, that will be included in the resulting id_token value as a claim. The app can verify this value to mitigate token replay attacks. The value typically is a randomized, unique string that can be used to identify the origin o
                 * @param resource The App ID URI of the target web API (secured resource). To find the App ID URI, in the Azure Portal, click Azure Active Directory, click Application registrations, open the application's Settings page, then click Properties. It may also be an external resource like https://graph.microsoft.com. This is required in one of either the authorization or token requests. To ensure fewer authentication prompts place it in the authorization request to ensure consent is received from the user.
                 * @param prompt Indicates the type of user interaction that is required. The only valid values at this time are login, none, and consent. The prompt=login claim forces the user to enter their credentials on that request, which negates single sign-on. The prompt=none claim is the opposite. This claim ensures that the user isn't presented with any interactive prompt at. If the request can't be completed silently via single sign-on, the Microsoft identity platform endpoint returns an error. The prompt=consent claim triggers the OAuth consent dialog after the user signs in. The dialog asks the user to grant permissions to the app.
                 * @param loginHint 
                 * @param domainHint 
                 * @param cancellationToken 
                 */
                Authorize(
                    tenant : string,
                    clientId : string,
                    responseType : string[],
                    redirectUri : string,
                    responseMode : string,
                    scope : string[],
                    state : string,
                    nounce : string,
                    resource? : string,
                    prompt? : string,
                    loginHint? : string,
                    domainHint? : string,
                    cancellationToken? : CancellationToken
                ): Observable<void>;
                /**
                 * This observable is hot and monitors all the responses from the Authorize invocations.
                 */
                AuthorizeObservable : Observable<void>;
                /**
                 * Returns the openid configuration
                 * @param tenant You can use the {tenant} value in the path of the request to control who can sign in to the application. The allowed values are common, organizations, consumers, and tenant identifiers
                 * @param cancellationToken 
                 */
                OpenIdConfiguration(
                    tenant : string,
                    cancellationToken? : CancellationToken
                ): Observable<Security.Types.OpenIDConnnectDiscovery>;
                /**
                 * This observable is hot and monitors all the responses from the OpenIdConfiguration invocations.
                 */
                OpenIdConfigurationObservable : Observable<Security.Types.OpenIDConnnectDiscovery>;
                /**
                 * Returns the openid keys used for signing.
                 * @param tenant You can use the {tenant} value in the path of the request to control who can sign in to the application. The allowed values are common, organizations, consumers, and tenant identifiers
                 * @param cancellationToken 
                 */
                OpenIdKeys(
                    tenant : string,
                    cancellationToken? : CancellationToken
                ): Observable<Security.Types.OpenIDKeys>;
                /**
                 * This observable is hot and monitors all the responses from the OpenIdKeys invocations.
                 */
                OpenIdKeysObservable : Observable<Security.Types.OpenIDKeys>;
            }
            /**
             * Defines access to the microsoft oauth implementation
             */
            export class MicrosoftRemoteImpl  extends SolidRpc.RpcServiceImpl implements IMicrosoftRemote {
                constructor() {
                    super();
                    this.AuthorizeSubject = new Subject<void>();
                    this.AuthorizeObservable = this.AuthorizeSubject.asObservable().pipe(share());
                    this.OpenIdConfigurationSubject = new Subject<Security.Types.OpenIDConnnectDiscovery>();
                    this.OpenIdConfigurationObservable = this.OpenIdConfigurationSubject.asObservable().pipe(share());
                    this.OpenIdKeysSubject = new Subject<Security.Types.OpenIDKeys>();
                    this.OpenIdKeysObservable = this.OpenIdKeysSubject.asObservable().pipe(share());
                }
                /**
                 * When your web app needs to authenticate the user, it can direct the user to the /authorize endpoint. This request is similar to the first leg of the OAuth 2.0 authorization code flow, with these important distinctions:
                 * @param tenant You can use the {tenant} value in the path of the request to control who can sign in to the application. The allowed values are common, organizations, consumers, and tenant identifiers
                 * @param clientId 
                 * @param responseType 
                 * @param redirectUri 
                 * @param responseMode 
                 * @param scope The redirect URI of your app, where authentication responses can be sent and received by your app. It must exactly match one of the redirect URIs you registered in the portal, except that it must be URL encoded. If not present, the endpoint will pick one registered redirect_uri at random to send the user back to.
                 * @param state A value included in the request, generated by the app, that will be included in the resulting id_token value as a claim. The app can verify this value to mitigate token replay attacks. The value typically is a randomized, unique string that can be used to identify the origin o
                 * @param nounce A value included in the request, generated by the app, that will be included in the resulting id_token value as a claim. The app can verify this value to mitigate token replay attacks. The value typically is a randomized, unique string that can be used to identify the origin o
                 * @param resource The App ID URI of the target web API (secured resource). To find the App ID URI, in the Azure Portal, click Azure Active Directory, click Application registrations, open the application's Settings page, then click Properties. It may also be an external resource like https://graph.microsoft.com. This is required in one of either the authorization or token requests. To ensure fewer authentication prompts place it in the authorization request to ensure consent is received from the user.
                 * @param prompt Indicates the type of user interaction that is required. The only valid values at this time are login, none, and consent. The prompt=login claim forces the user to enter their credentials on that request, which negates single sign-on. The prompt=none claim is the opposite. This claim ensures that the user isn't presented with any interactive prompt at. If the request can't be completed silently via single sign-on, the Microsoft identity platform endpoint returns an error. The prompt=consent claim triggers the OAuth consent dialog after the user signs in. The dialog asks the user to grant permissions to the app.
                 * @param loginHint 
                 * @param domainHint 
                 * @param cancellationToken 
                 */
                Authorize(
                    tenant : string,
                    clientId : string,
                    responseType : string[],
                    redirectUri : string,
                    responseMode : string,
                    scope : string[],
                    state : string,
                    nounce : string,
                    resource? : string,
                    prompt? : string,
                    loginHint? : string,
                    domainHint? : string,
                    cancellationToken? : CancellationToken
                ): Observable<void> {
                    let uri = 'https://login.microsoftonline.com/{tenant}/oauth2/authorize';
                    uri = uri.replace('{tenant}', this.enocodeUriValue(tenant.toString()));
                    return this.request<void>('get', uri, {
                        'client_id': clientId,
                        'response_type': responseType,
                        'redirect_uri': redirectUri,
                        'response_mode': responseMode,
                        'scope': scope,
                        'state': state,
                        'nounce': nounce,
                        'resource': resource,
                        'prompt': prompt,
                        'login_hint': loginHint,
                        'domain_hint': domainHint,
}, null, null, cancellationToken, function(code : number, data : any) {
                        if(code == 200) {
                            return null;
                        } else {
                            throw 'Response code != 200('+code+')';
                        }
                    }, this.AuthorizeSubject);
                }
                /**
                 * This observable is hot and monitors all the responses from the Authorize invocations.
                 */
                AuthorizeObservable : Observable<void>;
                private AuthorizeSubject : Subject<void>;
                /**
                 * Returns the openid configuration
                 * @param tenant You can use the {tenant} value in the path of the request to control who can sign in to the application. The allowed values are common, organizations, consumers, and tenant identifiers
                 * @param cancellationToken 
                 */
                OpenIdConfiguration(
                    tenant : string,
                    cancellationToken? : CancellationToken
                ): Observable<Security.Types.OpenIDConnnectDiscovery> {
                    let uri = 'https://login.microsoftonline.com/{tenant}/v2.0/.well-known/openid-configuration';
                    uri = uri.replace('{tenant}', this.enocodeUriValue(tenant.toString()));
                    return this.request<Security.Types.OpenIDConnnectDiscovery>('get', uri, null, null, null, cancellationToken, function(code : number, data : any) {
                        if(code == 200) {
                            return new Security.Types.OpenIDConnnectDiscovery(data);
                        } else {
                            throw 'Response code != 200('+code+')';
                        }
                    }, this.OpenIdConfigurationSubject);
                }
                /**
                 * This observable is hot and monitors all the responses from the OpenIdConfiguration invocations.
                 */
                OpenIdConfigurationObservable : Observable<Security.Types.OpenIDConnnectDiscovery>;
                private OpenIdConfigurationSubject : Subject<Security.Types.OpenIDConnnectDiscovery>;
                /**
                 * Returns the openid keys used for signing.
                 * @param tenant You can use the {tenant} value in the path of the request to control who can sign in to the application. The allowed values are common, organizations, consumers, and tenant identifiers
                 * @param cancellationToken 
                 */
                OpenIdKeys(
                    tenant : string,
                    cancellationToken? : CancellationToken
                ): Observable<Security.Types.OpenIDKeys> {
                    let uri = 'https://login.microsoftonline.com/{tenant}/discovery/v2.0/keys';
                    uri = uri.replace('{tenant}', this.enocodeUriValue(tenant.toString()));
                    return this.request<Security.Types.OpenIDKeys>('get', uri, null, null, null, cancellationToken, function(code : number, data : any) {
                        if(code == 200) {
                            return new Security.Types.OpenIDKeys(data);
                        } else {
                            throw 'Response code != 200('+code+')';
                        }
                    }, this.OpenIdKeysSubject);
                }
                /**
                 * This observable is hot and monitors all the responses from the OpenIdKeys invocations.
                 */
                OpenIdKeysObservable : Observable<Security.Types.OpenIDKeys>;
                private OpenIdKeysSubject : Subject<Security.Types.OpenIDKeys>;
            }
            /**
             * Instance for the IMicrosoftRemote type. Implemented by the MicrosoftRemoteImpl
             */
            export var MicrosoftRemoteInstance : IMicrosoftRemote = new MicrosoftRemoteImpl();
        }
        export namespace Oidc {
            /**
             * Defines logic for the oidc server
             */
            export interface IOidcServer {
                /**
                 * Returns the /.well-known/openid-configuration file
                 * @param cancellationToken 
                 */
                OAuth2Discovery(
                    cancellationToken? : CancellationToken
                ): Observable<Security.Types.OpenIDConnnectDiscovery>;
                /**
                 * This observable is hot and monitors all the responses from the OAuth2Discovery invocations.
                 */
                OAuth2DiscoveryObservable : Observable<Security.Types.OpenIDConnnectDiscovery>;
                /**
                 * Returns the keys
                 * @param cancellationToken 
                 */
                OAuth2Keys(
                    cancellationToken? : CancellationToken
                ): Observable<Security.Types.OpenIDKeys>;
                /**
                 * This observable is hot and monitors all the responses from the OAuth2Keys invocations.
                 */
                OAuth2KeysObservable : Observable<Security.Types.OpenIDKeys>;
                /**
                 * authenticates a user
                 * @param cancellationToken 
                 */
                OAuth2TokenGet(
                    cancellationToken? : CancellationToken
                ): Observable<Security.Types.TokenResponse>;
                /**
                 * This observable is hot and monitors all the responses from the OAuth2TokenGet invocations.
                 */
                OAuth2TokenGetObservable : Observable<Security.Types.TokenResponse>;
                /**
                 * authenticates a user
                 * @param grantType 
                 * @param clientId 
                 * @param clientSecret 
                 * @param username The user name
                 * @param password The the user password
                 * @param scope The the scopes
                 * @param code The the code
                 * @param redirectUri 
                 * @param codeVerifier 
                 * @param refreshToken 
                 * @param cancellationToken 
                 */
                OAuth2TokenPost(
                    grantType? : string,
                    clientId? : string,
                    clientSecret? : string,
                    username? : string,
                    password? : string,
                    scope? : string[],
                    code? : string,
                    redirectUri? : string,
                    codeVerifier? : string,
                    refreshToken? : string,
                    cancellationToken? : CancellationToken
                ): Observable<Security.Types.TokenResponse>;
                /**
                 * This observable is hot and monitors all the responses from the OAuth2TokenPost invocations.
                 */
                OAuth2TokenPostObservable : Observable<Security.Types.TokenResponse>;
                /**
                 * authorizes a user
                 * @param scope REQUIRED. OpenID Connect requests MUST contain the openid scope value. If the openid scope value is not present, the behavior is entirely unspecified. Other scope values MAY be present. Scope values used that are not understood by an implementation SHOULD be ignored. See Sections 5.4 and 11 for additional scope values defined by this specification.
                 * @param responseType 
                 * @param clientId 
                 * @param redirectUri 
                 * @param state RECOMMENDED. Opaque value used to maintain state between the request and the callback. Typically, Cross-Site Request Forgery (CSRF, XSRF) mitigation is done by cryptographically binding the value of this parameter with a browser cookie.
                 * @param cancellationToken 
                 */
                OAuth2AuthorizeGet(
                    scope : string[],
                    responseType : string,
                    clientId : string,
                    redirectUri? : string,
                    state? : string,
                    cancellationToken? : CancellationToken
                ): Observable<Security.Types.WebContent>;
                /**
                 * This observable is hot and monitors all the responses from the OAuth2AuthorizeGet invocations.
                 */
                OAuth2AuthorizeGetObservable : Observable<Security.Types.WebContent>;
                /**
                 * authorizes a user
                 * @param cancellationToken 
                 */
                OAuth2AuthorizePost(
                    cancellationToken? : CancellationToken
                ): Observable<Security.Types.WebContent>;
                /**
                 * This observable is hot and monitors all the responses from the OAuth2AuthorizePost invocations.
                 */
                OAuth2AuthorizePostObservable : Observable<Security.Types.WebContent>;
            }
            /**
             * Defines logic for the oidc server
             */
            export class OidcServerImpl  extends SolidRpc.RpcServiceImpl implements IOidcServer {
                constructor() {
                    super();
                    this.OAuth2DiscoverySubject = new Subject<Security.Types.OpenIDConnnectDiscovery>();
                    this.OAuth2DiscoveryObservable = this.OAuth2DiscoverySubject.asObservable().pipe(share());
                    this.OAuth2KeysSubject = new Subject<Security.Types.OpenIDKeys>();
                    this.OAuth2KeysObservable = this.OAuth2KeysSubject.asObservable().pipe(share());
                    this.OAuth2TokenGetSubject = new Subject<Security.Types.TokenResponse>();
                    this.OAuth2TokenGetObservable = this.OAuth2TokenGetSubject.asObservable().pipe(share());
                    this.OAuth2TokenPostSubject = new Subject<Security.Types.TokenResponse>();
                    this.OAuth2TokenPostObservable = this.OAuth2TokenPostSubject.asObservable().pipe(share());
                    this.OAuth2AuthorizeGetSubject = new Subject<Security.Types.WebContent>();
                    this.OAuth2AuthorizeGetObservable = this.OAuth2AuthorizeGetSubject.asObservable().pipe(share());
                    this.OAuth2AuthorizePostSubject = new Subject<Security.Types.WebContent>();
                    this.OAuth2AuthorizePostObservable = this.OAuth2AuthorizePostSubject.asObservable().pipe(share());
                }
                /**
                 * Returns the /.well-known/openid-configuration file
                 * @param cancellationToken 
                 */
                OAuth2Discovery(
                    cancellationToken? : CancellationToken
                ): Observable<Security.Types.OpenIDConnnectDiscovery> {
                    let uri = 'https://localhost/SolidRpc/Security/Services/Oidc/discovery';
                    return this.request<Security.Types.OpenIDConnnectDiscovery>('get', uri, null, null, null, cancellationToken, function(code : number, data : any) {
                        if(code == 200) {
                            return new Security.Types.OpenIDConnnectDiscovery(data);
                        } else {
                            throw 'Response code != 200('+code+')';
                        }
                    }, this.OAuth2DiscoverySubject);
                }
                /**
                 * This observable is hot and monitors all the responses from the OAuth2Discovery invocations.
                 */
                OAuth2DiscoveryObservable : Observable<Security.Types.OpenIDConnnectDiscovery>;
                private OAuth2DiscoverySubject : Subject<Security.Types.OpenIDConnnectDiscovery>;
                /**
                 * Returns the keys
                 * @param cancellationToken 
                 */
                OAuth2Keys(
                    cancellationToken? : CancellationToken
                ): Observable<Security.Types.OpenIDKeys> {
                    let uri = 'https://localhost/SolidRpc/Security/Services/Oidc/keys';
                    return this.request<Security.Types.OpenIDKeys>('get', uri, null, null, null, cancellationToken, function(code : number, data : any) {
                        if(code == 200) {
                            return new Security.Types.OpenIDKeys(data);
                        } else {
                            throw 'Response code != 200('+code+')';
                        }
                    }, this.OAuth2KeysSubject);
                }
                /**
                 * This observable is hot and monitors all the responses from the OAuth2Keys invocations.
                 */
                OAuth2KeysObservable : Observable<Security.Types.OpenIDKeys>;
                private OAuth2KeysSubject : Subject<Security.Types.OpenIDKeys>;
                /**
                 * authenticates a user
                 * @param cancellationToken 
                 */
                OAuth2TokenGet(
                    cancellationToken? : CancellationToken
                ): Observable<Security.Types.TokenResponse> {
                    let uri = 'https://localhost/SolidRpc/Security/Services/Oidc/token';
                    return this.request<Security.Types.TokenResponse>('get', uri, null, null, null, cancellationToken, function(code : number, data : any) {
                        if(code == 200) {
                            return new Security.Types.TokenResponse(data);
                        } else {
                            throw 'Response code != 200('+code+')';
                        }
                    }, this.OAuth2TokenGetSubject);
                }
                /**
                 * This observable is hot and monitors all the responses from the OAuth2TokenGet invocations.
                 */
                OAuth2TokenGetObservable : Observable<Security.Types.TokenResponse>;
                private OAuth2TokenGetSubject : Subject<Security.Types.TokenResponse>;
                /**
                 * authenticates a user
                 * @param grantType 
                 * @param clientId 
                 * @param clientSecret 
                 * @param username The user name
                 * @param password The the user password
                 * @param scope The the scopes
                 * @param code The the code
                 * @param redirectUri 
                 * @param codeVerifier 
                 * @param refreshToken 
                 * @param cancellationToken 
                 */
                OAuth2TokenPost(
                    grantType? : string,
                    clientId? : string,
                    clientSecret? : string,
                    username? : string,
                    password? : string,
                    scope? : string[],
                    code? : string,
                    redirectUri? : string,
                    codeVerifier? : string,
                    refreshToken? : string,
                    cancellationToken? : CancellationToken
                ): Observable<Security.Types.TokenResponse> {
                    let uri = 'https://localhost/SolidRpc/Security/Services/Oidc/token';
                    return this.request<Security.Types.TokenResponse>('post', uri, null, null, null, cancellationToken, function(code : number, data : any) {
                        if(code == 200) {
                            return new Security.Types.TokenResponse(data);
                        } else {
                            throw 'Response code != 200('+code+')';
                        }
                    }, this.OAuth2TokenPostSubject);
                }
                /**
                 * This observable is hot and monitors all the responses from the OAuth2TokenPost invocations.
                 */
                OAuth2TokenPostObservable : Observable<Security.Types.TokenResponse>;
                private OAuth2TokenPostSubject : Subject<Security.Types.TokenResponse>;
                /**
                 * authorizes a user
                 * @param scope REQUIRED. OpenID Connect requests MUST contain the openid scope value. If the openid scope value is not present, the behavior is entirely unspecified. Other scope values MAY be present. Scope values used that are not understood by an implementation SHOULD be ignored. See Sections 5.4 and 11 for additional scope values defined by this specification.
                 * @param responseType 
                 * @param clientId 
                 * @param redirectUri 
                 * @param state RECOMMENDED. Opaque value used to maintain state between the request and the callback. Typically, Cross-Site Request Forgery (CSRF, XSRF) mitigation is done by cryptographically binding the value of this parameter with a browser cookie.
                 * @param cancellationToken 
                 */
                OAuth2AuthorizeGet(
                    scope : string[],
                    responseType : string,
                    clientId : string,
                    redirectUri? : string,
                    state? : string,
                    cancellationToken? : CancellationToken
                ): Observable<Security.Types.WebContent> {
                    let uri = 'https://localhost/SolidRpc/Security/Services/Oidc/authorize';
                    return this.request<Security.Types.WebContent>('get', uri, {
                        'scope': scope,
                        'response_type': responseType,
                        'client_id': clientId,
                        'redirect_uri': redirectUri,
                        'state': state,
}, null, null, cancellationToken, function(code : number, data : any) {
                        if(code == 200) {
                            return new Security.Types.WebContent(data);
                        } else {
                            throw 'Response code != 200('+code+')';
                        }
                    }, this.OAuth2AuthorizeGetSubject);
                }
                /**
                 * This observable is hot and monitors all the responses from the OAuth2AuthorizeGet invocations.
                 */
                OAuth2AuthorizeGetObservable : Observable<Security.Types.WebContent>;
                private OAuth2AuthorizeGetSubject : Subject<Security.Types.WebContent>;
                /**
                 * authorizes a user
                 * @param cancellationToken 
                 */
                OAuth2AuthorizePost(
                    cancellationToken? : CancellationToken
                ): Observable<Security.Types.WebContent> {
                    let uri = 'https://localhost/SolidRpc/Security/Services/Oidc/authorize';
                    return this.request<Security.Types.WebContent>('post', uri, null, null, null, cancellationToken, function(code : number, data : any) {
                        if(code == 200) {
                            return new Security.Types.WebContent(data);
                        } else {
                            throw 'Response code != 200('+code+')';
                        }
                    }, this.OAuth2AuthorizePostSubject);
                }
                /**
                 * This observable is hot and monitors all the responses from the OAuth2AuthorizePost invocations.
                 */
                OAuth2AuthorizePostObservable : Observable<Security.Types.WebContent>;
                private OAuth2AuthorizePostSubject : Subject<Security.Types.WebContent>;
            }
            /**
             * Instance for the IOidcServer type. Implemented by the OidcServerImpl
             */
            export var OidcServerInstance : IOidcServer = new OidcServerImpl();
        }
        /**
         * Defines logic for solid rpc security
         */
        export interface ISolidRpcSecurity {
            /**
             * Returns the login page
             * @param cancellationToken 
             */
            LoginPage(
                cancellationToken? : CancellationToken
            ): Observable<Security.Types.WebContent>;
            /**
             * This observable is hot and monitors all the responses from the LoginPage invocations.
             */
            LoginPageObservable : Observable<Security.Types.WebContent>;
            /**
             * Returns the script paths to use for logging in.
             * @param cancellationToken 
             */
            LoginScripts(
                cancellationToken? : CancellationToken
            ): Observable<string[]>;
            /**
             * This observable is hot and monitors all the responses from the LoginScripts invocations.
             */
            LoginScriptsObservable : Observable<string[]>;
            /**
             * Returns the script to embedd to enable login
             * @param cancellationToken 
             */
            LoginScript(
                cancellationToken? : CancellationToken
            ): Observable<Security.Types.WebContent>;
            /**
             * This observable is hot and monitors all the responses from the LoginScript invocations.
             */
            LoginScriptObservable : Observable<Security.Types.WebContent>;
            /**
             * Returns the status at each login provider
             * @param cancellationToken 
             */
            LoginProviders(
                cancellationToken? : CancellationToken
            ): Observable<Security.Types.LoginProvider[]>;
            /**
             * This observable is hot and monitors all the responses from the LoginProviders invocations.
             */
            LoginProvidersObservable : Observable<Security.Types.LoginProvider[]>;
            /**
             * Returns the current profile claims
             * @param cancellationToken 
             */
            Profile(
                cancellationToken? : CancellationToken
            ): Observable<Security.Types.Claim[]>;
            /**
             * This observable is hot and monitors all the responses from the Profile invocations.
             */
            ProfileObservable : Observable<Security.Types.Claim[]>;
        }
        /**
         * Defines logic for solid rpc security
         */
        export class SolidRpcSecurityImpl  extends SolidRpc.RpcServiceImpl implements ISolidRpcSecurity {
            constructor() {
                super();
                this.LoginPageSubject = new Subject<Security.Types.WebContent>();
                this.LoginPageObservable = this.LoginPageSubject.asObservable().pipe(share());
                this.LoginScriptsSubject = new Subject<string[]>();
                this.LoginScriptsObservable = this.LoginScriptsSubject.asObservable().pipe(share());
                this.LoginScriptSubject = new Subject<Security.Types.WebContent>();
                this.LoginScriptObservable = this.LoginScriptSubject.asObservable().pipe(share());
                this.LoginProvidersSubject = new Subject<Security.Types.LoginProvider[]>();
                this.LoginProvidersObservable = this.LoginProvidersSubject.asObservable().pipe(share());
                this.ProfileSubject = new Subject<Security.Types.Claim[]>();
                this.ProfileObservable = this.ProfileSubject.asObservable().pipe(share());
            }
            /**
             * Returns the login page
             * @param cancellationToken 
             */
            LoginPage(
                cancellationToken? : CancellationToken
            ): Observable<Security.Types.WebContent> {
                let uri = 'https://localhost/SolidRpc/Security/Services/LoginPage';
                return this.request<Security.Types.WebContent>('get', uri, null, null, null, cancellationToken, function(code : number, data : any) {
                    if(code == 200) {
                        return new Security.Types.WebContent(data);
                    } else {
                        throw 'Response code != 200('+code+')';
                    }
                }, this.LoginPageSubject);
            }
            /**
             * This observable is hot and monitors all the responses from the LoginPage invocations.
             */
            LoginPageObservable : Observable<Security.Types.WebContent>;
            private LoginPageSubject : Subject<Security.Types.WebContent>;
            /**
             * Returns the script paths to use for logging in.
             * @param cancellationToken 
             */
            LoginScripts(
                cancellationToken? : CancellationToken
            ): Observable<string[]> {
                let uri = 'https://localhost/SolidRpc/Security/Services/LoginScripts';
                return this.request<string[]>('get', uri, null, null, null, cancellationToken, function(code : number, data : any) {
                    if(code == 200) {
                        return Array.from(data).map(o => o.toString());
                    } else {
                        throw 'Response code != 200('+code+')';
                    }
                }, this.LoginScriptsSubject);
            }
            /**
             * This observable is hot and monitors all the responses from the LoginScripts invocations.
             */
            LoginScriptsObservable : Observable<string[]>;
            private LoginScriptsSubject : Subject<string[]>;
            /**
             * Returns the script to embedd to enable login
             * @param cancellationToken 
             */
            LoginScript(
                cancellationToken? : CancellationToken
            ): Observable<Security.Types.WebContent> {
                let uri = 'https://localhost/SolidRpc/Security/Services/LoginScript';
                return this.request<Security.Types.WebContent>('get', uri, null, null, null, cancellationToken, function(code : number, data : any) {
                    if(code == 200) {
                        return new Security.Types.WebContent(data);
                    } else {
                        throw 'Response code != 200('+code+')';
                    }
                }, this.LoginScriptSubject);
            }
            /**
             * This observable is hot and monitors all the responses from the LoginScript invocations.
             */
            LoginScriptObservable : Observable<Security.Types.WebContent>;
            private LoginScriptSubject : Subject<Security.Types.WebContent>;
            /**
             * Returns the status at each login provider
             * @param cancellationToken 
             */
            LoginProviders(
                cancellationToken? : CancellationToken
            ): Observable<Security.Types.LoginProvider[]> {
                let uri = 'https://localhost/SolidRpc/Security/Services/LoginProviders';
                return this.request<Security.Types.LoginProvider[]>('get', uri, null, null, null, cancellationToken, function(code : number, data : any) {
                    if(code == 200) {
                        return Array.from(data).map(o => new Security.Types.LoginProvider(o));
                    } else {
                        throw 'Response code != 200('+code+')';
                    }
                }, this.LoginProvidersSubject);
            }
            /**
             * This observable is hot and monitors all the responses from the LoginProviders invocations.
             */
            LoginProvidersObservable : Observable<Security.Types.LoginProvider[]>;
            private LoginProvidersSubject : Subject<Security.Types.LoginProvider[]>;
            /**
             * Returns the current profile claims
             * @param cancellationToken 
             */
            Profile(
                cancellationToken? : CancellationToken
            ): Observable<Security.Types.Claim[]> {
                let uri = 'https://localhost/SolidRpc/Security/Services/Profile';
                return this.request<Security.Types.Claim[]>('get', uri, null, null, null, cancellationToken, function(code : number, data : any) {
                    if(code == 200) {
                        return Array.from(data).map(o => new Security.Types.Claim(o));
                    } else {
                        throw 'Response code != 200('+code+')';
                    }
                }, this.ProfileSubject);
            }
            /**
             * This observable is hot and monitors all the responses from the Profile invocations.
             */
            ProfileObservable : Observable<Security.Types.Claim[]>;
            private ProfileSubject : Subject<Security.Types.Claim[]>;
        }
        /**
         * Instance for the ISolidRpcSecurity type. Implemented by the SolidRpcSecurityImpl
         */
        export var SolidRpcSecurityInstance : ISolidRpcSecurity = new SolidRpcSecurityImpl();
    }
    export namespace Types {
        /**
         * 
         */
        export class Claim {
            constructor(obj?: any) {
                for(let prop in obj) {
                    switch(prop) {
                        case "name":
                            if (obj.name) { this.Name = obj.name.toString(); }
                            break;
                        case "value":
                            if (obj.value) { this.Value = obj.value.toString(); }
                            break;
                    }
                }
            }
            toJson(arr: string[] | null): string | null {
                let returnString = false
                if(arr == null) {
                    arr = [];
                    returnString = true;
                }
                arr.push('{');
                if(this.Name) { arr.push('"name": '); arr.push(JSON.stringify(this.Name)); arr.push(','); } 
                if(this.Value) { arr.push('"value": '); arr.push(JSON.stringify(this.Value)); arr.push(','); } 
                if(arr[arr.length-1] == ',') arr[arr.length-1] = '}'; else arr.push('}');
                if(returnString) return arr.join("");
                return null;
            }
            /**
             * The name of the claim
             */
            Name: string | null = null;
            /**
             * The value of the claim
             */
            Value: string | null = null;
        }
        /**
         * success
         */
        export class FacebookAccessToken {
            constructor(obj?: any) {
                for(let prop in obj) {
                    switch(prop) {
                        case "access_token":
                            if (obj.access_token) { this.AccessToken = obj.access_token.toString(); }
                            break;
                        case "token_type":
                            if (obj.token_type) { this.TokenType = obj.token_type.toString(); }
                            break;
                    }
                }
            }
            toJson(arr: string[] | null): string | null {
                let returnString = false
                if(arr == null) {
                    arr = [];
                    returnString = true;
                }
                arr.push('{');
                if(this.AccessToken) { arr.push('"access_token": '); arr.push(JSON.stringify(this.AccessToken)); arr.push(','); } 
                if(this.TokenType) { arr.push('"token_type": '); arr.push(JSON.stringify(this.TokenType)); arr.push(','); } 
                if(arr[arr.length-1] == ',') arr[arr.length-1] = '}'; else arr.push('}');
                if(returnString) return arr.join("");
                return null;
            }
            /**
             * 
             */
            AccessToken: string | null = null;
            /**
             * 
             */
            TokenType: string | null = null;
        }
        /**
         * success
         */
        export class FacebookDebugToken {
            constructor(obj?: any) {
                for(let prop in obj) {
                    switch(prop) {
                        case "data":
                            if (obj.data) { this.Data = new Security.Types.FacebookDebugTokenData(obj.data); }
                            break;
                    }
                }
            }
            toJson(arr: string[] | null): string | null {
                let returnString = false
                if(arr == null) {
                    arr = [];
                    returnString = true;
                }
                arr.push('{');
                if(this.Data) { arr.push('"data": '); this.Data.toJson(arr); arr.push(','); } 
                if(arr[arr.length-1] == ',') arr[arr.length-1] = '}'; else arr.push('}');
                if(returnString) return arr.join("");
                return null;
            }
            /**
             * 
             */
            Data: Security.Types.FacebookDebugTokenData | null = null;
        }
        /**
         * 
         */
        export class FacebookDebugTokenData {
            constructor(obj?: any) {
                for(let prop in obj) {
                    switch(prop) {
                        case "app_id":
                            if (obj.app_id) { this.AppId = Number(obj.app_id); }
                            break;
                        case "type":
                            if (obj.type) { this.Type = obj.type.toString(); }
                            break;
                        case "application":
                            if (obj.application) { this.Application = obj.application.toString(); }
                            break;
                        case "data_access_expires_at":
                            if (obj.data_access_expires_at) { this.DataAccessExpiresAt = Number(obj.data_access_expires_at); }
                            break;
                        case "error":
                            if (obj.error) { this.Error = new Security.Types.FacebookDebugTokenDataError(obj.error); }
                            break;
                        case "expires_at":
                            if (obj.expires_at) { this.ExpiresAt = Number(obj.expires_at); }
                            break;
                        case "is_valid":
                            if (obj.is_valid) { this.IsValid = [true, 'true', 1].some(o => o === obj.is_valid); }
                            break;
                        case "scopes":
                            if (obj.scopes) { this.Scopes = Array.from(obj.scopes).map(o => o.toString()); }
                            break;
                        case "user_id":
                            if (obj.user_id) { this.UserId = Number(obj.user_id); }
                            break;
                    }
                }
            }
            toJson(arr: string[] | null): string | null {
                let returnString = false
                if(arr == null) {
                    arr = [];
                    returnString = true;
                }
                arr.push('{');
                if(this.AppId) { arr.push('"app_id": '); arr.push(JSON.stringify(this.AppId)); arr.push(','); } 
                if(this.Type) { arr.push('"type": '); arr.push(JSON.stringify(this.Type)); arr.push(','); } 
                if(this.Application) { arr.push('"application": '); arr.push(JSON.stringify(this.Application)); arr.push(','); } 
                if(this.DataAccessExpiresAt) { arr.push('"data_access_expires_at": '); arr.push(JSON.stringify(this.DataAccessExpiresAt)); arr.push(','); } 
                if(this.Error) { arr.push('"error": '); this.Error.toJson(arr); arr.push(','); } 
                if(this.ExpiresAt) { arr.push('"expires_at": '); arr.push(JSON.stringify(this.ExpiresAt)); arr.push(','); } 
                if(this.IsValid) { arr.push('"is_valid": '); arr.push(JSON.stringify(this.IsValid)); arr.push(','); } 
                if(this.Scopes) { arr.push('"scopes": '); this.Scopes.forEach(o => arr.push(JSON.stringify(o))); arr.push(','); } 
                if(this.UserId) { arr.push('"user_id": '); arr.push(JSON.stringify(this.UserId)); arr.push(','); } 
                if(arr[arr.length-1] == ',') arr[arr.length-1] = '}'; else arr.push('}');
                if(returnString) return arr.join("");
                return null;
            }
            /**
             * 
             */
            AppId: number | null = null;
            /**
             * 
             */
            Type: string | null = null;
            /**
             * 
             */
            Application: string | null = null;
            /**
             * 
             */
            DataAccessExpiresAt: number | null = null;
            /**
             * 
             */
            Error: Security.Types.FacebookDebugTokenDataError | null = null;
            /**
             * 
             */
            ExpiresAt: number | null = null;
            /**
             * 
             */
            IsValid: boolean | null = null;
            /**
             * 
             */
            Scopes: string[] | null = null;
            /**
             * 
             */
            UserId: number | null = null;
        }
        /**
         * 
         */
        export class FacebookDebugTokenDataError {
            constructor(obj?: any) {
                for(let prop in obj) {
                    switch(prop) {
                        case "code":
                            if (obj.code) { this.Code = Number(obj.code); }
                            break;
                        case "message":
                            if (obj.message) { this.Message = obj.message.toString(); }
                            break;
                        case "subcode":
                            if (obj.subcode) { this.Subcode = Number(obj.subcode); }
                            break;
                    }
                }
            }
            toJson(arr: string[] | null): string | null {
                let returnString = false
                if(arr == null) {
                    arr = [];
                    returnString = true;
                }
                arr.push('{');
                if(this.Code) { arr.push('"code": '); arr.push(JSON.stringify(this.Code)); arr.push(','); } 
                if(this.Message) { arr.push('"message": '); arr.push(JSON.stringify(this.Message)); arr.push(','); } 
                if(this.Subcode) { arr.push('"subcode": '); arr.push(JSON.stringify(this.Subcode)); arr.push(','); } 
                if(arr[arr.length-1] == ',') arr[arr.length-1] = '}'; else arr.push('}');
                if(returnString) return arr.join("");
                return null;
            }
            /**
             * 
             */
            Code: number | null = null;
            /**
             * 
             */
            Message: string | null = null;
            /**
             * 
             */
            Subcode: number | null = null;
        }
        /**
         * success
         */
        export class LoginProvider {
            constructor(obj?: any) {
                for(let prop in obj) {
                    switch(prop) {
                        case "name":
                            if (obj.name) { this.Name = obj.name.toString(); }
                            break;
                        case "status":
                            if (obj.status) { this.Status = obj.status.toString(); }
                            break;
                        case "script":
                            if (obj.script) { this.Script = Array.from(obj.script).map(o => o.toString()); }
                            break;
                        case "meta":
                            if (obj.meta) { this.Meta = Array.from(obj.meta).map(o => new Security.Types.LoginProviderMeta(o)); }
                            break;
                        case "buttonHtml":
                            if (obj.buttonHtml) { this.ButtonHtml = obj.buttonHtml.toString(); }
                            break;
                    }
                }
            }
            toJson(arr: string[] | null): string | null {
                let returnString = false
                if(arr == null) {
                    arr = [];
                    returnString = true;
                }
                arr.push('{');
                if(this.Name) { arr.push('"name": '); arr.push(JSON.stringify(this.Name)); arr.push(','); } 
                if(this.Status) { arr.push('"status": '); arr.push(JSON.stringify(this.Status)); arr.push(','); } 
                if(this.Script) { arr.push('"script": '); this.Script.forEach(o => arr.push(JSON.stringify(o))); arr.push(','); } 
                if(this.Meta) { arr.push('"meta": '); this.Meta.forEach(o => o.toJson(arr)); arr.push(','); } 
                if(this.ButtonHtml) { arr.push('"buttonHtml": '); arr.push(JSON.stringify(this.ButtonHtml)); arr.push(','); } 
                if(arr[arr.length-1] == ',') arr[arr.length-1] = '}'; else arr.push('}');
                if(returnString) return arr.join("");
                return null;
            }
            /**
             * The name of the provider
             */
            Name: string | null = null;
            /**
             * The user status @ the provider. LoggedIn or NotLoggedIn
             */
            Status: string | null = null;
            /**
             * The script uris
             */
            Script: string[] | null = null;
            /**
             * The script uris
             */
            Meta: Security.Types.LoginProviderMeta[] | null = null;
            /**
             * The html for the login button
             */
            ButtonHtml: string | null = null;
        }
        /**
         * 
         */
        export class LoginProviderMeta {
            constructor(obj?: any) {
                for(let prop in obj) {
                    switch(prop) {
                        case "name":
                            if (obj.name) { this.Name = obj.name.toString(); }
                            break;
                        case "content":
                            if (obj.content) { this.Content = obj.content.toString(); }
                            break;
                    }
                }
            }
            toJson(arr: string[] | null): string | null {
                let returnString = false
                if(arr == null) {
                    arr = [];
                    returnString = true;
                }
                arr.push('{');
                if(this.Name) { arr.push('"name": '); arr.push(JSON.stringify(this.Name)); arr.push(','); } 
                if(this.Content) { arr.push('"content": '); arr.push(JSON.stringify(this.Content)); arr.push(','); } 
                if(arr[arr.length-1] == ',') arr[arr.length-1] = '}'; else arr.push('}');
                if(returnString) return arr.join("");
                return null;
            }
            /**
             * The name of the meta data
             */
            Name: string | null = null;
            /**
             * The content of the meta data
             */
            Content: string | null = null;
        }
        /**
         * success
         */
        export class OpenIDConnnectDiscovery {
            constructor(obj?: any) {
                for(let prop in obj) {
                    switch(prop) {
                        case "issuer":
                            if (obj.issuer) { this.Issuer = obj.issuer.toString(); }
                            break;
                        case "authorization_endpoint":
                            if (obj.authorization_endpoint) { this.AuthorizationEndpoint = obj.authorization_endpoint.toString(); }
                            break;
                        case "token_endpoint":
                            if (obj.token_endpoint) { this.TokenEndpoint = obj.token_endpoint.toString(); }
                            break;
                        case "userinfo_endpoint":
                            if (obj.userinfo_endpoint) { this.UserinfoEndpoint = obj.userinfo_endpoint.toString(); }
                            break;
                        case "revocation_endpoint":
                            if (obj.revocation_endpoint) { this.RevocationEndpoint = obj.revocation_endpoint.toString(); }
                            break;
                        case "device_authorization_endpoint":
                            if (obj.device_authorization_endpoint) { this.DeviceAuthorizationEndpoint = obj.device_authorization_endpoint.toString(); }
                            break;
                        case "jwks_uri":
                            if (obj.jwks_uri) { this.JwksUri = obj.jwks_uri.toString(); }
                            break;
                        case "scopes_supported":
                            if (obj.scopes_supported) { this.ScopesSupported = Array.from(obj.scopes_supported).map(o => o.toString()); }
                            break;
                        case "grant_types_supported":
                            if (obj.grant_types_supported) { this.GrantTypesSupported = Array.from(obj.grant_types_supported).map(o => o.toString()); }
                            break;
                        case "response_modes_supported":
                            if (obj.response_modes_supported) { this.ResponseModesSupported = Array.from(obj.response_modes_supported).map(o => o.toString()); }
                            break;
                        case "subject_types_supported":
                            if (obj.subject_types_supported) { this.SubjectTypesSupported = Array.from(obj.subject_types_supported).map(o => o.toString()); }
                            break;
                        case "id_token_signing_alg_values_supported":
                            if (obj.id_token_signing_alg_values_supported) { this.IdTokenSigningAlgValuesSupported = Array.from(obj.id_token_signing_alg_values_supported).map(o => o.toString()); }
                            break;
                        case "end_session_endpoint":
                            if (obj.end_session_endpoint) { this.EndSessionEndpoint = obj.end_session_endpoint.toString(); }
                            break;
                        case "response_types_supported":
                            if (obj.response_types_supported) { this.ResponseTypesSupported = Array.from(obj.response_types_supported).map(o => o.toString()); }
                            break;
                        case "claims_supported":
                            if (obj.claims_supported) { this.ClaimsSupported = Array.from(obj.claims_supported).map(o => o.toString()); }
                            break;
                        case "token_endpoint_auth_methods_supported":
                            if (obj.token_endpoint_auth_methods_supported) { this.TokenEndpointAuthMethodsSupported = Array.from(obj.token_endpoint_auth_methods_supported).map(o => o.toString()); }
                            break;
                        case "code_challenge_methods_supported":
                            if (obj.code_challenge_methods_supported) { this.CodeChallengeMethodsSupported = Array.from(obj.code_challenge_methods_supported).map(o => o.toString()); }
                            break;
                        case "request_uri_parameter_supported":
                            if (obj.request_uri_parameter_supported) { this.RequestUriParameterSupported = [true, 'true', 1].some(o => o === obj.request_uri_parameter_supported); }
                            break;
                        case "http_logout_supported":
                            if (obj.http_logout_supported) { this.HttpLogoutSupported = [true, 'true', 1].some(o => o === obj.http_logout_supported); }
                            break;
                        case "frontchannel_logout_supported":
                            if (obj.frontchannel_logout_supported) { this.FrontchannelLogoutSupported = [true, 'true', 1].some(o => o === obj.frontchannel_logout_supported); }
                            break;
                        case "rbac_url":
                            if (obj.rbac_url) { this.RbacUrl = obj.rbac_url.toString(); }
                            break;
                        case "msgraph_host":
                            if (obj.msgraph_host) { this.MsgraphHost = obj.msgraph_host.toString(); }
                            break;
                        case "cloud_graph_host_name":
                            if (obj.cloud_graph_host_name) { this.CloudGraphHostName = obj.cloud_graph_host_name.toString(); }
                            break;
                        case "cloud_instance_name":
                            if (obj.cloud_instance_name) { this.CloudInstanceName = obj.cloud_instance_name.toString(); }
                            break;
                        case "tenant_region_scope":
                            if (obj.tenant_region_scope) { this.TenantRegionScope = obj.tenant_region_scope.toString(); }
                            break;
                    }
                }
            }
            toJson(arr: string[] | null): string | null {
                let returnString = false
                if(arr == null) {
                    arr = [];
                    returnString = true;
                }
                arr.push('{');
                if(this.Issuer) { arr.push('"issuer": '); arr.push(JSON.stringify(this.Issuer)); arr.push(','); } 
                if(this.AuthorizationEndpoint) { arr.push('"authorization_endpoint": '); arr.push(JSON.stringify(this.AuthorizationEndpoint)); arr.push(','); } 
                if(this.TokenEndpoint) { arr.push('"token_endpoint": '); arr.push(JSON.stringify(this.TokenEndpoint)); arr.push(','); } 
                if(this.UserinfoEndpoint) { arr.push('"userinfo_endpoint": '); arr.push(JSON.stringify(this.UserinfoEndpoint)); arr.push(','); } 
                if(this.RevocationEndpoint) { arr.push('"revocation_endpoint": '); arr.push(JSON.stringify(this.RevocationEndpoint)); arr.push(','); } 
                if(this.DeviceAuthorizationEndpoint) { arr.push('"device_authorization_endpoint": '); arr.push(JSON.stringify(this.DeviceAuthorizationEndpoint)); arr.push(','); } 
                if(this.JwksUri) { arr.push('"jwks_uri": '); arr.push(JSON.stringify(this.JwksUri)); arr.push(','); } 
                if(this.ScopesSupported) { arr.push('"scopes_supported": '); this.ScopesSupported.forEach(o => arr.push(JSON.stringify(o))); arr.push(','); } 
                if(this.GrantTypesSupported) { arr.push('"grant_types_supported": '); this.GrantTypesSupported.forEach(o => arr.push(JSON.stringify(o))); arr.push(','); } 
                if(this.ResponseModesSupported) { arr.push('"response_modes_supported": '); this.ResponseModesSupported.forEach(o => arr.push(JSON.stringify(o))); arr.push(','); } 
                if(this.SubjectTypesSupported) { arr.push('"subject_types_supported": '); this.SubjectTypesSupported.forEach(o => arr.push(JSON.stringify(o))); arr.push(','); } 
                if(this.IdTokenSigningAlgValuesSupported) { arr.push('"id_token_signing_alg_values_supported": '); this.IdTokenSigningAlgValuesSupported.forEach(o => arr.push(JSON.stringify(o))); arr.push(','); } 
                if(this.EndSessionEndpoint) { arr.push('"end_session_endpoint": '); arr.push(JSON.stringify(this.EndSessionEndpoint)); arr.push(','); } 
                if(this.ResponseTypesSupported) { arr.push('"response_types_supported": '); this.ResponseTypesSupported.forEach(o => arr.push(JSON.stringify(o))); arr.push(','); } 
                if(this.ClaimsSupported) { arr.push('"claims_supported": '); this.ClaimsSupported.forEach(o => arr.push(JSON.stringify(o))); arr.push(','); } 
                if(this.TokenEndpointAuthMethodsSupported) { arr.push('"token_endpoint_auth_methods_supported": '); this.TokenEndpointAuthMethodsSupported.forEach(o => arr.push(JSON.stringify(o))); arr.push(','); } 
                if(this.CodeChallengeMethodsSupported) { arr.push('"code_challenge_methods_supported": '); this.CodeChallengeMethodsSupported.forEach(o => arr.push(JSON.stringify(o))); arr.push(','); } 
                if(this.RequestUriParameterSupported) { arr.push('"request_uri_parameter_supported": '); arr.push(JSON.stringify(this.RequestUriParameterSupported)); arr.push(','); } 
                if(this.HttpLogoutSupported) { arr.push('"http_logout_supported": '); arr.push(JSON.stringify(this.HttpLogoutSupported)); arr.push(','); } 
                if(this.FrontchannelLogoutSupported) { arr.push('"frontchannel_logout_supported": '); arr.push(JSON.stringify(this.FrontchannelLogoutSupported)); arr.push(','); } 
                if(this.RbacUrl) { arr.push('"rbac_url": '); arr.push(JSON.stringify(this.RbacUrl)); arr.push(','); } 
                if(this.MsgraphHost) { arr.push('"msgraph_host": '); arr.push(JSON.stringify(this.MsgraphHost)); arr.push(','); } 
                if(this.CloudGraphHostName) { arr.push('"cloud_graph_host_name": '); arr.push(JSON.stringify(this.CloudGraphHostName)); arr.push(','); } 
                if(this.CloudInstanceName) { arr.push('"cloud_instance_name": '); arr.push(JSON.stringify(this.CloudInstanceName)); arr.push(','); } 
                if(this.TenantRegionScope) { arr.push('"tenant_region_scope": '); arr.push(JSON.stringify(this.TenantRegionScope)); arr.push(','); } 
                if(arr[arr.length-1] == ',') arr[arr.length-1] = '}'; else arr.push('}');
                if(returnString) return arr.join("");
                return null;
            }
            /**
             * REQUIRED. URL using the https scheme with no query or fragment component that the OP asserts as its Issuer Identifier. If Issuer discovery is supported (see Section 2), this value MUST be identical to the issuer value returned by WebFinger. This also MUST be identical to the iss Claim value in ID Tokens issued from this Issuer.
             */
            Issuer: string | null = null;
            /**
             * REQUIRED. URL of the OP's OAuth 2.0 Authorization Endpoint [OpenID.Core].
             */
            AuthorizationEndpoint: string | null = null;
            /**
             * URL of the OP's OAuth 2.0 Token Endpoint [OpenID.Core]. This is REQUIRED unless only the Implicit Flow is used.
             */
            TokenEndpoint: string | null = null;
            /**
             * RECOMMENDED. URL of the OP's UserInfo Endpoint [OpenID.Core]. This URL MUST use the https scheme and MAY contain port, path, and query parameter components.
             */
            UserinfoEndpoint: string | null = null;
            /**
             * 
             */
            RevocationEndpoint: string | null = null;
            /**
             * OPTIONAL. URL of the authorization server's device authorization endpoint defined in Section 3.1.
             */
            DeviceAuthorizationEndpoint: string | null = null;
            /**
             * REQUIRED. URL of the OP's JSON Web Key Set [JWK] document. This contains the signing key(s) the RP uses to validate signatures from the OP. The JWK Set MAY also contain the Server's encryption key(s), which are used by RPs to encrypt requests to the Server. When both signing and encryption keys are made available, a use (Key Use) parameter value is REQUIRED for all keys in the referenced JWK Set to indicate each key's intended usage. Although some algorithms allow the same key to be used for both signatures and encryption, doing so is NOT RECOMMENDED, as it is less secure. The JWK x5c parameter MAY be used to provide X.509 representations of keys provided. When used, the bare key values MUST still be present and MUST match those in the certificate.
             */
            JwksUri: string | null = null;
            /**
             * RECOMMENDED. JSON array containing a list of the OAuth 2.0 [RFC6749] scope values that this server supports. The server MUST support the openid scope value. Servers MAY choose not to advertise some supported scope values even when this parameter is used, although those defined in [OpenID.Core] SHOULD be listed, if supported.
             */
            ScopesSupported: string[] | null = null;
            /**
             * OPTIONAL. JSON array containing a list of the OAuth 2.0 Grant Type values that this OP supports. Dynamic OpenID Providers MUST support the authorization_code and implicit Grant Type values and MAY support other Grant Types. If omitted, the default value is ["authorization_code", "implicit"]"
             */
            GrantTypesSupported: string[] | null = null;
            /**
             * 
             */
            ResponseModesSupported: string[] | null = null;
            /**
             * 
             */
            SubjectTypesSupported: string[] | null = null;
            /**
             * 
             */
            IdTokenSigningAlgValuesSupported: string[] | null = null;
            /**
             * 
             */
            EndSessionEndpoint: string | null = null;
            /**
             * REQUIRED. JSON array containing a list of the OAuth 2.0 response_type values that this OP supports. Dynamic OpenID Providers MUST support the code, id_token, and the token id_token Response Type values.
             */
            ResponseTypesSupported: string[] | null = null;
            /**
             * 
             */
            ClaimsSupported: string[] | null = null;
            /**
             * 
             */
            TokenEndpointAuthMethodsSupported: string[] | null = null;
            /**
             * 
             */
            CodeChallengeMethodsSupported: string[] | null = null;
            /**
             * 
             */
            RequestUriParameterSupported: boolean | null = null;
            /**
             * 
             */
            HttpLogoutSupported: boolean | null = null;
            /**
             * 
             */
            FrontchannelLogoutSupported: boolean | null = null;
            /**
             * 
             */
            RbacUrl: string | null = null;
            /**
             * 
             */
            MsgraphHost: string | null = null;
            /**
             * 
             */
            CloudGraphHostName: string | null = null;
            /**
             * 
             */
            CloudInstanceName: string | null = null;
            /**
             * 
             */
            TenantRegionScope: string | null = null;
        }
        /**
         * 
         */
        export class OpenIDKey {
            constructor(obj?: any) {
                for(let prop in obj) {
                    switch(prop) {
                        case "alg":
                            if (obj.alg) { this.Alg = obj.alg.toString(); }
                            break;
                        case "kty":
                            if (obj.kty) { this.Kty = obj.kty.toString(); }
                            break;
                        case "use":
                            if (obj.use) { this.Use = obj.use.toString(); }
                            break;
                        case "kid":
                            if (obj.kid) { this.Kid = obj.kid.toString(); }
                            break;
                        case "x5u":
                            if (obj.x5u) { this.X5u = obj.x5u.toString(); }
                            break;
                        case "x5t":
                            if (obj.x5t) { this.X5t = obj.x5t.toString(); }
                            break;
                        case "x5c":
                            if (obj.x5c) { this.X5c = Array.from(obj.x5c).map(o => o.toString()); }
                            break;
                        case "n":
                            if (obj.n) { this.N = obj.n.toString(); }
                            break;
                        case "e":
                            if (obj.e) { this.E = obj.e.toString(); }
                            break;
                        case "issuer":
                            if (obj.issuer) { this.Issuer = obj.issuer.toString(); }
                            break;
                    }
                }
            }
            toJson(arr: string[] | null): string | null {
                let returnString = false
                if(arr == null) {
                    arr = [];
                    returnString = true;
                }
                arr.push('{');
                if(this.Alg) { arr.push('"alg": '); arr.push(JSON.stringify(this.Alg)); arr.push(','); } 
                if(this.Kty) { arr.push('"kty": '); arr.push(JSON.stringify(this.Kty)); arr.push(','); } 
                if(this.Use) { arr.push('"use": '); arr.push(JSON.stringify(this.Use)); arr.push(','); } 
                if(this.Kid) { arr.push('"kid": '); arr.push(JSON.stringify(this.Kid)); arr.push(','); } 
                if(this.X5u) { arr.push('"x5u": '); arr.push(JSON.stringify(this.X5u)); arr.push(','); } 
                if(this.X5t) { arr.push('"x5t": '); arr.push(JSON.stringify(this.X5t)); arr.push(','); } 
                if(this.X5c) { arr.push('"x5c": '); this.X5c.forEach(o => arr.push(JSON.stringify(o))); arr.push(','); } 
                if(this.N) { arr.push('"n": '); arr.push(JSON.stringify(this.N)); arr.push(','); } 
                if(this.E) { arr.push('"e": '); arr.push(JSON.stringify(this.E)); arr.push(','); } 
                if(this.Issuer) { arr.push('"issuer": '); arr.push(JSON.stringify(this.Issuer)); arr.push(','); } 
                if(arr[arr.length-1] == ',') arr[arr.length-1] = '}'; else arr.push('}');
                if(returnString) return arr.join("");
                return null;
            }
            /**
             * (Algorithm) Parameter
             */
            Alg: string | null = null;
            /**
             * (Key Type) Parameter
             */
            Kty: string | null = null;
            /**
             * (Public Key Use) Parameter
             */
            Use: string | null = null;
            /**
             * (Key ID) Parameter
             */
            Kid: string | null = null;
            /**
             * (X.509 URL) Parameter
             */
            X5u: string | null = null;
            /**
             * (X.509 Certificate SHA-1 Thumbprint) Parameter
             */
            X5t: string | null = null;
            /**
             * (X.509 Certificate Chain) Parameter
             */
            X5c: string[] | null = null;
            /**
             * 
             */
            N: string | null = null;
            /**
             * 
             */
            E: string | null = null;
            /**
             * 
             */
            Issuer: string | null = null;
        }
        /**
         * success
         */
        export class OpenIDKeys {
            constructor(obj?: any) {
                for(let prop in obj) {
                    switch(prop) {
                        case "keys":
                            if (obj.keys) { this.Keys = Array.from(obj.keys).map(o => new Security.Types.OpenIDKey(o)); }
                            break;
                    }
                }
            }
            toJson(arr: string[] | null): string | null {
                let returnString = false
                if(arr == null) {
                    arr = [];
                    returnString = true;
                }
                arr.push('{');
                if(this.Keys) { arr.push('"keys": '); this.Keys.forEach(o => o.toJson(arr)); arr.push(','); } 
                if(arr[arr.length-1] == ',') arr[arr.length-1] = '}'; else arr.push('}');
                if(returnString) return arr.join("");
                return null;
            }
            /**
             * 
             */
            Keys: Security.Types.OpenIDKey[] | null = null;
        }
        /**
         * success
         */
        export class TokenResponse {
            constructor(obj?: any) {
                for(let prop in obj) {
                    switch(prop) {
                        case "access_token":
                            if (obj.access_token) { this.AccessToken = obj.access_token.toString(); }
                            break;
                        case "token_type":
                            if (obj.token_type) { this.TokenType = obj.token_type.toString(); }
                            break;
                        case "expires_in":
                            if (obj.expires_in) { this.ExpiresIn = obj.expires_in.toString(); }
                            break;
                        case "refresh_token":
                            if (obj.refresh_token) { this.RefreshToken = obj.refresh_token.toString(); }
                            break;
                        case "scope":
                            if (obj.scope) { this.Scope = obj.scope.toString(); }
                            break;
                    }
                }
            }
            toJson(arr: string[] | null): string | null {
                let returnString = false
                if(arr == null) {
                    arr = [];
                    returnString = true;
                }
                arr.push('{');
                if(this.AccessToken) { arr.push('"access_token": '); arr.push(JSON.stringify(this.AccessToken)); arr.push(','); } 
                if(this.TokenType) { arr.push('"token_type": '); arr.push(JSON.stringify(this.TokenType)); arr.push(','); } 
                if(this.ExpiresIn) { arr.push('"expires_in": '); arr.push(JSON.stringify(this.ExpiresIn)); arr.push(','); } 
                if(this.RefreshToken) { arr.push('"refresh_token": '); arr.push(JSON.stringify(this.RefreshToken)); arr.push(','); } 
                if(this.Scope) { arr.push('"scope": '); arr.push(JSON.stringify(this.Scope)); arr.push(','); } 
                if(arr[arr.length-1] == ',') arr[arr.length-1] = '}'; else arr.push('}');
                if(returnString) return arr.join("");
                return null;
            }
            /**
             * REQUIRED.  The access token issued by the authorization server.
             */
            AccessToken: string | null = null;
            /**
             * REQUIRED.  The type of the token issued as described in Section 7.1.  Value is case insensitive.
             */
            TokenType: string | null = null;
            /**
             * RECOMMENDED.  The lifetime in seconds of the access token.  For example, the value '3600' denotes that the access token will expire in one hour from the time the response was generated. If omitted, the authorization server SHOULD provide the expiration time via other means or document the default value.
             */
            ExpiresIn: string | null = null;
            /**
             * OPTIONAL.  The refresh token, which can be used to obtain new access tokens using the same authorization grant as describedin Section 6.
             */
            RefreshToken: string | null = null;
            /**
             * OPTIONAL, if identical to the scope requested by the client; otherwise, REQUIRED.  The scope of the access token as described by Section 3.3.
             */
            Scope: string | null = null;
        }
        /**
         * success
         */
        export class WebContent {
            constructor(obj?: any) {
                for(let prop in obj) {
                    switch(prop) {
                        case "content":
                            if (obj.content) { this.Content = new Uint8Array(obj.content); }
                            break;
                        case "contentType":
                            if (obj.contentType) { this.ContentType = obj.contentType.toString(); }
                            break;
                        case "charSet":
                            if (obj.charSet) { this.CharSet = obj.charSet.toString(); }
                            break;
                        case "location":
                            if (obj.location) { this.Location = obj.location.toString(); }
                            break;
                    }
                }
            }
            toJson(arr: string[] | null): string | null {
                let returnString = false
                if(arr == null) {
                    arr = [];
                    returnString = true;
                }
                arr.push('{');
                if(this.Content) { arr.push('"content": '); arr.push(JSON.stringify(this.Content)); arr.push(','); } 
                if(this.ContentType) { arr.push('"contentType": '); arr.push(JSON.stringify(this.ContentType)); arr.push(','); } 
                if(this.CharSet) { arr.push('"charSet": '); arr.push(JSON.stringify(this.CharSet)); arr.push(','); } 
                if(this.Location) { arr.push('"location": '); arr.push(JSON.stringify(this.Location)); arr.push(','); } 
                if(arr[arr.length-1] == ',') arr[arr.length-1] = '}'; else arr.push('}');
                if(returnString) return arr.join("");
                return null;
            }
            /**
             * The content
             */
            Content: Uint8Array | null = null;
            /**
             * The content type
             */
            ContentType: string | null = null;
            /**
             * The charset - set if content is text
             */
            CharSet: string | null = null;
            /**
             * The location of the data.
             */
            Location: string | null = null;
        }
    }
}