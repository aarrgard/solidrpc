using System.CodeDom.Compiler;
using System.Threading.Tasks;
using SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Cms.Estate;
using System.Threading;
namespace SolidRpc.Tests.Swagger.CodeGen.Vitec.Services {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public interface IPlot {
        /// <summary>
        /// Metod f�r att uppdatera en tomt.
        /// </summary>
        /// <param name="customerId">Kund-id</param>
        /// <param name="id">Id p� tomten</param>
        /// <param name="plot">Tomtinformationen som ska uppdateras</param>
        /// <param name="cancellationToken"></param>
        /// <exception cref="SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Services.Plot.PlotUpdate.NoContentException">No Content</exception>
        Task PlotUpdate(
            string customerId,
            string id,
            CmsPlot plot,
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// Metod f�r att skapa en tomt.&lt;br /&gt;
        /// </summary>
        /// <param name="customerId">Kund-id</param>
        /// <param name="plot">Tomtinformationen som ska uppdateras</param>
        /// <param name="cancellationToken"></param>
        Task<string> PlotCreate(
            string customerId,
            CmsPlot plot,
            CancellationToken cancellationToken = default(CancellationToken));
    
    }
}