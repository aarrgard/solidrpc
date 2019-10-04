using System.CodeDom.Compiler;
namespace SolidRpc.Tests.Swagger.CodeGen.Petstore.Types.Services.Pet.FindPetsByStatus {
    /// <summary>
    /// Invalid status value
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
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