using System.CodeDom.Compiler;
namespace SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Services.Plot.PlotUpdate {
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