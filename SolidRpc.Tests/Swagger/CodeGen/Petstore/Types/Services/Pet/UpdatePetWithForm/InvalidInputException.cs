namespace SolidRpc.Tests.Swagger.CodeGen.Petstore.Types.Services.Pet.UpdatePetWithForm {
    /// <summary>
    /// Invalid input
    /// </summary>
    public class InvalidInputException : System.Exception {
        /// <summary>
        /// Constructs a new instance
        /// </summary>
        public InvalidInputException() : base("Invalid input")
        {
            Data["HttpStatusCode"] = 405;
        }
    }
}