namespace SolidRpc.Abstractions.OpenApi.Transport
{
    /// <summary>
    /// The invocation strategy
    /// </summary>
    public enum InvocationStrategy
    {
        /// <summary>
        /// Invokes methods on the registered proxy
        /// </summary>
        Invoke = 0,

        /// <summary>
        /// Forwards call the the transport that does the invocation.
        /// </summary>
        Forward = 1
    }
}