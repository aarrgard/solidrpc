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
        /// Sets 
        /// </summary>
        /// <param name="securityDefinitionName"></param>
        /// <param name="headerName"></param>
        void AddApiKeyAuth(string securityDefinitionName, string headerName);
    }
}