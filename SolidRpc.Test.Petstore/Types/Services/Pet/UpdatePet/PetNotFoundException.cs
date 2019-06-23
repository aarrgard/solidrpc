namespace SolidRpc.Test.Petstore.Types.Services.Pet.UpdatePet {
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