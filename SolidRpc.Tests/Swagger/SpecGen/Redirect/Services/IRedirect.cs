using SolidRpc.Tests.Swagger.SpecGen.TwoComplexArgs.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace SolidRpc.Tests.Swagger.SpecGen.Redirect.Services
{
    /// <summary>
    /// Tests method with two complex types
    /// </summary>
    public interface IRedirect
    {
        /// <summary>
        /// Redirects
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        Types.Redirect Redirect(Types.Redirect r);

        /// <summary>
        /// The redirected resource.
        /// </summary>
        /// <returns></returns>
        Types.Redirect Redirected();
    }
}
