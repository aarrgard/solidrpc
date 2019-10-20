using System.CodeDom.Compiler;
using System.Threading.Tasks;
using System.Collections.Generic;
using SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Task.Models;
using System.Threading;
namespace SolidRpc.Tests.Swagger.CodeGen.Vitec.Services {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public interface ITask {
        /// <summary>
        /// H�mtar alla f�rdefinierade typade uppgifter
        /// </summary>
        /// <param name="customerId">Kund id</param>
        /// <param name="cancellationToken"></param>
        Task<IEnumerable<PredefinedTask>> TaskGetTasks(
            string customerId,
            CancellationToken cancellationToken = default(CancellationToken));
    
    }
}