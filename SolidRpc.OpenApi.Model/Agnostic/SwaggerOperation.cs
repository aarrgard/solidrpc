using System.Collections.Generic;

namespace SolidRpc.OpenApi.Model.Agnostic
{
    public class SwaggerOperation
    {
        /// <summary>
        /// Constructs a new instance
        /// </summary>
        public SwaggerOperation()
        {
            Exceptions = new SwaggerDefinition[0];
            Parameters = new SwaggerOperationParameter[0];
        }

        /// <summary>
        /// The operation tags
        /// </summary>
        public IEnumerable<SwaggerTag> Tags { get; set; }

        /// <summary>
        /// The operation id.
        /// </summary>
        public string OperationId { get; set; }

        /// <summary>
        /// The exceptions raised by this operation.
        /// </summary>
        public IEnumerable<SwaggerDefinition> Exceptions { get; set; }

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

        /// <summary>
        /// The security definitions
        /// </summary>
        public IEnumerable<IDictionary<string, IEnumerable<string>>> Security { get; set; }
    }
}
