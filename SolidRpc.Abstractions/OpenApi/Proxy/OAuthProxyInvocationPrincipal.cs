namespace SolidRpc.Abstractions.OpenApi.Proxy
{
    /// <summary>
    /// The principal to select when invoking methods
    /// </summary>
    public enum OAuthProxyInvocationPrincipal
    {
        /// <summary>
        /// Do not pass along the authorization header
        /// </summary>
        None = 0,

        /// <summary>
        /// Use the client jwt token when invoking proxies
        /// </summary>
        Client = 1,

        /// <summary>
        /// Proxy the frontend user.
        /// </summary>
        Proxy = 2,

        /// <summary>
        /// Delegate the frontend user to the backend call.
        /// </summary>
        Delegate = 3
    }
}