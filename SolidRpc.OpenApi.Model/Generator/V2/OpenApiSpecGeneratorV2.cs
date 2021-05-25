using SolidRpc.Abstractions.OpenApi.Model;
using SolidRpc.Abstractions.Types;
using SolidRpc.OpenApi.Model.CSharp;
using SolidRpc.OpenApi.Model.V2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
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

        /// <summary>
        /// Creates the swagger spec
        /// </summary>
        /// <param name="openApiSpecResolver"></param>
        /// <param name="cSharpRepository"></param>
        /// <returns></returns>
        public override IOpenApiSpec CreateSwaggerSpec(IOpenApiSpecResolver openApiSpecResolver, ICSharpRepository cSharpRepository)
        {
            var swaggerObject = new SwaggerObject((ModelBase)openApiSpecResolver)
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
                .Where(o => o.ParameterType.RuntimeType != typeof(CancellationToken))
                .Where(o => o.ParameterType.RuntimeType != typeof(IPrincipal));

            if (parameters.Any(o => o.ParameterType.IsFileType))
            {
                parameters = parameters.Where(o =>
                {
                    if (FileContentTemplate.PropertyTypes.TryGetValue(o.Name.ToLower(), out Type t))
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
                    ParameterObject po = operationObject.GetParameter(o.Name);
                    po.Description = o.Comment?.Summary;
                    po.Required = !o.Optional;
                    if(schema.GetBaseType() == "object")
                    {
                        po.In = "formData";
                        po.Schema = schema;
                    }
                    else if(po.Required)
                    {
                        po.In = "path";
                        SetItemProps(po, true, o.ParameterType);
                        if (po.Type == "array") po.CollectionFormat = "csv";
                    }
                    else
                    {
                        po.In = "query";
                        SetItemProps(po, true, o.ParameterType);
                        if (po.Type == "array") po.CollectionFormat = "csv";
                    }
                });

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
            foreach (var fileType in operationObject.GetParameters().Where(o => o.IsBinaryType()))
            {
                fileType.In = "formData";
                fileType.Type = "file";
                fileType.Schema = null;
            }

            //
            // remove all the http request references from the arguments
            //
            bool foundHttpRequestArg = false;
            foreach (var httpRequest in operationObject.GetParameters().Where(o => o.IsHttpRequestType()))
            {
                foundHttpRequestArg = true;
                operationObject.Parameters = operationObject.Parameters.Where(o => !ReferenceEquals(o, httpRequest)).ToList();
            }
            //
            // if we have more than one "formData" parameters - convert all the refs to strings
            //
            var formDataParameters = operationObject.GetParameters()
                .Where(o => o.In == "formData").ToList();
            if (formDataParameters.Count > 1)
            {
                formDataParameters.Where(o => o.Schema?.GetRefSchema() != null).ToList().ForEach(o =>
                {
                    var refSchema = o.Schema.GetRefSchema();
                    o.Schema = null;
                    o.Type = "string";
                    o.Description = $"Serialized {refSchema.GetOperationName()}:{o.Description}";
                });
            }
            else
            {
                formDataParameters.Where(o => o.Type != "file").ToList().ForEach(o => o.In = "body");
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


            // if we removed httprequest arg and no other parameters exists
            // we put the call on all the other operations available.
            if (foundHttpRequestArg && !operationObject.GetParameters().Any())
            {
                var pathItem = operationObject.GetParent<PathItemObject>();
                if (pathItem.Get == null) pathItem.Get = CloneOperation(operationObject, "Get");
                if (pathItem.Put == null) pathItem.Put = CloneOperation(operationObject, "Put");
                if (pathItem.Post == null) pathItem.Post = CloneOperation(operationObject, "Post");
                if (pathItem.Head == null) pathItem.Head = CloneOperation(operationObject, "Head");
                if (pathItem.Options == null) pathItem.Options = CloneOperation(operationObject, "Options");
                if (pathItem.Patch == null) pathItem.Patch = CloneOperation(operationObject, "Patch");
                if (pathItem.Delete == null) pathItem.Delete = CloneOperation(operationObject, "Delete");
            }

            // let the operation id reflect the number of path arguments there are
            var pathArgs = operationObject.GetParameters().Where(o => o.In == "path");
            if(pathArgs.Any())
            {
                operationObject.OperationId = $"{operationObject.OperationId}#{pathArgs.Count()}";
            }

            return operationObject;
        }

        private OperationObject CloneOperation(OperationObject operationObject, string method)
        {
            var resolver = operationObject.GetParent<IOpenApiSpecResolver>();
            var newOp = resolver.OpenApiParser.CloneNode(operationObject);
            newOp.OperationId = $"{operationObject.OperationId}#{method}";
            return newOp;
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
                itemBase.Items = GetSchema(itemBase, false, type.EnumerableType);
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
                return;
            }
            else if (type.IsEnumType)
            {
                CreateEnum(itemBase, (ICSharpEnum)type);
                return;
            }
            else if (type.IsDictionaryType(out ICSharpType keyType, out ICSharpType valueType))
            {
                if(keyType.RuntimeType != typeof(string))
                {
                    throw new Exception("Dictionaries must have a key type of string.");
                }
                itemBase.Type = "object";
                var so = (SchemaObject)itemBase;
                so.AdditionalProperties = so.AdditionalProperties ?? new SchemaObject(so);
                SetItemProps(so.AdditionalProperties, false, valueType);
                return;
            }
            else
            {
                var refName = CreateRefObject(itemBase, type);
                itemBase.Ref = refName;
                return;
            }
        }
        private void CreateEnum(ItemBase node, ICSharpEnum e)
        {
            node.Type = "string";
            node.Description = e.Comment?.Summary;
            node.Enum = e.EnumValues.Select(o => o.Name).ToList();
        }

        private string CreateRefObject(ModelBase node, ICSharpType type)
        {
            var defName = TypeDefinitionNameMapper(type);
            var definitions = node.GetParent<SwaggerObject>().GetDefinitions();
            if(!definitions.ContainsKey(defName))
            {
                var so = new SchemaObject(definitions)
                {
                    Type = "object",
                    Description = type.Comment?.Summary
                };
                definitions[defName] = so;
                if(type is ICSharpClass clazz)
                {
                    so.Properties = CreateDefinitionsObject(so, clazz);
                }
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