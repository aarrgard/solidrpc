namespace SolidRpc.Test.Petstore.Types.Services.Pet.FindPetsByStatus {
    /// <summary>
    /// Invalid status value
    /// </summary>
    public class InvalidStatusValueException : System.Exception {
        /// <summary>
        /// Constructs a new instance
        /// </summary>
        public InvalidStatusValueException() : base("Invalid status value")
        {
            Data["HttpStatusCode"] = 400;
        }
    }
}