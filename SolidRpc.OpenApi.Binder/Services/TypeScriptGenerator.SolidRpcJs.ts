import { default as axios, AxiosRequestConfig, CancelToken, Method } from 'axios';
import { default as CancellationToken } from 'cancellationtoken';
import { Observable, Subscriber, Subject } from 'rxjs';

export namespace SolidRpcJs {
    // the global namespace
    var rootNS: Namespace;

    /** Represents a a namespace */
    export class Namespace {
        private modifiedCallbacks: { (): void }[] = [];

        /**
         * Constructs a new namespace
         * @param parent
         * @param name
         */
        constructor(parent: Namespace | null, name: string) {
            this.parent = parent;
            this.name = name;
        }

        /** The namespace name */
        name: string;

        /** The parent namespace */
        parent: Namespace | null;

        /** Adds a callback that is invoked when the scope is modified*/
        addModifiedCallback(callback: { (): void }): Namespace {
            this.modifiedCallbacks.push(callback);
            return this;
        };

        /** Invoke this method to signal all the callbacks*/
        modified(): Namespace {
            this.modifiedCallbacks.forEach(o => o());
            return this;
        };

        /** Declare a child namespace*/
        declareNamespace(ns: string): Namespace {
            var scope = this as any;
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
        declareVariable(name: string): any {
            let x = (this as any)[name];
            if (x === undefined) {
                (this as any)[name] = x = {};
            }
            return x;
        }
        /** The string value for supplied key */
        getStringValue(key: string, defaultValue: string): string {
            let wrk: Namespace | null = this;
            let val = (wrk as any)[key];
            wrk = wrk.parent;
            while (val === undefined && wrk != null) {
                val = (wrk as any)[key];
                wrk = wrk.parent;
            }
            if (val !== undefined) {
                return val;
            }
            return defaultValue;
        }
        /** The string value for supplied key */
        setStringValue(key: string, value: string) {
            (this as any)[key] = value;
        }
    }

    function GetRootNamespace(): Namespace {
        if (typeof rootNS === 'undefined') {
            if (typeof window === 'undefined') {
                rootNS = new Namespace(null, '');
            } else {
                rootNS = (window as any)['_solidrpc_rootns_'];
                if (rootNS === undefined) {
                    rootNS = new Namespace(null, '');
                    (window as any)['_solidrpc_rootns_'] = rootNS;
                }
            }
        }
        return rootNS;
    }

    /** The root namespace */
    export var rootNamespace = GetRootNamespace();

    /**
     * invokes the specified function if supplied argument has a value.
     * @param input the value to check for null
     * @param onnotnull the function that converts non null values
     */
    export function ifnull(input: any, onnull: () => void, onnotnull: (input: any) => void)  {
        if (input === null) { onnull(); return; }
        if (input === undefined) { onnull(); return; }
        return onnotnull(input);
    }

    /**
      * invokes the specified function if supplied argument has a value.
      * @param input the value to check for null
      * @param onnotnull the function that converts non null values
      */
    export function ifnotnull<T>(input: any, onnotnull: (input: any) => T): T | null {
        if (input === null) return null;
        if (input === undefined) return null;
        return onnotnull(input);
    }

     /**
     * Encodes the supplied uri value
     * @param input the value to encode
     */
    export function encodeUriValue(input: string): string {
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
    export function toJson(input: any): string | null {
        if (input && input.toJson) {
            return input.toJson();
        }
        return JSON.stringify(input);
    }

    /**
     * Resets the registered callbacks
     * Use this to modify the uri or headers.
     * @param uri the uri to convert
     */
    export function resetPreFlight(): void {
        rootPreFlight = function (req: RpcServiceRequest, cont: () => void): void { cont(); }
    }

    /**
     * Use this method to register callbacks prior to sending a request to the service
     * Use this to modify the uri or headers.
     * @param uri the uri to convert
     */
    export function addPreFlight(newPreFlight: (req: RpcServiceRequest, cont: () => void) => void): void {
        var oldPreFlight = rootPreFlight;
        rootPreFlight = function (req: RpcServiceRequest, cont: () => void): void { oldPreFlight(req, () => { newPreFlight(req, () => cont()); }); }
    }

    var rootPreFlight: (req: RpcServiceRequest, cont: () => void) => void = function (req: RpcServiceRequest, cont: () => void): void { cont(); }

    /** Represents a service request */
    export class RpcServiceRequest {
        constructor(method: string, url: string, query: any, headers: any, data: any) {
            this.method = method;
            this.url = url;
            this.query = query;
            this.headers = headers;
            this.data = data;
        }

        /** The method - GET, POST, etc */
        method: string;

        /** The uri */
        url: string;

        /** the query */
        query: any;

        /** the headers */
        headers: any;

        /** the data */
        data: any;

        /** returns the url(with query string) */
        getUrl(): string {
            //
            // setup query string
            //
            let sQuery = '';
            for (const property in this.query) {
                sQuery += `&${property}=${SolidRpcJs.encodeUriValue(this.query[property])}`
            }

            if (sQuery.length > 0) {
                return this.url + '?' + sQuery.substr(1)
            }

            return this.url;
        }
   }

    /** Represents a service request */
    export class RpcServiceRequestTyped<T> extends RpcServiceRequest {
        constructor(method: string, uri: string, query: any, headers: any, data: any, cancellationToken: CancellationToken | undefined, conv: (status: number, body: any) => T, bcast: Subject<T>) {
            super(method, uri, query, headers, data);
            this.cancellationToken = cancellationToken;
            this.conv = conv;
            this.bcast = bcast;
            if (this.headers == null) {
                this.headers = {};
            }
        }
        /** the cancellation token */
        cancellationToken: CancellationToken | undefined;

        /** the converter*/
        conv: (status: number, body: any) => T;

        /** the broadcast subject*/
        bcast: Subject<T>;

        /** invokes the request */
        invoke(): Observable<T> {
            return Observable.create((subscriber: Subscriber<T>) => {
                //
                // setup cancellation token
                //
                let cancelToken: CancelToken;
                let source = axios.CancelToken.source();
                if (this.cancellationToken) {
                    this.cancellationToken.onCancelled(reason => { source.cancel(reason); });
                }
                cancelToken = source.token;

                // transform using registered preflight methods
                rootPreFlight(this, () => {

                    let axiosMethod: Method | undefined = undefined;
                    switch (this.method) {
                        case 'get': axiosMethod = 'get'; break;
                        case 'delete': axiosMethod = 'delete'; break;
                        case 'head': axiosMethod = 'head'; break;
                        case 'options': axiosMethod = 'options'; break;
                        case 'post': axiosMethod = 'post'; break;
                        case 'put': axiosMethod = 'put'; break;
                        case 'patch': axiosMethod = 'patch'; break;
                        case 'purge': axiosMethod = 'purge'; break;
                        case 'link': axiosMethod = 'link'; break;
                        case 'unlink': axiosMethod = 'unlink'; break;
                    }

                    //
                    // send request
                    //
                    let requestConfig: AxiosRequestConfig = {
                        method: axiosMethod,
                        url: this.getUrl(),
                        headers: this.headers,
                        data: this.data,
                        cancelToken: cancelToken,
                        transformRequest: [
                            (data, headers) => {
                                return data;
                            },
                        ]
                    }
                    axios.request<any>(requestConfig)
                        .then(resp => {
                            try {
                                let converted = this.conv(resp.status, resp.data);
                                subscriber.next(converted);
                                this.bcast.next(converted);
                            } catch (err) {
                                console.error('err:' + err);
                                subscriber.error(err);
                            }
                        }).catch(err => {
                            console.error('err:' + err);
                            subscriber.error(err);
                            this.bcast.error(err);
                        }).finally(() => {
                            subscriber.complete();
                        });

                });
                return () => { };
            });

        }
    }
};
