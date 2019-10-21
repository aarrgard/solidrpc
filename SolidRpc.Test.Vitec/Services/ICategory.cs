using System.CodeDom.Compiler;
using System.Threading.Tasks;
using SolidRpc.Test.Vitec.Types.Category.Models;
using System.Threading;
namespace SolidRpc.Test.Vitec.Services {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public interface ICategory {
        /// <summary>
        /// H&#228;mtar kategorier. F&#246;r att kunna h&#228;mta kategorier s&#229; kr&#228;vs det en giltig API nyckel och ett kundid
        /// </summary>
        /// <param name="customerId">Kund id</param>
        /// <param name="cancellationToken"></param>
        Task<Categories> CategoryGetCategories(
            string customerId,
            CancellationToken cancellationToken = default(CancellationToken));
    
    }
}