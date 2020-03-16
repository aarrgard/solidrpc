using System.CodeDom.Compiler;
namespace SolidRpc.Test.Petstore.Types.Services.User.GetUserByName {
    /// <summary>
    /// Invalid username supplied
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class InvalidUsernameSuppliedException : System.Exception {
        /// <summary>
        /// Constructs a new instance
        /// </summary>
        public InvalidUsernameSuppliedException() : base("Invalid username supplied")
        {
            Data["HttpStatusCode"] = 400;
        }
    }
}