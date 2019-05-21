using System;
using System.Linq;
using SolidRpc.Swagger.Generator.Code;
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
                var className = new QualifiedName(settings.RootNamespace, settings.ServiceNamespace, operation.Tags.First());
                return new CSharpMethod()
                {
                    ReturnType = settings.ItemMapper(settings, operation.ReturnType),
                    InterfaceName = className,
                    MethodName = operation.OperationId,
                    Parameters = operation.Parameters.Select(o => new CSharpMethodParameter()
                    {
                        Name = o.Name,
                        ParameterType = settings.ItemMapper(settings, o.ParameterType)
                    }).ToList()
                };
            };
            ItemMapper = (settings, swaggerDef) =>
            {
                if(string.IsNullOrEmpty(swaggerDef.Name)) throw new Exception("Name is null or empty");
                var name = swaggerDef.Name;
                if(!swaggerDef.IsReservedName)
                {
                    if (swaggerDef.SwaggerOperation != null)
                    {
                        name = swaggerDef.SwaggerOperation.OperationId + name;
                    }
                    name = new QualifiedName(settings.RootNamespace, settings.TypeNamespace, name);
                }
                var csObj = new CSharpObject(name);
                csObj.IsArray = swaggerDef.IsArray;
                csObj.Properties = swaggerDef.Properties.Select(o => new CSharpProperty()
                {
                    PropertyName = o.Name,
                    PropertyType = settings.ItemMapper(settings, o.Type)
                });
                return csObj;
            };
            TypeNamespace = "Types";
            ServiceNamespace = "Services";
        }

        /// <summary>
        /// The swagger json.
        /// </summary>
        public string SwaggerSpec { get; set; }

        /// <summary>
        /// The namespace to add to all the generated classes.
        /// </summary>
        public string RootNamespace { get; set; }

        /// <summary>
        /// The namespace to append to the root namespace for all the types
        /// </summary>
        public string TypeNamespace { get; set; }

        /// <summary>
        /// The namespace to append to the root namespace for all the services(interfaces)
        /// </summary>
        public string ServiceNamespace { get; set; }

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
