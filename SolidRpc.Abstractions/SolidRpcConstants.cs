using System;
using System.Collections.Generic;
using System.Text;

namespace SolidRpc.Abstractions
{
    /// <summary>
    /// Contains some constants 
    /// </summary>
    public static class SolidRpcConstants
    {
        /// <summary>
        /// characters to be used when splittung up words in a openapi spec
        /// </summary>
        public static readonly char[] OpenApiWordSeparators = new[] { ' ', '/', '.', '_', '#' };
    }
}
