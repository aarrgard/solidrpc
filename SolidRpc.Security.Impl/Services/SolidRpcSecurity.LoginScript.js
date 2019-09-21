
var solidRpcSecurity = SolidRpc.declareNamespace('SolidRpc.Security');

//
// these are the oidc client settings
//
solidRpcSecurity.OidcClientSettings = {
    authority: '{oidc-client-authority}',
    client_id: '{oidc-client-client_id}',
    redirect_uri: '{oidc-client-redirect_uri}'
};

//
// this method is invoked by the providers when a valid access token has been received.
// the response from the server is the new access token.
//
solidRpcSecurity.doLogin = function (provider, postback) {
    alert(provider + ' is logged in.');
    let xhr = new XMLHttpRequest();
    xhr.onreadystatechange = function () {
        if (this.readyState === 4 && this.status === 200) {
            let response = JSON.parse(this.responseText);
            alert(response);
            ns.accessToken = response;
        }
    };
    xhr.open("GET", postback);
    xhr.setRequestHeader("Accept", 'application/json');
    xhr.send();
};
