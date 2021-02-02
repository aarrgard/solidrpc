namespace SolidRpc.Abstractions.OpenApi.OAuth2
{
    /// <summary>
    /// Configures the token checks to perform
    /// </summary>
    public interface IAuthorityTokenChecks
    {
        /// <summary>
        /// should the actory be validated
        /// </summary>
        bool ValidateActor { get; set; }

        /// <summary>
        /// Should the audience be validated
        /// </summary>
        bool ValidateAudience { get; set; }

        /// <summary>
        /// Should the issuer be validated
        /// </summary>
        bool ValidateIssuer { get; set; }

        /// <summary>
        /// Should the signing key ve validated
        /// </summary>
        bool ValidateIssuerSigningKey { get; set; }

        /// <summary>
        /// Should the lifetime be validated
        /// </summary>
        bool ValidateLifetime { get; set; }

        /// <summary>
        /// Do we require expiration time
        /// </summary>
        bool RequireExpirationTime { get; set; }
        
        /// <summary>
        /// Does the token have to be signed
        /// </summary>
        bool RequireSignedTokens { get; set; }
        
        /// <summary>
        /// Do we require an audience
        /// </summary>
        bool RequireAudience { get; set; }

    }
}