using SolidRpc.OpenApi.Generator.Model.CSharp;
using SolidRpc.OpenApi.Generator.Types;
using SolidRpc.OpenApi.Model;
using SolidRpc.OpenApi.Model.V2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace SolidRpc.OpenApi.Generator.V2
{
    public class OpenApiSpecGeneratorV2 : OpenApiSpecGenerator
    {
        public OpenApiSpecGeneratorV2(SettingsSpecGen settings) : base(settings)
        {
        }

        public IEnumerable<ICSharpInterface> Interfaces => CSharpRepository.Interfaces
                .Where(o => o.RuntimeType == null)
                .Where(o => o.EnumerableType == null)
                .Where(o => o.TaskType == null)
                .OrderBy(o => o.FullName);

        protected override IOpenApiSpec CreateSwaggerSpecFromTypesInRepo()
        {
            var swaggerObject = new SwaggerObject(null)
            {
                Swagger = "2.0",
                Host = "localhost",
                BasePath = Settings.BasePath,
            };
            swaggerObject.Paths = CreatePaths(swaggerObject);
            swaggerObject.Info = new InfoObject(swaggerObject)
            {
                Title = Settings.Title,
                Version = Settings.Version,
            };
            swaggerObject.Info.License = CreateLicense(swaggerObject.Info);
            swaggerObject.Info.Contact = CreateContact(swaggerObject.Info);
            swaggerObject.Tags = Interfaces.Select(o => CreateTag(swaggerObject, o));
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

        private PathsObject CreatePaths(SwaggerObject swaggerObject)
        {
            var paths = new PathsObject(swaggerObject);
            Interfaces.SelectMany(o => o.Methods)
                .OrderBy(o => o.FullName)
                .ToList().ForEach(o =>
            {
                var path = MapPath(o.FullName);
                if(!string.IsNullOrEmpty(Settings.BasePath) && path.StartsWith(Settings.BasePath))
                {
                    path = path.Substring(Settings.BasePath.Length);
                }
                paths[path] = CreatePathItemObject(paths, o);
            });
            return paths;
        }

        private PathItemObject CreatePathItemObject(PathsObject paths, ICSharpMethod method)
        {
            var pathItemObject = new PathItemObject(paths);
            pathItemObject.Post = CreateOperationObject(pathItemObject, method);
            return pathItemObject;
        }

        private OperationObject CreateOperationObject(PathItemObject pathItemObject, ICSharpMethod method)
        {
            var operationObject = new OperationObject(pathItemObject);
            operationObject.Tags = new string[] { CreateTag(null, (ICSharpType)method.Parent).Name };
            operationObject.OperationId = method.Name;
            operationObject.Description = method.Comment?.Summary;
            method.Parameters
                .Where(o => o.ParameterType.RuntimeType != typeof(CancellationToken))
                .ToList()
                .ForEach(o =>
                {

                    //
                    // get the schema for the property
                    //
                    var schema = GetSchema(operationObject, false, o.ParameterType);
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
                        po.In = "query";

                        SetItemProps(po, true, o.ParameterType);
                    }
                });
            operationObject.Responses = CreateResponses(operationObject, method);

            //
            // Handle file parameters
            //
            if(operationObject.Parameters.Any(o => o.Type == "file"))
            {
                operationObject.Parameters.Where(o => o.Type == "file").ToList().ForEach(o => o.In = "formData");
                // remove the content-type and filename parameters
                operationObject.Parameters = operationObject.Parameters
                    .Where(o => !o.Name.Equals("contenttype", StringComparison.InvariantCultureIgnoreCase))
                    .Where(o => !o.Name.Equals("filename", StringComparison.InvariantCultureIgnoreCase))
                    .ToList();
            }

            //
            // set the consumes and produces based on parameters and responses
            //
            if (operationObject.Parameters.Any(o => o.In == "formData"))
            {
                operationObject.AddConsumes("multipart/form-data");
            }
            else if(operationObject.Parameters.Any(o => o.In == "body"))
            {
                operationObject.AddConsumes("application/json");
            }
            if (operationObject.Responses.Any(o => o.Value.Schema?.Type == "file"))
            {
                operationObject.AddProduces("application/octet-stream");
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
            var runtimeProps = type.Properties.ToDictionary(o => o.Name, o => o.PropertyType.RuntimeType);
            if(TypeExtensions.IsFileType(type.FullName, runtimeProps))
            {
                if (canHandleFile)
                {
                    itemBase.Type = "file";
                    return;
                }
            }
            if (type.RuntimeType != null)
            {
                if (type.RuntimeType == typeof(bool))
                {
                    itemBase.Type = "boolean";
                    return;
                }
                else if (type.RuntimeType == typeof(string))
                {
                    itemBase.Type = "string";
                    return;
                }
                else if (type.RuntimeType == typeof(short))
                {
                    itemBase.Type = "number";
                    itemBase.Format = "int16";
                    return;
                }
                else if (type.RuntimeType == typeof(int))
                {
                    itemBase.Type = "number";
                    itemBase.Format = "int32";
                    return;
                }
                else if (type.RuntimeType == typeof(long))
                {
                    itemBase.Type = "number";
                    itemBase.Format = "int64";
                    return;
                }
                else if (type.RuntimeType == typeof(System.IO.Stream))
                {
                    itemBase.Type = "string";
                    itemBase.Format = "binary";
                    return;
                }
                else if (type.RuntimeType == typeof(System.DateTime))
                {
                    itemBase.Type = "string";
                    itemBase.Format = "date-time";
                    return;
                }

                throw new NotImplementedException(type.RuntimeType.GetType().FullName);
            }
            if (type.EnumerableType != null)
            {
                itemBase.Type = "array";
                itemBase.Items = GetSchema(itemBase, canHandleFile, type.EnumerableType);
                return;
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