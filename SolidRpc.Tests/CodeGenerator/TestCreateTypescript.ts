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
            export namespace ICodeNamespaceGenerator {
                let CreateCodeNamespaceSubject = new Subject<Types.Code.CodeNamespace>();
                /**
                 * This observable is hot and monitors all the responses from the CreateCodeNamespace invocations.
                 */
                export var CreateCodeNamespaceObservable = CreateCodeNamespaceSubject.asObservable().pipe(share());
                /**
                 * Creates a code namespace for supplied assembly name
                 * @param assemblyName 
                 * @param cancellationToken 
                 */
                export function CreateCodeNamespace(
                    assemblyName : string,
                    cancellationToken? : CancellationToken
                ): SolidRpcJs.RpcServiceRequestTyped<Types.Code.CodeNamespace> {
                    let ns = SolidRpcJs.rootNamespace.declareNamespace('SolidRpc.Abstractions.Services.Code.ICodeNamespaceGenerator');
                    let uri = ns.getStringValue('baseUrl','https://localhost/') + 'SolidRpc/Abstractions/Services/Code/ICodeNamespaceGenerator/CreateCodeNamespace/{assemblyName}';
                    SolidRpcJs.ifnull(assemblyName, () => { uri = uri.replace('{assemblyName}', ''); }, nn =>  { uri = uri.replace('{assemblyName}', SolidRpcJs.encodeUriValue(nn.toString())); });
                    let query: { [index: string]: any } = {};
                    let headers: { [index: string]: any } = {};
                    return new SolidRpcJs.RpcServiceRequestTyped<Types.Code.CodeNamespace>('get', uri, query, headers, null, cancellationToken, function(code : number, data : any) {
                        if(code == 200) {
                            return new Types.Code.CodeNamespace(data);
                        } else {
                            throw 'Response code != 200('+code+')';
                        }
                    }, CreateCodeNamespaceSubject);
                }
                }
            /**
             * The npm generator
             */
            export namespace INpmGenerator {
                let CreateNpmPackageSubject = new Subject<Types.Code.NpmPackage[]>();
                /**
                 * This observable is hot and monitors all the responses from the CreateNpmPackage invocations.
                 */
                export var CreateNpmPackageObservable = CreateNpmPackageSubject.asObservable().pipe(share());
                /**
                 * Returns the files that should be stored in the node_modules directory
                 * @param assemblyNames The name of the assemblies to create an npm package for.
                 * @param cancellationToken 
                 */
                export function CreateNpmPackage(
                    assemblyNames : string[],
                    cancellationToken? : CancellationToken
                ): SolidRpcJs.RpcServiceRequestTyped<Types.Code.NpmPackage[]> {
                    let ns = SolidRpcJs.rootNamespace.declareNamespace('SolidRpc.Abstractions.Services.Code.INpmGenerator');
                    let uri = ns.getStringValue('baseUrl','https://localhost/') + 'SolidRpc/Abstractions/Services/Code/INpmGenerator/CreateNpmPackage/{assemblyNames}';
                    SolidRpcJs.ifnull(assemblyNames, () => { uri = uri.replace('{assemblyNames}', ''); }, nn =>  { uri = uri.replace('{assemblyNames}', SolidRpcJs.encodeUriValue(nn.toString())); });
                    let query: { [index: string]: any } = {};
                    let headers: { [index: string]: any } = {};
                    return new SolidRpcJs.RpcServiceRequestTyped<Types.Code.NpmPackage[]>('get', uri, query, headers, null, cancellationToken, function(code : number, data : any) {
                        if(code == 200) {
                            return Array.from(data).map(o => new Types.Code.NpmPackage(o));
                        } else {
                            throw 'Response code != 200('+code+')';
                        }
                    }, CreateNpmPackageSubject);
                }
                let CreateInitialZipSubject = new Subject<Types.FileContent>();
                /**
                 * This observable is hot and monitors all the responses from the CreateInitialZip invocations.
                 */
                export var CreateInitialZipObservable = CreateInitialZipSubject.asObservable().pipe(share());
                /**
                 * Returns a zip containing the code to get started
                 * @param cancellationToken 
                 */
                export function CreateInitialZip(
                    cancellationToken? : CancellationToken
                ): SolidRpcJs.RpcServiceRequestTyped<Types.FileContent> {
                    let ns = SolidRpcJs.rootNamespace.declareNamespace('SolidRpc.Abstractions.Services.Code.INpmGenerator');
                    let uri = ns.getStringValue('baseUrl','https://localhost/') + 'SolidRpc/Abstractions/Services/Code/INpmGenerator/CreateInitialZip';
                    let query: { [index: string]: any } = {};
                    let headers: { [index: string]: any } = {};
                    return new SolidRpcJs.RpcServiceRequestTyped<Types.FileContent>('get', uri, query, headers, null, cancellationToken, function(code : number, data : any) {
                        if(code == 200) {
                            return new Types.FileContent(data);
                        } else {
                            throw 'Response code != 200('+code+')';
                        }
                    }, CreateInitialZipSubject);
                }
                }
            /**
             * instance responsible for generating code structures
             */
            export namespace ITypescriptGenerator {
                let CreateTypesTsForAssemblyAsyncSubject = new Subject<string>();
                /**
                 * This observable is hot and monitors all the responses from the CreateTypesTsForAssemblyAsync invocations.
                 */
                export var CreateTypesTsForAssemblyAsyncObservable = CreateTypesTsForAssemblyAsyncSubject.asObservable().pipe(share());
                /**
                 * Creates a types.ts file from supplied assembly name
                 * @param assemblyName 
                 * @param cancellationToken 
                 */
                export function CreateTypesTsForAssemblyAsync(
                    assemblyName : string,
                    cancellationToken? : CancellationToken
                ): SolidRpcJs.RpcServiceRequestTyped<string> {
                    let ns = SolidRpcJs.rootNamespace.declareNamespace('SolidRpc.Abstractions.Services.Code.ITypescriptGenerator');
                    let uri = ns.getStringValue('baseUrl','https://localhost/') + 'SolidRpc/Abstractions/Services/Code/ITypescriptGenerator/CreateTypesTsForAssemblyAsync/{assemblyName}';
                    SolidRpcJs.ifnull(assemblyName, () => { uri = uri.replace('{assemblyName}', ''); }, nn =>  { uri = uri.replace('{assemblyName}', SolidRpcJs.encodeUriValue(nn.toString())); });
                    let query: { [index: string]: any } = {};
                    let headers: { [index: string]: any } = {};
                    return new SolidRpcJs.RpcServiceRequestTyped<string>('get', uri, query, headers, null, cancellationToken, function(code : number, data : any) {
                        if(code == 200) {
                            return data as string;
                        } else {
                            throw 'Response code != 200('+code+')';
                        }
                    }, CreateTypesTsForAssemblyAsyncSubject);
                }
                let CreateTypesTsForCodeNamespaceAsyncSubject = new Subject<string>();
                /**
                 * This observable is hot and monitors all the responses from the CreateTypesTsForCodeNamespaceAsync invocations.
                 */
                export var CreateTypesTsForCodeNamespaceAsyncObservable = CreateTypesTsForCodeNamespaceAsyncSubject.asObservable().pipe(share());
                /**
                 * Creates a types.ts file from supplied code namespace
                 * @param codeNamespace 
                 * @param cancellationToken 
                 */
                export function CreateTypesTsForCodeNamespaceAsync(
                    codeNamespace : Types.Code.CodeNamespace,
                    cancellationToken? : CancellationToken
                ): SolidRpcJs.RpcServiceRequestTyped<string> {
                    let ns = SolidRpcJs.rootNamespace.declareNamespace('SolidRpc.Abstractions.Services.Code.ITypescriptGenerator');
                    let uri = ns.getStringValue('baseUrl','https://localhost/') + 'SolidRpc/Abstractions/Services/Code/ITypescriptGenerator/CreateTypesTsForCodeNamespaceAsync';
                    let query: { [index: string]: any } = {};
                    let headers: { [index: string]: any } = {};
                    headers['Content-Type']='application/json';
                    return new SolidRpcJs.RpcServiceRequestTyped<string>('post', uri, query, headers, SolidRpcJs.toJson(codeNamespace), cancellationToken, function(code : number, data : any) {
                        if(code == 200) {
                            return data as string;
                        } else {
                            throw 'Response code != 200('+code+')';
                        }
                    }, CreateTypesTsForCodeNamespaceAsyncSubject);
                }
                }
        }
        export namespace RateLimit {
            /**
             * Service that we can invoke to throttle requests.
             */
            export namespace ISolidRpcRateLimit {
                let GetRateLimitTokenAsyncSubject = new Subject<Types.RateLimit.RateLimitToken>();
                /**
                 * This observable is hot and monitors all the responses from the GetRateLimitTokenAsync invocations.
                 */
                export var GetRateLimitTokenAsyncObservable = GetRateLimitTokenAsyncSubject.asObservable().pipe(share());
                /**
                 * Returns the rate limit token.
                 * @param resourceName 
                 * @param timeout 
                 * @param cancellationToken 
                 */
                export function GetRateLimitTokenAsync(
                    resourceName : string,
                    timeout : Date,
                    cancellationToken? : CancellationToken
                ): SolidRpcJs.RpcServiceRequestTyped<Types.RateLimit.RateLimitToken> {
                    let ns = SolidRpcJs.rootNamespace.declareNamespace('SolidRpc.Abstractions.Services.RateLimit.ISolidRpcRateLimit');
                    let uri = ns.getStringValue('baseUrl','https://localhost/') + 'SolidRpc/Abstractions/Services/RateLimit/ISolidRpcRateLimit/GetRateLimitTokenAsync/{resourceName}/{timeout}';
                    SolidRpcJs.ifnull(resourceName, () => { uri = uri.replace('{resourceName}', ''); }, nn =>  { uri = uri.replace('{resourceName}', SolidRpcJs.encodeUriValue(nn.toString())); });
                    SolidRpcJs.ifnull(timeout, () => { uri = uri.replace('{timeout}', ''); }, nn =>  { uri = uri.replace('{timeout}', SolidRpcJs.encodeUriValue(nn.toString())); });
                    let query: { [index: string]: any } = {};
                    let headers: { [index: string]: any } = {};
                    return new SolidRpcJs.RpcServiceRequestTyped<Types.RateLimit.RateLimitToken>('get', uri, query, headers, null, cancellationToken, function(code : number, data : any) {
                        if(code == 200) {
                            return new Types.RateLimit.RateLimitToken(data);
                        } else {
                            throw 'Response code != 200('+code+')';
                        }
                    }, GetRateLimitTokenAsyncSubject);
                }
                let GetSingeltonTokenAsyncSubject = new Subject<Types.RateLimit.RateLimitToken>();
                /**
                 * This observable is hot and monitors all the responses from the GetSingeltonTokenAsync invocations.
                 */
                export var GetSingeltonTokenAsyncObservable = GetSingeltonTokenAsyncSubject.asObservable().pipe(share());
                /**
                 * Returns the singelton token for supplied key. This call implies a rate limit setting of max 1 concurrent call.
                 * @param resourceName 
                 * @param timeout 
                 * @param cancellationToken 
                 */
                export function GetSingeltonTokenAsync(
                    resourceName : string,
                    timeout : Date,
                    cancellationToken? : CancellationToken
                ): SolidRpcJs.RpcServiceRequestTyped<Types.RateLimit.RateLimitToken> {
                    let ns = SolidRpcJs.rootNamespace.declareNamespace('SolidRpc.Abstractions.Services.RateLimit.ISolidRpcRateLimit');
                    let uri = ns.getStringValue('baseUrl','https://localhost/') + 'SolidRpc/Abstractions/Services/RateLimit/ISolidRpcRateLimit/GetSingeltonTokenAsync/{resourceName}/{timeout}';
                    SolidRpcJs.ifnull(resourceName, () => { uri = uri.replace('{resourceName}', ''); }, nn =>  { uri = uri.replace('{resourceName}', SolidRpcJs.encodeUriValue(nn.toString())); });
                    SolidRpcJs.ifnull(timeout, () => { uri = uri.replace('{timeout}', ''); }, nn =>  { uri = uri.replace('{timeout}', SolidRpcJs.encodeUriValue(nn.toString())); });
                    let query: { [index: string]: any } = {};
                    let headers: { [index: string]: any } = {};
                    return new SolidRpcJs.RpcServiceRequestTyped<Types.RateLimit.RateLimitToken>('get', uri, query, headers, null, cancellationToken, function(code : number, data : any) {
                        if(code == 200) {
                            return new Types.RateLimit.RateLimitToken(data);
                        } else {
                            throw 'Response code != 200('+code+')';
                        }
                    }, GetSingeltonTokenAsyncSubject);
                }
                let ReturnRateLimitTokenAsyncSubject = new Subject<void>();
                /**
                 * This observable is hot and monitors all the responses from the ReturnRateLimitTokenAsync invocations.
                 */
                export var ReturnRateLimitTokenAsyncObservable = ReturnRateLimitTokenAsyncSubject.asObservable().pipe(share());
                /**
                 * Returns a rate limit token.
                 * @param rateLimitToken 
                 * @param cancellationToken 
                 */
                export function ReturnRateLimitTokenAsync(
                    rateLimitToken : Types.RateLimit.RateLimitToken,
                    cancellationToken? : CancellationToken
                ): SolidRpcJs.RpcServiceRequestTyped<void> {
                    let ns = SolidRpcJs.rootNamespace.declareNamespace('SolidRpc.Abstractions.Services.RateLimit.ISolidRpcRateLimit');
                    let uri = ns.getStringValue('baseUrl','https://localhost/') + 'SolidRpc/Abstractions/Services/RateLimit/ISolidRpcRateLimit/ReturnRateLimitTokenAsync';
                    let query: { [index: string]: any } = {};
                    let headers: { [index: string]: any } = {};
                    headers['Content-Type']='application/json';
                    return new SolidRpcJs.RpcServiceRequestTyped<void>('post', uri, query, headers, SolidRpcJs.toJson(rateLimitToken), cancellationToken, function(code : number, data : any) {
                        if(code == 200) {
                            return null;
                        } else {
                            throw 'Response code != 200('+code+')';
                        }
                    }, ReturnRateLimitTokenAsyncSubject);
                }
                let GetRateLimitSettingsAsyncSubject = new Subject<Types.RateLimit.RateLimitSetting[]>();
                /**
                 * This observable is hot and monitors all the responses from the GetRateLimitSettingsAsync invocations.
                 */
                export var GetRateLimitSettingsAsyncObservable = GetRateLimitSettingsAsyncSubject.asObservable().pipe(share());
                /**
                 * Returns the rate limit settings
                 * @param cancellationToken 
                 */
                export function GetRateLimitSettingsAsync(
                    cancellationToken? : CancellationToken
                ): SolidRpcJs.RpcServiceRequestTyped<Types.RateLimit.RateLimitSetting[]> {
                    let ns = SolidRpcJs.rootNamespace.declareNamespace('SolidRpc.Abstractions.Services.RateLimit.ISolidRpcRateLimit');
                    let uri = ns.getStringValue('baseUrl','https://localhost/') + 'SolidRpc/Abstractions/Services/RateLimit/ISolidRpcRateLimit/GetRateLimitSettingsAsync';
                    let query: { [index: string]: any } = {};
                    let headers: { [index: string]: any } = {};
                    return new SolidRpcJs.RpcServiceRequestTyped<Types.RateLimit.RateLimitSetting[]>('get', uri, query, headers, null, cancellationToken, function(code : number, data : any) {
                        if(code == 200) {
                            return Array.from(data).map(o => new Types.RateLimit.RateLimitSetting(o));
                        } else {
                            throw 'Response code != 200('+code+')';
                        }
                    }, GetRateLimitSettingsAsyncSubject);
                }
                let UpdateRateLimitSettingSubject = new Subject<void>();
                /**
                 * This observable is hot and monitors all the responses from the UpdateRateLimitSetting invocations.
                 */
                export var UpdateRateLimitSettingObservable = UpdateRateLimitSettingSubject.asObservable().pipe(share());
                /**
                 * Updates the rate limit settings
                 * @param setting 
                 * @param cancellationToken 
                 */
                export function UpdateRateLimitSetting(
                    setting : Types.RateLimit.RateLimitSetting,
                    cancellationToken? : CancellationToken
                ): SolidRpcJs.RpcServiceRequestTyped<void> {
                    let ns = SolidRpcJs.rootNamespace.declareNamespace('SolidRpc.Abstractions.Services.RateLimit.ISolidRpcRateLimit');
                    let uri = ns.getStringValue('baseUrl','https://localhost/') + 'SolidRpc/Abstractions/Services/RateLimit/ISolidRpcRateLimit/UpdateRateLimitSetting';
                    let query: { [index: string]: any } = {};
                    let headers: { [index: string]: any } = {};
                    headers['Content-Type']='application/json';
                    return new SolidRpcJs.RpcServiceRequestTyped<void>('post', uri, query, headers, SolidRpcJs.toJson(setting), cancellationToken, function(code : number, data : any) {
                        if(code == 200) {
                            return null;
                        } else {
                            throw 'Response code != 200('+code+')';
                        }
                    }, UpdateRateLimitSettingSubject);
                }
                }
        }
        /**
         * Provides logic for the acme challange
         */
        export namespace ISolidRpcAcmeChallenge {
            let SetAcmeChallengeAsyncSubject = new Subject<void>();
            /**
             * This observable is hot and monitors all the responses from the SetAcmeChallengeAsync invocations.
             */
            export var SetAcmeChallengeAsyncObservable = SetAcmeChallengeAsyncSubject.asObservable().pipe(share());
            /**
             * Sets the acme challenge. The part before the . is the filename and all of the 
             *             supplied challenge is provided in the file.
             * @param challenge 
             * @param cancellation 
             */
            export function SetAcmeChallengeAsync(
                challenge : string,
                cancellation? : CancellationToken
            ): SolidRpcJs.RpcServiceRequestTyped<void> {
                let ns = SolidRpcJs.rootNamespace.declareNamespace('SolidRpc.Abstractions.Services.ISolidRpcAcmeChallenge');
                let uri = ns.getStringValue('baseUrl','https://localhost/') + 'SolidRpc/Abstractions/Services/ISolidRpcAcmeChallenge/SetAcmeChallengeAsync/{challenge}';
                SolidRpcJs.ifnull(challenge, () => { uri = uri.replace('{challenge}', ''); }, nn =>  { uri = uri.replace('{challenge}', SolidRpcJs.encodeUriValue(nn.toString())); });
                let query: { [index: string]: any } = {};
                let headers: { [index: string]: any } = {};
                return new SolidRpcJs.RpcServiceRequestTyped<void>('get', uri, query, headers, null, cancellation, function(code : number, data : any) {
                    if(code == 200) {
                        return null;
                    } else {
                        throw 'Response code != 200('+code+')';
                    }
                }, SetAcmeChallengeAsyncSubject);
            }
            let GetAcmeChallengeAsyncSubject = new Subject<Types.FileContent>();
            /**
             * This observable is hot and monitors all the responses from the GetAcmeChallengeAsync invocations.
             */
            export var GetAcmeChallengeAsyncObservable = GetAcmeChallengeAsyncSubject.asObservable().pipe(share());
            /**
             * Returns the acme challenge
             * @param key 
             * @param cancellation 
             */
            export function GetAcmeChallengeAsync(
                key : string,
                cancellation? : CancellationToken
            ): SolidRpcJs.RpcServiceRequestTyped<Types.FileContent> {
                let ns = SolidRpcJs.rootNamespace.declareNamespace('SolidRpc.Abstractions.Services.ISolidRpcAcmeChallenge');
                let uri = ns.getStringValue('baseUrl','https://localhost/') + 'SolidRpc/Abstractions/.well-known/acme-challenge/{key}';
                SolidRpcJs.ifnull(key, () => { uri = uri.replace('{key}', ''); }, nn =>  { uri = uri.replace('{key}', SolidRpcJs.encodeUriValue(nn.toString())); });
                let query: { [index: string]: any } = {};
                let headers: { [index: string]: any } = {};
                return new SolidRpcJs.RpcServiceRequestTyped<Types.FileContent>('get', uri, query, headers, null, cancellation, function(code : number, data : any) {
                    if(code == 200) {
                        return new Types.FileContent(data);
                    } else {
                        throw 'Response code != 200('+code+')';
                    }
                }, GetAcmeChallengeAsyncSubject);
            }
            }
        /**
         * The content handler uses the ISolidRpcContentStore to deliver static or proxied content.
         *             
         *             This handler can be invoked from a configured proxy or mapped directly in a .Net Core Handler.
         */
        export namespace ISolidRpcContentHandler {
            let GetContentSubject = new Subject<Types.FileContent>();
            /**
             * This observable is hot and monitors all the responses from the GetContent invocations.
             */
            export var GetContentObservable = GetContentSubject.asObservable().pipe(share());
            /**
             * Returns the content for supplied path.
             *             
             *             Note that the path is marked as optional(default value set). This is so that the parameter
             *             is placed in the query string instead of path.
             * @param path The path to get the content for
             * @param cancellationToken 
             */
            export function GetContent(
                path? : string,
                cancellationToken? : CancellationToken
            ): SolidRpcJs.RpcServiceRequestTyped<Types.FileContent> {
                let ns = SolidRpcJs.rootNamespace.declareNamespace('SolidRpc.Abstractions.Services.ISolidRpcContentHandler');
                let uri = ns.getStringValue('baseUrl','https://localhost/') + 'SolidRpc/Abstractions/Services/ISolidRpcContentHandler/GetContent';
                let query: { [index: string]: any } = {};
                SolidRpcJs.ifnotnull(path, x => { query['path'] = x; });
                let headers: { [index: string]: any } = {};
                return new SolidRpcJs.RpcServiceRequestTyped<Types.FileContent>('get', uri, query, headers, null, cancellationToken, function(code : number, data : any) {
                    if(code == 200) {
                        return new Types.FileContent(data);
                    } else {
                        throw 'Response code != 200('+code+')';
                    }
                }, GetContentSubject);
            }
            let GetPathMappingsAsyncSubject = new Subject<Types.NameValuePair[]>();
            /**
             * This observable is hot and monitors all the responses from the GetPathMappingsAsync invocations.
             */
            export var GetPathMappingsAsyncObservable = GetPathMappingsAsyncSubject.asObservable().pipe(share());
            /**
             * Returns all the path mappings.
             * @param redirects 
             * @param cancellationToken 
             */
            export function GetPathMappingsAsync(
                redirects : boolean,
                cancellationToken? : CancellationToken
            ): SolidRpcJs.RpcServiceRequestTyped<Types.NameValuePair[]> {
                let ns = SolidRpcJs.rootNamespace.declareNamespace('SolidRpc.Abstractions.Services.ISolidRpcContentHandler');
                let uri = ns.getStringValue('baseUrl','https://localhost/') + 'SolidRpc/Abstractions/Services/ISolidRpcContentHandler/GetPathMappingsAsync/{redirects}';
                SolidRpcJs.ifnull(redirects, () => { uri = uri.replace('{redirects}', ''); }, nn =>  { uri = uri.replace('{redirects}', SolidRpcJs.encodeUriValue(nn.toString())); });
                let query: { [index: string]: any } = {};
                let headers: { [index: string]: any } = {};
                return new SolidRpcJs.RpcServiceRequestTyped<Types.NameValuePair[]>('get', uri, query, headers, null, cancellationToken, function(code : number, data : any) {
                    if(code == 200) {
                        return Array.from(data).map(o => new Types.NameValuePair(o));
                    } else {
                        throw 'Response code != 200('+code+')';
                    }
                }, GetPathMappingsAsyncSubject);
            }
            let GetProtectedContentAsyncSubject = new Subject<Types.FileContent>();
            /**
             * This observable is hot and monitors all the responses from the GetProtectedContentAsync invocations.
             */
            export var GetProtectedContentAsyncObservable = GetProtectedContentAsyncSubject.asObservable().pipe(share());
            /**
             * Returns the protected content for supplied resource
             * @param resource 
             * @param cancellationToken 
             */
            export function GetProtectedContentAsync(
                resource : Uint8Array,
                cancellationToken? : CancellationToken
            ): SolidRpcJs.RpcServiceRequestTyped<Types.FileContent> {
                let ns = SolidRpcJs.rootNamespace.declareNamespace('SolidRpc.Abstractions.Services.ISolidRpcContentHandler');
                let uri = ns.getStringValue('baseUrl','https://localhost/') + 'SolidRpc/Abstractions/Services/ISolidRpcContentHandler/GetProtectedContentAsync/{resource}';
                SolidRpcJs.ifnull(resource, () => { uri = uri.replace('{resource}', ''); }, nn =>  { uri = uri.replace('{resource}', SolidRpcJs.encodeUriValue(nn.toString())); });
                let query: { [index: string]: any } = {};
                let headers: { [index: string]: any } = {};
                return new SolidRpcJs.RpcServiceRequestTyped<Types.FileContent>('get', uri, query, headers, null, cancellationToken, function(code : number, data : any) {
                    if(code == 200) {
                        return new Types.FileContent(data);
                    } else {
                        throw 'Response code != 200('+code+')';
                    }
                }, GetProtectedContentAsyncSubject);
            }
            }
        /**
         * Represents a solid rpc host.
         */
        export namespace ISolidRpcHost {
            let GetHostIdSubject = new Subject<string>();
            /**
             * This observable is hot and monitors all the responses from the GetHostId invocations.
             */
            export var GetHostIdObservable = GetHostIdSubject.asObservable().pipe(share());
            /**
             * Returns the id of this host
             * @param cancellationToken 
             */
            export function GetHostId(
                cancellationToken? : CancellationToken
            ): SolidRpcJs.RpcServiceRequestTyped<string> {
                let ns = SolidRpcJs.rootNamespace.declareNamespace('SolidRpc.Abstractions.Services.ISolidRpcHost');
                let uri = ns.getStringValue('baseUrl','https://localhost/') + 'SolidRpc/Abstractions/Services/ISolidRpcHost/GetHostId';
                let query: { [index: string]: any } = {};
                let headers: { [index: string]: any } = {};
                return new SolidRpcJs.RpcServiceRequestTyped<string>('get', uri, query, headers, null, cancellationToken, function(code : number, data : any) {
                    if(code == 200) {
                        return data as string;
                    } else {
                        throw 'Response code != 200('+code+')';
                    }
                }, GetHostIdSubject);
            }
            let GetHostInstanceSubject = new Subject<Types.SolidRpcHostInstance>();
            /**
             * This observable is hot and monitors all the responses from the GetHostInstance invocations.
             */
            export var GetHostInstanceObservable = GetHostInstanceSubject.asObservable().pipe(share());
            /**
             * Returns the id of this host. This method can be used to determine if a host is up and running by
             *             comparing the returned value with the instance that we want to send to. If a host goes down it is 
             *             removed from the router and another instance probably responds to the call.
             * @param cancellationToken 
             */
            export function GetHostInstance(
                cancellationToken? : CancellationToken
            ): SolidRpcJs.RpcServiceRequestTyped<Types.SolidRpcHostInstance> {
                let ns = SolidRpcJs.rootNamespace.declareNamespace('SolidRpc.Abstractions.Services.ISolidRpcHost');
                let uri = ns.getStringValue('baseUrl','https://localhost/') + 'SolidRpc/Abstractions/Services/ISolidRpcHost/GetHostInstance';
                let query: { [index: string]: any } = {};
                let headers: { [index: string]: any } = {};
                return new SolidRpcJs.RpcServiceRequestTyped<Types.SolidRpcHostInstance>('get', uri, query, headers, null, cancellationToken, function(code : number, data : any) {
                    if(code == 200) {
                        return new Types.SolidRpcHostInstance(data);
                    } else {
                        throw 'Response code != 200('+code+')';
                    }
                }, GetHostInstanceSubject);
            }
            let SyncHostsFromStoreSubject = new Subject<Types.SolidRpcHostInstance[]>();
            /**
             * This observable is hot and monitors all the responses from the SyncHostsFromStore invocations.
             */
            export var SyncHostsFromStoreObservable = SyncHostsFromStoreSubject.asObservable().pipe(share());
            /**
             * This method is invoked on all the hosts in a store when a new host is available.
             * @param cancellationToken 
             */
            export function SyncHostsFromStore(
                cancellationToken? : CancellationToken
            ): SolidRpcJs.RpcServiceRequestTyped<Types.SolidRpcHostInstance[]> {
                let ns = SolidRpcJs.rootNamespace.declareNamespace('SolidRpc.Abstractions.Services.ISolidRpcHost');
                let uri = ns.getStringValue('baseUrl','https://localhost/') + 'SolidRpc/Abstractions/Services/ISolidRpcHost/SyncHostsFromStore';
                let query: { [index: string]: any } = {};
                let headers: { [index: string]: any } = {};
                return new SolidRpcJs.RpcServiceRequestTyped<Types.SolidRpcHostInstance[]>('get', uri, query, headers, null, cancellationToken, function(code : number, data : any) {
                    if(code == 200) {
                        return Array.from(data).map(o => new Types.SolidRpcHostInstance(o));
                    } else {
                        throw 'Response code != 200('+code+')';
                    }
                }, SyncHostsFromStoreSubject);
            }
            let CheckHostSubject = new Subject<Types.SolidRpcHostInstance>();
            /**
             * This observable is hot and monitors all the responses from the CheckHost invocations.
             */
            export var CheckHostObservable = CheckHostSubject.asObservable().pipe(share());
            /**
             * Invokes the "GetHostInstance" targeted for supplied instance and resturns the result
             * @param hostInstance 
             * @param cancellationToken 
             */
            export function CheckHost(
                hostInstance : Types.SolidRpcHostInstance,
                cancellationToken? : CancellationToken
            ): SolidRpcJs.RpcServiceRequestTyped<Types.SolidRpcHostInstance> {
                let ns = SolidRpcJs.rootNamespace.declareNamespace('SolidRpc.Abstractions.Services.ISolidRpcHost');
                let uri = ns.getStringValue('baseUrl','https://localhost/') + 'SolidRpc/Abstractions/Services/ISolidRpcHost/CheckHost';
                let query: { [index: string]: any } = {};
                let headers: { [index: string]: any } = {};
                headers['Content-Type']='application/json';
                return new SolidRpcJs.RpcServiceRequestTyped<Types.SolidRpcHostInstance>('post', uri, query, headers, SolidRpcJs.toJson(hostInstance), cancellationToken, function(code : number, data : any) {
                    if(code == 200) {
                        return new Types.SolidRpcHostInstance(data);
                    } else {
                        throw 'Response code != 200('+code+')';
                    }
                }, CheckHostSubject);
            }
            let GetHostConfigurationSubject = new Subject<Types.NameValuePair[]>();
            /**
             * This observable is hot and monitors all the responses from the GetHostConfiguration invocations.
             */
            export var GetHostConfigurationObservable = GetHostConfigurationSubject.asObservable().pipe(share());
            /**
             * Returns the host configuration.
             * @param cancellationToken 
             */
            export function GetHostConfiguration(
                cancellationToken? : CancellationToken
            ): SolidRpcJs.RpcServiceRequestTyped<Types.NameValuePair[]> {
                let ns = SolidRpcJs.rootNamespace.declareNamespace('SolidRpc.Abstractions.Services.ISolidRpcHost');
                let uri = ns.getStringValue('baseUrl','https://localhost/') + 'SolidRpc/Abstractions/Services/ISolidRpcHost/GetHostConfiguration';
                let query: { [index: string]: any } = {};
                let headers: { [index: string]: any } = {};
                return new SolidRpcJs.RpcServiceRequestTyped<Types.NameValuePair[]>('get', uri, query, headers, null, cancellationToken, function(code : number, data : any) {
                    if(code == 200) {
                        return Array.from(data).map(o => new Types.NameValuePair(o));
                    } else {
                        throw 'Response code != 200('+code+')';
                    }
                }, GetHostConfigurationSubject);
            }
            let IsAliveSubject = new Subject<void>();
            /**
             * This observable is hot and monitors all the responses from the IsAlive invocations.
             */
            export var IsAliveObservable = IsAliveSubject.asObservable().pipe(share());
            /**
             * Function that determines if the host is alive.
             * @param cancellationToken 
             */
            export function IsAlive(
                cancellationToken? : CancellationToken
            ): SolidRpcJs.RpcServiceRequestTyped<void> {
                let ns = SolidRpcJs.rootNamespace.declareNamespace('SolidRpc.Abstractions.Services.ISolidRpcHost');
                let uri = ns.getStringValue('baseUrl','https://localhost/') + 'SolidRpc/Abstractions/Services/ISolidRpcHost/IsAlive';
                let query: { [index: string]: any } = {};
                let headers: { [index: string]: any } = {};
                return new SolidRpcJs.RpcServiceRequestTyped<void>('get', uri, query, headers, null, cancellationToken, function(code : number, data : any) {
                    if(code == 200) {
                        return null;
                    } else {
                        throw 'Response code != 200('+code+')';
                    }
                }, IsAliveSubject);
            }
            let BaseAddressSubject = new Subject<string>();
            /**
             * This observable is hot and monitors all the responses from the BaseAddress invocations.
             */
            export var BaseAddressObservable = BaseAddressSubject.asObservable().pipe(share());
            /**
             * Returns the base url for this host
             * @param cancellationToken 
             */
            export function BaseAddress(
                cancellationToken? : CancellationToken
            ): SolidRpcJs.RpcServiceRequestTyped<string> {
                let ns = SolidRpcJs.rootNamespace.declareNamespace('SolidRpc.Abstractions.Services.ISolidRpcHost');
                let uri = ns.getStringValue('baseUrl','https://localhost/') + 'SolidRpc/Abstractions/Services/ISolidRpcHost/BaseAddress';
                let query: { [index: string]: any } = {};
                let headers: { [index: string]: any } = {};
                return new SolidRpcJs.RpcServiceRequestTyped<string>('get', uri, query, headers, null, cancellationToken, function(code : number, data : any) {
                    if(code == 200) {
                        return data as string;
                    } else {
                        throw 'Response code != 200('+code+')';
                    }
                }, BaseAddressSubject);
            }
            let AllowedCorsOriginsSubject = new Subject<string[]>();
            /**
             * This observable is hot and monitors all the responses from the AllowedCorsOrigins invocations.
             */
            export var AllowedCorsOriginsObservable = AllowedCorsOriginsSubject.asObservable().pipe(share());
            /**
             * Returns the list of allowed cors origins.
             * @param cancellationToken 
             */
            export function AllowedCorsOrigins(
                cancellationToken? : CancellationToken
            ): SolidRpcJs.RpcServiceRequestTyped<string[]> {
                let ns = SolidRpcJs.rootNamespace.declareNamespace('SolidRpc.Abstractions.Services.ISolidRpcHost');
                let uri = ns.getStringValue('baseUrl','https://localhost/') + 'SolidRpc/Abstractions/Services/ISolidRpcHost/AllowedCorsOrigins';
                let query: { [index: string]: any } = {};
                let headers: { [index: string]: any } = {};
                return new SolidRpcJs.RpcServiceRequestTyped<string[]>('get', uri, query, headers, null, cancellationToken, function(code : number, data : any) {
                    if(code == 200) {
                        return Array.from(data).map(o => o as string);
                    } else {
                        throw 'Response code != 200('+code+')';
                    }
                }, AllowedCorsOriginsSubject);
            }
            }
        /**
         * The host store is responsible for storing persistent information about 
         *             a host in a cluster. Usually hosts are placed behind a load balancer that
         *             can route to a specific host based on some cookie or header information.
         */
        export namespace ISolidRpcHostStore {
            let AddHostInstanceAsyncSubject = new Subject<void>();
            /**
             * This observable is hot and monitors all the responses from the AddHostInstanceAsync invocations.
             */
            export var AddHostInstanceAsyncObservable = AddHostInstanceAsyncSubject.asObservable().pipe(share());
            /**
             * Adds a host instance to the host store.
             * @param hostInstance 
             * @param cancellationToken 
             */
            export function AddHostInstanceAsync(
                hostInstance : Types.SolidRpcHostInstance,
                cancellationToken? : CancellationToken
            ): SolidRpcJs.RpcServiceRequestTyped<void> {
                let ns = SolidRpcJs.rootNamespace.declareNamespace('SolidRpc.Abstractions.Services.ISolidRpcHostStore');
                let uri = ns.getStringValue('baseUrl','https://localhost/') + 'SolidRpc/Abstractions/Services/ISolidRpcHostStore/AddHostInstanceAsync';
                let query: { [index: string]: any } = {};
                let headers: { [index: string]: any } = {};
                headers['Content-Type']='application/json';
                return new SolidRpcJs.RpcServiceRequestTyped<void>('post', uri, query, headers, SolidRpcJs.toJson(hostInstance), cancellationToken, function(code : number, data : any) {
                    if(code == 200) {
                        return null;
                    } else {
                        throw 'Response code != 200('+code+')';
                    }
                }, AddHostInstanceAsyncSubject);
            }
            let RemoveHostInstanceAsyncSubject = new Subject<void>();
            /**
             * This observable is hot and monitors all the responses from the RemoveHostInstanceAsync invocations.
             */
            export var RemoveHostInstanceAsyncObservable = RemoveHostInstanceAsyncSubject.asObservable().pipe(share());
            /**
             * Removes a host instance from the store.
             * @param hostInstance 
             * @param cancellationToken 
             */
            export function RemoveHostInstanceAsync(
                hostInstance : Types.SolidRpcHostInstance,
                cancellationToken? : CancellationToken
            ): SolidRpcJs.RpcServiceRequestTyped<void> {
                let ns = SolidRpcJs.rootNamespace.declareNamespace('SolidRpc.Abstractions.Services.ISolidRpcHostStore');
                let uri = ns.getStringValue('baseUrl','https://localhost/') + 'SolidRpc/Abstractions/Services/ISolidRpcHostStore/RemoveHostInstanceAsync';
                let query: { [index: string]: any } = {};
                let headers: { [index: string]: any } = {};
                headers['Content-Type']='application/json';
                return new SolidRpcJs.RpcServiceRequestTyped<void>('post', uri, query, headers, SolidRpcJs.toJson(hostInstance), cancellationToken, function(code : number, data : any) {
                    if(code == 200) {
                        return null;
                    } else {
                        throw 'Response code != 200('+code+')';
                    }
                }, RemoveHostInstanceAsyncSubject);
            }
            let ListHostInstancesAsyncSubject = new Subject<Types.SolidRpcHostInstance[]>();
            /**
             * This observable is hot and monitors all the responses from the ListHostInstancesAsync invocations.
             */
            export var ListHostInstancesAsyncObservable = ListHostInstancesAsyncSubject.asObservable().pipe(share());
            /**
             * Lists the stored host instances
             * @param cancellationToken 
             */
            export function ListHostInstancesAsync(
                cancellationToken? : CancellationToken
            ): SolidRpcJs.RpcServiceRequestTyped<Types.SolidRpcHostInstance[]> {
                let ns = SolidRpcJs.rootNamespace.declareNamespace('SolidRpc.Abstractions.Services.ISolidRpcHostStore');
                let uri = ns.getStringValue('baseUrl','https://localhost/') + 'SolidRpc/Abstractions/Services/ISolidRpcHostStore/ListHostInstancesAsync';
                let query: { [index: string]: any } = {};
                let headers: { [index: string]: any } = {};
                return new SolidRpcJs.RpcServiceRequestTyped<Types.SolidRpcHostInstance[]>('get', uri, query, headers, null, cancellationToken, function(code : number, data : any) {
                    if(code == 200) {
                        return Array.from(data).map(o => new Types.SolidRpcHostInstance(o));
                    } else {
                        throw 'Response code != 200('+code+')';
                    }
                }, ListHostInstancesAsyncSubject);
            }
            }
        /**
         * Interfaces that defines the logic for OAuth2 support.
         */
        export namespace ISolidRpcOAuth2 {
            let GetAuthorizationCodeTokenAsyncSubject = new Subject<Types.FileContent>();
            /**
             * This observable is hot and monitors all the responses from the GetAuthorizationCodeTokenAsync invocations.
             */
            export var GetAuthorizationCodeTokenAsyncObservable = GetAuthorizationCodeTokenAsyncSubject.asObservable().pipe(share());
            /**
             * This method returns a html page that authorizes the user. When the user
             *             has been authorized the supplied callback is invoked with the "access_token"
             *             supplied as a query parameter along with the "state".
             *             
             *             Use this method to retreive tokens from a standalone node instance or from a SPA(single page app)
             *             
             *             Start a local http server and supply the address to the handler.
             * @param callbackUri the callback
             * @param state the state
             * @param scopes the scopes
             * @param cancellationToken 
             */
            export function GetAuthorizationCodeTokenAsync(
                callbackUri? : string,
                state? : string,
                scopes? : string[],
                cancellationToken? : CancellationToken
            ): SolidRpcJs.RpcServiceRequestTyped<Types.FileContent> {
                let ns = SolidRpcJs.rootNamespace.declareNamespace('SolidRpc.Abstractions.Services.ISolidRpcOAuth2');
                let uri = ns.getStringValue('baseUrl','https://localhost/') + 'SolidRpc/Abstractions/Services/ISolidRpcOAuth2/GetAuthorizationCodeTokenAsync';
                let query: { [index: string]: any } = {};
                SolidRpcJs.ifnotnull(callbackUri, x => { query['callbackUri'] = x; });
                SolidRpcJs.ifnotnull(state, x => { query['state'] = x; });
                SolidRpcJs.ifnotnull(scopes, x => { query['scopes'] = x; });
                let headers: { [index: string]: any } = {};
                return new SolidRpcJs.RpcServiceRequestTyped<Types.FileContent>('get', uri, query, headers, null, cancellationToken, function(code : number, data : any) {
                    if(code == 200) {
                        return new Types.FileContent(data);
                    } else {
                        throw 'Response code != 200('+code+')';
                    }
                }, GetAuthorizationCodeTokenAsyncSubject);
            }
            let TokenCallbackAsyncSubject = new Subject<Types.FileContent>();
            /**
             * This observable is hot and monitors all the responses from the TokenCallbackAsync invocations.
             */
            export var TokenCallbackAsyncObservable = TokenCallbackAsyncSubject.asObservable().pipe(share());
            /**
             * This is the method that is invoked when a user has been authenticated
             *             and a valid token is supplied. The authentication uses the "authorization code" flow
             *             so this method retreives the access token using supplied code.
             * @param code code token to use
             * @param state the state
             * @param cancellation 
             */
            export function TokenCallbackAsync(
                code? : string,
                state? : string,
                cancellation? : CancellationToken
            ): SolidRpcJs.RpcServiceRequestTyped<Types.FileContent> {
                let ns = SolidRpcJs.rootNamespace.declareNamespace('SolidRpc.Abstractions.Services.ISolidRpcOAuth2');
                let uri = ns.getStringValue('baseUrl','https://localhost/') + 'SolidRpc/Abstractions/Services/ISolidRpcOAuth2/TokenCallbackAsync';
                let query: { [index: string]: any } = {};
                SolidRpcJs.ifnotnull(code, x => { query['code'] = x; });
                SolidRpcJs.ifnotnull(state, x => { query['state'] = x; });
                let headers: { [index: string]: any } = {};
                return new SolidRpcJs.RpcServiceRequestTyped<Types.FileContent>('get', uri, query, headers, null, cancellation, function(code : number, data : any) {
                    if(code == 200) {
                        return new Types.FileContent(data);
                    } else {
                        throw 'Response code != 200('+code+')';
                    }
                }, TokenCallbackAsyncSubject);
            }
            let RefreshTokenAsyncSubject = new Subject<Types.FileContent>();
            /**
             * This observable is hot and monitors all the responses from the RefreshTokenAsync invocations.
             */
            export var RefreshTokenAsyncObservable = RefreshTokenAsyncSubject.asObservable().pipe(share());
            /**
             * Use this method to refresh a token obtained from the callback.
             *             
             *             This method fetches a new token from the OAuth server using the refresh token stored as a cookie when authorizing for the first time.
             * @param accessToken the token to refresh - may be an expired token
             * @param cancellation 
             */
            export function RefreshTokenAsync(
                accessToken : string,
                cancellation? : CancellationToken
            ): SolidRpcJs.RpcServiceRequestTyped<Types.FileContent> {
                let ns = SolidRpcJs.rootNamespace.declareNamespace('SolidRpc.Abstractions.Services.ISolidRpcOAuth2');
                let uri = ns.getStringValue('baseUrl','https://localhost/') + 'SolidRpc/Abstractions/Services/ISolidRpcOAuth2/RefreshTokenAsync/{accessToken}';
                SolidRpcJs.ifnull(accessToken, () => { uri = uri.replace('{accessToken}', ''); }, nn =>  { uri = uri.replace('{accessToken}', SolidRpcJs.encodeUriValue(nn.toString())); });
                let query: { [index: string]: any } = {};
                let headers: { [index: string]: any } = {};
                return new SolidRpcJs.RpcServiceRequestTyped<Types.FileContent>('get', uri, query, headers, null, cancellation, function(code : number, data : any) {
                    if(code == 200) {
                        return new Types.FileContent(data);
                    } else {
                        throw 'Response code != 200('+code+')';
                    }
                }, RefreshTokenAsyncSubject);
            }
            let LogoutAsyncSubject = new Subject<Types.FileContent>();
            /**
             * This observable is hot and monitors all the responses from the LogoutAsync invocations.
             */
            export var LogoutAsyncObservable = LogoutAsyncSubject.asObservable().pipe(share());
            /**
             * Performs the logout @ the identity server.
             * @param callbackUri 
             * @param cancellationToken 
             */
            export function LogoutAsync(
                callbackUri? : string,
                cancellationToken? : CancellationToken
            ): SolidRpcJs.RpcServiceRequestTyped<Types.FileContent> {
                let ns = SolidRpcJs.rootNamespace.declareNamespace('SolidRpc.Abstractions.Services.ISolidRpcOAuth2');
                let uri = ns.getStringValue('baseUrl','https://localhost/') + 'SolidRpc/Abstractions/Services/ISolidRpcOAuth2/LogoutAsync';
                let query: { [index: string]: any } = {};
                SolidRpcJs.ifnotnull(callbackUri, x => { query['callbackUri'] = x; });
                let headers: { [index: string]: any } = {};
                return new SolidRpcJs.RpcServiceRequestTyped<Types.FileContent>('get', uri, query, headers, null, cancellationToken, function(code : number, data : any) {
                    if(code == 200) {
                        return new Types.FileContent(data);
                    } else {
                        throw 'Response code != 200('+code+')';
                    }
                }, LogoutAsyncSubject);
            }
            }
        /**
         * Implements logic for the oidc server
         */
        export namespace ISolidRpcOidc {
            let GetDiscoveryDocumentAsyncSubject = new Subject<Types.OAuth2.OpenIDConnectDiscovery>();
            /**
             * This observable is hot and monitors all the responses from the GetDiscoveryDocumentAsync invocations.
             */
            export var GetDiscoveryDocumentAsyncObservable = GetDiscoveryDocumentAsyncSubject.asObservable().pipe(share());
            /**
             * Returns the /.well-known/openid-configuration file
             * @param cancellationToken 
             */
            export function GetDiscoveryDocumentAsync(
                cancellationToken? : CancellationToken
            ): SolidRpcJs.RpcServiceRequestTyped<Types.OAuth2.OpenIDConnectDiscovery> {
                let ns = SolidRpcJs.rootNamespace.declareNamespace('SolidRpc.Abstractions.Services.ISolidRpcOidc');
                let uri = ns.getStringValue('baseUrl','https://localhost/') + 'SolidRpc/Abstractions/.well-known/openid-configuration';
                let query: { [index: string]: any } = {};
                let headers: { [index: string]: any } = {};
                return new SolidRpcJs.RpcServiceRequestTyped<Types.OAuth2.OpenIDConnectDiscovery>('get', uri, query, headers, null, cancellationToken, function(code : number, data : any) {
                    if(code == 200) {
                        return new Types.OAuth2.OpenIDConnectDiscovery(data);
                    } else {
                        throw 'Response code != 200('+code+')';
                    }
                }, GetDiscoveryDocumentAsyncSubject);
            }
            let GetKeysAsyncSubject = new Subject<Types.OAuth2.OpenIDKeys>();
            /**
             * This observable is hot and monitors all the responses from the GetKeysAsync invocations.
             */
            export var GetKeysAsyncObservable = GetKeysAsyncSubject.asObservable().pipe(share());
            /**
             * Returns the keys
             * @param cancellationToken 
             */
            export function GetKeysAsync(
                cancellationToken? : CancellationToken
            ): SolidRpcJs.RpcServiceRequestTyped<Types.OAuth2.OpenIDKeys> {
                let ns = SolidRpcJs.rootNamespace.declareNamespace('SolidRpc.Abstractions.Services.ISolidRpcOidc');
                let uri = ns.getStringValue('baseUrl','https://localhost/') + 'SolidRpc/Abstractions/Services/ISolidRpcOidc/GetKeysAsync';
                let query: { [index: string]: any } = {};
                let headers: { [index: string]: any } = {};
                return new SolidRpcJs.RpcServiceRequestTyped<Types.OAuth2.OpenIDKeys>('get', uri, query, headers, null, cancellationToken, function(code : number, data : any) {
                    if(code == 200) {
                        return new Types.OAuth2.OpenIDKeys(data);
                    } else {
                        throw 'Response code != 200('+code+')';
                    }
                }, GetKeysAsyncSubject);
            }
            let GetTokenAsyncSubject = new Subject<Types.OAuth2.TokenResponse>();
            /**
             * This observable is hot and monitors all the responses from the GetTokenAsync invocations.
             */
            export var GetTokenAsyncObservable = GetTokenAsyncSubject.asObservable().pipe(share());
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
            export function GetTokenAsync(
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
            ): SolidRpcJs.RpcServiceRequestTyped<Types.OAuth2.TokenResponse> {
                let ns = SolidRpcJs.rootNamespace.declareNamespace('SolidRpc.Abstractions.Services.ISolidRpcOidc');
                let uri = ns.getStringValue('baseUrl','https://localhost/') + 'SolidRpc/Abstractions/Services/ISolidRpcOidc/GetTokenAsync';
                let query: { [index: string]: any } = {};
                let headers: { [index: string]: any } = {};
                return new SolidRpcJs.RpcServiceRequestTyped<Types.OAuth2.TokenResponse>('post', uri, query, headers, null, cancellationToken, function(code : number, data : any) {
                    if(code == 200) {
                        return new Types.OAuth2.TokenResponse(data);
                    } else {
                        throw 'Response code != 200('+code+')';
                    }
                }, GetTokenAsyncSubject);
            }
            let AuthorizeAsyncSubject = new Subject<Types.FileContent>();
            /**
             * This observable is hot and monitors all the responses from the AuthorizeAsync invocations.
             */
            export var AuthorizeAsyncObservable = AuthorizeAsyncSubject.asObservable().pipe(share());
            /**
             * authorizes a user
             * @param scope REQUIRED. OpenID Connect requests MUST contain the openid scope value. If the openid scope value is not present, the behavior is entirely unspecified. Other scope values MAY be present. Scope values used that are not understood by an implementation SHOULD be ignored. See Sections 5.4 and 11 for additional scope values defined by this specification.
             * @param responseType 
             * @param clientId 
             * @param redirectUri 
             * @param state RECOMMENDED. Opaque value used to maintain state between the request and the callback. Typically, Cross-Site Request Forgery (CSRF, XSRF) mitigation is done by cryptographically binding the value of this parameter with a browser cookie.
             * @param responseMode 
             * @param nonce OPTIONAL. String value used to associate a Client session with an ID Token, and to mitigate replay attacks. The value is passed through unmodified from the Authentication Request to the ID Token. Sufficient entropy MUST be present in the nonce values used to prevent attackers from guessing values. For implementation notes, see Section 15.5.2.
             * @param cancellationToken 
             */
            export function AuthorizeAsync(
                scope : string[],
                responseType : string,
                clientId : string,
                redirectUri : string,
                state? : string,
                responseMode? : string,
                nonce? : string,
                cancellationToken? : CancellationToken
            ): SolidRpcJs.RpcServiceRequestTyped<Types.FileContent> {
                let ns = SolidRpcJs.rootNamespace.declareNamespace('SolidRpc.Abstractions.Services.ISolidRpcOidc');
                let uri = ns.getStringValue('baseUrl','https://localhost/') + 'SolidRpc/Abstractions/Services/ISolidRpcOidc/AuthorizeAsync';
                let query: { [index: string]: any } = {};
                SolidRpcJs.ifnotnull(scope, x => { query['scope'] = x; });
                SolidRpcJs.ifnotnull(responseType, x => { query['response_type'] = x; });
                SolidRpcJs.ifnotnull(clientId, x => { query['client_id'] = x; });
                SolidRpcJs.ifnotnull(redirectUri, x => { query['redirect_uri'] = x; });
                SolidRpcJs.ifnotnull(state, x => { query['state'] = x; });
                SolidRpcJs.ifnotnull(responseMode, x => { query['response_mode'] = x; });
                SolidRpcJs.ifnotnull(nonce, x => { query['nonce'] = x; });
                let headers: { [index: string]: any } = {};
                return new SolidRpcJs.RpcServiceRequestTyped<Types.FileContent>('get', uri, query, headers, null, cancellationToken, function(code : number, data : any) {
                    if(code == 200) {
                        return new Types.FileContent(data);
                    } else {
                        throw 'Response code != 200('+code+')';
                    }
                }, AuthorizeAsyncSubject);
            }
            let RevokeAsyncSubject = new Subject<void>();
            /**
             * This observable is hot and monitors all the responses from the RevokeAsync invocations.
             */
            export var RevokeAsyncObservable = RevokeAsyncSubject.asObservable().pipe(share());
            /**
             * revokes a token
             * @param clientId 
             * @param clientSecret 
             * @param token 
             * @param tokenHint 
             * @param cancellationToken 
             */
            export function RevokeAsync(
                clientId? : string,
                clientSecret? : string,
                token? : string,
                tokenHint? : string,
                cancellationToken? : CancellationToken
            ): SolidRpcJs.RpcServiceRequestTyped<void> {
                let ns = SolidRpcJs.rootNamespace.declareNamespace('SolidRpc.Abstractions.Services.ISolidRpcOidc');
                let uri = ns.getStringValue('baseUrl','https://localhost/') + 'SolidRpc/Abstractions/Services/ISolidRpcOidc/RevokeAsync';
                let query: { [index: string]: any } = {};
                let headers: { [index: string]: any } = {};
                return new SolidRpcJs.RpcServiceRequestTyped<void>('post', uri, query, headers, null, cancellationToken, function(code : number, data : any) {
                    if(code == 200) {
                        return null;
                    } else {
                        throw 'Response code != 200('+code+')';
                    }
                }, RevokeAsyncSubject);
            }
            }
        /**
         * Provides access to protected content
         */
        export namespace ISolidRpcProtectedContent {
            let GetProtectedContentAsyncSubject = new Subject<Types.FileContent>();
            /**
             * This observable is hot and monitors all the responses from the GetProtectedContentAsync invocations.
             */
            export var GetProtectedContentAsyncObservable = GetProtectedContentAsyncSubject.asObservable().pipe(share());
            /**
             * Returns the content for the supplied resource.
             * @param resource 
             * @param cancellationToken 
             */
            export function GetProtectedContentAsync(
                resource : string,
                cancellationToken? : CancellationToken
            ): SolidRpcJs.RpcServiceRequestTyped<Types.FileContent> {
                let ns = SolidRpcJs.rootNamespace.declareNamespace('SolidRpc.Abstractions.Services.ISolidRpcProtectedContent');
                let uri = ns.getStringValue('baseUrl','https://localhost/') + 'SolidRpc/Abstractions/Services/ISolidRpcProtectedContent/GetProtectedContentAsync/{resource}';
                SolidRpcJs.ifnull(resource, () => { uri = uri.replace('{resource}', ''); }, nn =>  { uri = uri.replace('{resource}', SolidRpcJs.encodeUriValue(nn.toString())); });
                let query: { [index: string]: any } = {};
                let headers: { [index: string]: any } = {};
                return new SolidRpcJs.RpcServiceRequestTyped<Types.FileContent>('get', uri, query, headers, null, cancellationToken, function(code : number, data : any) {
                    if(code == 200) {
                        return new Types.FileContent(data);
                    } else {
                        throw 'Response code != 200('+code+')';
                    }
                }, GetProtectedContentAsyncSubject);
            }
            }
    }
    export namespace Types {
        export namespace Code {
            /**
             * Represents an interface
             */
            export class CodeInterface {
                constructor(obj?: any) {
                    SolidRpcJs.ifnotnull(obj.description, val => { this.Description = val as string; });
                    SolidRpcJs.ifnotnull(obj.name, val => { this.Name = val as string; });
                    SolidRpcJs.ifnotnull(obj.methods, val => { this.Methods = Array.from(val).map(o => new CodeMethod(o)); });
                }
                toJson(arr: string[]): void {
                    arr.push('{');
                    if(this.Description) { arr.push('"description": '); arr.push(JSON.stringify(this.Description)); arr.push(','); } 
                    if(this.Name) { arr.push('"name": '); arr.push(JSON.stringify(this.Name)); arr.push(','); } 
                    if(this.Methods) { arr.push('"methods": '); for (let i = 0; i < this.Methods.length; i++) if(this.Methods[i]) {this.Methods[i].toJson(arr)}; arr.push(',');; arr.push(','); } 
                    if(arr[arr.length-1] == ',') arr[arr.length-1] = '}'; else arr.push('}');
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
                    SolidRpcJs.ifnotnull(obj.description, val => { this.Description = val as string; });
                    SolidRpcJs.ifnotnull(obj.name, val => { this.Name = val as string; });
                    SolidRpcJs.ifnotnull(obj.arguments, val => { this.Arguments = Array.from(val).map(o => new CodeMethodArg(o)); });
                    SolidRpcJs.ifnotnull(obj.returnType, val => { this.ReturnType = Array.from(val).map(o => o as string); });
                    SolidRpcJs.ifnotnull(obj.httpMethod, val => { this.HttpMethod = val as string; });
                    SolidRpcJs.ifnotnull(obj.httpBaseAddress, val => { this.HttpBaseAddress = val as string; });
                    SolidRpcJs.ifnotnull(obj.httpPath, val => { this.HttpPath = val as string; });
                }
                toJson(arr: string[]): void {
                    arr.push('{');
                    if(this.Description) { arr.push('"description": '); arr.push(JSON.stringify(this.Description)); arr.push(','); } 
                    if(this.Name) { arr.push('"name": '); arr.push(JSON.stringify(this.Name)); arr.push(','); } 
                    if(this.Arguments) { arr.push('"arguments": '); for (let i = 0; i < this.Arguments.length; i++) if(this.Arguments[i]) {this.Arguments[i].toJson(arr)}; arr.push(',');; arr.push(','); } 
                    if(this.ReturnType) { arr.push('"returnType": '); for (let i = 0; i < this.ReturnType.length; i++) arr.push(JSON.stringify(this.ReturnType[i])); arr.push(',');; arr.push(','); } 
                    if(this.HttpMethod) { arr.push('"httpMethod": '); arr.push(JSON.stringify(this.HttpMethod)); arr.push(','); } 
                    if(this.HttpBaseAddress) { arr.push('"httpBaseAddress": '); arr.push(JSON.stringify(this.HttpBaseAddress)); arr.push(','); } 
                    if(this.HttpPath) { arr.push('"httpPath": '); arr.push(JSON.stringify(this.HttpPath)); arr.push(','); } 
                    if(arr[arr.length-1] == ',') arr[arr.length-1] = '}'; else arr.push('}');
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
                    SolidRpcJs.ifnotnull(obj.description, val => { this.Description = val as string; });
                    SolidRpcJs.ifnotnull(obj.name, val => { this.Name = val as string; });
                    SolidRpcJs.ifnotnull(obj.argType, val => { this.ArgType = Array.from(val).map(o => o as string); });
                    SolidRpcJs.ifnotnull(obj.optional, val => { this.Optional = [true, 'true', 1].some(o => o === val); });
                    SolidRpcJs.ifnotnull(obj.httpName, val => { this.HttpName = val as string; });
                    SolidRpcJs.ifnotnull(obj.httpLocation, val => { this.HttpLocation = val as string; });
                }
                toJson(arr: string[]): void {
                    arr.push('{');
                    if(this.Description) { arr.push('"description": '); arr.push(JSON.stringify(this.Description)); arr.push(','); } 
                    if(this.Name) { arr.push('"name": '); arr.push(JSON.stringify(this.Name)); arr.push(','); } 
                    if(this.ArgType) { arr.push('"argType": '); for (let i = 0; i < this.ArgType.length; i++) arr.push(JSON.stringify(this.ArgType[i])); arr.push(',');; arr.push(','); } 
                    if(this.Optional) { arr.push('"optional": '); arr.push(JSON.stringify(this.Optional)); arr.push(','); } 
                    if(this.HttpName) { arr.push('"httpName": '); arr.push(JSON.stringify(this.HttpName)); arr.push(','); } 
                    if(this.HttpLocation) { arr.push('"httpLocation": '); arr.push(JSON.stringify(this.HttpLocation)); arr.push(','); } 
                    if(arr[arr.length-1] == ',') arr[arr.length-1] = '}'; else arr.push('}');
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
                    SolidRpcJs.ifnotnull(obj.name, val => { this.Name = val as string; });
                    SolidRpcJs.ifnotnull(obj.namespaces, val => { this.Namespaces = Array.from(val).map(o => new CodeNamespace(o)); });
                    SolidRpcJs.ifnotnull(obj.interfaces, val => { this.Interfaces = Array.from(val).map(o => new CodeInterface(o)); });
                    SolidRpcJs.ifnotnull(obj.types, val => { this.Types = Array.from(val).map(o => new CodeType(o)); });
                }
                toJson(arr: string[]): void {
                    arr.push('{');
                    if(this.Name) { arr.push('"name": '); arr.push(JSON.stringify(this.Name)); arr.push(','); } 
                    if(this.Namespaces) { arr.push('"namespaces": '); for (let i = 0; i < this.Namespaces.length; i++) if(this.Namespaces[i]) {this.Namespaces[i].toJson(arr)}; arr.push(',');; arr.push(','); } 
                    if(this.Interfaces) { arr.push('"interfaces": '); for (let i = 0; i < this.Interfaces.length; i++) if(this.Interfaces[i]) {this.Interfaces[i].toJson(arr)}; arr.push(',');; arr.push(','); } 
                    if(this.Types) { arr.push('"types": '); for (let i = 0; i < this.Types.length; i++) if(this.Types[i]) {this.Types[i].toJson(arr)}; arr.push(',');; arr.push(','); } 
                    if(arr[arr.length-1] == ',') arr[arr.length-1] = '}'; else arr.push('}');
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
                    SolidRpcJs.ifnotnull(obj.description, val => { this.Description = val as string; });
                    SolidRpcJs.ifnotnull(obj.name, val => { this.Name = val as string; });
                    SolidRpcJs.ifnotnull(obj.extends, val => { this.Extends = Array.from(val).map(o => o as string); });
                    SolidRpcJs.ifnotnull(obj.properties, val => { this.Properties = Array.from(val).map(o => new CodeTypeProperty(o)); });
                }
                toJson(arr: string[]): void {
                    arr.push('{');
                    if(this.Description) { arr.push('"description": '); arr.push(JSON.stringify(this.Description)); arr.push(','); } 
                    if(this.Name) { arr.push('"name": '); arr.push(JSON.stringify(this.Name)); arr.push(','); } 
                    if(this.Extends) { arr.push('"extends": '); for (let i = 0; i < this.Extends.length; i++) arr.push(JSON.stringify(this.Extends[i])); arr.push(',');; arr.push(','); } 
                    if(this.Properties) { arr.push('"properties": '); for (let i = 0; i < this.Properties.length; i++) if(this.Properties[i]) {this.Properties[i].toJson(arr)}; arr.push(',');; arr.push(','); } 
                    if(arr[arr.length-1] == ',') arr[arr.length-1] = '}'; else arr.push('}');
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
                    SolidRpcJs.ifnotnull(obj.description, val => { this.Description = val as string; });
                    SolidRpcJs.ifnotnull(obj.name, val => { this.Name = val as string; });
                    SolidRpcJs.ifnotnull(obj.propertyType, val => { this.PropertyType = Array.from(val).map(o => o as string); });
                    SolidRpcJs.ifnotnull(obj.httpName, val => { this.HttpName = val as string; });
                }
                toJson(arr: string[]): void {
                    arr.push('{');
                    if(this.Description) { arr.push('"description": '); arr.push(JSON.stringify(this.Description)); arr.push(','); } 
                    if(this.Name) { arr.push('"name": '); arr.push(JSON.stringify(this.Name)); arr.push(','); } 
                    if(this.PropertyType) { arr.push('"propertyType": '); for (let i = 0; i < this.PropertyType.length; i++) arr.push(JSON.stringify(this.PropertyType[i])); arr.push(',');; arr.push(','); } 
                    if(this.HttpName) { arr.push('"httpName": '); arr.push(JSON.stringify(this.HttpName)); arr.push(','); } 
                    if(arr[arr.length-1] == ',') arr[arr.length-1] = '}'; else arr.push('}');
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
                    SolidRpcJs.ifnotnull(obj.name, val => { this.Name = val as string; });
                    SolidRpcJs.ifnotnull(obj.files, val => { this.Files = Array.from(val).map(o => new NpmPackageFile(o)); });
                }
                toJson(arr: string[]): void {
                    arr.push('{');
                    if(this.Name) { arr.push('"name": '); arr.push(JSON.stringify(this.Name)); arr.push(','); } 
                    if(this.Files) { arr.push('"files": '); for (let i = 0; i < this.Files.length; i++) if(this.Files[i]) {this.Files[i].toJson(arr)}; arr.push(',');; arr.push(','); } 
                    if(arr[arr.length-1] == ',') arr[arr.length-1] = '}'; else arr.push('}');
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
                    SolidRpcJs.ifnotnull(obj.filePath, val => { this.FilePath = val as string; });
                    SolidRpcJs.ifnotnull(obj.content, val => { this.Content = val as string; });
                }
                toJson(arr: string[]): void {
                    arr.push('{');
                    if(this.FilePath) { arr.push('"filePath": '); arr.push(JSON.stringify(this.FilePath)); arr.push(','); } 
                    if(this.Content) { arr.push('"content": '); arr.push(JSON.stringify(this.Content)); arr.push(','); } 
                    if(arr[arr.length-1] == ',') arr[arr.length-1] = '}'; else arr.push('}');
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
                    SolidRpcJs.ifnotnull(obj.issuer, val => { this.Issuer = val as string; });
                    SolidRpcJs.ifnotnull(obj.authorization_endpoint, val => { this.AuthorizationEndpoint = val as string; });
                    SolidRpcJs.ifnotnull(obj.token_endpoint, val => { this.TokenEndpoint = val as string; });
                    SolidRpcJs.ifnotnull(obj.userinfo_endpoint, val => { this.UserinfoEndpoint = val as string; });
                    SolidRpcJs.ifnotnull(obj.revocation_endpoint, val => { this.RevocationEndpoint = val as string; });
                    SolidRpcJs.ifnotnull(obj.device_authorization_endpoint, val => { this.DeviceAuthorizationEndpoint = val as string; });
                    SolidRpcJs.ifnotnull(obj.jwks_uri, val => { this.JwksUri = val as string; });
                    SolidRpcJs.ifnotnull(obj.scopes_supported, val => { this.ScopesSupported = Array.from(val).map(o => o as string); });
                    SolidRpcJs.ifnotnull(obj.grant_types_supported, val => { this.GrantTypesSupported = Array.from(val).map(o => o as string); });
                    SolidRpcJs.ifnotnull(obj.response_modes_supported, val => { this.ResponseModesSupported = Array.from(val).map(o => o as string); });
                    SolidRpcJs.ifnotnull(obj.subject_types_supported, val => { this.SubjectTypesSupported = Array.from(val).map(o => o as string); });
                    SolidRpcJs.ifnotnull(obj.id_token_signing_alg_values_supported, val => { this.IdTokenSigningAlgValuesSupported = Array.from(val).map(o => o as string); });
                    SolidRpcJs.ifnotnull(obj.end_session_endpoint, val => { this.EndSessionEndpoint = val as string; });
                    SolidRpcJs.ifnotnull(obj.response_types_supported, val => { this.ResponseTypesSupported = Array.from(val).map(o => o as string); });
                    SolidRpcJs.ifnotnull(obj.claims_supported, val => { this.ClaimsSupported = Array.from(val).map(o => o as string); });
                    SolidRpcJs.ifnotnull(obj.token_endpoint_auth_methods_supported, val => { this.TokenEndpointAuthMethodsSupported = Array.from(val).map(o => o as string); });
                    SolidRpcJs.ifnotnull(obj.code_challenge_methods_supported, val => { this.CodeChallengeMethodsSupported = Array.from(val).map(o => o as string); });
                    SolidRpcJs.ifnotnull(obj.request_uri_parameter_supported, val => { this.RequestUriParameterSupported = [true, 'true', 1].some(o => o === val); });
                    SolidRpcJs.ifnotnull(obj.http_logout_supported, val => { this.HttpLogoutSupported = [true, 'true', 1].some(o => o === val); });
                    SolidRpcJs.ifnotnull(obj.frontchannel_logout_supported, val => { this.FrontchannelLogoutSupported = [true, 'true', 1].some(o => o === val); });
                    SolidRpcJs.ifnotnull(obj.rbac_url, val => { this.RbacUrl = val as string; });
                    SolidRpcJs.ifnotnull(obj.msgraph_host, val => { this.MsgraphHost = val as string; });
                    SolidRpcJs.ifnotnull(obj.cloud_graph_host_name, val => { this.CloudGraphHostName = val as string; });
                    SolidRpcJs.ifnotnull(obj.cloud_instance_name, val => { this.CloudInstanceName = val as string; });
                    SolidRpcJs.ifnotnull(obj.tenant_region_scope, val => { this.TenantRegionScope = val as string; });
                }
                toJson(arr: string[]): void {
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
                    SolidRpcJs.ifnotnull(obj.alg, val => { this.Alg = val as string; });
                    SolidRpcJs.ifnotnull(obj.kty, val => { this.Kty = val as string; });
                    SolidRpcJs.ifnotnull(obj.use, val => { this.Use = val as string; });
                    SolidRpcJs.ifnotnull(obj.kid, val => { this.Kid = val as string; });
                    SolidRpcJs.ifnotnull(obj.x5u, val => { this.X5u = val as string; });
                    SolidRpcJs.ifnotnull(obj.x5t, val => { this.X5t = val as string; });
                    SolidRpcJs.ifnotnull(obj.x5c, val => { this.X5c = Array.from(val).map(o => o as string); });
                    SolidRpcJs.ifnotnull(obj.n, val => { this.N = val as string; });
                    SolidRpcJs.ifnotnull(obj.e, val => { this.E = val as string; });
                    SolidRpcJs.ifnotnull(obj.d, val => { this.D = val as string; });
                    SolidRpcJs.ifnotnull(obj.issuer, val => { this.Issuer = val as string; });
                }
                toJson(arr: string[]): void {
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
                    if(this.D) { arr.push('"d": '); arr.push(JSON.stringify(this.D)); arr.push(','); } 
                    if(this.Issuer) { arr.push('"issuer": '); arr.push(JSON.stringify(this.Issuer)); arr.push(','); } 
                    if(arr[arr.length-1] == ',') arr[arr.length-1] = '}'; else arr.push('}');
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
                D: string | null = null;
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
                    SolidRpcJs.ifnotnull(obj.keys, val => { this.Keys = Array.from(val).map(o => new OpenIDKey(o)); });
                }
                toJson(arr: string[]): void {
                    arr.push('{');
                    if(this.Keys) { arr.push('"keys": '); for (let i = 0; i < this.Keys.length; i++) if(this.Keys[i]) {this.Keys[i].toJson(arr)}; arr.push(',');; arr.push(','); } 
                    if(arr[arr.length-1] == ',') arr[arr.length-1] = '}'; else arr.push('}');
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
                    SolidRpcJs.ifnotnull(obj.access_token, val => { this.AccessToken = val as string; });
                    SolidRpcJs.ifnotnull(obj.token_type, val => { this.TokenType = val as string; });
                    SolidRpcJs.ifnotnull(obj.expires_in, val => { this.ExpiresIn = val as string; });
                    SolidRpcJs.ifnotnull(obj.refresh_token, val => { this.RefreshToken = val as string; });
                    SolidRpcJs.ifnotnull(obj.scope, val => { this.Scope = val as string; });
                }
                toJson(arr: string[]): void {
                    arr.push('{');
                    if(this.AccessToken) { arr.push('"access_token": '); arr.push(JSON.stringify(this.AccessToken)); arr.push(','); } 
                    if(this.TokenType) { arr.push('"token_type": '); arr.push(JSON.stringify(this.TokenType)); arr.push(','); } 
                    if(this.ExpiresIn) { arr.push('"expires_in": '); arr.push(JSON.stringify(this.ExpiresIn)); arr.push(','); } 
                    if(this.RefreshToken) { arr.push('"refresh_token": '); arr.push(JSON.stringify(this.RefreshToken)); arr.push(','); } 
                    if(this.Scope) { arr.push('"scope": '); arr.push(JSON.stringify(this.Scope)); arr.push(','); } 
                    if(arr[arr.length-1] == ',') arr[arr.length-1] = '}'; else arr.push('}');
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
        export namespace RateLimit {
            /**
             * Specifies the rate limit settings for the specified resource
             */
            export class RateLimitSetting {
                constructor(obj?: any) {
                    SolidRpcJs.ifnotnull(obj.ResourceName, val => { this.ResourceName = val as string; });
                    SolidRpcJs.ifnotnull(obj.MaxConcurrentCalls, val => { this.MaxConcurrentCalls = SolidRpcJs.ifnotnull<number>(val, (notnull) => Number(notnull)); });
                }
                toJson(arr: string[]): void {
                    arr.push('{');
                    if(this.ResourceName) { arr.push('"ResourceName": '); arr.push(JSON.stringify(this.ResourceName)); arr.push(','); } 
                    if(this.MaxConcurrentCalls) { arr.push('"MaxConcurrentCalls": '); arr.push(JSON.stringify(this.MaxConcurrentCalls)); arr.push(','); } 
                    if(arr[arr.length-1] == ',') arr[arr.length-1] = '}'; else arr.push('}');
                }
                /**
                 * The name of the resource.
                 */
                ResourceName: string | null = null;
                /**
                 * If set specifies the maximum number of concurrent calls
                 *             there can be to the resource
                 */
                MaxConcurrentCalls: number|null | null = null;
            }
            /**
             * Token returned from a resource request
             */
            export class RateLimitToken {
                constructor(obj?: any) {
                    SolidRpcJs.ifnotnull(obj.ResourceName, val => { this.ResourceName = val as string; });
                    SolidRpcJs.ifnotnull(obj.Id, val => { this.Id = val as string; });
                    SolidRpcJs.ifnotnull(obj.Expires, val => { this.Expires = new Date(val); });
                }
                toJson(arr: string[]): void {
                    arr.push('{');
                    if(this.ResourceName) { arr.push('"ResourceName": '); arr.push(JSON.stringify(this.ResourceName)); arr.push(','); } 
                    if(this.Id) { arr.push('"Id": '); arr.push(JSON.stringify(this.Id)); arr.push(','); } 
                    if(this.Expires) { arr.push('"Expires": '); arr.push(JSON.stringify(this.Expires)); arr.push(','); } 
                    if(arr[arr.length-1] == ',') arr[arr.length-1] = '}'; else arr.push('}');
                }
                /**
                 * The name of the resource.
                 */
                ResourceName: string | null = null;
                /**
                 * The unique id of this token. This guid may be empty if no token
                 *             was issued from the service.
                 */
                Id: string | null = null;
                /**
                 * The time when the token expires
                 */
                Expires: Date | null = null;
            }
        }
        /**
         * Represents a file content
         */
        export class FileContent {
            constructor(obj?: any) {
                SolidRpcJs.ifnotnull(obj.Content, val => { this.Content = new Uint8Array(val); });
                SolidRpcJs.ifnotnull(obj.CharSet, val => { this.CharSet = val as string; });
                SolidRpcJs.ifnotnull(obj.ContentType, val => { this.ContentType = val as string; });
                SolidRpcJs.ifnotnull(obj.FileName, val => { this.FileName = val as string; });
                SolidRpcJs.ifnotnull(obj.LastModified, val => { this.LastModified = SolidRpcJs.ifnotnull<Date>(val, (notnull) => new Date(notnull)); });
                SolidRpcJs.ifnotnull(obj.Location, val => { this.Location = val as string; });
                SolidRpcJs.ifnotnull(obj.ETag, val => { this.ETag = val as string; });
                SolidRpcJs.ifnotnull(obj.SetCookie, val => { this.SetCookie = val as string; });
            }
            toJson(arr: string[]): void {
                arr.push('{');
                if(this.Content) { arr.push('"Content": '); arr.push(JSON.stringify(this.Content)); arr.push(','); } 
                if(this.CharSet) { arr.push('"CharSet": '); arr.push(JSON.stringify(this.CharSet)); arr.push(','); } 
                if(this.ContentType) { arr.push('"ContentType": '); arr.push(JSON.stringify(this.ContentType)); arr.push(','); } 
                if(this.FileName) { arr.push('"FileName": '); arr.push(JSON.stringify(this.FileName)); arr.push(','); } 
                if(this.LastModified) { arr.push('"LastModified": '); arr.push(JSON.stringify(this.LastModified)); arr.push(','); } 
                if(this.Location) { arr.push('"Location": '); arr.push(JSON.stringify(this.Location)); arr.push(','); } 
                if(this.ETag) { arr.push('"ETag": '); arr.push(JSON.stringify(this.ETag)); arr.push(','); } 
                if(this.SetCookie) { arr.push('"SetCookie": '); arr.push(JSON.stringify(this.SetCookie)); arr.push(','); } 
                if(arr[arr.length-1] == ',') arr[arr.length-1] = '}'; else arr.push('}');
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
            /**
             * The SetCookie.
             */
            SetCookie: string | null = null;
        }
        /**
         * Represents a name/value pair
         */
        export class NameValuePair {
            constructor(obj?: any) {
                SolidRpcJs.ifnotnull(obj.Name, val => { this.Name = val as string; });
                SolidRpcJs.ifnotnull(obj.Value, val => { this.Value = val as string; });
            }
            toJson(arr: string[]): void {
                arr.push('{');
                if(this.Name) { arr.push('"Name": '); arr.push(JSON.stringify(this.Name)); arr.push(','); } 
                if(this.Value) { arr.push('"Value": '); arr.push(JSON.stringify(this.Value)); arr.push(','); } 
                if(arr[arr.length-1] == ',') arr[arr.length-1] = '}'; else arr.push('}');
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
                SolidRpcJs.ifnotnull(obj.HostId, val => { this.HostId = val as string; });
                SolidRpcJs.ifnotnull(obj.Started, val => { this.Started = new Date(val); });
                SolidRpcJs.ifnotnull(obj.LastAlive, val => { this.LastAlive = new Date(val); });
                SolidRpcJs.ifnotnull(obj.BaseAddress, val => { this.BaseAddress = val as string; });
                SolidRpcJs.ifnotnull(obj.HttpCookies, val => { this.HttpCookies = val as Record<string,string>; });
            }
            toJson(arr: string[]): void {
                arr.push('{');
                if(this.HostId) { arr.push('"HostId": '); arr.push(JSON.stringify(this.HostId)); arr.push(','); } 
                if(this.Started) { arr.push('"Started": '); arr.push(JSON.stringify(this.Started)); arr.push(','); } 
                if(this.LastAlive) { arr.push('"LastAlive": '); arr.push(JSON.stringify(this.LastAlive)); arr.push(','); } 
                if(this.BaseAddress) { arr.push('"BaseAddress": '); arr.push(JSON.stringify(this.BaseAddress)); arr.push(','); } 
                if(this.HttpCookies) { arr.push('"HttpCookies": '); arr.push(JSON.stringify(this.HttpCookies)); arr.push(','); } 
                if(arr[arr.length-1] == ',') arr[arr.length-1] = '}'; else arr.push('}');
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
