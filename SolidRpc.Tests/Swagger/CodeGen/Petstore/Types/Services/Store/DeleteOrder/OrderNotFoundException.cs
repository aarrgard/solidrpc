using System.CodeDom.Compiler;
namespace SolidRpc.Tests.Swagger.CodeGen.Petstore.Types.Services.Store.DeleteOrder {
    /// <summary>
    /// Order not found
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
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