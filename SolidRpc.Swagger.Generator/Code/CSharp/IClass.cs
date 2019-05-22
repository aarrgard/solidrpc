using System.Collections.Generic;

namespace SolidRpc.Swagger.Generator.Code.CSharp
{
    /// <summary>
    /// Represents a class
    /// </summary>
    public interface IClass : IMember, IQualifiedMember
    {
        /// <summary>
        /// Returns all the properties in the class.
        /// </summary>
        IEnumerable<IProperty> Properties { get; }

        /// <summary>
        /// Adds a property to this class.
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="propType"></param>
        IProperty AddProperty(string propertyName, IClass propType);
    }
}