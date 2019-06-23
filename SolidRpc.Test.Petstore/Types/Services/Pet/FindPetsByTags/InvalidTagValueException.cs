namespace SolidRpc.Test.Petstore.Types.Services.Pet.FindPetsByTags {
    /// <summary>
    /// Invalid tag value
    /// </summary>
    public class InvalidTagValueException : System.Exception {
        /// <summary>
        /// Constructs a new instance
        /// </summary>
        public InvalidTagValueException() : base("Invalid tag value")
        {
            Data["HttpStatusCode"] = 400;
        }
    }
}