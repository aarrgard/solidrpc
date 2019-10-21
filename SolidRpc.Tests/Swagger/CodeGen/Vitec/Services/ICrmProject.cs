using System.CodeDom.Compiler;
using System.Threading.Tasks;
using SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Cms.Estate;
using System.Threading;
using System.Collections.Generic;
namespace SolidRpc.Tests.Swagger.CodeGen.Vitec.Services {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public interface ICrmProject {
        /// <summary>
        /// Metod f&#246;r att uppdatera ett projekt
        /// </summary>
        /// <param name="customerId">Kundid</param>
        /// <param name="projectId">Id på projektet</param>
        /// <param name="project">Projektinformationen som ska uppdateras</param>
        /// <param name="cancellationToken"></param>
        /// <exception cref="SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Services.CrmProject.CrmProjectUpdate.NoContentException">No Content</exception>
        Task CrmProjectUpdate(
            string customerId,
            string projectId,
            CmsProject project,
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// Metod f&#246;r att koppla befintliga objekt till ett projekt
        /// </summary>
        /// <param name="customerId">Kundid</param>
        /// <param name="projectId">Id på projektet</param>
        /// <param name="estateIds">Id:n på objekt som ska kopplas till projektet</param>
        /// <param name="cancellationToken"></param>
        /// <exception cref="SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Services.CrmProject.CrmProjectAddEstateConnections.NoContentException">No Content</exception>
        Task CrmProjectAddEstateConnections(
            string customerId,
            string projectId,
            IEnumerable<string> estateIds,
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// Metod f&#246;r att koppla bort objekt fr&#229;n ett projekt
        /// </summary>
        /// <param name="customerId">Kundid</param>
        /// <param name="projectId">Id på projektet</param>
        /// <param name="estateId">Id på objekt som ska kopplas bort från projektet</param>
        /// <param name="cancellationToken"></param>
        /// <exception cref="SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Services.CrmProject.CrmProjectDeleteEstateConnections.NoContentException">No Content</exception>
        Task CrmProjectDeleteEstateConnections(
            string customerId,
            string projectId,
            string estateId,
            CancellationToken cancellationToken = default(CancellationToken));
    
    }
}