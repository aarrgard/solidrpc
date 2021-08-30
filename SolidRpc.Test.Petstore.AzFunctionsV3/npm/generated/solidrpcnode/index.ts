import { SolidRpcJs } from 'solidrpcjs';
import http from 'http';
import open from 'open';

let accessToken: string | null = null;
let globalCont = function () { };
let httpSrv: http.Server;

function getParameterByName(url: string | undefined, name: string): string | null {
    if (url == undefined) return null;
    var match = RegExp('[?&]' + name + '=([^&]*)')
        .exec(url);
    return match ?
        decodeURIComponent(match[1].replace(/\+/g, ' '))
        : null;
}

function startHttpServerIfNotStarted(httpSrvPort: number) {
    if (httpSrv === undefined) {
        // Create an instance of the http server to handle HTTP requests
        httpSrv = http.createServer((req, res) => {
            // Set a response type of plain text for the response
            res.writeHead(200, { 'Content-Type': 'text/html', 'Connection': 'close' });
            req.connection.setTimeout(1);

            // Send back a response and end the connection
            res.end('<html><body><script>window.close();</script>You may close this window...</body></html>\n');

            // get the access token
            accessToken = getParameterByName(req.url, 'access_token');
            globalCont();
        });

        // Start the server on port 3000
        httpSrv.listen(httpSrvPort, '127.0.0.1');
    }
}

export namespace SolidRpcNode {
    export function addOAuth2PreFlightCallback(srvBaseUrl: string) {
        SolidRpcJs.AddPreFlight((req: SolidRpcJs.RpcServiceRequest, cont: () => void) => {
            let setAccessTokenAndContinue = () => {
                req.headers['Authorization'] = 'Bearer ' + accessToken;
                cont();
            }
            if (accessToken == null) {
                globalCont = setAccessTokenAndContinue
                let httpSrvPort = 3341;
                startHttpServerIfNotStarted(httpSrvPort);
                let callbackUri: string = 'http%3A%2F%2Flocalhost%3A' + httpSrvPort + '%2Ftest';
                let state = 'my state 2';
                open(srvBaseUrl + '/SolidRpc/Abstractions/Services/ISolidRpcOAuth2/GetAuthorizationCodeTokenAsync?callbackUri=' + callbackUri + '&state=' + encodeURIComponent(state));
            } else {
                setAccessTokenAndContinue();
            }
        });

    }
};
