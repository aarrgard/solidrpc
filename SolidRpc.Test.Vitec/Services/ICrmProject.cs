using System.CodeDom.Compiler;
using System.Threading.Tasks;
using SolidRpc.Test.Vitec.Types.Cms.Estate;
using System.Threading;
using System.Collections.Generic;
namespace SolidRpc.Test.Vitec.Services {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public interface ICrmProject {
        /// <summary>
        /// Metod f�r att uppdatera ett projekt
        /// </summary>
        /// <param name="customerId">Kundid</param>
        /// <param name="projectId">Id p� projektet</param>
        /// <param name="project">Projektinformationen som ska uppdateras</param>
        /// <param name="cancellationToken"></param>
        /// <exception cref="SolidRpc.Test.Vitec.Types.Services.CrmProject.CrmProjectUpdate.NoContentException">No Content</exception>
        Task CrmProjectUpdate(
            string customerId,
            string projectId,
            CmsProject project,
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// Metod f�r att koppla befintliga objekt till ett projekt
        /// </summary>
        /// <param name="customerId">Kundid</param>
        /// <param name="projectId">Id p� projektet</param>
        /// <param name="estateIds">Id:n p� objekt som ska kopplas till projektet</param>
        /// <param name="cancellationToken"></param>
        /// <exception cref="SolidRpc.Test.Vitec.Types.Services.CrmProject.CrmProjectAddEstateConnections.NoContentException">No Content</exception>
        Task CrmProjectAddEstateConnections(
            string customerId,
            string projectId,
            IEnumerable<string> estateIds,
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// Metod f�r att koppla bort objekt fr�n ett projekt
        /// </summary>
        /// <param name="customerId">Kundid</param>
        /// <param name="projectId">Id p� projektet</param>
        /// <param name="estateId">Id p� objekt som ska kopplas bort fr�n projektet</param>
        /// <param name="cancellationToken"></param>
        /// <exception cref="SolidRpc.Test.Vitec.Types.Services.CrmProject.CrmProjectDeleteEstateConnections.NoContentException">No Content</exception>
        Task CrmProjectDeleteEstateConnections(
            string customerId,
            string projectId,
            string estateId,
            CancellationToken cancellationToken = default(CancellationToken));
    
    }
}