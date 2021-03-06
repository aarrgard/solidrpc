﻿import { default as axios, AxiosRequestConfig, CancelToken } from 'axios';
import { default as CancellationToken } from 'cancellationtoken';
import { Observable, Subscriber, Subject } from 'rxjs';
import { stringify } from 'qs';

export namespace SolidRpc {
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
        constructor(parent: Namespace, name: string) {
            this.parent = parent;
            this.name = name;
        }

        /** The namespace name */
        name: string;

        /** The parent namespace */
        parent: Namespace;

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
        var rootNs = (window as any)['_solidrpc_rootns_'];
        if (rootNs === undefined) {
            rootNs = new Namespace(null, '');
            (window as any)['_solidrpc_rootns_'] = rootNs;
        }
        return rootNs;
    }
    /** The root namespace */
    export var rootNamespace = GetRootNamespace();

    /** The default headers */
    export var defaultHeaders: any = rootNamespace.declareNamespace('SolidRpc').declareVariable('defaultHeaders');

    export class RpcServiceImpl {
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
        request<T>(method: string, uri: string, query: any, headers: any, data: any, cancellationToken: CancellationToken, conv: (status: number, body: any) => T, bcast: Subject<T>): Observable<T> {
            return Observable.create((subscriber: Subscriber<T>) => {
                //
                // setup cancellation token
                //
                let cancelToken: CancelToken;
                if (cancellationToken) {
                    let source = axios.CancelToken.source();
                    cancellationToken.onCancelled(reason => { source.cancel(reason); });
                    cancelToken = source.token;
                }

                //
                // setup query string
                //
                let sQuery = stringify(query, { addQueryPrefix: true });
                if (sQuery !== "?") {
                    uri = uri + sQuery;
                }

                //
                // add default headers
                //
                for (let h in defaultHeaders) {
                    if (headers == null) headers = {};
                    headers[h] = defaultHeaders[h];
                }

                //
                // send request
                //
                let requestConfig: AxiosRequestConfig = {
                    method: 'get',
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
