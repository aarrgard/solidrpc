namespace SolidRpc.Test.Petstore.Types.Services.User.LoginUser {
    /// <summary>
    /// Invalid username/password supplied
    /// </summary>
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