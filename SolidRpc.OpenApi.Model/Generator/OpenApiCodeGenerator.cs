using SolidRpc.Abstractions;
using SolidRpc.OpenApi.Model.Agnostic;
using SolidRpc.OpenApi.Model.CSharp;
using SolidRpc.OpenApi.Model.CSharp.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace SolidRpc.OpenApi.Model.Generator
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
        private static string CreateCamelCase(string token, bool capitalizeFirstChar)
        {
            var words = token.Split(SolidRpcConstants.OpenApiWordSeparators)
                .Where(o => !string.IsNullOrWhiteSpace(o))
                .ToList();
            return string.Join("", words.Take(1)
                .Select(o => capitalizeFirstChar ? CapitalizeFirstChar(o) : o)
                .Union(words.Skip(1).Select(o => CapitalizeFirstChar(o))));
        }
        private static string NameEndsWith(string name, string suffix)
        {
            if(!name.EndsWith(suffix))
            {
                name = name + suffix;
            }
            return name;
        }

        /// <summary>
        /// Constructs a new instance
        /// </summary>
        /// <param name="codeSettings"></param>
        protected OpenApiCodeGenerator(SettingsCodeGen codeSettings)
        {
            CodeSettings = codeSettings;
            OperationMapper = (settings, operation) =>
            {
                var className = new QualifiedName(
                    settings.ProjectNamespace,
                    settings.CodeNamespace,
                    settings.ServiceNamespace,
                    GetOperationTag(operation).Name);

                // Run the interface name mapper on the last name part
                className = new QualifiedName(
                    className.Names.Reverse().Skip(1).Reverse().Union(
                        className.Names.Reverse().Take(1).Select(o => InterfaceNameMapper(o))
                    ).ToArray()
                );

                return new OpenApi.Model.Agnostic.CSharpMethod()
                {
                    Exceptions = operation.Exceptions.Select(o => DefinitionMapper(settings, o)),
                    ReturnType = DefinitionMapper(settings, operation.ReturnType),
                    InterfaceName = className,
                    MethodName = MethodNameMapper(operation.OperationId),
                    Parameters = operation.Parameters.Select(o => new OpenApi.Model.Agnostic.CSharpMethodParameter()
                    {
                        Name = ParameterNameMapper(o.Name),
                        ParameterType = DefinitionMapper(settings, o.ParameterType),
                        Optional = !o.Required,
                        Description = o.Description
                    }).ToList(),
                    ClassSummary = MapDescription(GetOperationTag(operation).Description),
                    MethodSummary = MapDescription(operation.OperationDescription),
                    SecurityAttribute = MapSecurity(operation.Security)
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
                        var op = swaggerDef.SwaggerOperation;
                        var operationNs = $"{CodeSettings.ServiceNamespace}.{MethodNameMapper(GetOperationTag(op).Name)}.{MethodNameMapper(op.OperationId)}";
                        className = operationNs + "." + className;
                    }
                    className = new QualifiedName(
                        settings.ProjectNamespace,
                        settings.CodeNamespace,
                        settings.TypeNamespace,
                        ClassNameMapper(className));
                }
                var csObj = new CSharpObject(className);
                csObj.Description = swaggerDef.Description;
                csObj.ExceptionCode = swaggerDef.ExceptionCode;
                csObj.Properties = swaggerDef.Properties.Select(o => {
                    var ap = new OpenApi.Model.Agnostic.CSharpProperty()
                    {
                        PropertyName = PropertyNameMapper(o.Name),
                        PropertyType = DefinitionMapper(settings, o.Type),
                        Description = o.Description
                    };
                    if(o.Name != ap.PropertyName)
                    {
                        ap.DataMember = new CSharpDataMember()
                        {
                            Name = o.Name
                        };
                    }
                    return ap;
                });
                csObj.AdditionalProperties = DefinitionMapper(settings, swaggerDef.AdditionalProperties);
                return csObj;
            };
            InterfaceNameMapper = qn => NameStartsWithLetter(CapitalizeFirstChar(qn), 'I');
            ClassNameMapper = CapitalizeFirstChar;
            MethodNameMapper = (s) => { return CreateCamelCase(s, true); };
            ParameterNameMapper = (s) => { return CreateCamelCase(s, false); };
            PropertyNameMapper = (s) => { return CreateCamelCase(s, true); };
            ExceptionNameMapper = (s) => { return NameEndsWith(CreateCamelCase(s, true), "Exception"); };
            SecurityDefinitionMapper = (s) => { return NameEndsWith(CreateCamelCase(s, true), "Attribute"); };
        }

        private IEnumerable<IDictionary<string, IEnumerable<string>>> MapSecurity(IEnumerable<IDictionary<string, IEnumerable<string>>> security)
        {
            return security.Select(o => MapSecurity(o)).ToList();
        }

        private IDictionary<string, IEnumerable<string>> MapSecurity(IDictionary<string, IEnumerable<string>> security)
        {
            return security.ToList().ToDictionary(o => {
                var className = new QualifiedName(
                      CodeSettings.ProjectNamespace,
                      CodeSettings.CodeNamespace,
                      CodeSettings.SecurityNamespace,
                      SecurityDefinitionMapper(o.Key));

                return className.ToString();
            }, o => o.Value);
        }

        private SwaggerTag GetOperationTag(SwaggerOperation swaggerOperation)
        {
            var tag = swaggerOperation.Tags.FirstOrDefault();
            if(tag == null)
            {
                return new SwaggerTag()
                {
                    Name = swaggerOperation.OperationId,
                    Description = new SwaggerDescription() { Description = "Auto generated tag"  }
                };
            }
            return tag;
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

        /// <summary>
        /// The settings
        /// </summary>
        public SettingsCodeGen CodeSettings { get; }

        /// <summary>
        /// Method to map from a swagger operation to a C# method
        /// </summary>
        public Func<SettingsCodeGen, SwaggerOperation, OpenApi.Model.Agnostic.CSharpMethod> OperationMapper { get; set; }

        /// <summary>
        /// Method to map from a swagger object to a c# object.
        /// </summary>
        public Func<SettingsCodeGen, SwaggerDefinition, CSharpObject> DefinitionMapper { get; set; }

        /// <summary>
        /// Function that maps one method name to another.
        /// </summary>
        public Func<string, string> MethodNameMapper { get; set; }

        /// <summary>
        /// Function that maps one parameter name to another.
        /// </summary>
        public Func<string, string> ParameterNameMapper { get; set; }

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
        /// Function that maps the description of the return type on to an exception name.
        /// </summary>
        public Func<string, string> ExceptionNameMapper { get; set; }

        /// <summary>
        /// Function that maps the name of a security definition on to a class name(Attribute).
        /// </summary>
        public Func<string, string> SecurityDefinitionMapper { get; set; }

        /// <summary>
        /// Generates code 
        /// </summary>
        public ICSharpRepository GenerateCode()
        {
            var codeGenerator = (ICSharpRepository)new CSharpRepository();
            GenerateCode(codeGenerator);
            return codeGenerator;
        }

        /// <summary>
        /// Generates the code in supplied repository
        /// </summary>
        /// <param name="codeGenerator"></param>
        protected abstract void GenerateCode(ICSharpRepository codeGenerator);


        /// <summary>
        /// Returns the class that represents the supplied c# object.
        /// </summary>
        /// <param name="csharpRepository"></param>
        /// <param name="cSharpObject"></param>
        /// <returns></returns>
        protected ICSharpClass GetClass(ICSharpRepository csharpRepository, CSharpObject cSharpObject)
        {
            var cls = csharpRepository.GetClass(cSharpObject.Name);
            if(!cls.Initialized)
            {
                cls.Initialized = true;
                cls.ParseComment($"<summary>{cSharpObject.Description}</summary>");
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
                    if(prop.DataMember != null)
                    {
                        var attributeProps = new Dictionary<string, object>()
                        {
                            {  "Name", prop.DataMember.Name },
                            {  "EmitDefaultValue", false },
                        };
                        csProp.AddMember(new CSharpAttribute(csProp, typeof(DataMemberAttribute).FullName, attributeProps));
                    }
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

        /// <summary>
        /// Adds some usings clauses to supplied member
        /// </summary>
        /// <param name="member"></param>
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
