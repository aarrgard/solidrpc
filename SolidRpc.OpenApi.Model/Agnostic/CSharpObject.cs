using System;
using System.Collections.Generic;


namespace SolidRpc.OpenApi.Model.Agnostic
{
    /// <summary>
    /// Represents a c# object.
    /// </summary>
    public class CSharpObject
    {
        /// <summary>
        /// Constructs a new instance
        /// </summary>
        /// <param name="name"></param>
        public CSharpObject(string name)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Properties = new List<CSharpProperty>();
        }

        /// <summary>
        /// Constructs a new instance
        /// </summary>
        /// <param name="arrayElement"></param>
        public CSharpObject(CSharpObject arrayElement)
        {
            ArrayElement = arrayElement;
            Name = $"System.Collections.Generic.IEnumerable<{arrayElement.Name}>";
            Properties = new List<CSharpProperty>();
        }

        /// <summary>
        /// The full class name
        /// </summary>
        public QualifiedName Name { get; }

        /// <summary>
        /// The description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The properties in this object
        /// </summary>
        public IEnumerable<CSharpProperty> Properties { get; set; }

        /// <summary>
        /// The array object.
        /// </summary>
        public CSharpObject ArrayElement { get; }

        /// <summary>
        /// The additional properties
        /// </summary>
        public CSharpObject AdditionalProperties { get; set; }

        /// <summary>
        /// The exception code.
        /// </summary>
        public int? ExceptionCode { get; set; }
    }
}
