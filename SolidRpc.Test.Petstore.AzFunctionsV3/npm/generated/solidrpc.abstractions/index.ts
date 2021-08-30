import { default as CancellationToken } from 'cancellationtoken';
import { Observable, Subject } from 'rxjs';
import { share } from 'rxjs/operators'
import { SolidRpcJs } from 'solidrpcjs';
export namespace Abstractions {
    export namespace Services {
        export namespace Code {
            /**
             * 
             */
            export interface ICodeNamespaceGenerator {
                /**
                 * 
                 * @param assemblyName 
                 * @param cancellationToken 
                 */
                CreateCodeNamespace(
                    assemblyName : string,
                    cancellationToken? : CancellationToken
                ): Observable<Abstractions.Types.Code.CodeNamespace>;
                /**
                 * This observable is hot and monitors all the responses from the CreateCodeNamespace invocations.
                 */
                CreateCodeNamespaceObservable : Observable<Abstractions.Types.Code.CodeNamespace>;
            }
            /**
             * 
             */
            export class CodeNamespaceGeneratorImpl  extends SolidRpcJs.RpcServiceImpl implements ICodeNamespaceGenerator {
                constructor() {
                    super();
                    this.CreateCodeNamespaceSubject = new Subject<Abstractions.Types.Code.CodeNamespace>();
                    this.CreateCodeNamespaceObservable = this.CreateCodeNamespaceSubject.asObservable().pipe(share());
                }
                /**
                 * 
                 * @param assemblyName 
                 * @param cancellationToken 
                 */
                CreateCodeNamespace(
                    assemblyName : string,
                    cancellationToken? : CancellationToken
                ): Observable<Abstractions.Types.Code.CodeNamespace> {
                    let uri = 'http://localhost:7071/front/SolidRpc/Abstractions/Services/Code/ICodeNamespaceGenerator/CreateCodeNamespace/{assemblyName}';
                    uri = uri.replace('{assemblyName}', this.enocodeUriValue(assemblyName.toString()));
                    return this.request<Abstractions.Types.Code.CodeNamespace>(new SolidRpcJs.RpcServiceRequest('get', uri, null, null, null), cancellationToken, function(code : number, data : any) {
                        if(code == 200) {
                            return new Abstractions.Types.Code.CodeNamespace(data);
                        } else {
                            throw 'Response code != 200('+code+')';
                        }
                    }, this.CreateCodeNamespaceSubject);
                }
                /**
                 * This observable is hot and monitors all the responses from the CreateCodeNamespace invocations.
                 */
                CreateCodeNamespaceObservable : Observable<Abstractions.Types.Code.CodeNamespace>;
                private CreateCodeNamespaceSubject : Subject<Abstractions.Types.Code.CodeNamespace>;
            }
            /**
             * Instance for the ICodeNamespaceGenerator type. Implemented by the CodeNamespaceGeneratorImpl
             */
            export var CodeNamespaceGeneratorInstance : ICodeNamespaceGenerator = new CodeNamespaceGeneratorImpl();
            /**
             * 
             */
            export interface INpmGenerator {
                /**
                 * 
                 * @param assemblyNames 
                 * @param cancellationToken 
                 */
                CreateNpmPackage(
                    assemblyNames : string[],
                    cancellationToken? : CancellationToken
                ): Observable<Abstractions.Types.Code.NpmPackage[]>;
                /**
                 * This observable is hot and monitors all the responses from the CreateNpmPackage invocations.
                 */
                CreateNpmPackageObservable : Observable<Abstractions.Types.Code.NpmPackage[]>;
                /**
                 * 
                 * @param assemblyNames 
                 * @param cancellationToken 
                 */
                CreateNpmZip(
                    assemblyNames : string[],
                    cancellationToken? : CancellationToken
                ): Observable<Abstractions.Types.FileContent>;
                /**
                 * This observable is hot and monitors all the responses from the CreateNpmZip invocations.
                 */
                CreateNpmZipObservable : Observable<Abstractions.Types.FileContent>;
            }
            /**
             * 
             */
            export class NpmGeneratorImpl  extends SolidRpcJs.RpcServiceImpl implements INpmGenerator {
                constructor() {
                    super();
                    this.CreateNpmPackageSubject = new Subject<Abstractions.Types.Code.NpmPackage[]>();
                    this.CreateNpmPackageObservable = this.CreateNpmPackageSubject.asObservable().pipe(share());
                    this.CreateNpmZipSubject = new Subject<Abstractions.Types.FileContent>();
                    this.CreateNpmZipObservable = this.CreateNpmZipSubject.asObservable().pipe(share());
                }
                /**
                 * 
                 * @param assemblyNames 
                 * @param cancellationToken 
                 */
                CreateNpmPackage(
                    assemblyNames : string[],
                    cancellationToken? : CancellationToken
                ): Observable<Abstractions.Types.Code.NpmPackage[]> {
                    let uri = 'http://localhost:7071/front/SolidRpc/Abstractions/Services/Code/INpmGenerator/CreateNpmPackage/{assemblyNames}';
                    uri = uri.replace('{assemblyNames}', this.enocodeUriValue(assemblyNames.toString()));
                    return this.request<Abstractions.Types.Code.NpmPackage[]>(new SolidRpcJs.RpcServiceRequest('get', uri, null, null, null), cancellationToken, function(code : number, data : any) {
                        if(code == 200) {
                            return Array.from(data).map(o => new Abstractions.Types.Code.NpmPackage(o));
                        } else {
                            throw 'Response code != 200('+code+')';
                        }
                    }, this.CreateNpmPackageSubject);
                }
                /**
                 * This observable is hot and monitors all the responses from the CreateNpmPackage invocations.
                 */
                CreateNpmPackageObservable : Observable<Abstractions.Types.Code.NpmPackage[]>;
                private CreateNpmPackageSubject : Subject<Abstractions.Types.Code.NpmPackage[]>;
                /**
                 * 
                 * @param assemblyNames 
                 * @param cancellationToken 
                 */
                CreateNpmZip(
                    assemblyNames : string[],
                    cancellationToken? : CancellationToken
                ): Observable<Abstractions.Types.FileContent> {
                    let uri = 'http://localhost:7071/front/SolidRpc/Abstractions/Services/Code/INpmGenerator/CreateNpmZip/{assemblyNames}';
                    uri = uri.replace('{assemblyNames}', this.enocodeUriValue(assemblyNames.toString()));
                    return this.request<Abstractions.Types.FileContent>(new SolidRpcJs.RpcServiceRequest('get', uri, null, null, null), cancellationToken, function(code : number, data : any) {
                        if(code == 200) {
                            return new Abstractions.Types.FileContent(data);
                        } else {
                            throw 'Response code != 200('+code+')';
                        }
                    }, this.CreateNpmZipSubject);
                }
                /**
                 * This observable is hot and monitors all the responses from the CreateNpmZip invocations.
                 */
                CreateNpmZipObservable : Observable<Abstractions.Types.FileContent>;
                private CreateNpmZipSubject : Subject<Abstractions.Types.FileContent>;
            }
            /**
             * Instance for the INpmGenerator type. Implemented by the NpmGeneratorImpl
             */
            export var NpmGeneratorInstance : INpmGenerator = new NpmGeneratorImpl();
            /**
             * 
             */
            export interface ITypescriptGenerator {
                /**
                 * 
                 * @param assemblyName 
                 * @param cancellationToken 
                 */
                CreateTypesTsForAssemblyAsync(
                    assemblyName : string,
                    cancellationToken? : CancellationToken
                ): Observable<string>;
                /**
                 * This observable is hot and monitors all the responses from the CreateTypesTsForAssemblyAsync invocations.
                 */
                CreateTypesTsForAssemblyAsyncObservable : Observable<string>;
                /**
                 * 
                 * @param codeNamespace 
                 * @param cancellationToken 
                 */
                CreateTypesTsForCodeNamespaceAsync(
                    codeNamespace : Abstractions.Types.Code.CodeNamespace,
                    cancellationToken? : CancellationToken
                ): Observable<string>;
                /**
                 * This observable is hot and monitors all the responses from the CreateTypesTsForCodeNamespaceAsync invocations.
                 */
                CreateTypesTsForCodeNamespaceAsyncObservable : Observable<string>;
            }
            /**
             * 
             */
            export class TypescriptGeneratorImpl  extends SolidRpcJs.RpcServiceImpl implements ITypescriptGenerator {
                constructor() {
                    super();
                    this.CreateTypesTsForAssemblyAsyncSubject = new Subject<string>();
                    this.CreateTypesTsForAssemblyAsyncObservable = this.CreateTypesTsForAssemblyAsyncSubject.asObservable().pipe(share());
                    this.CreateTypesTsForCodeNamespaceAsyncSubject = new Subject<string>();
                    this.CreateTypesTsForCodeNamespaceAsyncObservable = this.CreateTypesTsForCodeNamespaceAsyncSubject.asObservable().pipe(share());
                }
                /**
                 * 
                 * @param assemblyName 
                 * @param cancellationToken 
                 */
                CreateTypesTsForAssemblyAsync(
                    assemblyName : string,
                    cancellationToken? : CancellationToken
                ): Observable<string> {
                    let uri = 'http://localhost:7071/front/SolidRpc/Abstractions/Services/Code/ITypescriptGenerator/CreateTypesTsForAssemblyAsync/{assemblyName}';
                    uri = uri.replace('{assemblyName}', this.enocodeUriValue(assemblyName.toString()));
                    return this.request<string>(new SolidRpcJs.RpcServiceRequest('get', uri, null, null, null), cancellationToken, function(code : number, data : any) {
                        if(code == 200) {
                            return data as string;
                        } else {
                            throw 'Response code != 200('+code+')';
                        }
                    }, this.CreateTypesTsForAssemblyAsyncSubject);
                }
                /**
                 * This observable is hot and monitors all the responses from the CreateTypesTsForAssemblyAsync invocations.
                 */
                CreateTypesTsForAssemblyAsyncObservable : Observable<string>;
                private CreateTypesTsForAssemblyAsyncSubject : Subject<string>;
                /**
                 * 
                 * @param codeNamespace 
                 * @param cancellationToken 
                 */
                CreateTypesTsForCodeNamespaceAsync(
                    codeNamespace : Abstractions.Types.Code.CodeNamespace,
                    cancellationToken? : CancellationToken
                ): Observable<string> {
                    let uri = 'http://localhost:7071/front/SolidRpc/Abstractions/Services/Code/ITypescriptGenerator/CreateTypesTsForCodeNamespaceAsync';
                    return this.request<string>(new SolidRpcJs.RpcServiceRequest('post', uri, null, {'Content-Type': 'application/json'}, this.toJson(codeNamespace)), cancellationToken, function(code : number, data : any) {
                        if(code == 200) {
                            return data as string;
                        } else {
                            throw 'Response code != 200('+code+')';
                        }
                    }, this.CreateTypesTsForCodeNamespaceAsyncSubject);
                }
                /**
                 * This observable is hot and monitors all the responses from the CreateTypesTsForCodeNamespaceAsync invocations.
                 */
                CreateTypesTsForCodeNamespaceAsyncObservable : Observable<string>;
                private CreateTypesTsForCodeNamespaceAsyncSubject : Subject<string>;
            }
            /**
             * Instance for the ITypescriptGenerator type. Implemented by the TypescriptGeneratorImpl
             */
            export var TypescriptGeneratorInstance : ITypescriptGenerator = new TypescriptGeneratorImpl();
        }
        /**
         * 
         */
        export interface ISolidRpcContentHandler {
            /**
             * 
             * @param path 
             * @param cancellationToken 
             */
            GetContent(
                path? : string,
                cancellationToken? : CancellationToken
            ): Observable<Abstractions.Types.FileContent>;
            /**
             * This observable is hot and monitors all the responses from the GetContent invocations.
             */
            GetContentObservable : Observable<Abstractions.Types.FileContent>;
        }
        /**
         * 
         */
        export class SolidRpcContentHandlerImpl  extends SolidRpcJs.RpcServiceImpl implements ISolidRpcContentHandler {
            constructor() {
                super();
                this.GetContentSubject = new Subject<Abstractions.Types.FileContent>();
                this.GetContentObservable = this.GetContentSubject.asObservable().pipe(share());
            }
            /**
             * 
             * @param path 
             * @param cancellationToken 
             */
            GetContent(
                path? : string,
                cancellationToken? : CancellationToken
            ): Observable<Abstractions.Types.FileContent> {
                let uri = 'http://localhost:7071/front/SolidRpc/Abstractions/Services/ISolidRpcContentHandler/GetContent';
                return this.request<Abstractions.Types.FileContent>(new SolidRpcJs.RpcServiceRequest('get', uri, {
                    'path': path,
}, null, null), cancellationToken, function(code : number, data : any) {
                    if(code == 200) {
                        return new Abstractions.Types.FileContent(data);
                    } else {
                        throw 'Response code != 200('+code+')';
                    }
                }, this.GetContentSubject);
            }
            /**
             * This observable is hot and monitors all the responses from the GetContent invocations.
             */
            GetContentObservable : Observable<Abstractions.Types.FileContent>;
            private GetContentSubject : Subject<Abstractions.Types.FileContent>;
        }
        /**
         * Instance for the ISolidRpcContentHandler type. Implemented by the SolidRpcContentHandlerImpl
         */
        export var SolidRpcContentHandlerInstance : ISolidRpcContentHandler = new SolidRpcContentHandlerImpl();
        /**
         * 
         */
        export interface ISolidRpcHost {
            /**
             * 
             * @param cancellationToken 
             */
            GetHostId(
                cancellationToken? : CancellationToken
            ): Observable<string>;
            /**
             * This observable is hot and monitors all the responses from the GetHostId invocations.
             */
            GetHostIdObservable : Observable<string>;
            /**
             * 
             * @param cancellationToken 
             */
            GetHostInstance(
                cancellationToken? : CancellationToken
            ): Observable<Abstractions.Types.SolidRpcHostInstance>;
            /**
             * This observable is hot and monitors all the responses from the GetHostInstance invocations.
             */
            GetHostInstanceObservable : Observable<Abstractions.Types.SolidRpcHostInstance>;
            /**
             * 
             * @param cancellationToken 
             */
            SyncHostsFromStore(
                cancellationToken? : CancellationToken
            ): Observable<Abstractions.Types.SolidRpcHostInstance[]>;
            /**
             * This observable is hot and monitors all the responses from the SyncHostsFromStore invocations.
             */
            SyncHostsFromStoreObservable : Observable<Abstractions.Types.SolidRpcHostInstance[]>;
            /**
             * 
             * @param hostInstance 
             * @param cancellationToken 
             */
            CheckHost(
                hostInstance : Abstractions.Types.SolidRpcHostInstance,
                cancellationToken? : CancellationToken
            ): Observable<Abstractions.Types.SolidRpcHostInstance>;
            /**
             * This observable is hot and monitors all the responses from the CheckHost invocations.
             */
            CheckHostObservable : Observable<Abstractions.Types.SolidRpcHostInstance>;
            /**
             * 
             * @param cancellationToken 
             */
            GetHostConfiguration(
                cancellationToken? : CancellationToken
            ): Observable<Abstractions.Types.NameValuePair[]>;
            /**
             * This observable is hot and monitors all the responses from the GetHostConfiguration invocations.
             */
            GetHostConfigurationObservable : Observable<Abstractions.Types.NameValuePair[]>;
            /**
             * 
             * @param cancellationToken 
             */
            IsAlive(
                cancellationToken? : CancellationToken
            ): Observable<void>;
            /**
             * This observable is hot and monitors all the responses from the IsAlive invocations.
             */
            IsAliveObservable : Observable<void>;
        }
        /**
         * 
         */
        export class SolidRpcHostImpl  extends SolidRpcJs.RpcServiceImpl implements ISolidRpcHost {
            constructor() {
                super();
                this.GetHostIdSubject = new Subject<string>();
                this.GetHostIdObservable = this.GetHostIdSubject.asObservable().pipe(share());
                this.GetHostInstanceSubject = new Subject<Abstractions.Types.SolidRpcHostInstance>();
                this.GetHostInstanceObservable = this.GetHostInstanceSubject.asObservable().pipe(share());
                this.SyncHostsFromStoreSubject = new Subject<Abstractions.Types.SolidRpcHostInstance[]>();
                this.SyncHostsFromStoreObservable = this.SyncHostsFromStoreSubject.asObservable().pipe(share());
                this.CheckHostSubject = new Subject<Abstractions.Types.SolidRpcHostInstance>();
                this.CheckHostObservable = this.CheckHostSubject.asObservable().pipe(share());
                this.GetHostConfigurationSubject = new Subject<Abstractions.Types.NameValuePair[]>();
                this.GetHostConfigurationObservable = this.GetHostConfigurationSubject.asObservable().pipe(share());
                this.IsAliveSubject = new Subject<void>();
                this.IsAliveObservable = this.IsAliveSubject.asObservable().pipe(share());
            }
            /**
             * 
             * @param cancellationToken 
             */
            GetHostId(
                cancellationToken? : CancellationToken
            ): Observable<string> {
                let uri = 'http://localhost:7071/front/SolidRpc/Abstractions/Services/ISolidRpcHost/GetHostId';
                return this.request<string>(new SolidRpcJs.RpcServiceRequest('get', uri, null, null, null), cancellationToken, function(code : number, data : any) {
                    if(code == 200) {
                        return data as string;
                    } else {
                        throw 'Response code != 200('+code+')';
                    }
                }, this.GetHostIdSubject);
            }
            /**
             * This observable is hot and monitors all the responses from the GetHostId invocations.
             */
            GetHostIdObservable : Observable<string>;
            private GetHostIdSubject : Subject<string>;
            /**
             * 
             * @param cancellationToken 
             */
            GetHostInstance(
                cancellationToken? : CancellationToken
            ): Observable<Abstractions.Types.SolidRpcHostInstance> {
                let uri = 'http://localhost:7071/front/SolidRpc/Abstractions/Services/ISolidRpcHost/GetHostInstance';
                return this.request<Abstractions.Types.SolidRpcHostInstance>(new SolidRpcJs.RpcServiceRequest('get', uri, null, null, null), cancellationToken, function(code : number, data : any) {
                    if(code == 200) {
                        return new Abstractions.Types.SolidRpcHostInstance(data);
                    } else {
                        throw 'Response code != 200('+code+')';
                    }
                }, this.GetHostInstanceSubject);
            }
            /**
             * This observable is hot and monitors all the responses from the GetHostInstance invocations.
             */
            GetHostInstanceObservable : Observable<Abstractions.Types.SolidRpcHostInstance>;
            private GetHostInstanceSubject : Subject<Abstractions.Types.SolidRpcHostInstance>;
            /**
             * 
             * @param cancellationToken 
             */
            SyncHostsFromStore(
                cancellationToken? : CancellationToken
            ): Observable<Abstractions.Types.SolidRpcHostInstance[]> {
                let uri = 'http://localhost:7071/front/SolidRpc/Abstractions/Services/ISolidRpcHost/SyncHostsFromStore';
                return this.request<Abstractions.Types.SolidRpcHostInstance[]>(new SolidRpcJs.RpcServiceRequest('get', uri, null, null, null), cancellationToken, function(code : number, data : any) {
                    if(code == 200) {
                        return Array.from(data).map(o => new Abstractions.Types.SolidRpcHostInstance(o));
                    } else {
                        throw 'Response code != 200('+code+')';
                    }
                }, this.SyncHostsFromStoreSubject);
            }
            /**
             * This observable is hot and monitors all the responses from the SyncHostsFromStore invocations.
             */
            SyncHostsFromStoreObservable : Observable<Abstractions.Types.SolidRpcHostInstance[]>;
            private SyncHostsFromStoreSubject : Subject<Abstractions.Types.SolidRpcHostInstance[]>;
            /**
             * 
             * @param hostInstance 
             * @param cancellationToken 
             */
            CheckHost(
                hostInstance : Abstractions.Types.SolidRpcHostInstance,
                cancellationToken? : CancellationToken
            ): Observable<Abstractions.Types.SolidRpcHostInstance> {
                let uri = 'http://localhost:7071/front/SolidRpc/Abstractions/Services/ISolidRpcHost/CheckHost';
                return this.request<Abstractions.Types.SolidRpcHostInstance>(new SolidRpcJs.RpcServiceRequest('post', uri, null, {'Content-Type': 'application/json'}, this.toJson(hostInstance)), cancellationToken, function(code : number, data : any) {
                    if(code == 200) {
                        return new Abstractions.Types.SolidRpcHostInstance(data);
                    } else {
                        throw 'Response code != 200('+code+')';
                    }
                }, this.CheckHostSubject);
            }
            /**
             * This observable is hot and monitors all the responses from the CheckHost invocations.
             */
            CheckHostObservable : Observable<Abstractions.Types.SolidRpcHostInstance>;
            private CheckHostSubject : Subject<Abstractions.Types.SolidRpcHostInstance>;
            /**
             * 
             * @param cancellationToken 
             */
            GetHostConfiguration(
                cancellationToken? : CancellationToken
            ): Observable<Abstractions.Types.NameValuePair[]> {
                let uri = 'http://localhost:7071/front/SolidRpc/Abstractions/Services/ISolidRpcHost/GetHostConfiguration';
                return this.request<Abstractions.Types.NameValuePair[]>(new SolidRpcJs.RpcServiceRequest('get', uri, null, null, null), cancellationToken, function(code : number, data : any) {
                    if(code == 200) {
                        return Array.from(data).map(o => new Abstractions.Types.NameValuePair(o));
                    } else {
                        throw 'Response code != 200('+code+')';
                    }
                }, this.GetHostConfigurationSubject);
            }
            /**
             * This observable is hot and monitors all the responses from the GetHostConfiguration invocations.
             */
            GetHostConfigurationObservable : Observable<Abstractions.Types.NameValuePair[]>;
            private GetHostConfigurationSubject : Subject<Abstractions.Types.NameValuePair[]>;
            /**
             * 
             * @param cancellationToken 
             */
            IsAlive(
                cancellationToken? : CancellationToken
            ): Observable<void> {
                let uri = 'http://localhost:7071/front/SolidRpc/Abstractions/Services/ISolidRpcHost/IsAlive';
                return this.request<void>(new SolidRpcJs.RpcServiceRequest('get', uri, null, null, null), cancellationToken, function(code : number, data : any) {
                    if(code == 200) {
                        return null;
                    } else {
                        throw 'Response code != 200('+code+')';
                    }
                }, this.IsAliveSubject);
            }
            /**
             * This observable is hot and monitors all the responses from the IsAlive invocations.
             */
            IsAliveObservable : Observable<void>;
            private IsAliveSubject : Subject<void>;
        }
        /**
         * Instance for the ISolidRpcHost type. Implemented by the SolidRpcHostImpl
         */
        export var SolidRpcHostInstance : ISolidRpcHost = new SolidRpcHostImpl();
        /**
         * 
         */
        export interface ISolidRpcOAuth2 {
            /**
             * 
             * @param callbackUri 
             * @param state 
             * @param scopes 
             * @param cancellationToken 
             */
            GetAuthorizationCodeTokenAsync(
                callbackUri? : string,
                state? : string,
                scopes? : string[],
                cancellationToken? : CancellationToken
            ): Observable<Abstractions.Types.FileContent>;
            /**
             * This observable is hot and monitors all the responses from the GetAuthorizationCodeTokenAsync invocations.
             */
            GetAuthorizationCodeTokenAsyncObservable : Observable<Abstractions.Types.FileContent>;
            /**
             * 
             * @param code 
             * @param state 
             * @param cancellation 
             */
            TokenCallbackAsync(
                code? : string,
                state? : string,
                cancellation? : CancellationToken
            ): Observable<Abstractions.Types.FileContent>;
            /**
             * This observable is hot and monitors all the responses from the TokenCallbackAsync invocations.
             */
            TokenCallbackAsyncObservable : Observable<Abstractions.Types.FileContent>;
        }
        /**
         * 
         */
        export class SolidRpcOAuth2Impl  extends SolidRpcJs.RpcServiceImpl implements ISolidRpcOAuth2 {
            constructor() {
                super();
                this.GetAuthorizationCodeTokenAsyncSubject = new Subject<Abstractions.Types.FileContent>();
                this.GetAuthorizationCodeTokenAsyncObservable = this.GetAuthorizationCodeTokenAsyncSubject.asObservable().pipe(share());
                this.TokenCallbackAsyncSubject = new Subject<Abstractions.Types.FileContent>();
                this.TokenCallbackAsyncObservable = this.TokenCallbackAsyncSubject.asObservable().pipe(share());
            }
            /**
             * 
             * @param callbackUri 
             * @param state 
             * @param scopes 
             * @param cancellationToken 
             */
            GetAuthorizationCodeTokenAsync(
                callbackUri? : string,
                state? : string,
                scopes? : string[],
                cancellationToken? : CancellationToken
            ): Observable<Abstractions.Types.FileContent> {
                let uri = 'http://localhost:7071/front/SolidRpc/Abstractions/Services/ISolidRpcOAuth2/GetAuthorizationCodeTokenAsync';
                return this.request<Abstractions.Types.FileContent>(new SolidRpcJs.RpcServiceRequest('get', uri, {
                    'callbackUri': callbackUri,
                    'state': state,
                    'scopes': scopes,
}, null, null), cancellationToken, function(code : number, data : any) {
                    if(code == 200) {
                        return new Abstractions.Types.FileContent(data);
                    } else {
                        throw 'Response code != 200('+code+')';
                    }
                }, this.GetAuthorizationCodeTokenAsyncSubject);
            }
            /**
             * This observable is hot and monitors all the responses from the GetAuthorizationCodeTokenAsync invocations.
             */
            GetAuthorizationCodeTokenAsyncObservable : Observable<Abstractions.Types.FileContent>;
            private GetAuthorizationCodeTokenAsyncSubject : Subject<Abstractions.Types.FileContent>;
            /**
             * 
             * @param code 
             * @param state 
             * @param cancellation 
             */
            TokenCallbackAsync(
                code? : string,
                state? : string,
                cancellation? : CancellationToken
            ): Observable<Abstractions.Types.FileContent> {
                let uri = 'http://localhost:7071/front/SolidRpc/Abstractions/Services/ISolidRpcOAuth2/TokenCallbackAsync';
                return this.request<Abstractions.Types.FileContent>(new SolidRpcJs.RpcServiceRequest('get', uri, {
                    'code': code,
                    'state': state,
}, null, null), cancellation, function(code : number, data : any) {
                    if(code == 200) {
                        return new Abstractions.Types.FileContent(data);
                    } else {
                        throw 'Response code != 200('+code+')';
                    }
                }, this.TokenCallbackAsyncSubject);
            }
            /**
             * This observable is hot and monitors all the responses from the TokenCallbackAsync invocations.
             */
            TokenCallbackAsyncObservable : Observable<Abstractions.Types.FileContent>;
            private TokenCallbackAsyncSubject : Subject<Abstractions.Types.FileContent>;
        }
        /**
         * Instance for the ISolidRpcOAuth2 type. Implemented by the SolidRpcOAuth2Impl
         */
        export var SolidRpcOAuth2Instance : ISolidRpcOAuth2 = new SolidRpcOAuth2Impl();
    }
    export namespace Types {
        export namespace Code {
            /**
             * 
             */
            export class CodeInterface {
                constructor(obj?: any) {
                    for(let prop in obj) {
                        switch(prop) {
                            case "description":
                                if (obj.description) { this.Description = obj.description as string; }
                                break;
                            case "name":
                                if (obj.name) { this.Name = obj.name as string; }
                                break;
                            case "methods":
                                if (obj.methods) { this.Methods = Array.from(obj.methods).map(o => new Abstractions.Types.Code.CodeMethod(o)); }
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
                    if(this.Description) { arr.push('"description": '); arr.push(JSON.stringify(this.Description)); arr.push(','); } 
                    if(this.Name) { arr.push('"name": '); arr.push(JSON.stringify(this.Name)); arr.push(','); } 
                    if(this.Methods) { arr.push('"methods": '); for (let i = 0; i < this.Methods.length; i++) this.Methods[i].toJson(arr); arr.push(',');; arr.push(','); } 
                    if(arr[arr.length-1] == ',') arr[arr.length-1] = '}'; else arr.push('}');
                    if(returnString) return arr.join("");
                    return null;
                }
                /**
                 * 
                 */
                Description: string | null = null;
                /**
                 * 
                 */
                Name: string | null = null;
                /**
                 * 
                 */
                Methods: Abstractions.Types.Code.CodeMethod[] | null = null;
            }
            /**
             * 
             */
            export class CodeMethod {
                constructor(obj?: any) {
                    for(let prop in obj) {
                        switch(prop) {
                            case "description":
                                if (obj.description) { this.Description = obj.description as string; }
                                break;
                            case "name":
                                if (obj.name) { this.Name = obj.name as string; }
                                break;
                            case "arguments":
                                if (obj.arguments) { this.Arguments = Array.from(obj.arguments).map(o => new Abstractions.Types.Code.CodeMethodArg(o)); }
                                break;
                            case "returnType":
                                if (obj.returnType) { this.ReturnType = Array.from(obj.returnType).map(o => o as string); }
                                break;
                            case "httpMethod":
                                if (obj.httpMethod) { this.HttpMethod = obj.httpMethod as string; }
                                break;
                            case "httpBaseAddress":
                                if (obj.httpBaseAddress) { this.HttpBaseAddress = obj.httpBaseAddress as string; }
                                break;
                            case "httpPath":
                                if (obj.httpPath) { this.HttpPath = obj.httpPath as string; }
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
                    if(this.Description) { arr.push('"description": '); arr.push(JSON.stringify(this.Description)); arr.push(','); } 
                    if(this.Name) { arr.push('"name": '); arr.push(JSON.stringify(this.Name)); arr.push(','); } 
                    if(this.Arguments) { arr.push('"arguments": '); for (let i = 0; i < this.Arguments.length; i++) this.Arguments[i].toJson(arr); arr.push(',');; arr.push(','); } 
                    if(this.ReturnType) { arr.push('"returnType": '); for (let i = 0; i < this.ReturnType.length; i++) arr.push(JSON.stringify(this.ReturnType[i])); arr.push(',');; arr.push(','); } 
                    if(this.HttpMethod) { arr.push('"httpMethod": '); arr.push(JSON.stringify(this.HttpMethod)); arr.push(','); } 
                    if(this.HttpBaseAddress) { arr.push('"httpBaseAddress": '); arr.push(JSON.stringify(this.HttpBaseAddress)); arr.push(','); } 
                    if(this.HttpPath) { arr.push('"httpPath": '); arr.push(JSON.stringify(this.HttpPath)); arr.push(','); } 
                    if(arr[arr.length-1] == ',') arr[arr.length-1] = '}'; else arr.push('}');
                    if(returnString) return arr.join("");
                    return null;
                }
                /**
                 * 
                 */
                Description: string | null = null;
                /**
                 * 
                 */
                Name: string | null = null;
                /**
                 * 
                 */
                Arguments: Abstractions.Types.Code.CodeMethodArg[] | null = null;
                /**
                 * 
                 */
                ReturnType: string[] | null = null;
                /**
                 * 
                 */
                HttpMethod: string | null = null;
                /**
                 * 
                 */
                HttpBaseAddress: string | null = null;
                /**
                 * 
                 */
                HttpPath: string | null = null;
            }
            /**
             * 
             */
            export class CodeMethodArg {
                constructor(obj?: any) {
                    for(let prop in obj) {
                        switch(prop) {
                            case "description":
                                if (obj.description) { this.Description = obj.description as string; }
                                break;
                            case "name":
                                if (obj.name) { this.Name = obj.name as string; }
                                break;
                            case "argType":
                                if (obj.argType) { this.ArgType = Array.from(obj.argType).map(o => o as string); }
                                break;
                            case "optional":
                                if (obj.optional) { this.Optional = [true, 'true', 1].some(o => o === obj.optional); }
                                break;
                            case "httpName":
                                if (obj.httpName) { this.HttpName = obj.httpName as string; }
                                break;
                            case "httpLocation":
                                if (obj.httpLocation) { this.HttpLocation = obj.httpLocation as string; }
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
                    if(this.Description) { arr.push('"description": '); arr.push(JSON.stringify(this.Description)); arr.push(','); } 
                    if(this.Name) { arr.push('"name": '); arr.push(JSON.stringify(this.Name)); arr.push(','); } 
                    if(this.ArgType) { arr.push('"argType": '); for (let i = 0; i < this.ArgType.length; i++) arr.push(JSON.stringify(this.ArgType[i])); arr.push(',');; arr.push(','); } 
                    if(this.Optional) { arr.push('"optional": '); arr.push(JSON.stringify(this.Optional)); arr.push(','); } 
                    if(this.HttpName) { arr.push('"httpName": '); arr.push(JSON.stringify(this.HttpName)); arr.push(','); } 
                    if(this.HttpLocation) { arr.push('"httpLocation": '); arr.push(JSON.stringify(this.HttpLocation)); arr.push(','); } 
                    if(arr[arr.length-1] == ',') arr[arr.length-1] = '}'; else arr.push('}');
                    if(returnString) return arr.join("");
                    return null;
                }
                /**
                 * 
                 */
                Description: string | null = null;
                /**
                 * 
                 */
                Name: string | null = null;
                /**
                 * 
                 */
                ArgType: string[] | null = null;
                /**
                 * 
                 */
                Optional: boolean | null = null;
                /**
                 * 
                 */
                HttpName: string | null = null;
                /**
                 * 
                 */
                HttpLocation: string | null = null;
            }
            /**
             * 
             */
            export class CodeNamespace {
                constructor(obj?: any) {
                    for(let prop in obj) {
                        switch(prop) {
                            case "name":
                                if (obj.name) { this.Name = obj.name as string; }
                                break;
                            case "namespaces":
                                if (obj.namespaces) { this.Namespaces = Array.from(obj.namespaces).map(o => new Abstractions.Types.Code.CodeNamespace(o)); }
                                break;
                            case "interfaces":
                                if (obj.interfaces) { this.Interfaces = Array.from(obj.interfaces).map(o => new Abstractions.Types.Code.CodeInterface(o)); }
                                break;
                            case "types":
                                if (obj.types) { this.Types = Array.from(obj.types).map(o => new Abstractions.Types.Code.CodeType(o)); }
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
                    if(this.Namespaces) { arr.push('"namespaces": '); for (let i = 0; i < this.Namespaces.length; i++) this.Namespaces[i].toJson(arr); arr.push(',');; arr.push(','); } 
                    if(this.Interfaces) { arr.push('"interfaces": '); for (let i = 0; i < this.Interfaces.length; i++) this.Interfaces[i].toJson(arr); arr.push(',');; arr.push(','); } 
                    if(this.Types) { arr.push('"types": '); for (let i = 0; i < this.Types.length; i++) this.Types[i].toJson(arr); arr.push(',');; arr.push(','); } 
                    if(arr[arr.length-1] == ',') arr[arr.length-1] = '}'; else arr.push('}');
                    if(returnString) return arr.join("");
                    return null;
                }
                /**
                 * 
                 */
                Name: string | null = null;
                /**
                 * 
                 */
                Namespaces: Abstractions.Types.Code.CodeNamespace[] | null = null;
                /**
                 * 
                 */
                Interfaces: Abstractions.Types.Code.CodeInterface[] | null = null;
                /**
                 * 
                 */
                Types: Abstractions.Types.Code.CodeType[] | null = null;
            }
            /**
             * 
             */
            export class CodeType {
                constructor(obj?: any) {
                    for(let prop in obj) {
                        switch(prop) {
                            case "description":
                                if (obj.description) { this.Description = obj.description as string; }
                                break;
                            case "name":
                                if (obj.name) { this.Name = obj.name as string; }
                                break;
                            case "extends":
                                if (obj.extends) { this.Extends = Array.from(obj.extends).map(o => o as string); }
                                break;
                            case "properties":
                                if (obj.properties) { this.Properties = Array.from(obj.properties).map(o => new Abstractions.Types.Code.CodeTypeProperty(o)); }
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
                    if(this.Description) { arr.push('"description": '); arr.push(JSON.stringify(this.Description)); arr.push(','); } 
                    if(this.Name) { arr.push('"name": '); arr.push(JSON.stringify(this.Name)); arr.push(','); } 
                    if(this.Extends) { arr.push('"extends": '); for (let i = 0; i < this.Extends.length; i++) arr.push(JSON.stringify(this.Extends[i])); arr.push(',');; arr.push(','); } 
                    if(this.Properties) { arr.push('"properties": '); for (let i = 0; i < this.Properties.length; i++) this.Properties[i].toJson(arr); arr.push(',');; arr.push(','); } 
                    if(arr[arr.length-1] == ',') arr[arr.length-1] = '}'; else arr.push('}');
                    if(returnString) return arr.join("");
                    return null;
                }
                /**
                 * 
                 */
                Description: string | null = null;
                /**
                 * 
                 */
                Name: string | null = null;
                /**
                 * 
                 */
                Extends: string[] | null = null;
                /**
                 * 
                 */
                Properties: Abstractions.Types.Code.CodeTypeProperty[] | null = null;
            }
            /**
             * 
             */
            export class CodeTypeProperty {
                constructor(obj?: any) {
                    for(let prop in obj) {
                        switch(prop) {
                            case "description":
                                if (obj.description) { this.Description = obj.description as string; }
                                break;
                            case "name":
                                if (obj.name) { this.Name = obj.name as string; }
                                break;
                            case "propertyType":
                                if (obj.propertyType) { this.PropertyType = Array.from(obj.propertyType).map(o => o as string); }
                                break;
                            case "httpName":
                                if (obj.httpName) { this.HttpName = obj.httpName as string; }
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
                    if(this.Description) { arr.push('"description": '); arr.push(JSON.stringify(this.Description)); arr.push(','); } 
                    if(this.Name) { arr.push('"name": '); arr.push(JSON.stringify(this.Name)); arr.push(','); } 
                    if(this.PropertyType) { arr.push('"propertyType": '); for (let i = 0; i < this.PropertyType.length; i++) arr.push(JSON.stringify(this.PropertyType[i])); arr.push(',');; arr.push(','); } 
                    if(this.HttpName) { arr.push('"httpName": '); arr.push(JSON.stringify(this.HttpName)); arr.push(','); } 
                    if(arr[arr.length-1] == ',') arr[arr.length-1] = '}'; else arr.push('}');
                    if(returnString) return arr.join("");
                    return null;
                }
                /**
                 * 
                 */
                Description: string | null = null;
                /**
                 * 
                 */
                Name: string | null = null;
                /**
                 * 
                 */
                PropertyType: string[] | null = null;
                /**
                 * 
                 */
                HttpName: string | null = null;
            }
            /**
             * 
             */
            export class NpmPackage {
                constructor(obj?: any) {
                    for(let prop in obj) {
                        switch(prop) {
                            case "name":
                                if (obj.name) { this.Name = obj.name as string; }
                                break;
                            case "files":
                                if (obj.files) { this.Files = Array.from(obj.files).map(o => new Abstractions.Types.Code.NpmPackageFile(o)); }
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
                    if(this.Files) { arr.push('"files": '); for (let i = 0; i < this.Files.length; i++) this.Files[i].toJson(arr); arr.push(',');; arr.push(','); } 
                    if(arr[arr.length-1] == ',') arr[arr.length-1] = '}'; else arr.push('}');
                    if(returnString) return arr.join("");
                    return null;
                }
                /**
                 * 
                 */
                Name: string | null = null;
                /**
                 * 
                 */
                Files: Abstractions.Types.Code.NpmPackageFile[] | null = null;
            }
            /**
             * 
             */
            export class NpmPackageFile {
                constructor(obj?: any) {
                    for(let prop in obj) {
                        switch(prop) {
                            case "filePath":
                                if (obj.filePath) { this.FilePath = obj.filePath as string; }
                                break;
                            case "content":
                                if (obj.content) { this.Content = obj.content as string; }
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
                    if(this.FilePath) { arr.push('"filePath": '); arr.push(JSON.stringify(this.FilePath)); arr.push(','); } 
                    if(this.Content) { arr.push('"content": '); arr.push(JSON.stringify(this.Content)); arr.push(','); } 
                    if(arr[arr.length-1] == ',') arr[arr.length-1] = '}'; else arr.push('}');
                    if(returnString) return arr.join("");
                    return null;
                }
                /**
                 * 
                 */
                FilePath: string | null = null;
                /**
                 * 
                 */
                Content: string | null = null;
            }
        }
        /**
         * 
         */
        export class FileContent {
            constructor(obj?: any) {
                for(let prop in obj) {
                    switch(prop) {
                        case "Content":
                            if (obj.Content) { this.Content = new Uint8Array(obj.Content); }
                            break;
                        case "CharSet":
                            if (obj.CharSet) { this.CharSet = obj.CharSet as string; }
                            break;
                        case "ContentType":
                            if (obj.ContentType) { this.ContentType = obj.ContentType as string; }
                            break;
                        case "FileName":
                            if (obj.FileName) { this.FileName = obj.FileName as string; }
                            break;
                        case "LastModified":
                            if (obj.LastModified) { this.LastModified = new Date(obj.LastModified); }
                            break;
                        case "Location":
                            if (obj.Location) { this.Location = obj.Location as string; }
                            break;
                        case "ETag":
                            if (obj.ETag) { this.ETag = obj.ETag as string; }
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
                if(this.Content) { arr.push('"Content": '); arr.push(JSON.stringify(this.Content)); arr.push(','); } 
                if(this.CharSet) { arr.push('"CharSet": '); arr.push(JSON.stringify(this.CharSet)); arr.push(','); } 
                if(this.ContentType) { arr.push('"ContentType": '); arr.push(JSON.stringify(this.ContentType)); arr.push(','); } 
                if(this.FileName) { arr.push('"FileName": '); arr.push(JSON.stringify(this.FileName)); arr.push(','); } 
                if(this.LastModified) { arr.push('"LastModified": '); arr.push(JSON.stringify(this.LastModified)); arr.push(','); } 
                if(this.Location) { arr.push('"Location": '); arr.push(JSON.stringify(this.Location)); arr.push(','); } 
                if(this.ETag) { arr.push('"ETag": '); arr.push(JSON.stringify(this.ETag)); arr.push(','); } 
                if(arr[arr.length-1] == ',') arr[arr.length-1] = '}'; else arr.push('}');
                if(returnString) return arr.join("");
                return null;
            }
            /**
             * 
             */
            Content: Uint8Array | null = null;
            /**
             * 
             */
            CharSet: string | null = null;
            /**
             * 
             */
            ContentType: string | null = null;
            /**
             * 
             */
            FileName: string | null = null;
            /**
             * 
             */
            LastModified: Date | null = null;
            /**
             * 
             */
            Location: string | null = null;
            /**
             * 
             */
            ETag: string | null = null;
        }
        /**
         * 
         */
        export class NameValuePair {
            constructor(obj?: any) {
                for(let prop in obj) {
                    switch(prop) {
                        case "Name":
                            if (obj.Name) { this.Name = obj.Name as string; }
                            break;
                        case "Value":
                            if (obj.Value) { this.Value = obj.Value as string; }
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
                if(this.Name) { arr.push('"Name": '); arr.push(JSON.stringify(this.Name)); arr.push(','); } 
                if(this.Value) { arr.push('"Value": '); arr.push(JSON.stringify(this.Value)); arr.push(','); } 
                if(arr[arr.length-1] == ',') arr[arr.length-1] = '}'; else arr.push('}');
                if(returnString) return arr.join("");
                return null;
            }
            /**
             * 
             */
            Name: string | null = null;
            /**
             * 
             */
            Value: string | null = null;
        }
        /**
         * 
         */
        export class SolidRpcHostInstance {
            constructor(obj?: any) {
                for(let prop in obj) {
                    switch(prop) {
                        case "HostId":
                            if (obj.HostId) { this.HostId = obj.HostId as string; }
                            break;
                        case "Started":
                            if (obj.Started) { this.Started = new Date(obj.Started); }
                            break;
                        case "LastAlive":
                            if (obj.LastAlive) { this.LastAlive = new Date(obj.LastAlive); }
                            break;
                        case "BaseAddress":
                            if (obj.BaseAddress) { this.BaseAddress = obj.BaseAddress as string; }
                            break;
                        case "HttpCookies":
                            if (obj.HttpCookies) { this.HttpCookies = obj.HttpCookies as Record<string,string>; }
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
                if(this.HostId) { arr.push('"HostId": '); arr.push(JSON.stringify(this.HostId)); arr.push(','); } 
                if(this.Started) { arr.push('"Started": '); arr.push(JSON.stringify(this.Started)); arr.push(','); } 
                if(this.LastAlive) { arr.push('"LastAlive": '); arr.push(JSON.stringify(this.LastAlive)); arr.push(','); } 
                if(this.BaseAddress) { arr.push('"BaseAddress": '); arr.push(JSON.stringify(this.BaseAddress)); arr.push(','); } 
                if(this.HttpCookies) { arr.push('"HttpCookies": '); arr.push(JSON.stringify(this.HttpCookies)); arr.push(','); } 
                if(arr[arr.length-1] == ',') arr[arr.length-1] = '}'; else arr.push('}');
                if(returnString) return arr.join("");
                return null;
            }
            /**
             * 
             */
            HostId: string | null = null;
            /**
             * 
             */
            Started: Date | null = null;
            /**
             * 
             */
            LastAlive: Date | null = null;
            /**
             * 
             */
            BaseAddress: string | null = null;
            /**
             * 
             */
            HttpCookies: Record<string,string> | null = null;
        }
    }
}
