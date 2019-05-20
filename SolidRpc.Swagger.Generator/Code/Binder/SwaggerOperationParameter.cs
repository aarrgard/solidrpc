using SolidRpc.Swagger.Generator.Code.CSharp;

namespace SolidRpc.Swagger.Generator.Code.Binder
{
    /// <summary>
    /// Represents a swagger operation parameter
    /// </summary>
    public class SwaggerOperationParameter
    {
        /// <summary>
        /// The operation name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The parameter type.
        /// </summary>
        public SwaggerDefinition ParameterType { get; set; }
    }
}