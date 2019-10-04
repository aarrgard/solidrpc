using System.CodeDom.Compiler;
namespace SolidRpc.Tests.Swagger.CodeGen.Petstore.Types.Services.Store.PlaceOrder {
    /// <summary>
    /// Invalid Order
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
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