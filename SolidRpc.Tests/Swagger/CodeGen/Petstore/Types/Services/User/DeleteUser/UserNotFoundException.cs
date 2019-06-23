namespace SolidRpc.Tests.Swagger.CodeGen.Petstore.Types.Services.User.DeleteUser {
    /// <summary>
    /// User not found
    /// </summary>
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