using System.CodeDom.Compiler;
namespace SolidRpc.Tests.Swagger.CodeGen.Petstore.Types.Services.User.UpdateUser {
    /// <summary>
    /// Invalid user supplied
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class InvalidUserSuppliedException : System.Exception {
        /// <summary>
        /// Constructs a new instance
        /// </summary>
        public InvalidUserSuppliedException() : base("Invalid user supplied")
        {
            Data["HttpStatusCode"] = 400;
        }
    }
}