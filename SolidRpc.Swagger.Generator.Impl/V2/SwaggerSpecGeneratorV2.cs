﻿using SolidRpc.Swagger.Generator.Model.CSharp;
using SolidRpc.Swagger.Model;
using SolidRpc.Swagger.Model.V2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

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
            var swaggerObject = new SwaggerObject(null)
            {
                Swagger = "2.0",
                Host = "localhost",
                BasePath = Settings.BasePath,
            };
            swaggerObject.Paths = CreatePaths(swaggerObject);
            //swaggerObject.Definitions = CreateDefinitions(swaggerObject);
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
                Name = Settings.TypeDefinitionNameMapper(o),
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
                var path = Settings.MapPath(o.FullName);
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
            operationObject.Parameters = method.Parameters
                .Where(o => o.ParameterType.RuntimeType != typeof(CancellationToken))
                .Select(o => {
                    var po = new ParameterObject(operationObject)
                    {
                        Description = o.Comment?.Summary,
                        Name = o.Name,
                        Required = !o.Optional,
                        In = "query"
                    };
                    SetItemProps(po, o.ParameterType);

                    return po;
                }).ToList();
            operationObject.Responses = CreateResponses(operationObject, method);
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
                    var responseSchema = GetSchema(responsesObject, method.ReturnType.TaskType ?? method.ReturnType);
                    responsesObject["200"] = new ResponseObject(responsesObject)
                    {
                        Schema = responseSchema,
                        Description = responseSchema.GetRefSchema()?.Description ?? responseSchema?.Description ?? "The response type"
                    };
                    break;
            }  
            if(!responsesObject.Any())
            {
                return null;
            }
            return responsesObject;
        }

        private SchemaObject GetSchema(ModelBase parent, ICSharpType type)
        {
            var so = new SchemaObject(parent);
            SetItemProps(so, type);
            return so;
         }

        private void SetItemProps(ItemBase itemBase, ICSharpType type)
        {
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
                itemBase.Items = GetSchema(itemBase, type.EnumerableType);
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
            var defName = Settings.TypeDefinitionNameMapper(clazz);
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
                so.Properties = CreateDefinitionsObject(so, clazz);
                swaggerObject.Definitions[defName] = so;
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
                var schema = GetSchema(definitionsObject, o.PropertyType);
                schema.Description = o.Comment?.Summary;
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