namespace SolidRpc.Swagger.Generator.Code.Binder
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
    }
}