using System.Collections.Generic;

namespace SolidRpc.Swagger.Generator.Code.Binder
{
    public class SwaggerOperation
    {
        /// <summary>
        /// The operation tags
        /// </summary>
        public IEnumerable<string> Tags { get; set; }

        /// <summary>
        /// The operation id.
        /// </summary>
        public string OperationId { get; set; }

        /// <summary>
        /// The return type
        /// </summary>
        public SwaggerDefinition ReturnType { get; set; }

        /// <summary>
        /// The operation parameters.
        /// </summary>
        public IEnumerable<SwaggerOperationParameter> Parameters { get; set; }

        /// <summary>
        /// The description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The summary
        /// </summary>
        public string Summary { get; set; }
    }
}
