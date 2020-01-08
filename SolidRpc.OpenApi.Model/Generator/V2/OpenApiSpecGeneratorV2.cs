using SolidRpc.Abstractions.OpenApi.Model;
using SolidRpc.OpenApi.Model.CSharp;
using SolidRpc.OpenApi.Model.V2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace SolidRpc.OpenApi.Model.Generator.V2
{
    /// <summary>
    /// Creates a swagger specification
    /// </summary>
    public class OpenApiSpecGeneratorV2 : OpenApiSpecGenerator
    {
        /// <summary>
        /// Constructs a new instance.
        /// </summary>
        public OpenApiSpecGeneratorV2() : this(new SettingsSpecGen())
        {
        }

        /// <summary>
        /// Constructs a new instance.
        /// </summary>
        /// <param name="settings"></param>
        public OpenApiSpecGeneratorV2(SettingsSpecGen settings = null) : base(settings)
        {
        }

        /// <summary>
        /// Returns all the interfaces in the repository
        /// </summary>
        /// <param name="cSharpRepository"></param>
        /// <returns></returns>
        public IEnumerable<ICSharpInterface> GetInterfaces(ICSharpRepository cSharpRepository)
        {
            return cSharpRepository.Interfaces
                .Where(o => o.RuntimeType == null)
                .Where(o => o.EnumerableType == null)
                .Where(o => o.TaskType == null)
                .OrderBy(o => o.FullName);
        }

        public override IOpenApiSpec CreateSwaggerSpec(IOpenApiSpecResolver openApiSpecResolver, ICSharpRepository cSharpRepository)
        {
            var swaggerObject = new SwaggerObject(null)
            {
                Swagger = "2.0",
                Host = "localhost",
                BasePath = Settings.BasePath,
            };
            swaggerObject.Schemes = new [] { "https" };
            swaggerObject.SetOpenApiSpecResolver(openApiSpecResolver, "");
            swaggerObject.Paths = CreatePaths(cSharpRepository, swaggerObject);
            swaggerObject.Info = new InfoObject(swaggerObject)
            {
                Title = Settings.Title ?? "OpenApi",
                Version = Settings.Version ?? "1.0.0",
                Description = Settings.Description
            };
            swaggerObject.Info.License = CreateLicense(swaggerObject.Info);
            swaggerObject.Info.Contact = CreateContact(swaggerObject.Info);
            swaggerObject.Tags = GetInterfaces(cSharpRepository)
                .Where(o => !o.IsGenericType)
                .Select(o => CreateTag(swaggerObject, o)).ToList();
            return swaggerObject;
        }

        private TagObject CreateTag(SwaggerObject swaggerObject, ICSharpType o)
        {
            return new TagObject(swaggerObject)
            {
                Name = TypeDefinitionNameMapper(o),
                Description = o.Comment?.Summary
            };
        }

        private PathsObject CreatePaths(ICSharpRepository cSharpRepository, SwaggerObject swaggerObject)
        {
            var paths = new PathsObject(swaggerObject);
            GetInterfaces(cSharpRepository).SelectMany(o => o.Methods)
                .OrderBy(o => o.FullName)
                .ToList().ForEach(m =>
            {
                var (pathItemObject, operation) = CreatePathItemObject(paths, m);
                var path = MapPath(m.FullName);
                operation.GetParameters()
                    .Where(o => o.In == "path")
                    .Select(o => o.Name)
                    .ToList().ForEach(o =>
                    {
                        path = $"{path}/{{{o}}}";

                    });
                if(!string.IsNullOrEmpty(Settings.BasePath) && path.StartsWith(Settings.BasePath))
                {
                    path = path.Substring(Settings.BasePath.Length);
                }
                paths[path] = pathItemObject;
            });
            return paths;
        }

        private (PathItemObject, OperationObject) CreatePathItemObject(PathsObject paths, ICSharpMethod method)
        {
            var pathItemObject = new PathItemObject(paths);
            var operation = CreateOperationObject(pathItemObject, method);

            var paramTypes = operation.GetParameters()
                .Select(o => o.In)
                .Distinct();
            var mustBePost = paramTypes
                .Where(o => o != "query")
                .Where(o => o != "path")
                .Any();
            if (mustBePost)
            {
                pathItemObject.Post = operation;
            }
            else
            {
                pathItemObject.Get = operation;
            }
            return (pathItemObject, operation);
        }

        private OperationObject CreateOperationObject(PathItemObject pathItemObject, ICSharpMethod method)
        {
            var operationObject = new OperationObject(pathItemObject);
            operationObject.Tags = new string[] { CreateTag(null, (ICSharpType)method.Parent).Name };
            operationObject.OperationId = method.Name;
            operationObject.Description = method.Comment?.Summary;

            var parameters = method.Parameters
                .Where(o => o.ParameterType.RuntimeType != typeof(CancellationToken));

            if (parameters.Any(o => o.ParameterType.IsFileType))
            {
                parameters = parameters.Where(o =>
                {
                    if (TypeExtensions.FileTypeProperties.TryGetValue(o.Name.ToLower(), out Type t))
                    {
                        return o.ParameterType.RuntimeType != t;
                    }
                    return true;
                });
            }

            parameters
                .ToList()
                .ForEach(o =>
                {

                    //
                    // get the schema for the property
                    //
                    var schema = GetSchema(operationObject, true, o.ParameterType);
                    ParameterObject po;
                    if (schema.GetBaseType() == "object")
                    {
                        po = operationObject.GetParameter("body");
                        po.In = "body";
                        po.Schema = po.Schema ?? new SchemaObject(po);
                        po.Schema.Type = "object";
                        po.Schema.Properties = po.Schema.Properties ?? new DefinitionsObject(po);
                        po.Schema.Properties[o.Name] = GetSchema(po.Schema, false, o.ParameterType);
                    }
                    else
                    {
                        po = operationObject.GetParameter(o.Name);
                        po.Description = o.Comment?.Summary;
                        po.Required = !o.Optional;
                        if(po.Required)
                        {
                            po.In = "path";
                        }
                        else
                        {
                            po.In = "query";
                        }

                        SetItemProps(po, true, o.ParameterType);
                    }
                });

            //
            // if body only contains one parameter - move it to the root level.
            //
            var bodyParameter = operationObject.GetParameters().FirstOrDefault(o => o.Name == "body");
            if (bodyParameter != null && bodyParameter.Schema.Properties.Count == 1)
            {
                bodyParameter.Name = bodyParameter.Schema.Properties.First().Key;
                bodyParameter.Schema = bodyParameter.Schema.Properties.First().Value;
            }

            //
            // Handle file parameters
            //
            var fileParam = operationObject.GetParameters().FirstOrDefault(o => o.Type == "file");
            if (fileParam!=null)
            {
                operationObject.Parameters.Where(o => o.Type == "file").ToList().ForEach(o => o.In = "formData");
                // remove the content-type and filename parameters
                operationObject.Parameters = operationObject.Parameters
                    .Where(o => !o.Name.Equals("contenttype", StringComparison.InvariantCultureIgnoreCase))
                    .Where(o => !o.Name.Equals("filename", StringComparison.InvariantCultureIgnoreCase))
                    .ToList();
            }
            foreach(var fileType in operationObject.GetParameters().Where(o => o.IsBinaryType()))
            {
                fileType.In = "formData";
                fileType.Type = "file";
                fileType.Schema = null;
            }

            operationObject.Responses = CreateResponses(operationObject, method);

            //
            // set the consumes and produces based on parameters and responses
            //
            if (operationObject.GetParameters().Any(o => o.In == "formData"))
            {
                operationObject.AddConsumes("multipart/form-data");
            }
            else if(operationObject.GetParameters().Any(o => o.In == "body"))
            {
                operationObject.AddConsumes("application/json");
            }
            if (operationObject.Responses.Any(o => o.Value.Schema?.Type == "file"))
            {
                operationObject.AddProduces("*/*");
            }
            else if (operationObject.Responses.Where(o => o.Key == "200").Any(o => o.Value.Schema != null))
            {
                operationObject.AddProduces("application/json");
            }
            return operationObject;
        }

        private ResponsesObject CreateResponses(OperationObject operationObject, ICSharpMethod method)
        {
            var responsesObject = new ResponsesObject(operationObject);
            switch (method.ReturnType.FullName)
            {
                case null:
                case "void":
                case "System.Threading.Tasks.Task":
                    break;
                default:
                    var responseSchema = GetSchema(responsesObject, true, method.ReturnType.TaskType ?? method.ReturnType);
                    responsesObject["200"] = new ResponseObject(responsesObject)
                    {
                        Schema = responseSchema,
                        Description = responseSchema.GetRefSchema()?.Description ?? responseSchema?.Description ?? "The response type"
                    };
                    break;
            }  
            if(!responsesObject.Any())
            {
                var successResponse = new ResponseObject(responsesObject);
                successResponse.Description = "Success";
                responsesObject["200"] = successResponse;
            }
            return responsesObject;
        }

        private SchemaObject GetSchema(ModelBase parent, bool canHandleFile, ICSharpType type)
        {
            var so = new SchemaObject(parent);
            SetItemProps(so, canHandleFile, type);
            return so;
         }

        private void SetItemProps(ItemBase itemBase, bool canHandleFile, ICSharpType type)
        {
            if (type.IsFileType)
            {
                if (canHandleFile)
                {
                    itemBase.Type = "file";
                    return;
                }
            }
            if (type.EnumerableType != null)
            {
                itemBase.Type = "array";
                itemBase.Items = GetSchema(itemBase, canHandleFile, type.EnumerableType);
                return;
            }
            else if (type.NullableType != null)
            {
                SetItemProps(itemBase, canHandleFile, type.NullableType);
                return;
            }
            else if (type.RuntimeType != null)
            {
                SchemaObject.SetTypeInfo(itemBase, type.RuntimeType);
            }
            else
            {
                var refName = CreateRefObject(itemBase, (ICSharpClass)type);
                itemBase.Ref = refName;
            }
        }
        private string CreateRefObject(ModelBase node, ICSharpClass clazz)
        {
            var defName = TypeDefinitionNameMapper(clazz);
            var swaggerObject = node.GetParent<SwaggerObject>();
            if(swaggerObject.Definitions == null)
            {
                swaggerObject.Definitions = new DefinitionsObject(swaggerObject);
            }
            if(!swaggerObject.Definitions.ContainsKey(defName))
            {
                var so = new SchemaObject(swaggerObject.Definitions)
                {
                    Type = "object",
                    Description = clazz.Comment?.Summary
                };
                swaggerObject.Definitions[defName] = so;
                so.Properties = CreateDefinitionsObject(so, clazz);
                so.Properties = CreateDefinitionsObject(so, clazz);
            }

            return $"#/definitions/{defName}";
        }

        private DefinitionsObject CreateDefinitionsObject(ModelBase parent, ICSharpClass clazz)
        {
            if (!clazz.Properties.Any())
            {
                return null;
            }
            var definitionsObject = new DefinitionsObject(parent);
            clazz.Properties.ToList().ForEach(o => {
                var schema = GetSchema(definitionsObject, false, o.PropertyType);
                if(schema.Ref == null)
                {
                    schema.Description = o.Comment?.Summary;
                }
                definitionsObject[o.Name] = schema;
            });
            return definitionsObject;
        }

        private ContactObject CreateContact(InfoObject info)
        {
            var contact = new ContactObject(info)
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

        private LicenseObject CreateLicense(InfoObject info)
        {
            if (string.IsNullOrEmpty(Settings.LicenseName))
            {
                return null;
            }
            return new LicenseObject(info)
            {
                Name = Settings.LicenseName,
                Url = Settings.LicenseUrl
            };
        }
    }
}