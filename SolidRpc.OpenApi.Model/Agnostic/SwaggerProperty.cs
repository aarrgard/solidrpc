namespace SolidRpc.OpenApi.Model.Agnostic
{
    /// <summary>
    /// Represents a property in an object definied in swagger
    /// </summary>
    public class SwaggerProperty
    {
        /// <summary>
        /// The name of the property.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The property type.
        /// </summary>
        public SwaggerDefinition Type { get; set; }

        /// <summary>
        /// The description of this property - null if not description available
        /// </summary>
        public string Description { get; set; }
    }
}