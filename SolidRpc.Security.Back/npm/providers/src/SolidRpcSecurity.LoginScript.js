import { SolidRpc } from 'solidrpc';
import { Security } from 'solidrpc-security';

export var currentAccessToken = null;

/**
 * Invoked from the providers when a new access token has been created.
 * @param {string} accessToken  the access token
 */
export function onLoggedIn(accessToken) {
    if (accessToken === currentAccessToken) {
        console.info('No change in access token - no action taken.');
        return;
    }
    console.log('New access token fetched ' + currentAccessToken + '->' + accessToken);

    //
    // update current access token and default headers
    //
    currentAccessToken = accessToken;
    SolidRpc.defaultHeaders['Authorization'] = 'Bearer: ' + accessToken;

    console.log(SolidRpc.defaultHeaders);

    //Security.Services.SolidRpcSecurityImplInstance;
}


/*
var solidRpcSecurity = SolidRpc.rootNamespace.declareNamespace('SolidRpc.Security');

//
// these are the oidc client settings
//
solidRpcSecurity.OidcClientSettings = {
    authority: '{oidc-client-authority}',
    client_id: '{oidc-client-client_id}',
    redirect_uri: '{oidc-client-redirect_uri}'
};
*/
