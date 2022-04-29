using System.CodeDom.Compiler;
namespace SolidRpc.Tests.Swagger.CodeGen.Stoer.Types.Services.PartnerAPIMortgageLeadsCommands.StartLead {
    /// <summary>
    /// Unauthorized
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class UnauthorizedException : System.Exception {
        /// <summary>
        /// Constructs a new instance
        /// </summary>
        public UnauthorizedException() : base("Unauthorized")
        {
            Data["HttpStatusCode"] = 401;
        }
    }
}