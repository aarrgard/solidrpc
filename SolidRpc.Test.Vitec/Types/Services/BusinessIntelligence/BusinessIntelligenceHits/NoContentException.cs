using System.CodeDom.Compiler;
namespace SolidRpc.Test.Vitec.Types.Services.BusinessIntelligence.BusinessIntelligenceHits {
    /// <summary>
    /// No Content
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class NoContentException : System.Exception {
        /// <summary>
        /// Constructs a new instance
        /// </summary>
        public NoContentException() : base("No Content")
        {
            Data["HttpStatusCode"] = 204;
        }
    }
}