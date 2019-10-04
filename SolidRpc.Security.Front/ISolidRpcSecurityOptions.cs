namespace SolidRpc.Security.Front
{
    /// <summary>
    /// Contains options for the security
    /// </summary>
    public interface ISolidRpcSecurityOptions
    {
        /// <summary>
        /// The authority
        /// </summary>
        string Authority { get; }

        /// <summary>
        /// The client id
        /// </summary>
        string ClientId { get; }

        /// <summary>
        /// The client secret
        /// </summary>
        string ClientSecret { get; }
    }
}