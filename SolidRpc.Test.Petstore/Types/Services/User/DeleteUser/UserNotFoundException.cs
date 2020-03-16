using System.CodeDom.Compiler;
namespace SolidRpc.Test.Petstore.Types.Services.User.DeleteUser {
    /// <summary>
    /// User not found
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class UserNotFoundException : System.Exception {
        /// <summary>
        /// Constructs a new instance
        /// </summary>
        public UserNotFoundException() : base("User not found")
        {
            Data["HttpStatusCode"] = 404;
        }
    }
}