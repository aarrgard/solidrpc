"use strict";
var __importDefault = (this && this.__importDefault) || function (mod) {
    return (mod && mod.__esModule) ? mod : { "default": mod };
};
Object.defineProperty(exports, "__esModule", { value: true });
exports.SolidRpcNode = void 0;
const solidrpcjs_1 = require("solidrpcjs");
const http_1 = __importDefault(require("http"));
const open_1 = __importDefault(require("open"));
let accessToken = null;
let globalCont = function () { };
let httpSrv;
function getParameterByName(url, name) {
    if (url == undefined)
        return null;
    var match = RegExp('[?&]' + name + '=([^&]*)')
        .exec(url);
    return match ?
        decodeURIComponent(match[1].replace(/\+/g, ' '))
        : null;
}
function startHttpServerIfNotStarted(httpSrvPort) {
    if (httpSrv === undefined) {
        // Create an instance of the http server to handle HTTP requests
        httpSrv = http_1.default.createServer((req, res) => {
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
var SolidRpcNode;
(function (SolidRpcNode) {
    function addOAuth2PreFlightCallback(srvBaseUrl) {
        solidrpcjs_1.SolidRpcJs.AddPreFlight((req, cont) => {
            let setAccessTokenAndContinue = () => {
                req.headers['Authorization'] = 'Bearer ' + accessToken;
                cont();
            };
            if (accessToken == null) {
                globalCont = setAccessTokenAndContinue;
                let httpSrvPort = 3341;
                startHttpServerIfNotStarted(httpSrvPort);
                let callbackUri = 'http%3A%2F%2Flocalhost%3A' + httpSrvPort + '%2Ftest';
                let state = 'my state 2';
                (0, open_1.default)(srvBaseUrl + '/SolidRpc/Abstractions/Services/ISolidRpcOAuth2/GetAuthorizationCodeTokenAsync?callbackUri=' + callbackUri + '&state=' + encodeURIComponent(state));
            }
            else {
                setAccessTokenAndContinue();
            }
        });
    }
    SolidRpcNode.addOAuth2PreFlightCallback = addOAuth2PreFlightCallback;
})(SolidRpcNode = exports.SolidRpcNode || (exports.SolidRpcNode = {}));
;
