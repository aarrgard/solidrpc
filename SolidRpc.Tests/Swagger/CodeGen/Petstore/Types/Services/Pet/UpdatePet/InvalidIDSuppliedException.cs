using System.CodeDom.Compiler;
namespace SolidRpc.Tests.Swagger.CodeGen.Petstore.Types.Services.Pet.UpdatePet {
    /// <summary>
    /// Invalid ID supplied
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class InvalidIDSuppliedException : System.Exception {
        /// <summary>
        /// Constructs a new instance
        /// </summary>
        public InvalidIDSuppliedException() : base("Invalid ID supplied")
        {
            Data["HttpStatusCode"] = 400;
        }
    }
}