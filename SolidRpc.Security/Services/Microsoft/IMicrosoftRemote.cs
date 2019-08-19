using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Threading;
using SolidRpc.Security.Types;
namespace SolidRpc.Security.Services.Microsoft {
    /// <summary>
    /// Defines access to the microsoft oauth implementation
    /// </summary>
    public interface IMicrosoftRemote {
        /// <summary>
        /// When your web app needs to authenticate the user, it can direct the user to the /authorize endpoint. This request is similar to the first leg of the OAuth 2.0 authorization code flow, with these important distinctions:
        /// </summary>
        /// <param name="tenant">You can use the {tenant} value in the path of the request to control who can sign in to the application. The allowed values are common, organizations, consumers, and tenant identifiers</param>
        /// <param name="clientId">The Application (client) ID that the Azure portal – App registrations experience assigned to your app.</param>
        /// <param name="responseType">Must include id_token for OpenID Connect sign-in. It might also include other response_type values, such as code.</param>
        /// <param name="scope">The redirect URI of your app, where authentication responses can be sent and received by your app. It must exactly match one of the redirect URIs you registered in the portal, except that it must be URL encoded. If not present, the endpoint will pick one registered redirect_uri at random to send the user back to.</param>
        /// <param name="nounce">A value included in the request, generated by the app, that will be included in the resulting id_token value as a claim. The app can verify this value to mitigate token replay attacks. The value typically is a randomized, unique string that can be used to identify the origin o</param>
        /// <param name="redirectUri">The redirect URI of your app, where authentication responses can be sent and received by your app. It must exactly match one of the redirect URIs you registered in the portal, except that it must be URL encoded. If not present, the endpoint will pick one registered redirect_uri at random to send the user back to.</param>
        /// <param name="responseMode">Specifies the method that should be used to send the resulting authorization code back to your app. Can be form_post or fragment. For web applications, we recommend using response_mode=form_post, to ensure the most secure transfer of tokens to your application.</param>
        /// <param name="state">A value included in the request, generated by the app, that will be included in the resulting id_token value as a claim. The app can verify this value to mitigate token replay attacks. The value typically is a randomized, unique string that can be used to identify the origin o</param>
        /// <param name="prompt">Indicates the type of user interaction that is required. The only valid values at this time are login, none, and consent. The prompt=login claim forces the user to enter their credentials on that request, which negates single sign-on. The prompt=none claim is the opposite. This claim ensures that the user isn't presented with any interactive prompt at. If the request can't be completed silently via single sign-on, the Microsoft identity platform endpoint returns an error. The prompt=consent claim triggers the OAuth consent dialog after the user signs in. The dialog asks the user to grant permissions to the app.</param>
        /// <param name="loginHint">You can use this parameter to pre-fill the username and email address field of the sign-in page for the user, if you know the username ahead of time. Often, apps use this parameter during reauthentication, after already extracting the username from an earlier sign-in by using the preferred_username claim.</param>
        /// <param name="domainHint">The realm of the user in a federated directory. This skips the email-based discovery process that the user goes through on the sign-in page, for a slightly more streamlined user experience. For tenants that are federated through an on-premises directory like AD FS, this often results in a seamless sign-in because of the existing login session.</param>
        /// <param name="cancellationToken"></param>
        Task Authorize(
            string tenant,
            Guid clientId,
            IEnumerable<string> responseType,
            IEnumerable<string> scope,
            string nounce,
            Uri redirectUri = default(Uri),
            string responseMode = default(string),
            string state = default(string),
            string prompt = default(string),
            string loginHint = default(string),
            string domainHint = default(string),
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// Returns the openid configuration
        /// </summary>
        /// <param name="tenant">You can use the {tenant} value in the path of the request to control who can sign in to the application. The allowed values are common, organizations, consumers, and tenant identifiers</param>
        /// <param name="cancellationToken"></param>
        Task<OpenIDConnnectDiscovery> OpenIdConfiguration(
            string tenant,
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// Returns the openid keys used for signing.
        /// </summary>
        /// <param name="tenant">You can use the {tenant} value in the path of the request to control who can sign in to the application. The allowed values are common, organizations, consumers, and tenant identifiers</param>
        /// <param name="cancellationToken"></param>
        Task<OpenIDKeys> OpenIdKeys(
            string tenant,
            CancellationToken cancellationToken = default(CancellationToken));
    
    }
}