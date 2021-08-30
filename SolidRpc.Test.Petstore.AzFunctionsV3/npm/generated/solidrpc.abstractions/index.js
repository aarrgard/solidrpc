"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.Abstractions = void 0;
const rxjs_1 = require("rxjs");
const operators_1 = require("rxjs/operators");
const solidrpcjs_1 = require("solidrpcjs");
var Abstractions;
(function (Abstractions) {
    let Services;
    (function (Services) {
        let Code;
        (function (Code) {
            /**
             *
             */
            class CodeNamespaceGeneratorImpl extends solidrpcjs_1.SolidRpcJs.RpcServiceImpl {
                constructor() {
                    super();
                    this.CreateCodeNamespaceSubject = new rxjs_1.Subject();
                    this.CreateCodeNamespaceObservable = this.CreateCodeNamespaceSubject.asObservable().pipe((0, operators_1.share)());
                }
                /**
                 *
                 * @param assemblyName
                 * @param cancellationToken
                 */
                CreateCodeNamespace(assemblyName, cancellationToken) {
                    let uri = 'http://localhost:7071/front/SolidRpc/Abstractions/Services/Code/ICodeNamespaceGenerator/CreateCodeNamespace/{assemblyName}';
                    uri = uri.replace('{assemblyName}', this.enocodeUriValue(assemblyName.toString()));
                    return this.request(new solidrpcjs_1.SolidRpcJs.RpcServiceRequest('get', uri, null, null, null), cancellationToken, function (code, data) {
                        if (code == 200) {
                            return new Abstractions.Types.Code.CodeNamespace(data);
                        }
                        else {
                            throw 'Response code != 200(' + code + ')';
                        }
                    }, this.CreateCodeNamespaceSubject);
                }
            }
            Code.CodeNamespaceGeneratorImpl = CodeNamespaceGeneratorImpl;
            /**
             * Instance for the ICodeNamespaceGenerator type. Implemented by the CodeNamespaceGeneratorImpl
             */
            Code.CodeNamespaceGeneratorInstance = new CodeNamespaceGeneratorImpl();
            /**
             *
             */
            class NpmGeneratorImpl extends solidrpcjs_1.SolidRpcJs.RpcServiceImpl {
                constructor() {
                    super();
                    this.CreateNpmPackageSubject = new rxjs_1.Subject();
                    this.CreateNpmPackageObservable = this.CreateNpmPackageSubject.asObservable().pipe((0, operators_1.share)());
                    this.CreateNpmZipSubject = new rxjs_1.Subject();
                    this.CreateNpmZipObservable = this.CreateNpmZipSubject.asObservable().pipe((0, operators_1.share)());
                }
                /**
                 *
                 * @param assemblyNames
                 * @param cancellationToken
                 */
                CreateNpmPackage(assemblyNames, cancellationToken) {
                    let uri = 'http://localhost:7071/front/SolidRpc/Abstractions/Services/Code/INpmGenerator/CreateNpmPackage/{assemblyNames}';
                    uri = uri.replace('{assemblyNames}', this.enocodeUriValue(assemblyNames.toString()));
                    return this.request(new solidrpcjs_1.SolidRpcJs.RpcServiceRequest('get', uri, null, null, null), cancellationToken, function (code, data) {
                        if (code == 200) {
                            return Array.from(data).map(o => new Abstractions.Types.Code.NpmPackage(o));
                        }
                        else {
                            throw 'Response code != 200(' + code + ')';
                        }
                    }, this.CreateNpmPackageSubject);
                }
                /**
                 *
                 * @param assemblyNames
                 * @param cancellationToken
                 */
                CreateNpmZip(assemblyNames, cancellationToken) {
                    let uri = 'http://localhost:7071/front/SolidRpc/Abstractions/Services/Code/INpmGenerator/CreateNpmZip/{assemblyNames}';
                    uri = uri.replace('{assemblyNames}', this.enocodeUriValue(assemblyNames.toString()));
                    return this.request(new solidrpcjs_1.SolidRpcJs.RpcServiceRequest('get', uri, null, null, null), cancellationToken, function (code, data) {
                        if (code == 200) {
                            return new Abstractions.Types.FileContent(data);
                        }
                        else {
                            throw 'Response code != 200(' + code + ')';
                        }
                    }, this.CreateNpmZipSubject);
                }
            }
            Code.NpmGeneratorImpl = NpmGeneratorImpl;
            /**
             * Instance for the INpmGenerator type. Implemented by the NpmGeneratorImpl
             */
            Code.NpmGeneratorInstance = new NpmGeneratorImpl();
            /**
             *
             */
            class TypescriptGeneratorImpl extends solidrpcjs_1.SolidRpcJs.RpcServiceImpl {
                constructor() {
                    super();
                    this.CreateTypesTsForAssemblyAsyncSubject = new rxjs_1.Subject();
                    this.CreateTypesTsForAssemblyAsyncObservable = this.CreateTypesTsForAssemblyAsyncSubject.asObservable().pipe((0, operators_1.share)());
                    this.CreateTypesTsForCodeNamespaceAsyncSubject = new rxjs_1.Subject();
                    this.CreateTypesTsForCodeNamespaceAsyncObservable = this.CreateTypesTsForCodeNamespaceAsyncSubject.asObservable().pipe((0, operators_1.share)());
                }
                /**
                 *
                 * @param assemblyName
                 * @param cancellationToken
                 */
                CreateTypesTsForAssemblyAsync(assemblyName, cancellationToken) {
                    let uri = 'http://localhost:7071/front/SolidRpc/Abstractions/Services/Code/ITypescriptGenerator/CreateTypesTsForAssemblyAsync/{assemblyName}';
                    uri = uri.replace('{assemblyName}', this.enocodeUriValue(assemblyName.toString()));
                    return this.request(new solidrpcjs_1.SolidRpcJs.RpcServiceRequest('get', uri, null, null, null), cancellationToken, function (code, data) {
                        if (code == 200) {
                            return data;
                        }
                        else {
                            throw 'Response code != 200(' + code + ')';
                        }
                    }, this.CreateTypesTsForAssemblyAsyncSubject);
                }
                /**
                 *
                 * @param codeNamespace
                 * @param cancellationToken
                 */
                CreateTypesTsForCodeNamespaceAsync(codeNamespace, cancellationToken) {
                    let uri = 'http://localhost:7071/front/SolidRpc/Abstractions/Services/Code/ITypescriptGenerator/CreateTypesTsForCodeNamespaceAsync';
                    return this.request(new solidrpcjs_1.SolidRpcJs.RpcServiceRequest('post', uri, null, { 'Content-Type': 'application/json' }, this.toJson(codeNamespace)), cancellationToken, function (code, data) {
                        if (code == 200) {
                            return data;
                        }
                        else {
                            throw 'Response code != 200(' + code + ')';
                        }
                    }, this.CreateTypesTsForCodeNamespaceAsyncSubject);
                }
            }
            Code.TypescriptGeneratorImpl = TypescriptGeneratorImpl;
            /**
             * Instance for the ITypescriptGenerator type. Implemented by the TypescriptGeneratorImpl
             */
            Code.TypescriptGeneratorInstance = new TypescriptGeneratorImpl();
        })(Code = Services.Code || (Services.Code = {}));
        /**
         *
         */
        class SolidRpcContentHandlerImpl extends solidrpcjs_1.SolidRpcJs.RpcServiceImpl {
            constructor() {
                super();
                this.GetContentSubject = new rxjs_1.Subject();
                this.GetContentObservable = this.GetContentSubject.asObservable().pipe((0, operators_1.share)());
            }
            /**
             *
             * @param path
             * @param cancellationToken
             */
            GetContent(path, cancellationToken) {
                let uri = 'http://localhost:7071/front/SolidRpc/Abstractions/Services/ISolidRpcContentHandler/GetContent';
                return this.request(new solidrpcjs_1.SolidRpcJs.RpcServiceRequest('get', uri, {
                    'path': path,
                }, null, null), cancellationToken, function (code, data) {
                    if (code == 200) {
                        return new Abstractions.Types.FileContent(data);
                    }
                    else {
                        throw 'Response code != 200(' + code + ')';
                    }
                }, this.GetContentSubject);
            }
        }
        Services.SolidRpcContentHandlerImpl = SolidRpcContentHandlerImpl;
        /**
         * Instance for the ISolidRpcContentHandler type. Implemented by the SolidRpcContentHandlerImpl
         */
        Services.SolidRpcContentHandlerInstance = new SolidRpcContentHandlerImpl();
        /**
         *
         */
        class SolidRpcHostImpl extends solidrpcjs_1.SolidRpcJs.RpcServiceImpl {
            constructor() {
                super();
                this.GetHostIdSubject = new rxjs_1.Subject();
                this.GetHostIdObservable = this.GetHostIdSubject.asObservable().pipe((0, operators_1.share)());
                this.GetHostInstanceSubject = new rxjs_1.Subject();
                this.GetHostInstanceObservable = this.GetHostInstanceSubject.asObservable().pipe((0, operators_1.share)());
                this.SyncHostsFromStoreSubject = new rxjs_1.Subject();
                this.SyncHostsFromStoreObservable = this.SyncHostsFromStoreSubject.asObservable().pipe((0, operators_1.share)());
                this.CheckHostSubject = new rxjs_1.Subject();
                this.CheckHostObservable = this.CheckHostSubject.asObservable().pipe((0, operators_1.share)());
                this.GetHostConfigurationSubject = new rxjs_1.Subject();
                this.GetHostConfigurationObservable = this.GetHostConfigurationSubject.asObservable().pipe((0, operators_1.share)());
                this.IsAliveSubject = new rxjs_1.Subject();
                this.IsAliveObservable = this.IsAliveSubject.asObservable().pipe((0, operators_1.share)());
            }
            /**
             *
             * @param cancellationToken
             */
            GetHostId(cancellationToken) {
                let uri = 'http://localhost:7071/front/SolidRpc/Abstractions/Services/ISolidRpcHost/GetHostId';
                return this.request(new solidrpcjs_1.SolidRpcJs.RpcServiceRequest('get', uri, null, null, null), cancellationToken, function (code, data) {
                    if (code == 200) {
                        return data;
                    }
                    else {
                        throw 'Response code != 200(' + code + ')';
                    }
                }, this.GetHostIdSubject);
            }
            /**
             *
             * @param cancellationToken
             */
            GetHostInstance(cancellationToken) {
                let uri = 'http://localhost:7071/front/SolidRpc/Abstractions/Services/ISolidRpcHost/GetHostInstance';
                return this.request(new solidrpcjs_1.SolidRpcJs.RpcServiceRequest('get', uri, null, null, null), cancellationToken, function (code, data) {
                    if (code == 200) {
                        return new Abstractions.Types.SolidRpcHostInstance(data);
                    }
                    else {
                        throw 'Response code != 200(' + code + ')';
                    }
                }, this.GetHostInstanceSubject);
            }
            /**
             *
             * @param cancellationToken
             */
            SyncHostsFromStore(cancellationToken) {
                let uri = 'http://localhost:7071/front/SolidRpc/Abstractions/Services/ISolidRpcHost/SyncHostsFromStore';
                return this.request(new solidrpcjs_1.SolidRpcJs.RpcServiceRequest('get', uri, null, null, null), cancellationToken, function (code, data) {
                    if (code == 200) {
                        return Array.from(data).map(o => new Abstractions.Types.SolidRpcHostInstance(o));
                    }
                    else {
                        throw 'Response code != 200(' + code + ')';
                    }
                }, this.SyncHostsFromStoreSubject);
            }
            /**
             *
             * @param hostInstance
             * @param cancellationToken
             */
            CheckHost(hostInstance, cancellationToken) {
                let uri = 'http://localhost:7071/front/SolidRpc/Abstractions/Services/ISolidRpcHost/CheckHost';
                return this.request(new solidrpcjs_1.SolidRpcJs.RpcServiceRequest('post', uri, null, { 'Content-Type': 'application/json' }, this.toJson(hostInstance)), cancellationToken, function (code, data) {
                    if (code == 200) {
                        return new Abstractions.Types.SolidRpcHostInstance(data);
                    }
                    else {
                        throw 'Response code != 200(' + code + ')';
                    }
                }, this.CheckHostSubject);
            }
            /**
             *
             * @param cancellationToken
             */
            GetHostConfiguration(cancellationToken) {
                let uri = 'http://localhost:7071/front/SolidRpc/Abstractions/Services/ISolidRpcHost/GetHostConfiguration';
                return this.request(new solidrpcjs_1.SolidRpcJs.RpcServiceRequest('get', uri, null, null, null), cancellationToken, function (code, data) {
                    if (code == 200) {
                        return Array.from(data).map(o => new Abstractions.Types.NameValuePair(o));
                    }
                    else {
                        throw 'Response code != 200(' + code + ')';
                    }
                }, this.GetHostConfigurationSubject);
            }
            /**
             *
             * @param cancellationToken
             */
            IsAlive(cancellationToken) {
                let uri = 'http://localhost:7071/front/SolidRpc/Abstractions/Services/ISolidRpcHost/IsAlive';
                return this.request(new solidrpcjs_1.SolidRpcJs.RpcServiceRequest('get', uri, null, null, null), cancellationToken, function (code, data) {
                    if (code == 200) {
                        return null;
                    }
                    else {
                        throw 'Response code != 200(' + code + ')';
                    }
                }, this.IsAliveSubject);
            }
        }
        Services.SolidRpcHostImpl = SolidRpcHostImpl;
        /**
         * Instance for the ISolidRpcHost type. Implemented by the SolidRpcHostImpl
         */
        Services.SolidRpcHostInstance = new SolidRpcHostImpl();
        /**
         *
         */
        class SolidRpcOAuth2Impl extends solidrpcjs_1.SolidRpcJs.RpcServiceImpl {
            constructor() {
                super();
                this.GetAuthorizationCodeTokenAsyncSubject = new rxjs_1.Subject();
                this.GetAuthorizationCodeTokenAsyncObservable = this.GetAuthorizationCodeTokenAsyncSubject.asObservable().pipe((0, operators_1.share)());
                this.TokenCallbackAsyncSubject = new rxjs_1.Subject();
                this.TokenCallbackAsyncObservable = this.TokenCallbackAsyncSubject.asObservable().pipe((0, operators_1.share)());
            }
            /**
             *
             * @param callbackUri
             * @param state
             * @param scopes
             * @param cancellationToken
             */
            GetAuthorizationCodeTokenAsync(callbackUri, state, scopes, cancellationToken) {
                let uri = 'http://localhost:7071/front/SolidRpc/Abstractions/Services/ISolidRpcOAuth2/GetAuthorizationCodeTokenAsync';
                return this.request(new solidrpcjs_1.SolidRpcJs.RpcServiceRequest('get', uri, {
                    'callbackUri': callbackUri,
                    'state': state,
                    'scopes': scopes,
                }, null, null), cancellationToken, function (code, data) {
                    if (code == 200) {
                        return new Abstractions.Types.FileContent(data);
                    }
                    else {
                        throw 'Response code != 200(' + code + ')';
                    }
                }, this.GetAuthorizationCodeTokenAsyncSubject);
            }
            /**
             *
             * @param code
             * @param state
             * @param cancellation
             */
            TokenCallbackAsync(code, state, cancellation) {
                let uri = 'http://localhost:7071/front/SolidRpc/Abstractions/Services/ISolidRpcOAuth2/TokenCallbackAsync';
                return this.request(new solidrpcjs_1.SolidRpcJs.RpcServiceRequest('get', uri, {
                    'code': code,
                    'state': state,
                }, null, null), cancellation, function (code, data) {
                    if (code == 200) {
                        return new Abstractions.Types.FileContent(data);
                    }
                    else {
                        throw 'Response code != 200(' + code + ')';
                    }
                }, this.TokenCallbackAsyncSubject);
            }
        }
        Services.SolidRpcOAuth2Impl = SolidRpcOAuth2Impl;
        /**
         * Instance for the ISolidRpcOAuth2 type. Implemented by the SolidRpcOAuth2Impl
         */
        Services.SolidRpcOAuth2Instance = new SolidRpcOAuth2Impl();
    })(Services = Abstractions.Services || (Abstractions.Services = {}));
    let Types;
    (function (Types) {
        let Code;
        (function (Code) {
            /**
             *
             */
            class CodeInterface {
                constructor(obj) {
                    /**
                     *
                     */
                    this.Description = null;
                    /**
                     *
                     */
                    this.Name = null;
                    /**
                     *
                     */
                    this.Methods = null;
                    for (let prop in obj) {
                        switch (prop) {
                            case "description":
                                if (obj.description) {
                                    this.Description = obj.description;
                                }
                                break;
                            case "name":
                                if (obj.name) {
                                    this.Name = obj.name;
                                }
                                break;
                            case "methods":
                                if (obj.methods) {
                                    this.Methods = Array.from(obj.methods).map(o => new Abstractions.Types.Code.CodeMethod(o));
                                }
                                break;
                        }
                    }
                }
                toJson(arr) {
                    let returnString = false;
                    if (arr == null) {
                        arr = [];
                        returnString = true;
                    }
                    arr.push('{');
                    if (this.Description) {
                        arr.push('"description": ');
                        arr.push(JSON.stringify(this.Description));
                        arr.push(',');
                    }
                    if (this.Name) {
                        arr.push('"name": ');
                        arr.push(JSON.stringify(this.Name));
                        arr.push(',');
                    }
                    if (this.Methods) {
                        arr.push('"methods": ');
                        for (let i = 0; i < this.Methods.length; i++)
                            this.Methods[i].toJson(arr);
                        arr.push(',');
                        ;
                        arr.push(',');
                    }
                    if (arr[arr.length - 1] == ',')
                        arr[arr.length - 1] = '}';
                    else
                        arr.push('}');
                    if (returnString)
                        return arr.join("");
                    return null;
                }
            }
            Code.CodeInterface = CodeInterface;
            /**
             *
             */
            class CodeMethod {
                constructor(obj) {
                    /**
                     *
                     */
                    this.Description = null;
                    /**
                     *
                     */
                    this.Name = null;
                    /**
                     *
                     */
                    this.Arguments = null;
                    /**
                     *
                     */
                    this.ReturnType = null;
                    /**
                     *
                     */
                    this.HttpMethod = null;
                    /**
                     *
                     */
                    this.HttpBaseAddress = null;
                    /**
                     *
                     */
                    this.HttpPath = null;
                    for (let prop in obj) {
                        switch (prop) {
                            case "description":
                                if (obj.description) {
                                    this.Description = obj.description;
                                }
                                break;
                            case "name":
                                if (obj.name) {
                                    this.Name = obj.name;
                                }
                                break;
                            case "arguments":
                                if (obj.arguments) {
                                    this.Arguments = Array.from(obj.arguments).map(o => new Abstractions.Types.Code.CodeMethodArg(o));
                                }
                                break;
                            case "returnType":
                                if (obj.returnType) {
                                    this.ReturnType = Array.from(obj.returnType).map(o => o);
                                }
                                break;
                            case "httpMethod":
                                if (obj.httpMethod) {
                                    this.HttpMethod = obj.httpMethod;
                                }
                                break;
                            case "httpBaseAddress":
                                if (obj.httpBaseAddress) {
                                    this.HttpBaseAddress = obj.httpBaseAddress;
                                }
                                break;
                            case "httpPath":
                                if (obj.httpPath) {
                                    this.HttpPath = obj.httpPath;
                                }
                                break;
                        }
                    }
                }
                toJson(arr) {
                    let returnString = false;
                    if (arr == null) {
                        arr = [];
                        returnString = true;
                    }
                    arr.push('{');
                    if (this.Description) {
                        arr.push('"description": ');
                        arr.push(JSON.stringify(this.Description));
                        arr.push(',');
                    }
                    if (this.Name) {
                        arr.push('"name": ');
                        arr.push(JSON.stringify(this.Name));
                        arr.push(',');
                    }
                    if (this.Arguments) {
                        arr.push('"arguments": ');
                        for (let i = 0; i < this.Arguments.length; i++)
                            this.Arguments[i].toJson(arr);
                        arr.push(',');
                        ;
                        arr.push(',');
                    }
                    if (this.ReturnType) {
                        arr.push('"returnType": ');
                        for (let i = 0; i < this.ReturnType.length; i++)
                            arr.push(JSON.stringify(this.ReturnType[i]));
                        arr.push(',');
                        ;
                        arr.push(',');
                    }
                    if (this.HttpMethod) {
                        arr.push('"httpMethod": ');
                        arr.push(JSON.stringify(this.HttpMethod));
                        arr.push(',');
                    }
                    if (this.HttpBaseAddress) {
                        arr.push('"httpBaseAddress": ');
                        arr.push(JSON.stringify(this.HttpBaseAddress));
                        arr.push(',');
                    }
                    if (this.HttpPath) {
                        arr.push('"httpPath": ');
                        arr.push(JSON.stringify(this.HttpPath));
                        arr.push(',');
                    }
                    if (arr[arr.length - 1] == ',')
                        arr[arr.length - 1] = '}';
                    else
                        arr.push('}');
                    if (returnString)
                        return arr.join("");
                    return null;
                }
            }
            Code.CodeMethod = CodeMethod;
            /**
             *
             */
            class CodeMethodArg {
                constructor(obj) {
                    /**
                     *
                     */
                    this.Description = null;
                    /**
                     *
                     */
                    this.Name = null;
                    /**
                     *
                     */
                    this.ArgType = null;
                    /**
                     *
                     */
                    this.Optional = null;
                    /**
                     *
                     */
                    this.HttpName = null;
                    /**
                     *
                     */
                    this.HttpLocation = null;
                    for (let prop in obj) {
                        switch (prop) {
                            case "description":
                                if (obj.description) {
                                    this.Description = obj.description;
                                }
                                break;
                            case "name":
                                if (obj.name) {
                                    this.Name = obj.name;
                                }
                                break;
                            case "argType":
                                if (obj.argType) {
                                    this.ArgType = Array.from(obj.argType).map(o => o);
                                }
                                break;
                            case "optional":
                                if (obj.optional) {
                                    this.Optional = [true, 'true', 1].some(o => o === obj.optional);
                                }
                                break;
                            case "httpName":
                                if (obj.httpName) {
                                    this.HttpName = obj.httpName;
                                }
                                break;
                            case "httpLocation":
                                if (obj.httpLocation) {
                                    this.HttpLocation = obj.httpLocation;
                                }
                                break;
                        }
                    }
                }
                toJson(arr) {
                    let returnString = false;
                    if (arr == null) {
                        arr = [];
                        returnString = true;
                    }
                    arr.push('{');
                    if (this.Description) {
                        arr.push('"description": ');
                        arr.push(JSON.stringify(this.Description));
                        arr.push(',');
                    }
                    if (this.Name) {
                        arr.push('"name": ');
                        arr.push(JSON.stringify(this.Name));
                        arr.push(',');
                    }
                    if (this.ArgType) {
                        arr.push('"argType": ');
                        for (let i = 0; i < this.ArgType.length; i++)
                            arr.push(JSON.stringify(this.ArgType[i]));
                        arr.push(',');
                        ;
                        arr.push(',');
                    }
                    if (this.Optional) {
                        arr.push('"optional": ');
                        arr.push(JSON.stringify(this.Optional));
                        arr.push(',');
                    }
                    if (this.HttpName) {
                        arr.push('"httpName": ');
                        arr.push(JSON.stringify(this.HttpName));
                        arr.push(',');
                    }
                    if (this.HttpLocation) {
                        arr.push('"httpLocation": ');
                        arr.push(JSON.stringify(this.HttpLocation));
                        arr.push(',');
                    }
                    if (arr[arr.length - 1] == ',')
                        arr[arr.length - 1] = '}';
                    else
                        arr.push('}');
                    if (returnString)
                        return arr.join("");
                    return null;
                }
            }
            Code.CodeMethodArg = CodeMethodArg;
            /**
             *
             */
            class CodeNamespace {
                constructor(obj) {
                    /**
                     *
                     */
                    this.Name = null;
                    /**
                     *
                     */
                    this.Namespaces = null;
                    /**
                     *
                     */
                    this.Interfaces = null;
                    /**
                     *
                     */
                    this.Types = null;
                    for (let prop in obj) {
                        switch (prop) {
                            case "name":
                                if (obj.name) {
                                    this.Name = obj.name;
                                }
                                break;
                            case "namespaces":
                                if (obj.namespaces) {
                                    this.Namespaces = Array.from(obj.namespaces).map(o => new Abstractions.Types.Code.CodeNamespace(o));
                                }
                                break;
                            case "interfaces":
                                if (obj.interfaces) {
                                    this.Interfaces = Array.from(obj.interfaces).map(o => new Abstractions.Types.Code.CodeInterface(o));
                                }
                                break;
                            case "types":
                                if (obj.types) {
                                    this.Types = Array.from(obj.types).map(o => new Abstractions.Types.Code.CodeType(o));
                                }
                                break;
                        }
                    }
                }
                toJson(arr) {
                    let returnString = false;
                    if (arr == null) {
                        arr = [];
                        returnString = true;
                    }
                    arr.push('{');
                    if (this.Name) {
                        arr.push('"name": ');
                        arr.push(JSON.stringify(this.Name));
                        arr.push(',');
                    }
                    if (this.Namespaces) {
                        arr.push('"namespaces": ');
                        for (let i = 0; i < this.Namespaces.length; i++)
                            this.Namespaces[i].toJson(arr);
                        arr.push(',');
                        ;
                        arr.push(',');
                    }
                    if (this.Interfaces) {
                        arr.push('"interfaces": ');
                        for (let i = 0; i < this.Interfaces.length; i++)
                            this.Interfaces[i].toJson(arr);
                        arr.push(',');
                        ;
                        arr.push(',');
                    }
                    if (this.Types) {
                        arr.push('"types": ');
                        for (let i = 0; i < this.Types.length; i++)
                            this.Types[i].toJson(arr);
                        arr.push(',');
                        ;
                        arr.push(',');
                    }
                    if (arr[arr.length - 1] == ',')
                        arr[arr.length - 1] = '}';
                    else
                        arr.push('}');
                    if (returnString)
                        return arr.join("");
                    return null;
                }
            }
            Code.CodeNamespace = CodeNamespace;
            /**
             *
             */
            class CodeType {
                constructor(obj) {
                    /**
                     *
                     */
                    this.Description = null;
                    /**
                     *
                     */
                    this.Name = null;
                    /**
                     *
                     */
                    this.Extends = null;
                    /**
                     *
                     */
                    this.Properties = null;
                    for (let prop in obj) {
                        switch (prop) {
                            case "description":
                                if (obj.description) {
                                    this.Description = obj.description;
                                }
                                break;
                            case "name":
                                if (obj.name) {
                                    this.Name = obj.name;
                                }
                                break;
                            case "extends":
                                if (obj.extends) {
                                    this.Extends = Array.from(obj.extends).map(o => o);
                                }
                                break;
                            case "properties":
                                if (obj.properties) {
                                    this.Properties = Array.from(obj.properties).map(o => new Abstractions.Types.Code.CodeTypeProperty(o));
                                }
                                break;
                        }
                    }
                }
                toJson(arr) {
                    let returnString = false;
                    if (arr == null) {
                        arr = [];
                        returnString = true;
                    }
                    arr.push('{');
                    if (this.Description) {
                        arr.push('"description": ');
                        arr.push(JSON.stringify(this.Description));
                        arr.push(',');
                    }
                    if (this.Name) {
                        arr.push('"name": ');
                        arr.push(JSON.stringify(this.Name));
                        arr.push(',');
                    }
                    if (this.Extends) {
                        arr.push('"extends": ');
                        for (let i = 0; i < this.Extends.length; i++)
                            arr.push(JSON.stringify(this.Extends[i]));
                        arr.push(',');
                        ;
                        arr.push(',');
                    }
                    if (this.Properties) {
                        arr.push('"properties": ');
                        for (let i = 0; i < this.Properties.length; i++)
                            this.Properties[i].toJson(arr);
                        arr.push(',');
                        ;
                        arr.push(',');
                    }
                    if (arr[arr.length - 1] == ',')
                        arr[arr.length - 1] = '}';
                    else
                        arr.push('}');
                    if (returnString)
                        return arr.join("");
                    return null;
                }
            }
            Code.CodeType = CodeType;
            /**
             *
             */
            class CodeTypeProperty {
                constructor(obj) {
                    /**
                     *
                     */
                    this.Description = null;
                    /**
                     *
                     */
                    this.Name = null;
                    /**
                     *
                     */
                    this.PropertyType = null;
                    /**
                     *
                     */
                    this.HttpName = null;
                    for (let prop in obj) {
                        switch (prop) {
                            case "description":
                                if (obj.description) {
                                    this.Description = obj.description;
                                }
                                break;
                            case "name":
                                if (obj.name) {
                                    this.Name = obj.name;
                                }
                                break;
                            case "propertyType":
                                if (obj.propertyType) {
                                    this.PropertyType = Array.from(obj.propertyType).map(o => o);
                                }
                                break;
                            case "httpName":
                                if (obj.httpName) {
                                    this.HttpName = obj.httpName;
                                }
                                break;
                        }
                    }
                }
                toJson(arr) {
                    let returnString = false;
                    if (arr == null) {
                        arr = [];
                        returnString = true;
                    }
                    arr.push('{');
                    if (this.Description) {
                        arr.push('"description": ');
                        arr.push(JSON.stringify(this.Description));
                        arr.push(',');
                    }
                    if (this.Name) {
                        arr.push('"name": ');
                        arr.push(JSON.stringify(this.Name));
                        arr.push(',');
                    }
                    if (this.PropertyType) {
                        arr.push('"propertyType": ');
                        for (let i = 0; i < this.PropertyType.length; i++)
                            arr.push(JSON.stringify(this.PropertyType[i]));
                        arr.push(',');
                        ;
                        arr.push(',');
                    }
                    if (this.HttpName) {
                        arr.push('"httpName": ');
                        arr.push(JSON.stringify(this.HttpName));
                        arr.push(',');
                    }
                    if (arr[arr.length - 1] == ',')
                        arr[arr.length - 1] = '}';
                    else
                        arr.push('}');
                    if (returnString)
                        return arr.join("");
                    return null;
                }
            }
            Code.CodeTypeProperty = CodeTypeProperty;
            /**
             *
             */
            class NpmPackage {
                constructor(obj) {
                    /**
                     *
                     */
                    this.Name = null;
                    /**
                     *
                     */
                    this.Files = null;
                    for (let prop in obj) {
                        switch (prop) {
                            case "name":
                                if (obj.name) {
                                    this.Name = obj.name;
                                }
                                break;
                            case "files":
                                if (obj.files) {
                                    this.Files = Array.from(obj.files).map(o => new Abstractions.Types.Code.NpmPackageFile(o));
                                }
                                break;
                        }
                    }
                }
                toJson(arr) {
                    let returnString = false;
                    if (arr == null) {
                        arr = [];
                        returnString = true;
                    }
                    arr.push('{');
                    if (this.Name) {
                        arr.push('"name": ');
                        arr.push(JSON.stringify(this.Name));
                        arr.push(',');
                    }
                    if (this.Files) {
                        arr.push('"files": ');
                        for (let i = 0; i < this.Files.length; i++)
                            this.Files[i].toJson(arr);
                        arr.push(',');
                        ;
                        arr.push(',');
                    }
                    if (arr[arr.length - 1] == ',')
                        arr[arr.length - 1] = '}';
                    else
                        arr.push('}');
                    if (returnString)
                        return arr.join("");
                    return null;
                }
            }
            Code.NpmPackage = NpmPackage;
            /**
             *
             */
            class NpmPackageFile {
                constructor(obj) {
                    /**
                     *
                     */
                    this.FilePath = null;
                    /**
                     *
                     */
                    this.Content = null;
                    for (let prop in obj) {
                        switch (prop) {
                            case "filePath":
                                if (obj.filePath) {
                                    this.FilePath = obj.filePath;
                                }
                                break;
                            case "content":
                                if (obj.content) {
                                    this.Content = obj.content;
                                }
                                break;
                        }
                    }
                }
                toJson(arr) {
                    let returnString = false;
                    if (arr == null) {
                        arr = [];
                        returnString = true;
                    }
                    arr.push('{');
                    if (this.FilePath) {
                        arr.push('"filePath": ');
                        arr.push(JSON.stringify(this.FilePath));
                        arr.push(',');
                    }
                    if (this.Content) {
                        arr.push('"content": ');
                        arr.push(JSON.stringify(this.Content));
                        arr.push(',');
                    }
                    if (arr[arr.length - 1] == ',')
                        arr[arr.length - 1] = '}';
                    else
                        arr.push('}');
                    if (returnString)
                        return arr.join("");
                    return null;
                }
            }
            Code.NpmPackageFile = NpmPackageFile;
        })(Code = Types.Code || (Types.Code = {}));
        /**
         *
         */
        class FileContent {
            constructor(obj) {
                /**
                 *
                 */
                this.Content = null;
                /**
                 *
                 */
                this.CharSet = null;
                /**
                 *
                 */
                this.ContentType = null;
                /**
                 *
                 */
                this.FileName = null;
                /**
                 *
                 */
                this.LastModified = null;
                /**
                 *
                 */
                this.Location = null;
                /**
                 *
                 */
                this.ETag = null;
                for (let prop in obj) {
                    switch (prop) {
                        case "Content":
                            if (obj.Content) {
                                this.Content = new Uint8Array(obj.Content);
                            }
                            break;
                        case "CharSet":
                            if (obj.CharSet) {
                                this.CharSet = obj.CharSet;
                            }
                            break;
                        case "ContentType":
                            if (obj.ContentType) {
                                this.ContentType = obj.ContentType;
                            }
                            break;
                        case "FileName":
                            if (obj.FileName) {
                                this.FileName = obj.FileName;
                            }
                            break;
                        case "LastModified":
                            if (obj.LastModified) {
                                this.LastModified = new Date(obj.LastModified);
                            }
                            break;
                        case "Location":
                            if (obj.Location) {
                                this.Location = obj.Location;
                            }
                            break;
                        case "ETag":
                            if (obj.ETag) {
                                this.ETag = obj.ETag;
                            }
                            break;
                    }
                }
            }
            toJson(arr) {
                let returnString = false;
                if (arr == null) {
                    arr = [];
                    returnString = true;
                }
                arr.push('{');
                if (this.Content) {
                    arr.push('"Content": ');
                    arr.push(JSON.stringify(this.Content));
                    arr.push(',');
                }
                if (this.CharSet) {
                    arr.push('"CharSet": ');
                    arr.push(JSON.stringify(this.CharSet));
                    arr.push(',');
                }
                if (this.ContentType) {
                    arr.push('"ContentType": ');
                    arr.push(JSON.stringify(this.ContentType));
                    arr.push(',');
                }
                if (this.FileName) {
                    arr.push('"FileName": ');
                    arr.push(JSON.stringify(this.FileName));
                    arr.push(',');
                }
                if (this.LastModified) {
                    arr.push('"LastModified": ');
                    arr.push(JSON.stringify(this.LastModified));
                    arr.push(',');
                }
                if (this.Location) {
                    arr.push('"Location": ');
                    arr.push(JSON.stringify(this.Location));
                    arr.push(',');
                }
                if (this.ETag) {
                    arr.push('"ETag": ');
                    arr.push(JSON.stringify(this.ETag));
                    arr.push(',');
                }
                if (arr[arr.length - 1] == ',')
                    arr[arr.length - 1] = '}';
                else
                    arr.push('}');
                if (returnString)
                    return arr.join("");
                return null;
            }
        }
        Types.FileContent = FileContent;
        /**
         *
         */
        class NameValuePair {
            constructor(obj) {
                /**
                 *
                 */
                this.Name = null;
                /**
                 *
                 */
                this.Value = null;
                for (let prop in obj) {
                    switch (prop) {
                        case "Name":
                            if (obj.Name) {
                                this.Name = obj.Name;
                            }
                            break;
                        case "Value":
                            if (obj.Value) {
                                this.Value = obj.Value;
                            }
                            break;
                    }
                }
            }
            toJson(arr) {
                let returnString = false;
                if (arr == null) {
                    arr = [];
                    returnString = true;
                }
                arr.push('{');
                if (this.Name) {
                    arr.push('"Name": ');
                    arr.push(JSON.stringify(this.Name));
                    arr.push(',');
                }
                if (this.Value) {
                    arr.push('"Value": ');
                    arr.push(JSON.stringify(this.Value));
                    arr.push(',');
                }
                if (arr[arr.length - 1] == ',')
                    arr[arr.length - 1] = '}';
                else
                    arr.push('}');
                if (returnString)
                    return arr.join("");
                return null;
            }
        }
        Types.NameValuePair = NameValuePair;
        /**
         *
         */
        class SolidRpcHostInstance {
            constructor(obj) {
                /**
                 *
                 */
                this.HostId = null;
                /**
                 *
                 */
                this.Started = null;
                /**
                 *
                 */
                this.LastAlive = null;
                /**
                 *
                 */
                this.BaseAddress = null;
                /**
                 *
                 */
                this.HttpCookies = null;
                for (let prop in obj) {
                    switch (prop) {
                        case "HostId":
                            if (obj.HostId) {
                                this.HostId = obj.HostId;
                            }
                            break;
                        case "Started":
                            if (obj.Started) {
                                this.Started = new Date(obj.Started);
                            }
                            break;
                        case "LastAlive":
                            if (obj.LastAlive) {
                                this.LastAlive = new Date(obj.LastAlive);
                            }
                            break;
                        case "BaseAddress":
                            if (obj.BaseAddress) {
                                this.BaseAddress = obj.BaseAddress;
                            }
                            break;
                        case "HttpCookies":
                            if (obj.HttpCookies) {
                                this.HttpCookies = obj.HttpCookies;
                            }
                            break;
                    }
                }
            }
            toJson(arr) {
                let returnString = false;
                if (arr == null) {
                    arr = [];
                    returnString = true;
                }
                arr.push('{');
                if (this.HostId) {
                    arr.push('"HostId": ');
                    arr.push(JSON.stringify(this.HostId));
                    arr.push(',');
                }
                if (this.Started) {
                    arr.push('"Started": ');
                    arr.push(JSON.stringify(this.Started));
                    arr.push(',');
                }
                if (this.LastAlive) {
                    arr.push('"LastAlive": ');
                    arr.push(JSON.stringify(this.LastAlive));
                    arr.push(',');
                }
                if (this.BaseAddress) {
                    arr.push('"BaseAddress": ');
                    arr.push(JSON.stringify(this.BaseAddress));
                    arr.push(',');
                }
                if (this.HttpCookies) {
                    arr.push('"HttpCookies": ');
                    arr.push(JSON.stringify(this.HttpCookies));
                    arr.push(',');
                }
                if (arr[arr.length - 1] == ',')
                    arr[arr.length - 1] = '}';
                else
                    arr.push('}');
                if (returnString)
                    return arr.join("");
                return null;
            }
        }
        Types.SolidRpcHostInstance = SolidRpcHostInstance;
    })(Types = Abstractions.Types || (Abstractions.Types = {}));
})(Abstractions = exports.Abstractions || (exports.Abstractions = {}));
