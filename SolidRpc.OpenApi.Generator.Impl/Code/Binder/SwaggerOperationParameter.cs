namespace SolidRpc.OpenApi.Generator.Code.Binder
{
    /// <summary>
    /// Represents a swagger operation parameter
    /// </summary>
    public class SwaggerOperationParameter
    {
        /// <summary>
        /// The description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The operation name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The parameter type.
        /// </summary>
        public SwaggerDefinition ParameterType { get; set; }

        /// <summary>
        /// Specifies if this parameter is required.
        /// </summary>
        public bool Required { get; set; }
    }
}