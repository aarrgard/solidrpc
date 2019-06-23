namespace SolidRpc.Tests.Swagger.CodeGen.Petstore.Types.Services.Store.PlaceOrder {
    /// <summary>
    /// Invalid Order
    /// </summary>
    public class InvalidOrderException : System.Exception {
        /// <summary>
        /// Constructs a new instance
        /// </summary>
        public InvalidOrderException() : base("Invalid Order")
        {
            Data["HttpStatusCode"] = 400;
        }
    }
}