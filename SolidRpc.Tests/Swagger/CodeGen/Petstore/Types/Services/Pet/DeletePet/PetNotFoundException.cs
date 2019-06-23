namespace SolidRpc.Tests.Swagger.CodeGen.Petstore.Types.Services.Pet.DeletePet {
    /// <summary>
    /// Pet not found
    /// </summary>
    public class PetNotFoundException : System.Exception {
        /// <summary>
        /// Constructs a new instance
        /// </summary>
        public PetNotFoundException() : base("Pet not found")
        {
            Data["HttpStatusCode"] = 404;
        }
    }
}