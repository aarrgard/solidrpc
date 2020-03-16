using System.CodeDom.Compiler;
namespace SolidRpc.Test.Petstore.Types.Services.Pet.UpdatePet {
    /// <summary>
    /// Validation exception
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
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