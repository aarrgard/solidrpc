namespace SolidRpc.Tests.Swagger.CodeGen.Petstore.Types.Services.Store.GetOrderById {
    /// <summary>
    /// Invalid ID supplied
    /// </summary>
    public class InvalidIDSuppliedException : System.Exception {
        /// <summary>
        /// Constructs a new instance
        /// </summary>
        public InvalidIDSuppliedException() : base("Invalid ID supplied")
        {
            Data["HttpStatusCode"] = 400;
        }
    }
}