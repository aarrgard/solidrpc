using System.CodeDom.Compiler;
namespace SolidRpc.Tests.Swagger.CodeGen.Petstore.Types.Services.User.LoginUser {
    /// <summary>
    /// Invalid username/password supplied
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class InvalidUsernamePasswordSuppliedException : System.Exception {
        /// <summary>
        /// Constructs a new instance
        /// </summary>
        public InvalidUsernamePasswordSuppliedException() : base("Invalid username/password supplied")
        {
            Data["HttpStatusCode"] = 400;
        }
    }
}