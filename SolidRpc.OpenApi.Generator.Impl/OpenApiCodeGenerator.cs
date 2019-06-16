using SolidRpc.OpenApi.Generator.Impl.Code.Binder;
using SolidRpc.OpenApi.Generator.Model.CSharp;
using SolidRpc.OpenApi.Generator.Model.CSharp.Impl;
using SolidRpc.OpenApi.Generator.Types;
using SolidRpc.OpenApi.Generator.V2;
using SolidRpc.OpenApi.Model;
using SolidRpc.OpenApi.Model.V2;
using SolidRpc.OpenApi.Model.V3;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SolidRpc.OpenApi.Generator
{
    /// <summary>
    /// Code generator to create interfaces and classes from a swagger specification.
    /// </summary>
    public abstract class OpenApiCodeGenerator
    {
        private static string CapitalizeFirstChar(string name)
        {
            if (char.IsUpper(name[0]))
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

        protected OpenApiCodeGenerator(SettingsCodeGen codeSettings)
        {
            CodeSettings = codeSettings;
            OperationMapper = (settings, operation) =>
            {
                var tag = operation.Tags.First();
                var className = new QualifiedName(
                    settings.ProjectNamespace,
                    settings.CodeNamespace,
                    settings.ServiceNamespace,
                    InterfaceNameMapper(tag.Name));
                return new Impl.Code.Binder.CSharpMethod()
                {
                    ReturnType = DefinitionMapper(settings, operation.ReturnType),
                    InterfaceName = className,
                    MethodName = MethodNameMapper(operation.OperationId),
                    Parameters = operation.Parameters.Select(o => new Impl.Code.Binder.CSharpMethodParameter()
                    {
                        Name = o.Name,
                        ParameterType = DefinitionMapper(settings, o.ParameterType),
                        Optional = !o.Required,
                        Description = o.Description
                    }).ToList(),
                    ClassSummary = MapDescription(tag.Description),
                    MethodSummary = MapDescription(operation.OperationDescription)
                };
            };
            DefinitionMapper = (settings, swaggerDef) =>
            {
                if (swaggerDef == null) return null;
                if (string.IsNullOrEmpty(swaggerDef.Name)) throw new Exception("Name is null or empty");
                if (swaggerDef.ArrayType != null)
                {
                    return new CSharpObject(DefinitionMapper(settings, swaggerDef.ArrayType));
                }
                var className = swaggerDef.Name;
                if (!swaggerDef.IsReservedName)
                {
                    if (swaggerDef.SwaggerOperation != null)
                    {
                        className = swaggerDef.SwaggerOperation.OperationId + className;
                    }
                    className = new QualifiedName(
                        settings.ProjectNamespace,
                        settings.CodeNamespace,
                        settings.TypeNamespace,
                        ClassNameMapper(className));
                }
                var csObj = new CSharpObject(className);
                csObj.Properties = swaggerDef.Properties.Select(o => new Impl.Code.Binder.CSharpProperty()
                {
                    PropertyName = PropertyNameMapper(o.Name),
                    PropertyType = DefinitionMapper(settings, o.Type),
                    Description = o.Description
                });
                csObj.AdditionalProperties = DefinitionMapper(settings, swaggerDef.AdditionalProperties);
                return csObj;
            };
            InterfaceNameMapper = qn => NameStartsWithLetter(CapitalizeFirstChar(qn), 'I');
            ClassNameMapper = CapitalizeFirstChar;
            MethodNameMapper = CapitalizeFirstChar;
            PropertyNameMapper = CapitalizeFirstChar;
        }

        private CSharpDescription MapDescription(SwaggerDescription description)
        {
            if(string.IsNullOrEmpty(description?.Description) &&
                string.IsNullOrEmpty(description?.ExternalUri))
            {
                return null;
            }
            return new CSharpDescription()
            {
                ExternalDescription = description.ExternalDescription,
                ExternalUri = description.ExternalUri,
                Summary = description.Description
            };
        }

        public SettingsCodeGen CodeSettings { get; }

        /// <summary>
        /// Method to map from a swagger operation to a C# method
        /// </summary>
        public Func<SettingsCodeGen, SwaggerOperation, Impl.Code.Binder.CSharpMethod> OperationMapper { get; set; }

        /// <summary>
        /// Method to map from a swagger object to a c# object.
        /// </summary>
        public Func<SettingsCodeGen, SwaggerDefinition, CSharpObject> DefinitionMapper { get; set; }

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

        /// <summary>
        /// Generates code 
        /// </summary>
        /// <param name="codeSettings"></param>
        public static FileData GenerateCode(SettingsCodeGen codeSettings)
        {
            var codeGenerator = (ICSharpRepository)new CSharpRepository();

            var model = OpenApiParser.ParseSwaggerSpec(codeSettings.SwaggerSpec);
            if (model is SwaggerObject v2)
            {
                new OpenApiCodeGeneratorV2(v2, codeSettings).GenerateCode(codeGenerator);
            }
            else if (model is OpenAPIObject v3)
            {
                new OpenApiCodeGeneratorV3(v3, codeSettings).GenerateCode(codeGenerator);
            }
            else
            {
                throw new Exception("Cannot parse swagger json.");
            }

            var codeWriter = new CodeWriterZip(codeSettings.ProjectNamespace);
            codeGenerator.WriteCode(codeWriter);
            codeWriter.Close();
            codeWriter.ZipOutputStream.Close();

            return new FileData()
            {
                ContentType = "application/zip",
                Filename = "project.zip",
                FileStream = new MemoryStream(codeWriter.MemoryStream.ToArray())
            };
        }
        protected abstract void GenerateCode(ICSharpRepository codeGenerator);

        protected ICSharpClass GetClass(ICSharpRepository csharpRepository, CSharpObject cSharpObject)
        {
            var cls = csharpRepository.GetClass(cSharpObject.Name);
            if(!cls.Initialized)
            {
                cls.Initialized = true;
                if (cSharpObject.AdditionalProperties != null)
                {
                    var dictType = csharpRepository.GetClass(cSharpObject.AdditionalProperties.Name);
                    var extType = csharpRepository.GetClass($"System.Collections.Generic.Dictionary<string,{dictType.FullName}>");
                    cls.AddExtends(extType);
                }
                // add missing properties
                foreach (var prop in cSharpObject.Properties)
                {
                    var propType = GetClass(csharpRepository, prop.PropertyType);
                    var csProp = new Model.CSharp.Impl.CSharpProperty(cls, prop.PropertyName, propType);
                    csProp.ParseComment($"<summary>{prop.Description}</summary>");
                    cls.AddMember(csProp);
                }
                if (cSharpObject.ArrayElement != null)
                {
                    GetClass(csharpRepository, cSharpObject.ArrayElement);
                }
                AddUsings(cls);
            }
            return cls;
        }

        protected void AddUsings(ICSharpType member)
        {
            var namespaces = new HashSet<string>();
            member.GetNamespaces(namespaces);
            if (namespaces.Any(o => o.Contains("<")))
            {
                throw new Exception();
            }
            namespaces.Where(o => !string.IsNullOrEmpty(o)).ToList().ForEach(ns =>
            {
                AddUsings(member, ns);
            });
        }

        private void AddUsings(ICSharpType member, string ns)
        {
            var usings = member.Members.OfType<ICSharpUsing>().Select(o => o.Name);
            if (!usings.Contains(ns)) member.AddMember(new CSharpUsing(member, ns));
        }
    }
}
