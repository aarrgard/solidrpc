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
        private static string CapitalizeFirstChar(string name)
        {
            if(char.IsUpper(name[0]))
            {
                return name;
            }
            return char.ToUpper(name[0]) + name.Substring(1);
        }
        private static string NameStartsWithLetter(string name, char letter)
        {
            if (name[0] == letter)
            {
                return name;
            }
            return letter + name;
        }

        public SwaggerCodeSettings()
        {
            OperationMapper = (settings, operation) =>
            {
                var className = new QualifiedName(
                    settings.ProjectNamespace,
                    settings.CodeNamespace,
                    settings.ServiceNamespace,
                    settings.InterfaceNameMapper(operation.Tags.First()));
                return new CSharpMethod()
                {
                    ReturnType = settings.DefinitionMapper(settings, operation.ReturnType),
                    InterfaceName = className,
                    MethodName = settings.MethodNameMapper(operation.OperationId),
                    Parameters = operation.Parameters.Select(o => new CSharpMethodParameter()
                    {
                        Name = o.Name,
                        ParameterType = settings.DefinitionMapper(settings, o.ParameterType),
                        Description = o.Description
                    }).ToList(),
                    ClassSummary = operation.TagDescription,
                    MethodSummary = $"{operation.OperationSummary} {operation.OperationDescription}".Trim()
                };
            };
            DefinitionMapper = (settings, swaggerDef) =>
            {
                if(string.IsNullOrEmpty(swaggerDef.Name)) throw new Exception("Name is null or empty");
                if(swaggerDef.ArrayType != null)
                {
                    return new CSharpObject(settings.DefinitionMapper(settings, swaggerDef.ArrayType));
                }
                var className = swaggerDef.Name;
                if(!swaggerDef.IsReservedName)
                {
                    if (swaggerDef.SwaggerOperation != null)
                    {
                        className = swaggerDef.SwaggerOperation.OperationId + className;
                    }
                    className = new QualifiedName(
                        settings.ProjectNamespace,
                        settings.CodeNamespace,
                        settings.TypeNamespace,
                        settings.ClassNameMapper(className));
                }
                var csObj = new CSharpObject(className);
                csObj.Properties = swaggerDef.Properties.Select(o => new CSharpProperty()
                {
                    PropertyName = settings.PropertyNameMapper(o.Name),
                    PropertyType = settings.DefinitionMapper(settings, o.Type),
                    Description = o.Description
                });
                return csObj;
            };
            InterfaceNameMapper = qn => NameStartsWithLetter(CapitalizeFirstChar(qn), 'I');
            ClassNameMapper = CapitalizeFirstChar;
            MethodNameMapper = CapitalizeFirstChar;
            PropertyNameMapper = CapitalizeFirstChar;
            TypeNamespace = "Types";
            ServiceNamespace = "Services";
            UseAsyncAwaitPattern = true;
        }

        /// <summary>
        /// The swagger json.
        /// </summary>
        public string SwaggerSpec { get; set; }

        /// <summary>
        /// The namespace that the project belongs to. This namespace 
        /// will not be included in the filepath.
        /// </summary>
        public string ProjectNamespace { get; set; }

        /// <summary>
        /// The namespace that will be added to the project namespace
        /// before adding the type or service namespace.
        /// </summary>
        public string CodeNamespace { get; set; }

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

        public bool UseAsyncAwaitPattern { get; set; }

        /// <summary>
        /// Method to map from a swagger operation to a C# method
        /// </summary>
        public Func<SwaggerCodeSettings, SwaggerOperation, CSharpMethod> OperationMapper { get; set; }

        /// <summary>
        /// Method to map from a swagger object to a c# object.
        /// </summary>
        public Func<SwaggerCodeSettings, SwaggerDefinition, CSharpObject> DefinitionMapper { get; set; }

        /// <summary>
        /// Function that maps one method name to another.
        /// </summary>
        public Func<string, string> MethodNameMapper { get; set; }

        /// <summary>
        /// Function that maps one interface name to another.
        /// </summary>
        public Func<string, string> InterfaceNameMapper { get; set; }

        /// <summary>
        /// Function that maps one class name to another.
        /// </summary>
        public Func<string, string> ClassNameMapper { get; set; }

        /// <summary>
        /// Function that maps one property name to another.
        /// </summary>
        public Func<string, string> PropertyNameMapper { get; set; }
    }
}
