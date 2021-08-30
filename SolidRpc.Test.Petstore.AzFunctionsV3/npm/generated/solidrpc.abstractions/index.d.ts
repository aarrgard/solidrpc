import { default as CancellationToken } from 'cancellationtoken';
import { Observable } from 'rxjs';
import { SolidRpcJs } from 'solidrpcjs';
export declare namespace Abstractions {
    namespace Services {
        namespace Code {
            /**
             *
             */
            interface ICodeNamespaceGenerator {
                /**
                 *
                 * @param assemblyName
                 * @param cancellationToken
                 */
                CreateCodeNamespace(assemblyName: string, cancellationToken?: CancellationToken): Observable<Abstractions.Types.Code.CodeNamespace>;
                /**
                 * This observable is hot and monitors all the responses from the CreateCodeNamespace invocations.
                 */
                CreateCodeNamespaceObservable: Observable<Abstractions.Types.Code.CodeNamespace>;
            }
            /**
             *
             */
            class CodeNamespaceGeneratorImpl extends SolidRpcJs.RpcServiceImpl implements ICodeNamespaceGenerator {
                constructor();
                /**
                 *
                 * @param assemblyName
                 * @param cancellationToken
                 */
                CreateCodeNamespace(assemblyName: string, cancellationToken?: CancellationToken): Observable<Abstractions.Types.Code.CodeNamespace>;
                /**
                 * This observable is hot and monitors all the responses from the CreateCodeNamespace invocations.
                 */
                CreateCodeNamespaceObservable: Observable<Abstractions.Types.Code.CodeNamespace>;
                private CreateCodeNamespaceSubject;
            }
            /**
             * Instance for the ICodeNamespaceGenerator type. Implemented by the CodeNamespaceGeneratorImpl
             */
            var CodeNamespaceGeneratorInstance: ICodeNamespaceGenerator;
            /**
             *
             */
            interface INpmGenerator {
                /**
                 *
                 * @param assemblyNames
                 * @param cancellationToken
                 */
                CreateNpmPackage(assemblyNames: string[], cancellationToken?: CancellationToken): Observable<Abstractions.Types.Code.NpmPackage[]>;
                /**
                 * This observable is hot and monitors all the responses from the CreateNpmPackage invocations.
                 */
                CreateNpmPackageObservable: Observable<Abstractions.Types.Code.NpmPackage[]>;
                /**
                 *
                 * @param assemblyNames
                 * @param cancellationToken
                 */
                CreateNpmZip(assemblyNames: string[], cancellationToken?: CancellationToken): Observable<Abstractions.Types.FileContent>;
                /**
                 * This observable is hot and monitors all the responses from the CreateNpmZip invocations.
                 */
                CreateNpmZipObservable: Observable<Abstractions.Types.FileContent>;
            }
            /**
             *
             */
            class NpmGeneratorImpl extends SolidRpcJs.RpcServiceImpl implements INpmGenerator {
                constructor();
                /**
                 *
                 * @param assemblyNames
                 * @param cancellationToken
                 */
                CreateNpmPackage(assemblyNames: string[], cancellationToken?: CancellationToken): Observable<Abstractions.Types.Code.NpmPackage[]>;
                /**
                 * This observable is hot and monitors all the responses from the CreateNpmPackage invocations.
                 */
                CreateNpmPackageObservable: Observable<Abstractions.Types.Code.NpmPackage[]>;
                private CreateNpmPackageSubject;
                /**
                 *
                 * @param assemblyNames
                 * @param cancellationToken
                 */
                CreateNpmZip(assemblyNames: string[], cancellationToken?: CancellationToken): Observable<Abstractions.Types.FileContent>;
                /**
                 * This observable is hot and monitors all the responses from the CreateNpmZip invocations.
                 */
                CreateNpmZipObservable: Observable<Abstractions.Types.FileContent>;
                private CreateNpmZipSubject;
            }
            /**
             * Instance for the INpmGenerator type. Implemented by the NpmGeneratorImpl
             */
            var NpmGeneratorInstance: INpmGenerator;
            /**
             *
             */
            interface ITypescriptGenerator {
                /**
                 *
                 * @param assemblyName
                 * @param cancellationToken
                 */
                CreateTypesTsForAssemblyAsync(assemblyName: string, cancellationToken?: CancellationToken): Observable<string>;
                /**
                 * This observable is hot and monitors all the responses from the CreateTypesTsForAssemblyAsync invocations.
                 */
                CreateTypesTsForAssemblyAsyncObservable: Observable<string>;
                /**
                 *
                 * @param codeNamespace
                 * @param cancellationToken
                 */
                CreateTypesTsForCodeNamespaceAsync(codeNamespace: Abstractions.Types.Code.CodeNamespace, cancellationToken?: CancellationToken): Observable<string>;
                /**
                 * This observable is hot and monitors all the responses from the CreateTypesTsForCodeNamespaceAsync invocations.
                 */
                CreateTypesTsForCodeNamespaceAsyncObservable: Observable<string>;
            }
            /**
             *
             */
            class TypescriptGeneratorImpl extends SolidRpcJs.RpcServiceImpl implements ITypescriptGenerator {
                constructor();
                /**
                 *
                 * @param assemblyName
                 * @param cancellationToken
                 */
                CreateTypesTsForAssemblyAsync(assemblyName: string, cancellationToken?: CancellationToken): Observable<string>;
                /**
                 * This observable is hot and monitors all the responses from the CreateTypesTsForAssemblyAsync invocations.
                 */
                CreateTypesTsForAssemblyAsyncObservable: Observable<string>;
                private CreateTypesTsForAssemblyAsyncSubject;
                /**
                 *
                 * @param codeNamespace
                 * @param cancellationToken
                 */
                CreateTypesTsForCodeNamespaceAsync(codeNamespace: Abstractions.Types.Code.CodeNamespace, cancellationToken?: CancellationToken): Observable<string>;
                /**
                 * This observable is hot and monitors all the responses from the CreateTypesTsForCodeNamespaceAsync invocations.
                 */
                CreateTypesTsForCodeNamespaceAsyncObservable: Observable<string>;
                private CreateTypesTsForCodeNamespaceAsyncSubject;
            }
            /**
             * Instance for the ITypescriptGenerator type. Implemented by the TypescriptGeneratorImpl
             */
            var TypescriptGeneratorInstance: ITypescriptGenerator;
        }
        /**
         *
         */
        interface ISolidRpcContentHandler {
            /**
             *
             * @param path
             * @param cancellationToken
             */
            GetContent(path?: string, cancellationToken?: CancellationToken): Observable<Abstractions.Types.FileContent>;
            /**
             * This observable is hot and monitors all the responses from the GetContent invocations.
             */
            GetContentObservable: Observable<Abstractions.Types.FileContent>;
        }
        /**
         *
         */
        class SolidRpcContentHandlerImpl extends SolidRpcJs.RpcServiceImpl implements ISolidRpcContentHandler {
            constructor();
            /**
             *
             * @param path
             * @param cancellationToken
             */
            GetContent(path?: string, cancellationToken?: CancellationToken): Observable<Abstractions.Types.FileContent>;
            /**
             * This observable is hot and monitors all the responses from the GetContent invocations.
             */
            GetContentObservable: Observable<Abstractions.Types.FileContent>;
            private GetContentSubject;
        }
        /**
         * Instance for the ISolidRpcContentHandler type. Implemented by the SolidRpcContentHandlerImpl
         */
        var SolidRpcContentHandlerInstance: ISolidRpcContentHandler;
        /**
         *
         */
        interface ISolidRpcHost {
            /**
             *
             * @param cancellationToken
             */
            GetHostId(cancellationToken?: CancellationToken): Observable<string>;
            /**
             * This observable is hot and monitors all the responses from the GetHostId invocations.
             */
            GetHostIdObservable: Observable<string>;
            /**
             *
             * @param cancellationToken
             */
            GetHostInstance(cancellationToken?: CancellationToken): Observable<Abstractions.Types.SolidRpcHostInstance>;
            /**
             * This observable is hot and monitors all the responses from the GetHostInstance invocations.
             */
            GetHostInstanceObservable: Observable<Abstractions.Types.SolidRpcHostInstance>;
            /**
             *
             * @param cancellationToken
             */
            SyncHostsFromStore(cancellationToken?: CancellationToken): Observable<Abstractions.Types.SolidRpcHostInstance[]>;
            /**
             * This observable is hot and monitors all the responses from the SyncHostsFromStore invocations.
             */
            SyncHostsFromStoreObservable: Observable<Abstractions.Types.SolidRpcHostInstance[]>;
            /**
             *
             * @param hostInstance
             * @param cancellationToken
             */
            CheckHost(hostInstance: Abstractions.Types.SolidRpcHostInstance, cancellationToken?: CancellationToken): Observable<Abstractions.Types.SolidRpcHostInstance>;
            /**
             * This observable is hot and monitors all the responses from the CheckHost invocations.
             */
            CheckHostObservable: Observable<Abstractions.Types.SolidRpcHostInstance>;
            /**
             *
             * @param cancellationToken
             */
            GetHostConfiguration(cancellationToken?: CancellationToken): Observable<Abstractions.Types.NameValuePair[]>;
            /**
             * This observable is hot and monitors all the responses from the GetHostConfiguration invocations.
             */
            GetHostConfigurationObservable: Observable<Abstractions.Types.NameValuePair[]>;
            /**
             *
             * @param cancellationToken
             */
            IsAlive(cancellationToken?: CancellationToken): Observable<void>;
            /**
             * This observable is hot and monitors all the responses from the IsAlive invocations.
             */
            IsAliveObservable: Observable<void>;
        }
        /**
         *
         */
        class SolidRpcHostImpl extends SolidRpcJs.RpcServiceImpl implements ISolidRpcHost {
            constructor();
            /**
             *
             * @param cancellationToken
             */
            GetHostId(cancellationToken?: CancellationToken): Observable<string>;
            /**
             * This observable is hot and monitors all the responses from the GetHostId invocations.
             */
            GetHostIdObservable: Observable<string>;
            private GetHostIdSubject;
            /**
             *
             * @param cancellationToken
             */
            GetHostInstance(cancellationToken?: CancellationToken): Observable<Abstractions.Types.SolidRpcHostInstance>;
            /**
             * This observable is hot and monitors all the responses from the GetHostInstance invocations.
             */
            GetHostInstanceObservable: Observable<Abstractions.Types.SolidRpcHostInstance>;
            private GetHostInstanceSubject;
            /**
             *
             * @param cancellationToken
             */
            SyncHostsFromStore(cancellationToken?: CancellationToken): Observable<Abstractions.Types.SolidRpcHostInstance[]>;
            /**
             * This observable is hot and monitors all the responses from the SyncHostsFromStore invocations.
             */
            SyncHostsFromStoreObservable: Observable<Abstractions.Types.SolidRpcHostInstance[]>;
            private SyncHostsFromStoreSubject;
            /**
             *
             * @param hostInstance
             * @param cancellationToken
             */
            CheckHost(hostInstance: Abstractions.Types.SolidRpcHostInstance, cancellationToken?: CancellationToken): Observable<Abstractions.Types.SolidRpcHostInstance>;
            /**
             * This observable is hot and monitors all the responses from the CheckHost invocations.
             */
            CheckHostObservable: Observable<Abstractions.Types.SolidRpcHostInstance>;
            private CheckHostSubject;
            /**
             *
             * @param cancellationToken
             */
            GetHostConfiguration(cancellationToken?: CancellationToken): Observable<Abstractions.Types.NameValuePair[]>;
            /**
             * This observable is hot and monitors all the responses from the GetHostConfiguration invocations.
             */
            GetHostConfigurationObservable: Observable<Abstractions.Types.NameValuePair[]>;
            private GetHostConfigurationSubject;
            /**
             *
             * @param cancellationToken
             */
            IsAlive(cancellationToken?: CancellationToken): Observable<void>;
            /**
             * This observable is hot and monitors all the responses from the IsAlive invocations.
             */
            IsAliveObservable: Observable<void>;
            private IsAliveSubject;
        }
        /**
         * Instance for the ISolidRpcHost type. Implemented by the SolidRpcHostImpl
         */
        var SolidRpcHostInstance: ISolidRpcHost;
        /**
         *
         */
        interface ISolidRpcOAuth2 {
            /**
             *
             * @param callbackUri
             * @param state
             * @param scopes
             * @param cancellationToken
             */
            GetAuthorizationCodeTokenAsync(callbackUri?: string, state?: string, scopes?: string[], cancellationToken?: CancellationToken): Observable<Abstractions.Types.FileContent>;
            /**
             * This observable is hot and monitors all the responses from the GetAuthorizationCodeTokenAsync invocations.
             */
            GetAuthorizationCodeTokenAsyncObservable: Observable<Abstractions.Types.FileContent>;
            /**
             *
             * @param code
             * @param state
             * @param cancellation
             */
            TokenCallbackAsync(code?: string, state?: string, cancellation?: CancellationToken): Observable<Abstractions.Types.FileContent>;
            /**
             * This observable is hot and monitors all the responses from the TokenCallbackAsync invocations.
             */
            TokenCallbackAsyncObservable: Observable<Abstractions.Types.FileContent>;
        }
        /**
         *
         */
        class SolidRpcOAuth2Impl extends SolidRpcJs.RpcServiceImpl implements ISolidRpcOAuth2 {
            constructor();
            /**
             *
             * @param callbackUri
             * @param state
             * @param scopes
             * @param cancellationToken
             */
            GetAuthorizationCodeTokenAsync(callbackUri?: string, state?: string, scopes?: string[], cancellationToken?: CancellationToken): Observable<Abstractions.Types.FileContent>;
            /**
             * This observable is hot and monitors all the responses from the GetAuthorizationCodeTokenAsync invocations.
             */
            GetAuthorizationCodeTokenAsyncObservable: Observable<Abstractions.Types.FileContent>;
            private GetAuthorizationCodeTokenAsyncSubject;
            /**
             *
             * @param code
             * @param state
             * @param cancellation
             */
            TokenCallbackAsync(code?: string, state?: string, cancellation?: CancellationToken): Observable<Abstractions.Types.FileContent>;
            /**
             * This observable is hot and monitors all the responses from the TokenCallbackAsync invocations.
             */
            TokenCallbackAsyncObservable: Observable<Abstractions.Types.FileContent>;
            private TokenCallbackAsyncSubject;
        }
        /**
         * Instance for the ISolidRpcOAuth2 type. Implemented by the SolidRpcOAuth2Impl
         */
        var SolidRpcOAuth2Instance: ISolidRpcOAuth2;
    }
    namespace Types {
        namespace Code {
            /**
             *
             */
            class CodeInterface {
                constructor(obj?: any);
                toJson(arr: string[] | null): string | null;
                /**
                 *
                 */
                Description: string | null;
                /**
                 *
                 */
                Name: string | null;
                /**
                 *
                 */
                Methods: Abstractions.Types.Code.CodeMethod[] | null;
            }
            /**
             *
             */
            class CodeMethod {
                constructor(obj?: any);
                toJson(arr: string[] | null): string | null;
                /**
                 *
                 */
                Description: string | null;
                /**
                 *
                 */
                Name: string | null;
                /**
                 *
                 */
                Arguments: Abstractions.Types.Code.CodeMethodArg[] | null;
                /**
                 *
                 */
                ReturnType: string[] | null;
                /**
                 *
                 */
                HttpMethod: string | null;
                /**
                 *
                 */
                HttpBaseAddress: string | null;
                /**
                 *
                 */
                HttpPath: string | null;
            }
            /**
             *
             */
            class CodeMethodArg {
                constructor(obj?: any);
                toJson(arr: string[] | null): string | null;
                /**
                 *
                 */
                Description: string | null;
                /**
                 *
                 */
                Name: string | null;
                /**
                 *
                 */
                ArgType: string[] | null;
                /**
                 *
                 */
                Optional: boolean | null;
                /**
                 *
                 */
                HttpName: string | null;
                /**
                 *
                 */
                HttpLocation: string | null;
            }
            /**
             *
             */
            class CodeNamespace {
                constructor(obj?: any);
                toJson(arr: string[] | null): string | null;
                /**
                 *
                 */
                Name: string | null;
                /**
                 *
                 */
                Namespaces: Abstractions.Types.Code.CodeNamespace[] | null;
                /**
                 *
                 */
                Interfaces: Abstractions.Types.Code.CodeInterface[] | null;
                /**
                 *
                 */
                Types: Abstractions.Types.Code.CodeType[] | null;
            }
            /**
             *
             */
            class CodeType {
                constructor(obj?: any);
                toJson(arr: string[] | null): string | null;
                /**
                 *
                 */
                Description: string | null;
                /**
                 *
                 */
                Name: string | null;
                /**
                 *
                 */
                Extends: string[] | null;
                /**
                 *
                 */
                Properties: Abstractions.Types.Code.CodeTypeProperty[] | null;
            }
            /**
             *
             */
            class CodeTypeProperty {
                constructor(obj?: any);
                toJson(arr: string[] | null): string | null;
                /**
                 *
                 */
                Description: string | null;
                /**
                 *
                 */
                Name: string | null;
                /**
                 *
                 */
                PropertyType: string[] | null;
                /**
                 *
                 */
                HttpName: string | null;
            }
            /**
             *
             */
            class NpmPackage {
                constructor(obj?: any);
                toJson(arr: string[] | null): string | null;
                /**
                 *
                 */
                Name: string | null;
                /**
                 *
                 */
                Files: Abstractions.Types.Code.NpmPackageFile[] | null;
            }
            /**
             *
             */
            class NpmPackageFile {
                constructor(obj?: any);
                toJson(arr: string[] | null): string | null;
                /**
                 *
                 */
                FilePath: string | null;
                /**
                 *
                 */
                Content: string | null;
            }
        }
        /**
         *
         */
        class FileContent {
            constructor(obj?: any);
            toJson(arr: string[] | null): string | null;
            /**
             *
             */
            Content: Uint8Array | null;
            /**
             *
             */
            CharSet: string | null;
            /**
             *
             */
            ContentType: string | null;
            /**
             *
             */
            FileName: string | null;
            /**
             *
             */
            LastModified: Date | null;
            /**
             *
             */
            Location: string | null;
            /**
             *
             */
            ETag: string | null;
        }
        /**
         *
         */
        class NameValuePair {
            constructor(obj?: any);
            toJson(arr: string[] | null): string | null;
            /**
             *
             */
            Name: string | null;
            /**
             *
             */
            Value: string | null;
        }
        /**
         *
         */
        class SolidRpcHostInstance {
            constructor(obj?: any);
            toJson(arr: string[] | null): string | null;
            /**
             *
             */
            HostId: string | null;
            /**
             *
             */
            Started: Date | null;
            /**
             *
             */
            LastAlive: Date | null;
            /**
             *
             */
            BaseAddress: string | null;
            /**
             *
             */
            HttpCookies: Record<string, string> | null;
        }
    }
}
