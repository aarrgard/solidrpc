using System.CodeDom.Compiler;
using System.Threading.Tasks;
using SolidRpc.Test.Vitec.Types.Cms.Estate;
using System.Threading;
namespace SolidRpc.Test.Vitec.Services {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public interface IPlot {
        /// <summary>
        /// Metod f&#246;r att uppdatera en tomt.
        /// </summary>
        /// <param name="customerId">Kund-id</param>
        /// <param name="id">Id på tomten</param>
        /// <param name="plot">Tomtinformationen som ska uppdateras</param>
        /// <param name="cancellationToken"></param>
        /// <exception cref="SolidRpc.Test.Vitec.Types.Services.Plot.PlotUpdate.NoContentException">No Content</exception>
        Task PlotUpdate(
            string customerId,
            string id,
            CmsPlot plot,
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// Metod f&#246;r att skapa en tomt.&lt;br /&gt;
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