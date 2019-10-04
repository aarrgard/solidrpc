using System.CodeDom.Compiler;
using System.Threading.Tasks;
using SolidRpc.Security.Types;
using System.Threading;
namespace SolidRpc.Security.Services.Oidc {
    /// <summary>
    /// Defines logic for the oidc client.
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public interface IOidcClient {
        /// <summary>
        /// Returns the settings for the client. Usually invoked from a javascript web app.
        /// These settings does not contain the secret which the client should keep for itself.
        /// It does however contain a &quot;redirect_uri&quot; that is valid for this client.
        /// </summary>
        /// <param name="cancellationToken"></param>
        Task<Settings> Settings(
            CancellationToken cancellationToken = default(CancellationToken));
    
    }
}