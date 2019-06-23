namespace SolidRpc.Test.Petstore.Types.Services.User.UpdateUser {
    /// <summary>
    /// Invalid user supplied
    /// </summary>
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