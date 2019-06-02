using System;
using System.Collections.Generic;
using System.Text;

namespace SolidRpc.Swagger.Generator.Code.Binder
{
    /// <summary>
    /// Represents a CSharp method
    /// </summary>
    public class CSharpProperty
    {
        /// <summary>
        /// The return type.
        /// </summary>
        public CSharpObject PropertyType { get; set; }

        /// <summary>
        /// The property name
        /// </summary>
        public string PropertyName { get; set; }

        /// <summary>
        /// The description of the property.
        /// </summary>
        public string Description { get; set; }
    }
}
