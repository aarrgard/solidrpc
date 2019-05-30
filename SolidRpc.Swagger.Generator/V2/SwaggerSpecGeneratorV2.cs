using System;
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
            };
            return swaggerObject;
        }

        private DefinitionsObject CreateDefinitions()
        {
            var definitionsObject = new DefinitionsObject();
            CSharpRepository.Classes
                .Where(o => o.RuntimeType == null)
                .OrderBy(o => o.FullName)
                .ToList().ForEach(o =>
                {
                    definitionsObject[o.FullName] = CreateSchemaObject(o);
                });
            return definitionsObject;
        }

        private SchemaObject CreateSchemaObject(ICSharpClass o)
        {
            return new SchemaObject()
            {
                Type = "object",
            };
        }

        private PathsObject CreatePaths()
        {
            var paths = new PathsObject();
            CSharpRepository.Interfaces
                .Where(o => o.RuntimeType == null)
                .SelectMany(o => o.Methods)
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
            operationObject.OperationId = method.Name;
            operationObject.Parameters = method.Parameters
                .Where(o => o.ParameterType.RuntimeType != typeof(CancellationToken))
                .Select(o => new ParameterObject()
                {
                    Name = o.Name,
                    Required = !o.Optional,
                    Schema = GetSchema(o.ParameterType)
            }).ToList();
            return operationObject;
        }

        private SchemaObject GetSchema(ICSharpType parameterType)
        {
            if(parameterType.RuntimeType != null)
            {
                if (parameterType.RuntimeType == typeof(string))
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
                    Type = "object",
                    Ref = $"#/definitions/{parameterType.FullName}"
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