using System.CodeDom.Compiler;
using System.Threading.Tasks;
using System.Collections.Generic;
using SolidRpc.Test.Vitec.Types.CustomField.Models;
using System.Threading;
namespace SolidRpc.Test.Vitec.Services {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public interface ICustomField {
        /// <summary>
        /// H�mtar egendefinerade f�lt.
        /// </summary>
        /// <param name="customerId">Kund id</param>
        /// <param name="cancellationToken"></param>
        Task<IEnumerable<CustomFields>> CustomFieldGet(
            string customerId,
            CancellationToken cancellationToken = default(CancellationToken));
    
    }
}