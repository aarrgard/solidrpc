using System.CodeDom.Compiler;
namespace SolidRpc.Tests.Swagger.CodeGen.Stoer.Types.Services.PartnerAPIMortgageLeadsCommands.StartLead {
    /// <summary>
    /// The request payload is missing information
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class TheRequestPayloadIsMissingInformationException : System.Exception {
        /// <summary>
        /// Constructs a new instance
        /// </summary>
        public TheRequestPayloadIsMissingInformationException() : base("The request payload is missing information")
        {
            Data["HttpStatusCode"] = 422;
        }
    }
}