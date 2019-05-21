using System;
using System.Collections.Generic;


namespace SolidRpc.Swagger.Generator.Code.Binder
{
    /// <summary>
    /// Represents a c# object.
    /// </summary>
    public class CSharpObject
    {
        public CSharpObject(string name)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Properties = new List<CSharpProperty>();
        }

        /// <summary>
        /// The full class name
        /// </summary>
        public QualifiedName Name { get; }

        /// <summary>
        /// Returns true if this is an array.
        /// </summary>
        public bool IsArray { get; set; }

        /// <summary>
        /// The properties in this object
        /// </summary>
        public IEnumerable<CSharpProperty> Properties { get; set; }
    }
}
