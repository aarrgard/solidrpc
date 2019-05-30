using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using SolidRpc.Swagger.Generator.Model.CSharp;
using SolidRpc.Swagger.Model;
using SolidRpc.Swagger.Model.V2;

namespace SolidRpc.Swagger.Generator.V2
{
    public class SwaggerSpecGeneratorV2 : SwaggerSpecGenerator
    {
        public SwaggerSpecGeneratorV2(SwaggerSpecSettings settings) : base(settings)
        {
        }

        public IEnumerable<ICSharpInterface> Interfaces => CSharpRepository.Interfaces
                .Where(o => o.RuntimeType == null)
                .Where(o => o.EnumerableType == null)
                .Where(o => o.TaskType == null)
                .OrderBy(o => o.FullName);

        protected override ISwaggerSpec CreateSwaggerSpecFromTypesInRepo()
        {
            var swaggerObject = new SwaggerObject
            {
                Swagger = "2.0",
                Info = new InfoObject()
                {
                    Title = Settings.Title,
                    Version = Settings.Version,
                    License = CreateLicense(),
                    Contact = CreateContact()
                },
                Host = "localhost",
                BasePath = Settings.BasePath,
                Paths = CreatePaths(),
                Definitions = CreateDefinitions(),
                Tags = Interfaces.Select(o => CreateTag(o))
            };
            return swaggerObject;
        }

        private TagObject CreateTag(ICSharpType o)
        {
            return new TagObject()
            {
                Name = Settings.TypeDefinitionNameMapper(o),
                Description = o.Comment?.Summary
            };
        }

        private DefinitionsObject CreateDefinitions()
        {
            var definitionsObject = new DefinitionsObject();
            CSharpRepository.Classes
                .Where(o => o.RuntimeType == null)
                .Where(o => o.EnumerableType == null)
                .Where(o => o.TaskType == null)
                .OrderBy(o => o.FullName)
                .ToList().ForEach(o =>
                {
                    var refName = Settings.TypeDefinitionNameMapper(o);
                    definitionsObject[refName] = CreateSchemaObject(o);
                });
            return definitionsObject;
        }

        private SchemaObject CreateSchemaObject(ICSharpClass clazz)
        {
            return new SchemaObject()
            {
                Type = "object",
                Description = clazz.Comment?.Summary,
                Properties = CreateDefinitionsObject(clazz)
            };
        }

        private DefinitionsObject CreateDefinitionsObject(ICSharpClass clazz)
        {
            if(!clazz.Properties.Any())
            {
                return null;
            }
            var definitionsObject = new DefinitionsObject();
            clazz.Properties.ToList().ForEach(o => {
                var schema = GetSchema(o.PropertyType);
                schema.Description = o.Comment?.Summary;
                definitionsObject[o.Name] = schema;
            });
            return definitionsObject;
        }

        private PathsObject CreatePaths()
        {
            var paths = new PathsObject();
            Interfaces.SelectMany(o => o.Methods)
                .OrderBy(o => o.FullName)
                .ToList().ForEach(o =>
            {
                var path = Settings.MapPath(o.FullName);
                if(!string.IsNullOrEmpty(Settings.BasePath) && path.StartsWith(Settings.BasePath))
                {
                    path = path.Substring(Settings.BasePath.Length);
                }
                paths[path] = CreatePathItemObject(o);
            });
            return paths;
        }

        private PathItemObject CreatePathItemObject(ICSharpMethod method)
        {
            var pathItemObject = new PathItemObject();
            pathItemObject.Post = CreateOperationObject(method);
            return pathItemObject;
        }

        private OperationObject CreateOperationObject(ICSharpMethod method)
        {
            var operationObject = new OperationObject();
            operationObject.Tags = new string[] { CreateTag((ICSharpType)method.Parent).Name };
            operationObject.OperationId = method.Name;
            operationObject.Description = method.Comment?.Summary;
            operationObject.Parameters = method.Parameters
                .Where(o => o.ParameterType.RuntimeType != typeof(CancellationToken))
                .Select(o => new ParameterObject()
                {
                    Description = o.Comment?.Summary,
                    Name = o.Name,
                    Required = !o.Optional,
                    Schema = GetSchema(o.ParameterType)
            }).ToList();
            operationObject.Responses = CreateResponses(method);
            return operationObject;
        }

        private ResponsesObject CreateResponses(ICSharpMethod method)
        {
            var responsesObject = new ResponsesObject();
            switch (method.ReturnType.FullName)
            {
                case null:
                case "void":
                case "System.Threading.Tasks.Task":
                    break;
                default:
                    responsesObject["200"] = new ResponseObject()
                    {
                        Schema = GetSchema(method.ReturnType.TaskType ?? method.ReturnType)
                    };
                    break;
            }  
            if(!responsesObject.Any())
            {
                return null;
            }
            return responsesObject;
        }

        private SchemaObject GetSchema(object p)
        {
            throw new NotImplementedException();
        }

        private SchemaObject GetSchema(ICSharpType parameterType)
        {
            if(parameterType.RuntimeType != null)
            {
                if (parameterType.RuntimeType == typeof(bool))
                {
                    return new SchemaObject() { Type = "boolean" };
                }
                else if (parameterType.RuntimeType == typeof(string))
                {
                    return new SchemaObject() { Type = "string" };
                }
                else if (parameterType.RuntimeType == typeof(short))
                {
                    return new SchemaObject() { Type = "number", Format = "int16" };
                }
                else if (parameterType.RuntimeType == typeof(int))
                {
                    return new SchemaObject() { Type = "number", Format = "int32" };
                }
                else if (parameterType.RuntimeType == typeof(long))
                {
                    return new SchemaObject() { Type = "number", Format = "int64" };
                }
                else if (parameterType.RuntimeType == typeof(System.IO.Stream))
                {
                    return new SchemaObject() { Type = "string", Format = "binary" };
                }
                else if (parameterType.RuntimeType == typeof(System.DateTime))
                {
                    return new SchemaObject() { Type = "string", Format = "date-time" };
                }

                throw new NotImplementedException(parameterType.RuntimeType.GetType().FullName);
            }
            if (parameterType.EnumerableType != null)
            {
                return new SchemaObject() { Type = "array", Items = GetSchema(parameterType.EnumerableType) };
            }
            else
            {
                return new SchemaObject()
                {
                    Description = parameterType.Comment?.Summary,
                    Type = "object",
                    Ref = $"#/definitions/{Settings.TypeDefinitionNameMapper(parameterType)}"
                };
            }
        }

        private ContactObject CreateContact()
        {
            var contact = new ContactObject()
            {
                Email = Settings.ContactEmail,
                Name = Settings.ContactName,
                Url = Settings.ContactUrl
            };
            if(string.IsNullOrEmpty(contact.Email) &&
                string.IsNullOrEmpty(contact.Name) &&
                string.IsNullOrEmpty(contact.Url))
            {
                return null;
            }
            return contact;
        }

        private LicenseObject CreateLicense()
        {
            if (string.IsNullOrEmpty(Settings.LicenseName))
            {
                return null;
            }
            return new LicenseObject()
            {
                Name = Settings.LicenseName,
                Url = Settings.LicenseUrl
            };
        }
    }
}