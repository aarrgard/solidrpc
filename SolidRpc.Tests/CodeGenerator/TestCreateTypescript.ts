import { default as CancellationToken } from 'cancellationtoken';
import { Observable, Subject } from 'rxjs';
import { share } from 'rxjs/operators'
import { SolidRpcJs } from 'solidrpcjs';
export namespace Abstractions {
    export namespace Services {
        export namespace Code {
            /**
             * instance responsible for generating code structures
             */
            export interface ICodeNamespaceGenerator {
                /**
                 * Creates a code namespace for supplied assembly name
                 * @param assemblyName 
                 * @param cancellationToken 
                 */
                CreateCodeNamespace(
                    assemblyName : string,
                    cancellationToken? : CancellationToken
                ): Observable<Types.Code.CodeNamespace>;
                /**
                 * This observable is hot and monitors all the responses from the CreateCodeNamespace invocations.
                 */
                CreateCodeNamespaceObservable : Observable<Types.Code.CodeNamespace>;
            }
            /**
             * instance responsible for generating code structures
             */
            export class CodeNamespaceGeneratorImpl  extends SolidRpcJs.RpcServiceImpl implements ICodeNamespaceGenerator {
                private Namespace: SolidRpcJs.Namespace;
                constructor() {
                    super();
                    this.Namespace = SolidRpcJs.rootNamespace.declareNamespace('SolidRpc.Abstractions.Services.Code.ICodeNamespaceGenerator');
                    this.CreateCodeNamespaceSubject = new Subject<Types.Code.CodeNamespace>();
                    this.CreateCodeNamespaceObservable = this.CreateCodeNamespaceSubject.asObservable().pipe(share());
                }
                /**
                 * Creates a code namespace for supplied assembly name
                 * @param assemblyName 
                 * @param cancellationToken 
                 */
                CreateCodeNamespace(
                    assemblyName : string,
                    cancellationToken? : CancellationToken
                ): Observable<Types.Code.CodeNamespace> {
                    let uri = this.Namespace.getStringValue('baseUri','https://localhost/') + 'SolidRpc/Abstractions/Services/Code/ICodeNamespaceGenerator/CreateCodeNamespace/{assemblyName}';
                    SolidRpcJs.ifnull(assemblyName, () => { uri = uri.replace('{assemblyName}', ''); }, nn =>  { uri = uri.replace('{assemblyName}', SolidRpcJs.encodeUriValue(nn.toString())); });
                    let query: { [index: string]: any } = {};
                    let headers: { [index: string]: any } = {};
                    return this.request<Types.Code.CodeNamespace>(new SolidRpcJs.RpcServiceRequest('get', uri, query, headers, null), cancellationToken, function(code : number, data : any) {
                        if(code == 200) {
                            return new Types.Code.CodeNamespace(data);
                        } else {
                            throw 'Response code != 200('+code+')';
                        }
                    }, this.CreateCodeNamespaceSubject);
                }
                /**
                 * This observable is hot and monitors all the responses from the CreateCodeNamespace invocations.
                 */
                CreateCodeNamespaceObservable : Observable<Types.Code.CodeNamespace>;
                private CreateCodeNamespaceSubject : Subject<Types.Code.CodeNamespace>;
            }
            /**
             * Instance for the ICodeNamespaceGenerator type. Implemented by the CodeNamespaceGeneratorImpl
             */
            export var CodeNamespaceGeneratorInstance : ICodeNamespaceGenerator = new CodeNamespaceGeneratorImpl();
            /**
             * The npm generator
             */
            export interface INpmGenerator {
                /**
                 * Returns the files that should be stored in the node_modules directory
                 * @param assemblyNames The name of the assemblies to create an npm package for.
                 * @param cancellationToken 
                 */
                CreateNpmPackage(
                    assemblyNames : string[],
                    cancellationToken? : CancellationToken
                ): Observable<Types.Code.NpmPackage[]>;
                /**
                 * This observable is hot and monitors all the responses from the CreateNpmPackage invocations.
                 */
                CreateNpmPackageObservable : Observable<Types.Code.NpmPackage[]>;
                /**
                 * Returns a zip containing the npm packages. This zip can be exploded in the node_modules directory.
                 * @param assemblyNames The name of the assembly to create an npm package for.
                 * @param cancellationToken 
                 */
                CreateNpmZip(
                    assemblyNames : string[],
                    cancellationToken? : CancellationToken
                ): Observable<Types.FileContent>;
                /**
                 * This observable is hot and monitors all the responses from the CreateNpmZip invocations.
                 */
                CreateNpmZipObservable : Observable<Types.FileContent>;
            }
            /**
             * The npm generator
             */
            export class NpmGeneratorImpl  extends SolidRpcJs.RpcServiceImpl implements INpmGenerator {
                private Namespace: SolidRpcJs.Namespace;
                constructor() {
                    super();
                    this.Namespace = SolidRpcJs.rootNamespace.declareNamespace('SolidRpc.Abstractions.Services.Code.INpmGenerator');
                    this.CreateNpmPackageSubject = new Subject<Types.Code.NpmPackage[]>();
                    this.CreateNpmPackageObservable = this.CreateNpmPackageSubject.asObservable().pipe(share());
                    this.CreateNpmZipSubject = new Subject<Types.FileContent>();
                    this.CreateNpmZipObservable = this.CreateNpmZipSubject.asObservable().pipe(share());
                }
                /**
                 * Returns the files that should be stored in the node_modules directory
                 * @param assemblyNames The name of the assemblies to create an npm package for.
                 * @param cancellationToken 
                 */
                CreateNpmPackage(
                    assemblyNames : string[],
                    cancellationToken? : CancellationToken
                ): Observable<Types.Code.NpmPackage[]> {
                    let uri = this.Namespace.getStringValue('baseUri','https://localhost/') + 'SolidRpc/Abstractions/Services/Code/INpmGenerator/CreateNpmPackage/{assemblyNames}';
                    SolidRpcJs.ifnull(assemblyNames, () => { uri = uri.replace('{assemblyNames}', ''); }, nn =>  { uri = uri.replace('{assemblyNames}', SolidRpcJs.encodeUriValue(nn.toString())); });
                    let query: { [index: string]: any } = {};
                    let headers: { [index: string]: any } = {};
                    return this.request<Types.Code.NpmPackage[]>(new SolidRpcJs.RpcServiceRequest('get', uri, query, headers, null), cancellationToken, function(code : number, data : any) {
                        if(code == 200) {
                            return Array.from(data).map(o => new Types.Code.NpmPackage(o));
                        } else {
                            throw 'Response code != 200('+code+')';
                        }
                    }, this.CreateNpmPackageSubject);
                }
                /**
                 * This observable is hot and monitors all the responses from the CreateNpmPackage invocations.
                 */
                CreateNpmPackageObservable : Observable<Types.Code.NpmPackage[]>;
                private CreateNpmPackageSubject : Subject<Types.Code.NpmPackage[]>;
                /**
                 * Returns a zip containing the npm packages. This zip can be exploded in the node_modules directory.
                 * @param assemblyNames The name of the assembly to create an npm package for.
                 * @param cancellationToken 
                 */
                CreateNpmZip(
                    assemblyNames : string[],
                    cancellationToken? : CancellationToken
                ): Observable<Types.FileContent> {
                    let uri = this.Namespace.getStringValue('baseUri','https://localhost/') + 'SolidRpc/Abstractions/Services/Code/INpmGenerator/CreateNpmZip/{assemblyNames}';
                    SolidRpcJs.ifnull(assemblyNames, () => { uri = uri.replace('{assemblyNames}', ''); }, nn =>  { uri = uri.replace('{assemblyNames}', SolidRpcJs.encodeUriValue(nn.toString())); });
                    let query: { [index: string]: any } = {};
                    let headers: { [index: string]: any } = {};
                    return this.request<Types.FileContent>(new SolidRpcJs.RpcServiceRequest('get', uri, query, headers, null), cancellationToken, function(code : number, data : any) {
                        if(code == 200) {
                            return new Types.FileContent(data);
                        } else {
                            throw 'Response code != 200('+code+')';
                        }
                    }, this.CreateNpmZipSubject);
                }
                /**
                 * This observable is hot and monitors all the responses from the CreateNpmZip invocations.
                 */
                CreateNpmZipObservable : Observable<Types.FileContent>;
                private CreateNpmZipSubject : Subject<Types.FileContent>;
            }
            /**
             * Instance for the INpmGenerator type. Implemented by the NpmGeneratorImpl
             */
            export var NpmGeneratorInstance : INpmGenerator = new NpmGeneratorImpl();
            /**
             * instance responsible for generating code structures
             */
            export interface ITypescriptGenerator {
                /**
                 * Creates a types.ts file from supplied assembly name
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
                 * Creates a types.ts file from supplied code namespace
                 * @param codeNamespace 
                 * @param cancellationToken 
                 */
                CreateTypesTsForCodeNamespaceAsync(
                    codeNamespace : Types.Code.CodeNamespace,
                    cancellationToken? : CancellationToken
                ): Observable<string>;
                /**
                 * This observable is hot and monitors all the responses from the CreateTypesTsForCodeNamespaceAsync invocations.
                 */
                CreateTypesTsForCodeNamespaceAsyncObservable : Observable<string>;
            }
            /**
             * instance responsible for generating code structures
             */
            export class TypescriptGeneratorImpl  extends SolidRpcJs.RpcServiceImpl implements ITypescriptGenerator {
                private Namespace: SolidRpcJs.Namespace;
                constructor() {
                    super();
                    this.Namespace = SolidRpcJs.rootNamespace.declareNamespace('SolidRpc.Abstractions.Services.Code.ITypescriptGenerator');
                    this.CreateTypesTsForAssemblyAsyncSubject = new Subject<string>();
                    this.CreateTypesTsForAssemblyAsyncObservable = this.CreateTypesTsForAssemblyAsyncSubject.asObservable().pipe(share());
                    this.CreateTypesTsForCodeNamespaceAsyncSubject = new Subject<string>();
                    this.CreateTypesTsForCodeNamespaceAsyncObservable = this.CreateTypesTsForCodeNamespaceAsyncSubject.asObservable().pipe(share());
                }
                /**
                 * Creates a types.ts file from supplied assembly name
                 * @param assemblyName 
                 * @param cancellationToken 
                 */
                CreateTypesTsForAssemblyAsync(
                    assemblyName : string,
                    cancellationToken? : CancellationToken
                ): Observable<string> {
                    let uri = this.Namespace.getStringValue('baseUri','https://localhost/') + 'SolidRpc/Abstractions/Services/Code/ITypescriptGenerator/CreateTypesTsForAssemblyAsync/{assemblyName}';
                    SolidRpcJs.ifnull(assemblyName, () => { uri = uri.replace('{assemblyName}', ''); }, nn =>  { uri = uri.replace('{assemblyName}', SolidRpcJs.encodeUriValue(nn.toString())); });
                    let query: { [index: string]: any } = {};
                    let headers: { [index: string]: any } = {};
                    return this.request<string>(new SolidRpcJs.RpcServiceRequest('get', uri, query, headers, null), cancellationToken, function(code : number, data : any) {
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
                 * Creates a types.ts file from supplied code namespace
                 * @param codeNamespace 
                 * @param cancellationToken 
                 */
                CreateTypesTsForCodeNamespaceAsync(
                    codeNamespace : Types.Code.CodeNamespace,
                    cancellationToken? : CancellationToken
                ): Observable<string> {
                    let uri = this.Namespace.getStringValue('baseUri','https://localhost/') + 'SolidRpc/Abstractions/Services/Code/ITypescriptGenerator/CreateTypesTsForCodeNamespaceAsync';
                    let query: { [index: string]: any } = {};
                    let headers: { [index: string]: any } = {};
                    headers['Content-Type']='application/json';
                    return this.request<string>(new SolidRpcJs.RpcServiceRequest('post', uri, query, headers, this.toJson(codeNamespace)), cancellationToken, function(code : number, data : any) {
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
         * The content handler uses the ISolidRpcContentStore to deliver static or proxied content.
         *             
         *             This handler can be invoked from a configured proxy or mapped directly in a .Net Core Handler.
         */
        export interface ISolidRpcContentHandler {
            /**
             * Returns the content for supplied path.
             *             
             *             Note that the path is marked as optional(default value set). This is so that the parameter
             *             is placed in the query string instead of path.
             * @param path The path to get the content for
             * @param cancellationToken 
             */
            GetContent(
                path? : string,
                cancellationToken? : CancellationToken
            ): Observable<Types.FileContent>;
            /**
             * This observable is hot and monitors all the responses from the GetContent invocations.
             */
            GetContentObservable : Observable<Types.FileContent>;
        }
        /**
         * The content handler uses the ISolidRpcContentStore to deliver static or proxied content.
         *             
         *             This handler can be invoked from a configured proxy or mapped directly in a .Net Core Handler.
         */
        export class SolidRpcContentHandlerImpl  extends SolidRpcJs.RpcServiceImpl implements ISolidRpcContentHandler {
            private Namespace: SolidRpcJs.Namespace;
            constructor() {
                super();
                this.Namespace = SolidRpcJs.rootNamespace.declareNamespace('SolidRpc.Abstractions.Services.ISolidRpcContentHandler');
                this.GetContentSubject = new Subject<Types.FileContent>();
                this.GetContentObservable = this.GetContentSubject.asObservable().pipe(share());
            }
            /**
             * Returns the content for supplied path.
             *             
             *             Note that the path is marked as optional(default value set). This is so that the parameter
             *             is placed in the query string instead of path.
             * @param path The path to get the content for
             * @param cancellationToken 
             */
            GetContent(
                path? : string,
                cancellationToken? : CancellationToken
            ): Observable<Types.FileContent> {
                let uri = this.Namespace.getStringValue('baseUri','https://localhost/') + 'SolidRpc/Abstractions/Services/ISolidRpcContentHandler/GetContent';
                let query: { [index: string]: any } = {};
                SolidRpcJs.ifnotnull(path, x => { query['path'] = x; });
                let headers: { [index: string]: any } = {};
                return this.request<Types.FileContent>(new SolidRpcJs.RpcServiceRequest('get', uri, query, headers, null), cancellationToken, function(code : number, data : any) {
                    if(code == 200) {
                        return new Types.FileContent(data);
                    } else {
                        throw 'Response code != 200('+code+')';
                    }
                }, this.GetContentSubject);
            }
            /**
             * This observable is hot and monitors all the responses from the GetContent invocations.
             */
            GetContentObservable : Observable<Types.FileContent>;
            private GetContentSubject : Subject<Types.FileContent>;
        }
        /**
         * Instance for the ISolidRpcContentHandler type. Implemented by the SolidRpcContentHandlerImpl
         */
        export var SolidRpcContentHandlerInstance : ISolidRpcContentHandler = new SolidRpcContentHandlerImpl();
        /**
         * Represents a solid rpc host.
         */
        export interface ISolidRpcHost {
            /**
             * Returns the id of this host
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
             * Returns the id of this host. This method can be used to determine if a host is up and running by
             *             comparing the returned value with the instance that we want to send to. If a host goes down it is 
             *             removed from the router and another instance probably responds to the call.
             * @param cancellationToken 
             */
            GetHostInstance(
                cancellationToken? : CancellationToken
            ): Observable<Types.SolidRpcHostInstance>;
            /**
             * This observable is hot and monitors all the responses from the GetHostInstance invocations.
             */
            GetHostInstanceObservable : Observable<Types.SolidRpcHostInstance>;
            /**
             * This method is invoked on all the hosts in a store when a new host is available.
             * @param cancellationToken 
             */
            SyncHostsFromStore(
                cancellationToken? : CancellationToken
            ): Observable<Types.SolidRpcHostInstance[]>;
            /**
             * This observable is hot and monitors all the responses from the SyncHostsFromStore invocations.
             */
            SyncHostsFromStoreObservable : Observable<Types.SolidRpcHostInstance[]>;
            /**
             * Invokes the "GetHostInstance" targeted for supplied instance and resturns the result
             * @param hostInstance 
             * @param cancellationToken 
             */
            CheckHost(
                hostInstance : Types.SolidRpcHostInstance,
                cancellationToken? : CancellationToken
            ): Observable<Types.SolidRpcHostInstance>;
            /**
             * This observable is hot and monitors all the responses from the CheckHost invocations.
             */
            CheckHostObservable : Observable<Types.SolidRpcHostInstance>;
            /**
             * Returns the host configuration.
             * @param cancellationToken 
             */
            GetHostConfiguration(
                cancellationToken? : CancellationToken
            ): Observable<Types.NameValuePair[]>;
            /**
             * This observable is hot and monitors all the responses from the GetHostConfiguration invocations.
             */
            GetHostConfigurationObservable : Observable<Types.NameValuePair[]>;
            /**
             * Function that determines if the host is alive.
             * @param cancellationToken 
             */
            IsAlive(
                cancellationToken? : CancellationToken
            ): Observable<void>;
            /**
             * This observable is hot and monitors all the responses from the IsAlive invocations.
             */
            IsAliveObservable : Observable<void>;
            /**
             * Returns the base url for this host
             * @param cancellationToken 
             */
            BaseAddress(
                cancellationToken? : CancellationToken
            ): Observable<string>;
            /**
             * This observable is hot and monitors all the responses from the BaseAddress invocations.
             */
            BaseAddressObservable : Observable<string>;
        }
        /**
         * Represents a solid rpc host.
         */
        export class SolidRpcHostImpl  extends SolidRpcJs.RpcServiceImpl implements ISolidRpcHost {
            private Namespace: SolidRpcJs.Namespace;
            constructor() {
                super();
                this.Namespace = SolidRpcJs.rootNamespace.declareNamespace('SolidRpc.Abstractions.Services.ISolidRpcHost');
                this.GetHostIdSubject = new Subject<string>();
                this.GetHostIdObservable = this.GetHostIdSubject.asObservable().pipe(share());
                this.GetHostInstanceSubject = new Subject<Types.SolidRpcHostInstance>();
                this.GetHostInstanceObservable = this.GetHostInstanceSubject.asObservable().pipe(share());
                this.SyncHostsFromStoreSubject = new Subject<Types.SolidRpcHostInstance[]>();
                this.SyncHostsFromStoreObservable = this.SyncHostsFromStoreSubject.asObservable().pipe(share());
                this.CheckHostSubject = new Subject<Types.SolidRpcHostInstance>();
                this.CheckHostObservable = this.CheckHostSubject.asObservable().pipe(share());
                this.GetHostConfigurationSubject = new Subject<Types.NameValuePair[]>();
                this.GetHostConfigurationObservable = this.GetHostConfigurationSubject.asObservable().pipe(share());
                this.IsAliveSubject = new Subject<void>();
                this.IsAliveObservable = this.IsAliveSubject.asObservable().pipe(share());
                this.BaseAddressSubject = new Subject<string>();
                this.BaseAddressObservable = this.BaseAddressSubject.asObservable().pipe(share());
            }
            /**
             * Returns the id of this host
             * @param cancellationToken 
             */
            GetHostId(
                cancellationToken? : CancellationToken
            ): Observable<string> {
                let uri = this.Namespace.getStringValue('baseUri','https://localhost/') + 'SolidRpc/Abstractions/Services/ISolidRpcHost/GetHostId';
                let query: { [index: string]: any } = {};
                let headers: { [index: string]: any } = {};
                return this.request<string>(new SolidRpcJs.RpcServiceRequest('get', uri, query, headers, null), cancellationToken, function(code : number, data : any) {
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
             * Returns the id of this host. This method can be used to determine if a host is up and running by
             *             comparing the returned value with the instance that we want to send to. If a host goes down it is 
             *             removed from the router and another instance probably responds to the call.
             * @param cancellationToken 
             */
            GetHostInstance(
                cancellationToken? : CancellationToken
            ): Observable<Types.SolidRpcHostInstance> {
                let uri = this.Namespace.getStringValue('baseUri','https://localhost/') + 'SolidRpc/Abstractions/Services/ISolidRpcHost/GetHostInstance';
                let query: { [index: string]: any } = {};
                let headers: { [index: string]: any } = {};
                return this.request<Types.SolidRpcHostInstance>(new SolidRpcJs.RpcServiceRequest('get', uri, query, headers, null), cancellationToken, function(code : number, data : any) {
                    if(code == 200) {
                        return new Types.SolidRpcHostInstance(data);
                    } else {
                        throw 'Response code != 200('+code+')';
                    }
                }, this.GetHostInstanceSubject);
            }
            /**
             * This observable is hot and monitors all the responses from the GetHostInstance invocations.
             */
            GetHostInstanceObservable : Observable<Types.SolidRpcHostInstance>;
            private GetHostInstanceSubject : Subject<Types.SolidRpcHostInstance>;
            /**
             * This method is invoked on all the hosts in a store when a new host is available.
             * @param cancellationToken 
             */
            SyncHostsFromStore(
                cancellationToken? : CancellationToken
            ): Observable<Types.SolidRpcHostInstance[]> {
                let uri = this.Namespace.getStringValue('baseUri','https://localhost/') + 'SolidRpc/Abstractions/Services/ISolidRpcHost/SyncHostsFromStore';
                let query: { [index: string]: any } = {};
                let headers: { [index: string]: any } = {};
                return this.request<Types.SolidRpcHostInstance[]>(new SolidRpcJs.RpcServiceRequest('get', uri, query, headers, null), cancellationToken, function(code : number, data : any) {
                    if(code == 200) {
                        return Array.from(data).map(o => new Types.SolidRpcHostInstance(o));
                    } else {
                        throw 'Response code != 200('+code+')';
                    }
                }, this.SyncHostsFromStoreSubject);
            }
            /**
             * This observable is hot and monitors all the responses from the SyncHostsFromStore invocations.
             */
            SyncHostsFromStoreObservable : Observable<Types.SolidRpcHostInstance[]>;
            private SyncHostsFromStoreSubject : Subject<Types.SolidRpcHostInstance[]>;
            /**
             * Invokes the "GetHostInstance" targeted for supplied instance and resturns the result
             * @param hostInstance 
             * @param cancellationToken 
             */
            CheckHost(
                hostInstance : Types.SolidRpcHostInstance,
                cancellationToken? : CancellationToken
            ): Observable<Types.SolidRpcHostInstance> {
                let uri = this.Namespace.getStringValue('baseUri','https://localhost/') + 'SolidRpc/Abstractions/Services/ISolidRpcHost/CheckHost';
                let query: { [index: string]: any } = {};
                let headers: { [index: string]: any } = {};
                headers['Content-Type']='application/json';
                return this.request<Types.SolidRpcHostInstance>(new SolidRpcJs.RpcServiceRequest('post', uri, query, headers, this.toJson(hostInstance)), cancellationToken, function(code : number, data : any) {
                    if(code == 200) {
                        return new Types.SolidRpcHostInstance(data);
                    } else {
                        throw 'Response code != 200('+code+')';
                    }
                }, this.CheckHostSubject);
            }
            /**
             * This observable is hot and monitors all the responses from the CheckHost invocations.
             */
            CheckHostObservable : Observable<Types.SolidRpcHostInstance>;
            private CheckHostSubject : Subject<Types.SolidRpcHostInstance>;
            /**
             * Returns the host configuration.
             * @param cancellationToken 
             */
            GetHostConfiguration(
                cancellationToken? : CancellationToken
            ): Observable<Types.NameValuePair[]> {
                let uri = this.Namespace.getStringValue('baseUri','https://localhost/') + 'SolidRpc/Abstractions/Services/ISolidRpcHost/GetHostConfiguration';
                let query: { [index: string]: any } = {};
                let headers: { [index: string]: any } = {};
                return this.request<Types.NameValuePair[]>(new SolidRpcJs.RpcServiceRequest('get', uri, query, headers, null), cancellationToken, function(code : number, data : any) {
                    if(code == 200) {
                        return Array.from(data).map(o => new Types.NameValuePair(o));
                    } else {
                        throw 'Response code != 200('+code+')';
                    }
                }, this.GetHostConfigurationSubject);
            }
            /**
             * This observable is hot and monitors all the responses from the GetHostConfiguration invocations.
             */
            GetHostConfigurationObservable : Observable<Types.NameValuePair[]>;
            private GetHostConfigurationSubject : Subject<Types.NameValuePair[]>;
            /**
             * Function that determines if the host is alive.
             * @param cancellationToken 
             */
            IsAlive(
                cancellationToken? : CancellationToken
            ): Observable<void> {
                let uri = this.Namespace.getStringValue('baseUri','https://localhost/') + 'SolidRpc/Abstractions/Services/ISolidRpcHost/IsAlive';
                let query: { [index: string]: any } = {};
                let headers: { [index: string]: any } = {};
                return this.request<void>(new SolidRpcJs.RpcServiceRequest('get', uri, query, headers, null), cancellationToken, function(code : number, data : any) {
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
            /**
             * Returns the base url for this host
             * @param cancellationToken 
             */
            BaseAddress(
                cancellationToken? : CancellationToken
            ): Observable<string> {
                let uri = this.Namespace.getStringValue('baseUri','https://localhost/') + 'SolidRpc/Abstractions/Services/ISolidRpcHost/BaseAddress';
                let query: { [index: string]: any } = {};
                let headers: { [index: string]: any } = {};
                return this.request<string>(new SolidRpcJs.RpcServiceRequest('get', uri, query, headers, null), cancellationToken, function(code : number, data : any) {
                    if(code == 200) {
                        return data as string;
                    } else {
                        throw 'Response code != 200('+code+')';
                    }
                }, this.BaseAddressSubject);
            }
            /**
             * This observable is hot and monitors all the responses from the BaseAddress invocations.
             */
            BaseAddressObservable : Observable<string>;
            private BaseAddressSubject : Subject<string>;
        }
        /**
         * Instance for the ISolidRpcHost type. Implemented by the SolidRpcHostImpl
         */
        export var SolidRpcHostInstance : ISolidRpcHost = new SolidRpcHostImpl();
        /**
         * Interfaces that defines the logic for OAuth2 support.
         */
        export interface ISolidRpcOAuth2 {
            /**
             * This is the method returns a html page that calls supplied callback
             *             after the token callback has been invoked. Use this method to
             *             retreive tokens from a standalone node instance.
             *             
             *             Start a local http server and supply the address to the handler.
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
            ): Observable<Types.FileContent>;
            /**
             * This observable is hot and monitors all the responses from the GetAuthorizationCodeTokenAsync invocations.
             */
            GetAuthorizationCodeTokenAsyncObservable : Observable<Types.FileContent>;
            /**
             * This is the method that is invoked when a user has been authenticated
             *             and a valid token is supplied.
             * @param code 
             * @param state 
             * @param cancellation 
             */
            TokenCallbackAsync(
                code? : string,
                state? : string,
                cancellation? : CancellationToken
            ): Observable<Types.FileContent>;
            /**
             * This observable is hot and monitors all the responses from the TokenCallbackAsync invocations.
             */
            TokenCallbackAsyncObservable : Observable<Types.FileContent>;
        }
        /**
         * Interfaces that defines the logic for OAuth2 support.
         */
        export class SolidRpcOAuth2Impl  extends SolidRpcJs.RpcServiceImpl implements ISolidRpcOAuth2 {
            private Namespace: SolidRpcJs.Namespace;
            constructor() {
                super();
                this.Namespace = SolidRpcJs.rootNamespace.declareNamespace('SolidRpc.Abstractions.Services.ISolidRpcOAuth2');
                this.GetAuthorizationCodeTokenAsyncSubject = new Subject<Types.FileContent>();
                this.GetAuthorizationCodeTokenAsyncObservable = this.GetAuthorizationCodeTokenAsyncSubject.asObservable().pipe(share());
                this.TokenCallbackAsyncSubject = new Subject<Types.FileContent>();
                this.TokenCallbackAsyncObservable = this.TokenCallbackAsyncSubject.asObservable().pipe(share());
            }
            /**
             * This is the method returns a html page that calls supplied callback
             *             after the token callback has been invoked. Use this method to
             *             retreive tokens from a standalone node instance.
             *             
             *             Start a local http server and supply the address to the handler.
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
            ): Observable<Types.FileContent> {
                let uri = this.Namespace.getStringValue('baseUri','https://localhost/') + 'SolidRpc/Abstractions/Services/ISolidRpcOAuth2/GetAuthorizationCodeTokenAsync';
                let query: { [index: string]: any } = {};
                SolidRpcJs.ifnotnull(callbackUri, x => { query['callbackUri'] = x; });
                SolidRpcJs.ifnotnull(state, x => { query['state'] = x; });
                SolidRpcJs.ifnotnull(scopes, x => { query['scopes'] = x; });
                let headers: { [index: string]: any } = {};
                return this.request<Types.FileContent>(new SolidRpcJs.RpcServiceRequest('get', uri, query, headers, null), cancellationToken, function(code : number, data : any) {
                    if(code == 200) {
                        return new Types.FileContent(data);
                    } else {
                        throw 'Response code != 200('+code+')';
                    }
                }, this.GetAuthorizationCodeTokenAsyncSubject);
            }
            /**
             * This observable is hot and monitors all the responses from the GetAuthorizationCodeTokenAsync invocations.
             */
            GetAuthorizationCodeTokenAsyncObservable : Observable<Types.FileContent>;
            private GetAuthorizationCodeTokenAsyncSubject : Subject<Types.FileContent>;
            /**
             * This is the method that is invoked when a user has been authenticated
             *             and a valid token is supplied.
             * @param code 
             * @param state 
             * @param cancellation 
             */
            TokenCallbackAsync(
                code? : string,
                state? : string,
                cancellation? : CancellationToken
            ): Observable<Types.FileContent> {
                let uri = this.Namespace.getStringValue('baseUri','https://localhost/') + 'SolidRpc/Abstractions/Services/ISolidRpcOAuth2/TokenCallbackAsync';
                let query: { [index: string]: any } = {};
                SolidRpcJs.ifnotnull(code, x => { query['code'] = x; });
                SolidRpcJs.ifnotnull(state, x => { query['state'] = x; });
                let headers: { [index: string]: any } = {};
                return this.request<Types.FileContent>(new SolidRpcJs.RpcServiceRequest('get', uri, query, headers, null), cancellation, function(code : number, data : any) {
                    if(code == 200) {
                        return new Types.FileContent(data);
                    } else {
                        throw 'Response code != 200('+code+')';
                    }
                }, this.TokenCallbackAsyncSubject);
            }
            /**
             * This observable is hot and monitors all the responses from the TokenCallbackAsync invocations.
             */
            TokenCallbackAsyncObservable : Observable<Types.FileContent>;
            private TokenCallbackAsyncSubject : Subject<Types.FileContent>;
        }
        /**
         * Instance for the ISolidRpcOAuth2 type. Implemented by the SolidRpcOAuth2Impl
         */
        export var SolidRpcOAuth2Instance : ISolidRpcOAuth2 = new SolidRpcOAuth2Impl();
        /**
         * Implements logic for the oidc server
         */
        export interface ISolidRpcOidc {
            /**
             * Returns the /.well-known/openid-configuration file
             * @param cancellationToken 
             */
            GetDiscoveryDocumentAsync(
                cancellationToken? : CancellationToken
            ): Observable<Types.OAuth2.OpenIDConnectDiscovery>;
            /**
             * This observable is hot and monitors all the responses from the GetDiscoveryDocumentAsync invocations.
             */
            GetDiscoveryDocumentAsyncObservable : Observable<Types.OAuth2.OpenIDConnectDiscovery>;
            /**
             * Returns the keys
             * @param cancellationToken 
             */
            GetKeysAsync(
                cancellationToken? : CancellationToken
            ): Observable<Types.OAuth2.OpenIDKeys>;
            /**
             * This observable is hot and monitors all the responses from the GetKeysAsync invocations.
             */
            GetKeysAsyncObservable : Observable<Types.OAuth2.OpenIDKeys>;
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
            GetTokenAsync(
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
            ): Observable<Types.OAuth2.TokenResponse>;
            /**
             * This observable is hot and monitors all the responses from the GetTokenAsync invocations.
             */
            GetTokenAsyncObservable : Observable<Types.OAuth2.TokenResponse>;
            /**
             * authorizes a user
             * @param scope REQUIRED. OpenID Connect requests MUST contain the openid scope value. If the openid scope value is not present, the behavior is entirely unspecified. Other scope values MAY be present. Scope values used that are not understood by an implementation SHOULD be ignored. See Sections 5.4 and 11 for additional scope values defined by this specification.
             * @param responseType 
             * @param clientId 
             * @param redirectUri 
             * @param state RECOMMENDED. Opaque value used to maintain state between the request and the callback. Typically, Cross-Site Request Forgery (CSRF, XSRF) mitigation is done by cryptographically binding the value of this parameter with a browser cookie.
             * @param cancellationToken 
             */
            AuthorizeAsync(
                scope : string[],
                responseType : string,
                clientId : string,
                redirectUri? : string,
                state? : string,
                cancellationToken? : CancellationToken
            ): Observable<Types.FileContent>;
            /**
             * This observable is hot and monitors all the responses from the AuthorizeAsync invocations.
             */
            AuthorizeAsyncObservable : Observable<Types.FileContent>;
        }
        /**
         * Implements logic for the oidc server
         */
        export class SolidRpcOidcImpl  extends SolidRpcJs.RpcServiceImpl implements ISolidRpcOidc {
            private Namespace: SolidRpcJs.Namespace;
            constructor() {
                super();
                this.Namespace = SolidRpcJs.rootNamespace.declareNamespace('SolidRpc.Abstractions.Services.ISolidRpcOidc');
                this.GetDiscoveryDocumentAsyncSubject = new Subject<Types.OAuth2.OpenIDConnectDiscovery>();
                this.GetDiscoveryDocumentAsyncObservable = this.GetDiscoveryDocumentAsyncSubject.asObservable().pipe(share());
                this.GetKeysAsyncSubject = new Subject<Types.OAuth2.OpenIDKeys>();
                this.GetKeysAsyncObservable = this.GetKeysAsyncSubject.asObservable().pipe(share());
                this.GetTokenAsyncSubject = new Subject<Types.OAuth2.TokenResponse>();
                this.GetTokenAsyncObservable = this.GetTokenAsyncSubject.asObservable().pipe(share());
                this.AuthorizeAsyncSubject = new Subject<Types.FileContent>();
                this.AuthorizeAsyncObservable = this.AuthorizeAsyncSubject.asObservable().pipe(share());
            }
            /**
             * Returns the /.well-known/openid-configuration file
             * @param cancellationToken 
             */
            GetDiscoveryDocumentAsync(
                cancellationToken? : CancellationToken
            ): Observable<Types.OAuth2.OpenIDConnectDiscovery> {
                let uri = this.Namespace.getStringValue('baseUri','https://localhost/') + 'SolidRpc/Abstractions/.well-known/openid-configuration';
                let query: { [index: string]: any } = {};
                let headers: { [index: string]: any } = {};
                return this.request<Types.OAuth2.OpenIDConnectDiscovery>(new SolidRpcJs.RpcServiceRequest('get', uri, query, headers, null), cancellationToken, function(code : number, data : any) {
                    if(code == 200) {
                        return new Types.OAuth2.OpenIDConnectDiscovery(data);
                    } else {
                        throw 'Response code != 200('+code+')';
                    }
                }, this.GetDiscoveryDocumentAsyncSubject);
            }
            /**
             * This observable is hot and monitors all the responses from the GetDiscoveryDocumentAsync invocations.
             */
            GetDiscoveryDocumentAsyncObservable : Observable<Types.OAuth2.OpenIDConnectDiscovery>;
            private GetDiscoveryDocumentAsyncSubject : Subject<Types.OAuth2.OpenIDConnectDiscovery>;
            /**
             * Returns the keys
             * @param cancellationToken 
             */
            GetKeysAsync(
                cancellationToken? : CancellationToken
            ): Observable<Types.OAuth2.OpenIDKeys> {
                let uri = this.Namespace.getStringValue('baseUri','https://localhost/') + 'SolidRpc/Abstractions/Services/ISolidRpcOidc/GetKeysAsync';
                let query: { [index: string]: any } = {};
                let headers: { [index: string]: any } = {};
                return this.request<Types.OAuth2.OpenIDKeys>(new SolidRpcJs.RpcServiceRequest('get', uri, query, headers, null), cancellationToken, function(code : number, data : any) {
                    if(code == 200) {
                        return new Types.OAuth2.OpenIDKeys(data);
                    } else {
                        throw 'Response code != 200('+code+')';
                    }
                }, this.GetKeysAsyncSubject);
            }
            /**
             * This observable is hot and monitors all the responses from the GetKeysAsync invocations.
             */
            GetKeysAsyncObservable : Observable<Types.OAuth2.OpenIDKeys>;
            private GetKeysAsyncSubject : Subject<Types.OAuth2.OpenIDKeys>;
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
            GetTokenAsync(
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
            ): Observable<Types.OAuth2.TokenResponse> {
                let uri = this.Namespace.getStringValue('baseUri','https://localhost/') + 'SolidRpc/Abstractions/Services/ISolidRpcOidc/GetTokenAsync';
                let query: { [index: string]: any } = {};
                let headers: { [index: string]: any } = {};
                return this.request<Types.OAuth2.TokenResponse>(new SolidRpcJs.RpcServiceRequest('post', uri, query, headers, null), cancellationToken, function(code : number, data : any) {
                    if(code == 200) {
                        return new Types.OAuth2.TokenResponse(data);
                    } else {
                        throw 'Response code != 200('+code+')';
                    }
                }, this.GetTokenAsyncSubject);
            }
            /**
             * This observable is hot and monitors all the responses from the GetTokenAsync invocations.
             */
            GetTokenAsyncObservable : Observable<Types.OAuth2.TokenResponse>;
            private GetTokenAsyncSubject : Subject<Types.OAuth2.TokenResponse>;
            /**
             * authorizes a user
             * @param scope REQUIRED. OpenID Connect requests MUST contain the openid scope value. If the openid scope value is not present, the behavior is entirely unspecified. Other scope values MAY be present. Scope values used that are not understood by an implementation SHOULD be ignored. See Sections 5.4 and 11 for additional scope values defined by this specification.
             * @param responseType 
             * @param clientId 
             * @param redirectUri 
             * @param state RECOMMENDED. Opaque value used to maintain state between the request and the callback. Typically, Cross-Site Request Forgery (CSRF, XSRF) mitigation is done by cryptographically binding the value of this parameter with a browser cookie.
             * @param cancellationToken 
             */
            AuthorizeAsync(
                scope : string[],
                responseType : string,
                clientId : string,
                redirectUri? : string,
                state? : string,
                cancellationToken? : CancellationToken
            ): Observable<Types.FileContent> {
                let uri = this.Namespace.getStringValue('baseUri','https://localhost/') + 'SolidRpc/Abstractions/Services/ISolidRpcOidc/AuthorizeAsync';
                let query: { [index: string]: any } = {};
                SolidRpcJs.ifnotnull(scope, x => { query['scope'] = x; });
                SolidRpcJs.ifnotnull(response_type, x => { query['response_type'] = x; });
                SolidRpcJs.ifnotnull(client_id, x => { query['client_id'] = x; });
                SolidRpcJs.ifnotnull(redirect_uri, x => { query['redirect_uri'] = x; });
                SolidRpcJs.ifnotnull(state, x => { query['state'] = x; });
                let headers: { [index: string]: any } = {};
                return this.request<Types.FileContent>(new SolidRpcJs.RpcServiceRequest('get', uri, query, headers, null), cancellationToken, function(code : number, data : any) {
                    if(code == 200) {
                        return new Types.FileContent(data);
                    } else {
                        throw 'Response code != 200('+code+')';
                    }
                }, this.AuthorizeAsyncSubject);
            }
            /**
             * This observable is hot and monitors all the responses from the AuthorizeAsync invocations.
             */
            AuthorizeAsyncObservable : Observable<Types.FileContent>;
            private AuthorizeAsyncSubject : Subject<Types.FileContent>;
        }
        /**
         * Instance for the ISolidRpcOidc type. Implemented by the SolidRpcOidcImpl
         */
        export var SolidRpcOidcInstance : ISolidRpcOidc = new SolidRpcOidcImpl();
    }
    export namespace Types {
        export namespace Code {
            /**
             * Represents an interface
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
                                if (obj.methods) { this.Methods = Array.from(obj.methods).map(o => new CodeMethod(o)); }
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
                    if(this.Methods) { arr.push('"methods": '); for (let i = 0; i < this.Methods.length; i++) if(this.Methods[i]) {this.Methods[i].toJson(arr)}; arr.push(',');; arr.push(','); } 
                    if(arr[arr.length-1] == ',') arr[arr.length-1] = '}'; else arr.push('}');
                    if(returnString) return arr.join("");
                    return null;
                }
                /**
                 * The description of this interface
                 */
                Description: string | null = null;
                /**
                 * The name of this interface
                 */
                Name: string | null = null;
                /**
                 * The methods in the interface
                 */
                Methods: CodeMethod[] | null = null;
            }
            /**
             * Represents a method
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
                                if (obj.arguments) { this.Arguments = Array.from(obj.arguments).map(o => new CodeMethodArg(o)); }
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
                    if(this.Arguments) { arr.push('"arguments": '); for (let i = 0; i < this.Arguments.length; i++) if(this.Arguments[i]) {this.Arguments[i].toJson(arr)}; arr.push(',');; arr.push(','); } 
                    if(this.ReturnType) { arr.push('"returnType": '); for (let i = 0; i < this.ReturnType.length; i++) arr.push(JSON.stringify(this.ReturnType[i])); arr.push(',');; arr.push(','); } 
                    if(this.HttpMethod) { arr.push('"httpMethod": '); arr.push(JSON.stringify(this.HttpMethod)); arr.push(','); } 
                    if(this.HttpBaseAddress) { arr.push('"httpBaseAddress": '); arr.push(JSON.stringify(this.HttpBaseAddress)); arr.push(','); } 
                    if(this.HttpPath) { arr.push('"httpPath": '); arr.push(JSON.stringify(this.HttpPath)); arr.push(','); } 
                    if(arr[arr.length-1] == ',') arr[arr.length-1] = '}'; else arr.push('}');
                    if(returnString) return arr.join("");
                    return null;
                }
                /**
                 * A description of the the method
                 */
                Description: string | null = null;
                /**
                 * The name of this method
                 */
                Name: string | null = null;
                /**
                 * The method arguments
                 */
                Arguments: CodeMethodArg[] | null = null;
                /**
                 * The return type of the method(fully qualified)
                 */
                ReturnType: string[] | null = null;
                /**
                 * The http method(GET,POST,etc.)
                 */
                HttpMethod: string | null = null;
                /**
                 * The base address to this method
                 */
                HttpBaseAddress: string | null = null;
                /**
                 * The http path relative to the base address
                 */
                HttpPath: string | null = null;
            }
            /**
             * Represents a method argument
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
                 * A description of the the argument
                 */
                Description: string | null = null;
                /**
                 * The name of the argument
                 */
                Name: string | null = null;
                /**
                 * The argument type(fully qualified)
                 */
                ArgType: string[] | null = null;
                /**
                 * Specifies if this argument is optional(not required)
                 */
                Optional: boolean | null = null;
                /**
                 * The name of the argument in the http protocol.
                 */
                HttpName: string | null = null;
                /**
                 * The location of the argument('path', 'query', 'header', 'body', 'body-inline')
                 */
                HttpLocation: string | null = null;
            }
            /**
             * represents a namespace
             */
            export class CodeNamespace {
                constructor(obj?: any) {
                    for(let prop in obj) {
                        switch(prop) {
                            case "name":
                                if (obj.name) { this.Name = obj.name as string; }
                                break;
                            case "namespaces":
                                if (obj.namespaces) { this.Namespaces = Array.from(obj.namespaces).map(o => new CodeNamespace(o)); }
                                break;
                            case "interfaces":
                                if (obj.interfaces) { this.Interfaces = Array.from(obj.interfaces).map(o => new CodeInterface(o)); }
                                break;
                            case "types":
                                if (obj.types) { this.Types = Array.from(obj.types).map(o => new CodeType(o)); }
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
                    if(this.Namespaces) { arr.push('"namespaces": '); for (let i = 0; i < this.Namespaces.length; i++) if(this.Namespaces[i]) {this.Namespaces[i].toJson(arr)}; arr.push(',');; arr.push(','); } 
                    if(this.Interfaces) { arr.push('"interfaces": '); for (let i = 0; i < this.Interfaces.length; i++) if(this.Interfaces[i]) {this.Interfaces[i].toJson(arr)}; arr.push(',');; arr.push(','); } 
                    if(this.Types) { arr.push('"types": '); for (let i = 0; i < this.Types.length; i++) if(this.Types[i]) {this.Types[i].toJson(arr)}; arr.push(',');; arr.push(','); } 
                    if(arr[arr.length-1] == ',') arr[arr.length-1] = '}'; else arr.push('}');
                    if(returnString) return arr.join("");
                    return null;
                }
                /**
                 * The name of this namespace part(not fully qualified).
                 */
                Name: string | null = null;
                /**
                 * The namespaces within this namespace
                 */
                Namespaces: CodeNamespace[] | null = null;
                /**
                 * The interfaces within this namespace
                 */
                Interfaces: CodeInterface[] | null = null;
                /**
                 * The types within this namespace
                 */
                Types: CodeType[] | null = null;
            }
            /**
             * Represents a type
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
                                if (obj.properties) { this.Properties = Array.from(obj.properties).map(o => new CodeTypeProperty(o)); }
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
                    if(this.Properties) { arr.push('"properties": '); for (let i = 0; i < this.Properties.length; i++) if(this.Properties[i]) {this.Properties[i].toJson(arr)}; arr.push(',');; arr.push(','); } 
                    if(arr[arr.length-1] == ',') arr[arr.length-1] = '}'; else arr.push('}');
                    if(returnString) return arr.join("");
                    return null;
                }
                /**
                 * A description of the the type
                 */
                Description: string | null = null;
                /**
                 * The name of the type
                 */
                Name: string | null = null;
                /**
                 * The type that this type extends
                 */
                Extends: string[] | null = null;
                /**
                 * The method arguments
                 */
                Properties: CodeTypeProperty[] | null = null;
            }
            /**
             * Represents a property in a type
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
                 * A description of the the property
                 */
                Description: string | null = null;
                /**
                 * The name of the property
                 */
                Name: string | null = null;
                /**
                 * The property type(fully qualified)
                 */
                PropertyType: string[] | null = null;
                /**
                 * The name of the property in the http protocol.
                 */
                HttpName: string | null = null;
            }
            /**
             * successful operation
             */
            export class NpmPackage {
                constructor(obj?: any) {
                    for(let prop in obj) {
                        switch(prop) {
                            case "name":
                                if (obj.name) { this.Name = obj.name as string; }
                                break;
                            case "files":
                                if (obj.files) { this.Files = Array.from(obj.files).map(o => new NpmPackageFile(o)); }
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
                    if(this.Files) { arr.push('"files": '); for (let i = 0; i < this.Files.length; i++) if(this.Files[i]) {this.Files[i].toJson(arr)}; arr.push(',');; arr.push(','); } 
                    if(arr[arr.length-1] == ',') arr[arr.length-1] = '}'; else arr.push('}');
                    if(returnString) return arr.join("");
                    return null;
                }
                /**
                 * The package name(folder name)
                 */
                Name: string | null = null;
                /**
                 * The files within the package
                 */
                Files: NpmPackageFile[] | null = null;
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
                 * The file path within the package
                 */
                FilePath: string | null = null;
                /**
                 * The file content(binary content not supported)
                 */
                Content: string | null = null;
            }
        }
        export namespace OAuth2 {
            /**
             * Represents a discovery document.
             */
            export class OpenIDConnectDiscovery {
                constructor(obj?: any) {
                    for(let prop in obj) {
                        switch(prop) {
                            case "issuer":
                                if (obj.issuer) { this.Issuer = obj.issuer as string; }
                                break;
                            case "authorization_endpoint":
                                if (obj.authorization_endpoint) { this.AuthorizationEndpoint = obj.authorization_endpoint as string; }
                                break;
                            case "token_endpoint":
                                if (obj.token_endpoint) { this.TokenEndpoint = obj.token_endpoint as string; }
                                break;
                            case "userinfo_endpoint":
                                if (obj.userinfo_endpoint) { this.UserinfoEndpoint = obj.userinfo_endpoint as string; }
                                break;
                            case "revocation_endpoint":
                                if (obj.revocation_endpoint) { this.RevocationEndpoint = obj.revocation_endpoint as string; }
                                break;
                            case "device_authorization_endpoint":
                                if (obj.device_authorization_endpoint) { this.DeviceAuthorizationEndpoint = obj.device_authorization_endpoint as string; }
                                break;
                            case "jwks_uri":
                                if (obj.jwks_uri) { this.JwksUri = obj.jwks_uri as string; }
                                break;
                            case "scopes_supported":
                                if (obj.scopes_supported) { this.ScopesSupported = Array.from(obj.scopes_supported).map(o => o as string); }
                                break;
                            case "grant_types_supported":
                                if (obj.grant_types_supported) { this.GrantTypesSupported = Array.from(obj.grant_types_supported).map(o => o as string); }
                                break;
                            case "response_modes_supported":
                                if (obj.response_modes_supported) { this.ResponseModesSupported = Array.from(obj.response_modes_supported).map(o => o as string); }
                                break;
                            case "subject_types_supported":
                                if (obj.subject_types_supported) { this.SubjectTypesSupported = Array.from(obj.subject_types_supported).map(o => o as string); }
                                break;
                            case "id_token_signing_alg_values_supported":
                                if (obj.id_token_signing_alg_values_supported) { this.IdTokenSigningAlgValuesSupported = Array.from(obj.id_token_signing_alg_values_supported).map(o => o as string); }
                                break;
                            case "end_session_endpoint":
                                if (obj.end_session_endpoint) { this.EndSessionEndpoint = obj.end_session_endpoint as string; }
                                break;
                            case "response_types_supported":
                                if (obj.response_types_supported) { this.ResponseTypesSupported = Array.from(obj.response_types_supported).map(o => o as string); }
                                break;
                            case "claims_supported":
                                if (obj.claims_supported) { this.ClaimsSupported = Array.from(obj.claims_supported).map(o => o as string); }
                                break;
                            case "token_endpoint_auth_methods_supported":
                                if (obj.token_endpoint_auth_methods_supported) { this.TokenEndpointAuthMethodsSupported = Array.from(obj.token_endpoint_auth_methods_supported).map(o => o as string); }
                                break;
                            case "code_challenge_methods_supported":
                                if (obj.code_challenge_methods_supported) { this.CodeChallengeMethodsSupported = Array.from(obj.code_challenge_methods_supported).map(o => o as string); }
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
                                if (obj.rbac_url) { this.RbacUrl = obj.rbac_url as string; }
                                break;
                            case "msgraph_host":
                                if (obj.msgraph_host) { this.MsgraphHost = obj.msgraph_host as string; }
                                break;
                            case "cloud_graph_host_name":
                                if (obj.cloud_graph_host_name) { this.CloudGraphHostName = obj.cloud_graph_host_name as string; }
                                break;
                            case "cloud_instance_name":
                                if (obj.cloud_instance_name) { this.CloudInstanceName = obj.cloud_instance_name as string; }
                                break;
                            case "tenant_region_scope":
                                if (obj.tenant_region_scope) { this.TenantRegionScope = obj.tenant_region_scope as string; }
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
                    if(this.ScopesSupported) { arr.push('"scopes_supported": '); for (let i = 0; i < this.ScopesSupported.length; i++) arr.push(JSON.stringify(this.ScopesSupported[i])); arr.push(',');; arr.push(','); } 
                    if(this.GrantTypesSupported) { arr.push('"grant_types_supported": '); for (let i = 0; i < this.GrantTypesSupported.length; i++) arr.push(JSON.stringify(this.GrantTypesSupported[i])); arr.push(',');; arr.push(','); } 
                    if(this.ResponseModesSupported) { arr.push('"response_modes_supported": '); for (let i = 0; i < this.ResponseModesSupported.length; i++) arr.push(JSON.stringify(this.ResponseModesSupported[i])); arr.push(',');; arr.push(','); } 
                    if(this.SubjectTypesSupported) { arr.push('"subject_types_supported": '); for (let i = 0; i < this.SubjectTypesSupported.length; i++) arr.push(JSON.stringify(this.SubjectTypesSupported[i])); arr.push(',');; arr.push(','); } 
                    if(this.IdTokenSigningAlgValuesSupported) { arr.push('"id_token_signing_alg_values_supported": '); for (let i = 0; i < this.IdTokenSigningAlgValuesSupported.length; i++) arr.push(JSON.stringify(this.IdTokenSigningAlgValuesSupported[i])); arr.push(',');; arr.push(','); } 
                    if(this.EndSessionEndpoint) { arr.push('"end_session_endpoint": '); arr.push(JSON.stringify(this.EndSessionEndpoint)); arr.push(','); } 
                    if(this.ResponseTypesSupported) { arr.push('"response_types_supported": '); for (let i = 0; i < this.ResponseTypesSupported.length; i++) arr.push(JSON.stringify(this.ResponseTypesSupported[i])); arr.push(',');; arr.push(','); } 
                    if(this.ClaimsSupported) { arr.push('"claims_supported": '); for (let i = 0; i < this.ClaimsSupported.length; i++) arr.push(JSON.stringify(this.ClaimsSupported[i])); arr.push(',');; arr.push(','); } 
                    if(this.TokenEndpointAuthMethodsSupported) { arr.push('"token_endpoint_auth_methods_supported": '); for (let i = 0; i < this.TokenEndpointAuthMethodsSupported.length; i++) arr.push(JSON.stringify(this.TokenEndpointAuthMethodsSupported[i])); arr.push(',');; arr.push(','); } 
                    if(this.CodeChallengeMethodsSupported) { arr.push('"code_challenge_methods_supported": '); for (let i = 0; i < this.CodeChallengeMethodsSupported.length; i++) arr.push(JSON.stringify(this.CodeChallengeMethodsSupported[i])); arr.push(',');; arr.push(','); } 
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
             * Represents a key
             */
            export class OpenIDKey {
                constructor(obj?: any) {
                    for(let prop in obj) {
                        switch(prop) {
                            case "alg":
                                if (obj.alg) { this.Alg = obj.alg as string; }
                                break;
                            case "kty":
                                if (obj.kty) { this.Kty = obj.kty as string; }
                                break;
                            case "use":
                                if (obj.use) { this.Use = obj.use as string; }
                                break;
                            case "kid":
                                if (obj.kid) { this.Kid = obj.kid as string; }
                                break;
                            case "x5u":
                                if (obj.x5u) { this.X5u = obj.x5u as string; }
                                break;
                            case "x5t":
                                if (obj.x5t) { this.X5t = obj.x5t as string; }
                                break;
                            case "x5c":
                                if (obj.x5c) { this.X5c = Array.from(obj.x5c).map(o => o as string); }
                                break;
                            case "n":
                                if (obj.n) { this.N = obj.n as string; }
                                break;
                            case "e":
                                if (obj.e) { this.E = obj.e as string; }
                                break;
                            case "issuer":
                                if (obj.issuer) { this.Issuer = obj.issuer as string; }
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
                    if(this.X5c) { arr.push('"x5c": '); for (let i = 0; i < this.X5c.length; i++) arr.push(JSON.stringify(this.X5c[i])); arr.push(',');; arr.push(','); } 
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
             * Represents a set of keys
             */
            export class OpenIDKeys {
                constructor(obj?: any) {
                    for(let prop in obj) {
                        switch(prop) {
                            case "keys":
                                if (obj.keys) { this.Keys = Array.from(obj.keys).map(o => new OpenIDKey(o)); }
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
                    if(this.Keys) { arr.push('"keys": '); for (let i = 0; i < this.Keys.length; i++) if(this.Keys[i]) {this.Keys[i].toJson(arr)}; arr.push(',');; arr.push(','); } 
                    if(arr[arr.length-1] == ',') arr[arr.length-1] = '}'; else arr.push('}');
                    if(returnString) return arr.join("");
                    return null;
                }
                /**
                 * The keys
                 */
                Keys: OpenIDKey[] | null = null;
            }
            /**
             * The response from a token request
             */
            export class TokenResponse {
                constructor(obj?: any) {
                    for(let prop in obj) {
                        switch(prop) {
                            case "access_token":
                                if (obj.access_token) { this.AccessToken = obj.access_token as string; }
                                break;
                            case "token_type":
                                if (obj.token_type) { this.TokenType = obj.token_type as string; }
                                break;
                            case "expires_in":
                                if (obj.expires_in) { this.ExpiresIn = obj.expires_in as string; }
                                break;
                            case "refresh_token":
                                if (obj.refresh_token) { this.RefreshToken = obj.refresh_token as string; }
                                break;
                            case "scope":
                                if (obj.scope) { this.Scope = obj.scope as string; }
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
        }
        /**
         * Represents a file content
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
                            if (obj.LastModified) { this.LastModified = SolidRpcJs.ifnotnull<Date>(obj.LastModified, (notnull) => new Date(notnull)); }
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
             * The file content.
             */
            Content: Uint8Array | null = null;
            /**
             * The content charset
             */
            CharSet: string | null = null;
            /**
             * The content type.
             */
            ContentType: string | null = null;
            /**
             * The file name
             */
            FileName: string | null = null;
            /**
             * The last modified date of the resource.
             */
            LastModified: Date|null | null = null;
            /**
             * The location of the content
             */
            Location: string | null = null;
            /**
             * The ETag.
             */
            ETag: string | null = null;
        }
        /**
         * Represents a name/value pair
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
             * The name
             */
            Name: string | null = null;
            /**
             * The value
             */
            Value: string | null = null;
        }
        /**
         * Represents a solid rpc host.
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
             * The unique id of this host. This id is regenerated every time a 
             *             new memory context is created
             */
            HostId: string | null = null;
            /**
             * The time this host was started.
             */
            Started: Date | null = null;
            /**
             * The last time this host was alive. This field is set(and returned) when a client
             *             invokes the ISolidRpcHost.GetHostInstance.
             */
            LastAlive: Date | null = null;
            /**
             * The base address of this host
             */
            BaseAddress: string | null = null;
            /**
             * The cookie to set in order to reach this host
             */
            HttpCookies: Record<string,string> | null = null;
        }
    }
}
