using System.Collections.Generic;

namespace SolidRpc.OpenApi.Generator.Impl.Code.Binder
{
    public class SwaggerOperation
    {
        /// <summary>
        /// The operation tags
        /// </summary>
        public IEnumerable<SwaggerTag> Tags { get; set; }

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
        public SwaggerDescription OperationDescription { get; set; }
    }
}
