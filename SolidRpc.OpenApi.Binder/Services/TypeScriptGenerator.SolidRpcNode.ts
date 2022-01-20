import { SolidRpcJs } from 'solidrpcjs';
import { Abstractions } from 'solidrpc-abstractions';
import http from 'http';
import open from 'open';
import fs from 'fs';
import net from 'net';

process.env.NODE_TLS_REJECT_UNAUTHORIZED = '0';

let accessToken: string | null = null;
let globalCont = function () { };
let httpSrv: http.Server;
const sockets = new Set<net.Socket>();

function close() {
    for (const socket of sockets) {
        socket.destroy();
        sockets.delete(socket);
    }
    httpSrv.close();
};
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

            close();
            globalCont();
        });

        httpSrv.on('connection', (socket) => {
            sockets.add(socket);
            httpSrv.once('close', () => {
                sockets.delete(socket);
            });
        });

        // Start the server on port 3000
        httpSrv.listen(httpSrvPort, '127.0.0.1');
    }
}

export namespace SolidRpcNode {
    /**
     * Adds the preflight callbacks that the node process can intercept
     */
    export function addOAuth2PreFlightCallback() {
        SolidRpcJs.addPreFlight((req: SolidRpcJs.RpcServiceRequest, cont: () => void) => {
            let setAccessTokenAndContinue = () => {
                req.headers['Authorization'] = 'Bearer ' + accessToken;
                cont();
            }
            if (accessToken == null) {
                globalCont = setAccessTokenAndContinue
                let httpSrvPort = 3341;
                startHttpServerIfNotStarted(httpSrvPort);
                let callbackUri = 'http://localhost:' + httpSrvPort + '/test';
                let state = 'my state 2';
                let url = Abstractions.Services.ISolidRpcOAuth2.GetAuthorizationCodeTokenAsync(callbackUri, state).getUrl();
                console.log("Opening url:" + url)
                open(url);
            } else {
                setAccessTokenAndContinue();
            }
        });
    }

    /**
     * Generates the packages
     * @param packageNames the packages to generate
     */
    export function generatePackages(packageNames: string[]) {
        Abstractions.Services.Code.INpmGenerator.CreateNpmPackage(packageNames).invoke().subscribe(packages => {
            packages.forEach(p => {
                console.log(p.Name);
                let packageDir = 'generated/' + p.Name;
                if (!fs.existsSync(packageDir)) {
                    fs.mkdir(packageDir, err => { if (err) throw err; });
                }
                if (p.Files != null) {
                    p.Files.forEach(file => {
                        if (file.Content == null) file.Content = '';
                        let fileName = packageDir + '/' + file.FilePath;
                        console.log('writing to file ' + fileName)
                        fs.writeFile(fileName, file.Content, err => { if (err) throw err; })
                    })
                }
            });
        });
    }
};
