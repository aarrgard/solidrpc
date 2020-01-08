window.solidRpcDoMicrosoftLogin = function() {
    document.location = '{authorizeEndpoint}';
}

window.solidRpcDoMicrosoftLogout = function(accessToken) {
    console.log('logging out from microsoft');
}