
using Microsoft.Extensions.Primitives;
using SolidRpc.Tests.Swagger.SpecGen.FormAsStructArg.Types;

namespace SolidRpc.Tests.Swagger.SpecGen.FormAsStructArg.Services
{
    /// <summary>
    /// Tests method with one complex type
    /// </summary>
    public interface IFormAsStructArg
    {
        /// <summary>
        /// Consumes one complex type an a simple string...
        /// </summary>
        /// <param name="formData">The string values</param>
        /// <returns></returns>
        FormData GetFormData(FormData formData);
    }
}
