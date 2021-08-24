import { default as axios, AxiosRequestConfig, CancelToken, Method } from 'axios';
import { default as CancellationToken } from 'cancellationtoken';
import { Observable, Subscriber, Subject } from 'rxjs';

export namespace SolidRpcJs {
    // the global namespace
    var rootNS: Namespace;

    /**
     * The actual transformer that is used to transform uris before invocation.
     * @param uri the uri to convert
     */
    export var uriTransformer = (s: string): string => s;

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

    /** The default headers */
    export var defaultHeaders: any = rootNamespace.declareNamespace('SolidRpc').declareVariable('defaultHeaders');

    export class RpcServiceImpl {
        /**
         * Encodes the supplied uri value
         * @param input the value to encode
         */
        enocodeUriValue(input: string): string {
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
        toJson(input: any): string | null {
            if (input && input.toJson) {
                return input.toJson();
            }
            return null;
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
        request<T>(method: string, uri: string, query: any, headers: any, data: any, cancellationToken: CancellationToken | undefined, conv: (status: number, body: any) => T, bcast: Subject<T>): Observable<T> {
            return Observable.create((subscriber: Subscriber<T>) => {
                //
                // setup cancellation token
                //
                let cancelToken: CancelToken;
                let source = axios.CancelToken.source();
                if (cancellationToken) {
                    cancellationToken.onCancelled(reason => { source.cancel(reason); });
                }
                cancelToken = source.token;

                //
                // setup query string
                //
                let sQuery = '';
                for (const property in query) {
                    sQuery += `&${property}=${this.enocodeUriValue(query[property])}`
                }

                if (sQuery.length > 0) {
                    uri = uri + '?' + sQuery.substr(1)
                }

                //
                // add default headers
                //
                for (let h in defaultHeaders) {
                    if (headers == null) headers = {};
                    headers[h] = defaultHeaders[h];
                }

                let axiosMethod: Method | undefined = undefined;
                switch (method) {
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
                    url: uri,
                    headers: headers,
                    data: data,
                    cancelToken: cancelToken
                }
                axios.request<any>(requestConfig)
                    .then(resp => {
                        try {
                            let converted = conv(resp.status, resp.data);
                            subscriber.next(converted);
                            bcast.next(converted);
                        } catch (err) {
                            subscriber.error(err);
                        }
                    }).catch(err => {
                        subscriber.error(err);
                        bcast.error(err);
                    }).finally(() => {
                        subscriber.complete();
                    });
                return () => { };
            });
        }
    };
};
