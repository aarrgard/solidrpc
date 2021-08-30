import { default as CancellationToken } from 'cancellationtoken';
import { Observable, Subject } from 'rxjs';
export declare namespace SolidRpcJs {
    /** Represents a a namespace */
    class Namespace {
        private modifiedCallbacks;
        /**
         * Constructs a new namespace
         * @param parent
         * @param name
         */
        constructor(parent: Namespace | null, name: string);
        /** The namespace name */
        name: string;
        /** The parent namespace */
        parent: Namespace | null;
        /** Adds a callback that is invoked when the scope is modified*/
        addModifiedCallback(callback: {
            (): void;
        }): Namespace;
        /** Invoke this method to signal all the callbacks*/
        modified(): Namespace;
        /** Declare a child namespace*/
        declareNamespace(ns: string): Namespace;
        /** Declare a child namespace*/
        declareVariable(name: string): any;
    }
    /** The root namespace */
    var rootNamespace: Namespace;
    /**
     * Resets the registered callbacks
     * Use this to modify the uri or headers.
     * @param uri the uri to convert
     */
    function ResetPreFlight(): void;
    /**
     * Use this method to register callbacks prior to sending a request to the service
     * Use this to modify the uri or headers.
     * @param uri the uri to convert
     */
    function AddPreFlight(newPreFlight: (req: RpcServiceRequest, cont: () => void) => void): void;
    /** Represents a service request */
    class RpcServiceRequest {
        constructor(method: string, uri: string, query: any, headers: any, data: any);
        /** The method - GET, POST, etc */
        method: string;
        /** The uri */
        uri: string;
        /** the query */
        query: any;
        /** the headers */
        headers: any;
        /** the data */
        data: any;
    }
    /** Base class for a service implementation */
    class RpcServiceImpl {
        /**
         * Encodes the supplied uri value
         * @param input the value to encode
         */
        enocodeUriValue(input: string): string;
        /**
         * Encodes the supplied uri value
         * @param input the value to encode
         */
        toJson(input: any): string | null;
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
        request<T>(req: RpcServiceRequest, cancellationToken: CancellationToken | undefined, conv: (status: number, body: any) => T, bcast: Subject<T>): Observable<T>;
    }
}
