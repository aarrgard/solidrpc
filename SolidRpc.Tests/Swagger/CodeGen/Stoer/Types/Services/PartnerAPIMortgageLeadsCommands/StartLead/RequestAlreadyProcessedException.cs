using System.CodeDom.Compiler;
namespace SolidRpc.Tests.Swagger.CodeGen.Stoer.Types.Services.PartnerAPIMortgageLeadsCommands.StartLead {
    /// <summary>
    /// Request already processed
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class RequestAlreadyProcessedException : System.Exception {
        /// <summary>
        /// Constructs a new instance
        /// </summary>
        public RequestAlreadyProcessedException() : base("Request already processed")
        {
            Data["HttpStatusCode"] = 204;
        }
    }
}