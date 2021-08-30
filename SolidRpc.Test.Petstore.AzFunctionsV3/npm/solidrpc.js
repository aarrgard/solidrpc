const { SolidRpcJs  } = require('solidrpcjs')
const { SolidRpcNode } = require('solidrpcnode')
const abstr = require('solidrpc.abstractions')

SolidRpcNode.addOAuth2PreFlightCallback('http://localhost:7071/front');

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

function wait() {
    setTimeout(wait, 1000);
}
wait();
