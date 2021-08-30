"use strict";
var __importDefault = (this && this.__importDefault) || function (mod) {
    return (mod && mod.__esModule) ? mod : { "default": mod };
};
Object.defineProperty(exports, "__esModule", { value: true });
exports.SolidRpcJs = void 0;
const axios_1 = __importDefault(require("axios"));
const rxjs_1 = require("rxjs");
var SolidRpcJs;
(function (SolidRpcJs) {
    // the global namespace
    var rootNS;
    /** Represents a a namespace */
    class Namespace {
        /**
         * Constructs a new namespace
         * @param parent
         * @param name
         */
        constructor(parent, name) {
            this.modifiedCallbacks = [];
            this.parent = parent;
            this.name = name;
        }
        /** Adds a callback that is invoked when the scope is modified*/
        addModifiedCallback(callback) {
            this.modifiedCallbacks.push(callback);
            return this;
        }
        ;
        /** Invoke this method to signal all the callbacks*/
        modified() {
            this.modifiedCallbacks.forEach(o => o());
            return this;
        }
        ;
        /** Declare a child namespace*/
        declareNamespace(ns) {
            var scope = this;
            ns.split('.').forEach(o => {
                var childNs = scope[o];
                if (childNs == undefined) {
                    childNs = new Namespace(scope, o);
                    scope[o] = childNs;
                }
                scope = childNs;
            });
            return scope;
        }
        /** Declare a child namespace*/
        declareVariable(name) {
            let x = this[name];
            if (x === undefined) {
                this[name] = x = {};
            }
            return x;
        }
    }
    SolidRpcJs.Namespace = Namespace;
    function GetRootNamespace() {
        if (typeof rootNS === 'undefined') {
            if (typeof window === 'undefined') {
                rootNS = new Namespace(null, '');
            }
            else {
                rootNS = window['_solidrpc_rootns_'];
                if (rootNS === undefined) {
                    rootNS = new Namespace(null, '');
                    window['_solidrpc_rootns_'] = rootNS;
                }
            }
        }
        return rootNS;
    }
    /** The root namespace */
    SolidRpcJs.rootNamespace = GetRootNamespace();
    /**
     * Resets the registered callbacks
     * Use this to modify the uri or headers.
     * @param uri the uri to convert
     */
    function ResetPreFlight() {
        rootPreFlight = function (req, cont) { cont(); };
    }
    SolidRpcJs.ResetPreFlight = ResetPreFlight;
    /**
     * Use this method to register callbacks prior to sending a request to the service
     * Use this to modify the uri or headers.
     * @param uri the uri to convert
     */
    function AddPreFlight(newPreFlight) {
        var oldPreFlight = rootPreFlight;
        rootPreFlight = function (req, cont) { oldPreFlight(req, () => { newPreFlight(req, () => cont()); }); };
    }
    SolidRpcJs.AddPreFlight = AddPreFlight;
    var rootPreFlight = function (req, cont) { cont(); };
    /** Represents a service request */
    class RpcServiceRequest {
        constructor(method, uri, query, headers, data) {
            this.method = method;
            this.uri = uri;
            this.query = query;
            this.headers = headers;
            this.data = data;
            if (this.headers == null) {
                this.headers = {};
            }
        }
    }
    SolidRpcJs.RpcServiceRequest = RpcServiceRequest;
    /** Base class for a service implementation */
    class RpcServiceImpl {
        /**
         * Encodes the supplied uri value
         * @param input the value to encode
         */
        enocodeUriValue(input) {
            input = encodeURI(input);
            input = input.replace(/\//g, '%2F');
            input = input.replace(/:/g, '%3a');
            input = input.replace(/\+/g, '%2b');
            return input;
        }
        /**
         * Encodes the supplied uri value
         * @param input the value to encode
         */
        toJson(input) {
            if (input && input.toJson) {
                return input.toJson();
            }
            return input;
        }
        /**
         * Performs the underlying request
         * @param method
         * @param uri
         * @param query
         * @param headers
         * @param body
         * @param cancellationToken
         * @param conv
         */
        request(req, cancellationToken, conv, bcast) {
            return rxjs_1.Observable.create((subscriber) => {
                //
                // setup cancellation token
                //
                let cancelToken;
                let source = axios_1.default.CancelToken.source();
                if (cancellationToken) {
                    cancellationToken.onCancelled(reason => { source.cancel(reason); });
                }
                cancelToken = source.token;
                //
                // setup query string
                //
                let sQuery = '';
                for (const property in req.query) {
                    sQuery += `&${property}=${this.enocodeUriValue(req.query[property])}`;
                }
                if (sQuery.length > 0) {
                    req.uri = req.uri + '?' + sQuery.substr(1);
                }
                // transform using registered preflight methods
                rootPreFlight(req, () => {
                    let axiosMethod = undefined;
                    switch (req.method) {
                        case 'get':
                            axiosMethod = 'get';
                            break;
                        case 'delete':
                            axiosMethod = 'delete';
                            break;
                        case 'head':
                            axiosMethod = 'head';
                            break;
                        case 'options':
                            axiosMethod = 'options';
                            break;
                        case 'post':
                            axiosMethod = 'post';
                            break;
                        case 'put':
                            axiosMethod = 'put';
                            break;
                        case 'patch':
                            axiosMethod = 'patch';
                            break;
                        case 'purge':
                            axiosMethod = 'purge';
                            break;
                        case 'link':
                            axiosMethod = 'link';
                            break;
                        case 'unlink':
                            axiosMethod = 'unlink';
                            break;
                    }
                    //
                    // send request
                    //
                    let requestConfig = {
                        method: axiosMethod,
                        url: req.uri,
                        headers: req.headers,
                        data: req.data,
                        cancelToken: cancelToken
                    };
                    axios_1.default.request(requestConfig)
                        .then(resp => {
                        try {
                            let converted = conv(resp.status, resp.data);
                            subscriber.next(converted);
                            bcast.next(converted);
                        }
                        catch (err) {
                            subscriber.error(err);
                        }
                    }).catch(err => {
                        subscriber.error(err);
                        bcast.error(err);
                    }).finally(() => {
                        subscriber.complete();
                    });
                });
                return () => { };
            });
        }
    }
    SolidRpcJs.RpcServiceImpl = RpcServiceImpl;
    ;
})(SolidRpcJs = exports.SolidRpcJs || (exports.SolidRpcJs = {}));
;
