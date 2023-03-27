using System;
using System.Collections.Generic;
using System.Linq;
using SolidRpc.Abstractions.OpenApi.Model;
using SolidRpc.OpenApi.Model.Agnostic;
using SolidRpc.OpenApi.Model.CSharp;
using SolidRpc.OpenApi.Model.CSharp.Impl;
using SolidRpc.OpenApi.Model.V2;

namespace SolidRpc.OpenApi.Model.Generator.V2
{
    /// <summary>
    /// Creates code from a swagger spec
    /// </summary>
    public class OpenApiCodeGeneratorV2 : OpenApiCodeGenerator
    {
        /// <summary>
        /// Constructs a new instance
        /// </summary>
        /// <param name="swaggerObject"></param>
        /// <param name="codeSettings"></param>
        public OpenApiCodeGeneratorV2(SwaggerObject swaggerObject, SettingsCodeGen codeSettings)
            : base(codeSettings)
        {
            SwaggerObject = swaggerObject;
        }

        /// <summary>
        /// The swagger spec we are working on
        /// </summary>
        public SwaggerObject SwaggerObject { get; }

        /// <summary>
        /// The operations
        /// </summary>
        public IEnumerable<OperationObject> Operations => SwaggerObject.Paths
            .Values.SelectMany(o => new[] {
                o.Delete,
                o.Get,
                o.Head,
                o.Options,
                o.Patch,
                o.Post,
                o.Put
            }).Where(o => o != null);

        /// <summary>
        /// Generates code into supplied code repository
        /// </summary>
        /// <param name="cSharpRepository"></param>
        protected override void GenerateCode(ICSharpRepository cSharpRepository)
        {
            //
            // map security definitions on to attributes
            //  
            SwaggerObject.GetSecurityDefinitions().ToList().ForEach(o =>
            {
                var className = new QualifiedName(
                      CodeSettings.ProjectNamespace,
                      CodeSettings.CodeNamespace,
                      CodeSettings.SecurityNamespace,
                      SecurityDefinitionMapper(o.Key));

                var securityAttribute = cSharpRepository.GetClass(className);
                securityAttribute.SetModifier("public");
                securityAttribute.AddExtends(cSharpRepository.GetClass(typeof(Attribute).FullName));

                var scopes = new CSharp.Impl.CSharpProperty(securityAttribute, "Scopes", cSharpRepository.GetClass(typeof(string[]).FullName));
                securityAttribute.AddMember(scopes);
            });

            // iterate all operations
            var cSharpMethods = Operations.Select(op =>
            {
                var swaggerOperation = new SwaggerOperation();
                swaggerOperation.Tags = GetTags(op);
                swaggerOperation.OperationId = op.GetOperationId();
                swaggerOperation.OperationDescription = SwaggerDescription.Create($"{op.Summary} {op.Description}", op.ExternalDocs?.Description, op.ExternalDocs?.Url);
                swaggerOperation.Exceptions = GetExceptions(swaggerOperation, op);
                swaggerOperation.ReturnType = GetReturnType(swaggerOperation, op);
                swaggerOperation.Parameters = CreateParameters(swaggerOperation, op.GetParameters());
                swaggerOperation.Security = CreateSecurity(op.Security);
                return OperationMapper(CodeSettings, swaggerOperation);
            });

            cSharpMethods = MergeMethods(cSharpMethods);

            cSharpMethods.ToList().ForEach(csm =>
            {
                var i = cSharpRepository.GetInterface(csm.InterfaceName);
                i.SetModifier("public");
                AddCodeGeneratorAttribute(i);

                // construct code comment for interface
                var comment = $"<summary>{csm.ClassSummary?.Summary}</summary>";
                comment += $"<a href=\"{csm.ClassSummary?.ExternalUri}\">{csm.ClassSummary?.ExternalDescription}</a>";
                i.ParseComment(comment);

                // handle return type
                var returnType = GetType(cSharpRepository, csm.ReturnType);
                if (CodeSettings.UseAsyncAwaitPattern)
                {
                    returnType = CreateTask(returnType);
                }

                // construct comment for method
                comment = $"<summary>{csm.MethodSummary?.Summary}</summary>";
                comment += $"<a href=\"{csm.MethodSummary?.ExternalUri}\">{csm.MethodSummary?.ExternalDescription}</a>";
                foreach (var e in csm.Exceptions)
                {
                    var ex = (ICSharpClass)GetType(cSharpRepository, e);
                    //
                    // if more than one exeptions has the same description
                    // we might have som problem to determine which one goes
                    // with wich http code - use first. We do not want to add 
                    // more than one constructor.
                    //
                    if(!ex.Members.OfType<ICSharpTypeExtends>().Any())
                    {
                        ex.AddExtends(cSharpRepository.GetClass(typeof(Exception).FullName));
                        comment += $"<exception cref=\"{ex.FullName}\">{ex.Comment?.Summary}</exception>";
                        var ctr = new Model.CSharp.Impl.CSharpConstructor(ex, $"\"{ex.Comment?.Summary}\"", $"Data[\"HttpStatusCode\"] = {e.ExceptionCode};");
                        ctr.ParseComment("<summary>Constructs a new instance</summary>");
                        ex.AddMember(ctr);
                    }
                }

                var m = new Model.CSharp.Impl.CSharpMethod(i, csm.MethodName, returnType);
                m.ParseComment(comment);

                foreach (var attrGroup in csm.SecurityAttribute)
                {
                    foreach (var attr in attrGroup)
                    {
                        var attrData = new Dictionary<string, object>();
                        if(attr.Value.Any())
                        {
                            attrData["Scopes"] = attr.Value;
                        }
                        var csAttr = new Model.CSharp.Impl.CSharpAttribute(m, attr.Key, null, attrData);
                        m.AddMember(csAttr);
                    }
                }

                foreach (var p in csm.Parameters.OrderBy(o => o.Optional ? 1 : 0))
                {
                    var parameterType = (ICSharpType)GetType(cSharpRepository, p.ParameterType);
                    string defaultValue = null;
                    if (p.Optional)
                    {
                        if (parameterType.RuntimeType?.IsValueType ?? false)
                        {
                            parameterType = cSharpRepository.GetType($"System.Nullable<{parameterType.FullName}>");
                        }
                        defaultValue = $"null";
                    }

                    var mp = new Model.CSharp.Impl.CSharpMethodParameter(m, p.Name, parameterType, p.Optional, defaultValue);
                    mp.ParseComment($"<summary>{p.Description}</summary>");
                    m.AddMember(mp);
                }
                if (CodeSettings.UseAsyncAwaitPattern)
                {
                    var parameterType = cSharpRepository.GetType("System.Threading.CancellationToken");
                    var mp = new Model.CSharp.Impl.CSharpMethodParameter(m, "cancellationToken", parameterType, true, $"default({parameterType.FullName})");
                    m.AddMember(mp);
                }
                i.AddMember(m);
                AddUsings(i);
            });

            SwaggerObject.Definitions?.Values.ToList().ForEach(o =>
            {
                var swaggerDef = GetSwaggerDefinition(null, o);
                var cSharpObject = DefinitionMapper(CodeSettings, swaggerDef);
                GetType(cSharpRepository, cSharpObject);
            });
          }


        private IEnumerable<IDictionary<string, IEnumerable<string>>> CreateSecurity(IEnumerable<SecurityRequirementObject> security)
        {
            if(security == null)
            {
                return new IDictionary<string, IEnumerable<string>>[0];
            }
            return security.Select(o => (IDictionary<string, IEnumerable<string>>)o.ToDictionary(o2 => o2.Key, o2 => o2.Value)).ToArray();
        }

        private IEnumerable<SwaggerTag> GetTags(OperationObject op)
        {
            if (op.Tags == null)
            {
                var so = op.GetParent<SwaggerObject>();
                return new[] { new SwaggerTag()
                    {
                        Name = so.Title,
                        Description = SwaggerDescription.Create(so.Info?.Description, null, null)
                    }
                };
                throw new ArgumentNullException(op.OperationId);
            }
            if(op.GetParent<SwaggerObject>().Tags == null)
            {
                return op.Tags.Select(o => new SwaggerTag()
                {
                    Name = o
                });
            }
            return op.GetParent<SwaggerObject>().Tags
                .Where(o => op.Tags.Contains(o.Name))
                .Select(o => new SwaggerTag()
                {
                    Name = o.Name,
                    Description = SwaggerDescription.Create(o.Description, o.ExternalDocs?.Description, o.ExternalDocs?.Url)
                });
        }

        private ICSharpType CreateTask(ICSharpType returnType)
        {
            var repository = returnType.GetParent<ICSharpRepository>();
            if (returnType.Name == SwaggerDefinition.TypeVoid)
            {
                return repository.GetClass($"System.Threading.Tasks.Task");
            }
            else
            {
                return repository.GetClass($"System.Threading.Tasks.Task<{returnType.FullName}>");
            }
        }

        private IEnumerable<SwaggerDefinition> GetExceptions(SwaggerOperation swaggerOperation, OperationObject op)
        {
            int dummy;
            return op.Responses
                .Where(o => !string.Equals(o.Key, "200")) // ok
                //.Where(o => !string.Equals(o.Key, "204")) // ok - no content
                .Where(o => !string.Equals(o.Key, "default", StringComparison.InvariantCultureIgnoreCase))
                .Where(o => int.TryParse(o.Key, out dummy))
                .Select(ro =>
                {
                    var def = new SwaggerDefinition(swaggerOperation, ExceptionNameMapper(ro.Value.Description));
                    def.Description = ro.Value.Description;
                    def.ExceptionCode = int.Parse(ro.Key);
                    return def;
                }).ToList();
        }

        private SwaggerDefinition GetReturnType(SwaggerOperation swaggerOperation, OperationObject op)
        {
            var responseDef = op.Responses
                .Where(o => string.Equals(o.Key, "200"))
                .Select(ro => {
                var def = new SwaggerDefinition(null, SwaggerDefinition.TypeVoid);
                if (ro.Value.Schema != null)
                {
                    def = GetSwaggerDefinition(swaggerOperation, ro.Value.Schema);
                }
                def.Description = ro.Value.Description;
                return def;
            }).FirstOrDefault();
            if(responseDef == null)
            {
                responseDef = new SwaggerDefinition(null, SwaggerDefinition.TypeVoid);
            }
            return responseDef;
        }

        private IEnumerable<SwaggerOperationParameter> CreateParameters(SwaggerOperation swaggerOperation, IEnumerable<ParameterObject> parameters)
        {
            return parameters.Select(o => CreateParameter(swaggerOperation, o)).ToList();
        }

        private SwaggerOperationParameter CreateParameter(SwaggerOperation swaggerOperation, ParameterObject arg)
        {
            return new SwaggerOperationParameter()
            {
                Name = arg.Name,
                ParameterType = GetSwaggerDefinition(swaggerOperation, arg),
                Description = arg.Description,
                Required = arg.Required
            };
        }

        private SwaggerDefinition GetSwaggerDefinition(SwaggerOperation swaggerOperation, ParameterObject parameterObject)
        {
            switch(parameterObject.In)
            {
                case "body":
                    return GetSwaggerDefinition(swaggerOperation, parameterObject.Schema);
                default:
                    return GetSwaggerDefinition(swaggerOperation, (ItemBase)parameterObject);
            }
        }
        private SwaggerDefinition GetSwaggerDefinition(SwaggerOperation swaggerOperation, ItemBase schema, IDictionary<string, SwaggerDefinition> refs = null, string refKey = null)
        {
            if(refs == null)
            {
                refs = new Dictionary<string, SwaggerDefinition>();
            }
            SwaggerDefinition sd = null;
            if(refKey != null)
            {
                if (refs.TryGetValue(refKey, out sd))
                {
                    return sd;
                }
            }
            if (schema == null)
            {
                throw new ArgumentNullException(nameof(schema));
            }
            if(!string.IsNullOrEmpty(schema.Ref))
            {
                var parts = schema.Ref.Split('#');
                if(parts.Length != 2)
                {
                    throw new Exception($"Cannot handle ref {schema.Ref}");
                }
                var schemaHolder = (IOpenApiSpec)schema.GetParent<SwaggerObject>();
                if (!string.IsNullOrEmpty(parts[0]))
                {
                    if(!schemaHolder.OpenApiSpecResolver.TryResolveApiSpec(parts[0], out schemaHolder, schemaHolder.OpenApiSpecResolverAddress))
                    {
                        throw new Exception($"Cannot find open api spec {parts[0]}");
                    }
                }
                var prefix = "/definitions/";
                if (!parts[1].StartsWith(prefix))
                {
                    throw new Exception($"Cannot find open api spec {parts[0]}");
                }
                refKey = parts[1].Substring(prefix.Length);
                var d = ((SwaggerObject)schemaHolder).GetDefinitions()[refKey];
                if(d == null)
                {
                    throw new Exception($"Cannot find open api definition {parts[1]} in file {schemaHolder.OpenApiSpecResolverAddress}");
                }
                var rd = GetSwaggerDefinition(null, d, refs, refKey);
                return rd;
            }
            switch(schema.Type)
            {
                case "object":
                    sd = new SwaggerDefinition(swaggerOperation, schema.GetOperationName());
                    if(refKey != null)
                    {
                        refs[refKey] = sd;
                    }
                    if(schema is SchemaObject so)
                    {
                        if(so.Properties != null)
                        {
                            sd.Properties = so.Properties?.Select(o => new SwaggerProperty()
                            {
                                Name = o.Key,
                                Type = GetSwaggerDefinition(swaggerOperation, o.Value, refs),
                                Description = o.Value.Description,
                                Required = o.Value.Required?.Contains(o.Key) ?? false
                            }).ToList();
                        }

                        if (so.AdditionalProperties != null)
                        {
                            sd.AdditionalProperties = GetSwaggerDefinition(swaggerOperation, so.AdditionalProperties, refs);
                        }
                    }
                    return sd;
                case "array":
                    var arrayType = GetSwaggerDefinition(swaggerOperation, schema.Items, refs);
                    return new SwaggerDefinition(arrayType);
                case "string":
                    switch (schema.Format)
                    {
                        case null:
                            return new SwaggerDefinition(swaggerOperation, SwaggerDefinition.TypeString);
                        case "date":
                            return new SwaggerDefinition(swaggerOperation, SwaggerDefinition.TypeDateTime);
                        case "date-time":
                            return new SwaggerDefinition(swaggerOperation, SwaggerDefinition.TypeDateTimeOffset);
                        case "uuid":
                            return new SwaggerDefinition(swaggerOperation, SwaggerDefinition.TypeGuid);
                        case "byte":
                        case "binary":
                            return new SwaggerDefinition(swaggerOperation, SwaggerDefinition.TypeStream);
                        case "uri":
                            return new SwaggerDefinition(swaggerOperation, SwaggerDefinition.TypeUri);
                        default:
                            throw new Exception("Cannot handle schema format:" + schema.Format);
                    }
                case "integer":
                    switch (schema.Format)
                    {
                        case null:
                        case "":
                        case "int64":
                            return new SwaggerDefinition(swaggerOperation, SwaggerDefinition.TypeLong);
                        case "int32":
                            return new SwaggerDefinition(swaggerOperation, SwaggerDefinition.TypeInt);
                        default:
                            throw new Exception("Cannot handle schema format:" + schema.Format);
                    }
                case "file":
                    return new SwaggerDefinition(swaggerOperation, SwaggerDefinition.TypeStream);
                case "boolean":
                    return new SwaggerDefinition(swaggerOperation, SwaggerDefinition.TypeBoolean);
                case "number":
                    switch (schema.Format)
                    {
                        case null:
                        case "":
                        case "float":
                            return new SwaggerDefinition(swaggerOperation, SwaggerDefinition.TypeFloat);
                        case "double":
                            return new SwaggerDefinition(swaggerOperation, SwaggerDefinition.TypeDouble);
                        default:
                            throw new Exception("Cannot handle schema format:" + schema.Format);
                    }
                default:
                    throw new Exception("Cannot handle schema type:"+schema.Type);
            }
        }
    }
}
