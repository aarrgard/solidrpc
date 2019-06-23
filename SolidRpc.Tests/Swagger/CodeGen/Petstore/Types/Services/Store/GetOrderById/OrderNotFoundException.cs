namespace SolidRpc.Tests.Swagger.CodeGen.Petstore.Types.Services.Store.GetOrderById {
    /// <summary>
    /// Order not found
    /// </summary>
    public class OrderNotFoundException : System.Exception {
        /// <summary>
        /// Constructs a new instance
        /// </summary>
        public OrderNotFoundException() : base("Order not found")
        {
            Data["HttpStatusCode"] = 404;
        }
    }
}