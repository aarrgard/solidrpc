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
        /// Sets 
        /// </summary>
        /// <param name="keyName"></param>
        void AddSolidRpcSecurityKey(string keyName);
    }
}