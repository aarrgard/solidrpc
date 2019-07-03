namespace SolidRpc.Test.Petstore.Types.Services.User.GetUserByName {
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