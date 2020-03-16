using System.CodeDom.Compiler;
namespace SolidRpc.Test.Petstore.Types.Services.Pet.AddPet {
    /// <summary>
    /// Invalid input
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
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