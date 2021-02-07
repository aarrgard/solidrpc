using SolidRpc.Abstractions.Types.OAuth2;
using System.Collections.Generic;

namespace SolidRpc.Abstractions.OpenApi.Model
{
    /// <summary>
    /// Represents an operation.
    /// </summary>
    public interface IOpenApiOperation
    {
        /// <summary>
        /// Returns the operation id.
        /// </summary>
        string OperationId { get; }

        /// <summary>
        /// Returns the description
        /// </summary>
        string Description { get; set; }

        /// <summary>
        /// Sets the security key
        /// </summary>
        /// <param name="securityDefinitionName"></param>
        /// <param name="headerName"></param>
        void AddApiKeyAuth(string securityDefinitionName, string headerName);

        /// <summary>
        /// Sets the oauth2 authentication
        /// </summary>
        /// <param name="authDoc"></param>
        /// <param name="flow"></param>
        /// <param name="scopes"></param>
        /// <returns>The security definition name for the supplied authority</returns>
        void AddOAuth2Auth(
            OpenIDConnnectDiscovery authDoc,
            string flow, 
            IEnumerable<string> scopes);
    }
}