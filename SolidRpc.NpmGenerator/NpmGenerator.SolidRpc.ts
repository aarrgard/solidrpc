import { default as axios, AxiosRequestConfig, CancelToken } from 'axios';
import { default as CancellationToken } from 'cancellationtoken';
import { Observable, Subscriber } from 'rxjs';
import { stringify } from 'qs';

export namespace SolidRpc {
    /**
     * The actual transformer that is used to transform uris before invocation.
     * @param uri the uri to convert
     */
    export var uriTransformer = (s: string): string => s;

    /** The default headers */
    export var defaultHeaders: any = {};

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
        request<T>(method: string, uri: string, query: any, headers: any, data: any, cancellationToken: CancellationToken, conv: (status: number, body: any) => T): Observable<T> {
            return Observable.create((subscriber: Subscriber<T>) => {
                let cancelToken: CancelToken;
                if (cancellationToken) {
                    let source = axios.CancelToken.source();
                    cancellationToken.onCancelled(reason => { source.cancel(reason); });
                    cancelToken = source.token;
                }
                let sQuery = stringify(query, { addQueryPrefix: true });
                if (sQuery !== "?") {
                    uri = uri + sQuery;
                }
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
                            subscriber.next(conv(resp.status, resp.data));
                        } catch (err) {
                            subscriber.error(err);
                        }
                    }).catch(err => {
                        subscriber.error(err);
                    }).finally(() => {
                        subscriber.complete();
                    });
                return () => { };
            });
        }
    };
};
