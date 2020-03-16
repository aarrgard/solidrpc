using System.CodeDom.Compiler;
namespace SolidRpc.Test.Petstore.Types.Services.Pet.FindPetsByTags {
    /// <summary>
    /// Invalid tag value
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
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