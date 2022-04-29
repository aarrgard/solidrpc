using System.CodeDom.Compiler;
using System.Threading.Tasks;
using SolidRpc.Tests.Swagger.CodeGen.Stoer.Types.Se.stoer.brokering.partnerapi.app;
using SolidRpc.Tests.Swagger.CodeGen.Stoer.Types.Se.stoer.brokering.partnerapi.api.leads;
using System.Threading;
namespace SolidRpc.Tests.Swagger.CodeGen.Stoer.Services {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public interface IPartnerAPIMortgageLeadsCommands {
        /// <summary>
        /// Start a mortgage lead from partner.
        /// </summary>
        /// <param name="body"></param>
        /// <param name="cancellationToken"></param>
        /// <exception cref="SolidRpc.Tests.Swagger.CodeGen.Stoer.Types.Services.PartnerAPIMortgageLeadsCommands.StartLead.RequestAlreadyProcessedException">Request already processed</exception>
        /// <exception cref="SolidRpc.Tests.Swagger.CodeGen.Stoer.Types.Services.PartnerAPIMortgageLeadsCommands.StartLead.UnauthorizedException">Unauthorized</exception>
        /// <exception cref="SolidRpc.Tests.Swagger.CodeGen.Stoer.Types.Services.PartnerAPIMortgageLeadsCommands.StartLead.TheRequestedOperationIsNotAllowedException">The requested operation is not allowed</exception>
        /// <exception cref="SolidRpc.Tests.Swagger.CodeGen.Stoer.Types.Services.PartnerAPIMortgageLeadsCommands.StartLead.TheRequestPayloadIsMissingInformationException">The request payload is missing information</exception>
        /// <exception cref="SolidRpc.Tests.Swagger.CodeGen.Stoer.Types.Services.PartnerAPIMortgageLeadsCommands.StartLead.InternalServerErrorException">Internal server error</exception>
        Task<StartMortgageLeadResponse> StartLead(
            StartLeadCommand body = null,
            CancellationToken cancellationToken = default(CancellationToken));
    
    }
}