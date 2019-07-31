using System;
using System.Collections.Generic;
using System.Text;

namespace SolidRpc.Abstractions.OpenApi.Binder
{
    /// <summary>
    /// Interface that can be implemented to resolve the base uri.
    /// </summary>
    public interface IBaseUriTransformer
    {
        /// <summary>
        /// The base uri.
        /// </summary>
        Uri TransformUri(Uri uri);
    }
}
