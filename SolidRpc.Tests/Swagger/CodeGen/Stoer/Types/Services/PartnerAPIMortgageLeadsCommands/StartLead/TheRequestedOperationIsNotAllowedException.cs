using System.CodeDom.Compiler;
namespace SolidRpc.Tests.Swagger.CodeGen.Stoer.Types.Services.PartnerAPIMortgageLeadsCommands.StartLead {
    /// <summary>
    /// The requested operation is not allowed
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class TheRequestedOperationIsNotAllowedException : System.Exception {
        /// <summary>
        /// Constructs a new instance
        /// </summary>
        public TheRequestedOperationIsNotAllowedException() : base("The requested operation is not allowed")
        {
            Data["HttpStatusCode"] = 409;
        }
    }
}