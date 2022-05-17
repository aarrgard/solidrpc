using SolidRpc.Tests.Swagger.SpecGen.FormAsStructArg.Types;
using System.Collections.Generic;

namespace SolidRpc.Tests.Swagger.SpecGen.FormAsStructArg.Services
{
    /// <summary>
    /// Tests a form with some data
    /// </summary>
    public interface IFormAsStructArg
    {
        /// <summary>
        /// Processes the form data
        /// </summary>
        /// <param name="formData">The string values</param>
        /// <returns></returns>
        FormData GetFormData(FormData formData);

        /// <summary>
        /// Proxying strings should still work.
        /// </summary>
        /// <param name="dummy"></param>
        /// <param name="forms"></param>
        /// <returns></returns>
        IEnumerable<FormData> ProxyForms(string dummy, IEnumerable<FormData> forms);
    }
}
