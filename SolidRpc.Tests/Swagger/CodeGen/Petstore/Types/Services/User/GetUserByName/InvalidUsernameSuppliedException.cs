namespace SolidRpc.Tests.Swagger.CodeGen.Petstore.Types.Services.User.GetUserByName {
    /// <summary>
    /// Invalid username supplied
    /// </summary>
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