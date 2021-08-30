const solidrpc = require('solidrpc')
const abstr = require('solidrpc.abstractions')
const http = require('http');
const open = require('open');

let accessToken = '';
let globalCont = function () { };
function getParameterByName(url, name) {
    var match = RegExp('[?&]' + name + '=([^&]*)')
        .exec(url);
    return match ?
        decodeURIComponent(match[1].replace(/\+/g, ' '))
        : null;
}

// Create an instance of the http server to handle HTTP requests
var sockets = []
let srv = http.createServer((req, res) => {
    // Set a response type of plain text for the response
    res.writeHead(200, { 'Content-Type': 'text/html', 'Connection': 'close' });
    req.connection.setTimeout(1);
    sockets.push(req.connection);

    // Send back a response and end the connection
    res.end('<html><body><script>window.close();</script>You may close this window...</body></html>\n');

    // get the access token
    accessToken = getParameterByName(req.url, 'access_token');
    globalCont();

    sockets.push(req.connection);
});

// Start the server on port 3000
srv.listen(3000, '127.0.0.1');

solidrpc.SolidRpcJs.AddPreFlight((req, cont) => {
    let setAccessTokenAndContinue = () => {
        req.headers['Authorization'] = 'Bearer ' + accessToken;
        cont();
    }
    if (accessToken == '') {
        globalCont = setAccessTokenAndContinue
        open('http://localhost:7071/front/SolidRpc/Abstractions/Services/ISolidRpcOAuth2/GetAuthorizationCodeTokenAsync?callbackUri=http%3A%2F%2Flocalhost%3A3000%2Ftest&state=my%20state');
    } else {
        setAccessTokenAndContinue();
    }
});

abstr.Abstractions.Services.SolidRpcHostInstance.GetHostIdObservable.subscribe({
    next: x => { console.log('1:' + x); },
    error: e => { console.log('2:' + e); },
});

(async function () {
    //await Issuer.discover('https://accounts.google.com').then(function (googleIssuer) {
    //    console.log('Discovered issuer %s %O', googleIssuer.issuer, googleIssuer.metadata);
    //});

    var hostId = await abstr.Abstractions.Services.SolidRpcHostInstance.GetHostId().toPromise();
    console.log('3:' + hostId);
    process.exit(0);
})();
