namespace SolidRpc.Tests.Swagger.CodeGen.Petstore.Types.Services.Pet.UpdatePet {
    /// <summary>
    /// Validation exception
    /// </summary>
    public class ValidationException : System.Exception {
        /// <summary>
        /// Constructs a new instance
        /// </summary>
        public ValidationException() : base("Validation exception")
        {
            Data["HttpStatusCode"] = 405;
        }
    }
}