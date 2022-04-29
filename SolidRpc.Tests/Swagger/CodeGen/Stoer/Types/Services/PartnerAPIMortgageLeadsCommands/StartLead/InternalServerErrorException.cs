using System.CodeDom.Compiler;
namespace SolidRpc.Tests.Swagger.CodeGen.Stoer.Types.Services.PartnerAPIMortgageLeadsCommands.StartLead {
    /// <summary>
    /// Internal server error
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class InternalServerErrorException : System.Exception {
        /// <summary>
        /// Constructs a new instance
        /// </summary>
        public InternalServerErrorException() : base("Internal server error")
        {
            Data["HttpStatusCode"] = 500;
        }
    }
}