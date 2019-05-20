using System;
using System.Linq;
using SolidRpc.Swagger.Generator.Code.Binder;

namespace SolidRpc.Swagger.Generator
{
    /// <summary>
    /// The settings for generating code from a swagger specification.
    /// </summary>
    public class SwaggerCodeSettings
    {
        public SwaggerCodeSettings()
        {
            OperationMapper = (settings, operation) =>
            {
                var className = new QualifiedName(settings.Namespace, operation.Tags.First());
                return new CSharpMethod()
                {
                    ReturnType = settings.ItemMapper(settings, operation.ReturnType),
                    ClassName = className,
                    MethodName = operation.OperationId,
                    Parameters = operation.Parameters.Select(o => new CSharpMethodParameter()
                    {
                        Name = o.Name,
                        ParameterType = settings.ItemMapper(settings, o.ParameterType)
                    }).ToList()
                };
            };
            ItemMapper = (settings, item) =>
            {
                return new CSharpObject()
                {
                    Name = item.Name
                };
            };

        }

        /// <summary>
        /// The swagger json.
        /// </summary>
        public string SwaggerSpec { get; set; }

        /// <summary>
        /// The namespace to add to all the generated classes.
        /// </summary>
        public string Namespace { get; set; }

        /// <summary>
        /// The output path. May be a folder or zip.
        /// </summary>
        public string OutputPath { get; set; }

        /// <summary>
        /// Method to map from a swagger operation to a C# method
        /// </summary>
        public Func<SwaggerCodeSettings, SwaggerOperation, CSharpMethod> OperationMapper { get; set; }

        /// <summary>
        /// Method to map from a swagger object to a c# object.
        /// </summary>
        public Func<SwaggerCodeSettings, SwaggerDefinition, CSharpObject> ItemMapper { get; set; }
    }
}
