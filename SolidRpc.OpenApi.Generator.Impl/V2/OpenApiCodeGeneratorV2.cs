using System;
using System.Collections.Generic;
using System.Linq;
using SolidRpc.OpenApi.Generator.Impl.Code.Binder;
using SolidRpc.OpenApi.Generator.Model.CSharp;
using SolidRpc.OpenApi.Generator.Types;
using SolidRpc.OpenApi.Model.V2;

namespace SolidRpc.OpenApi.Generator.V2
{
    public class OpenApiCodeGeneratorV2 : OpenApiCodeGenerator
    {
        public OpenApiCodeGeneratorV2(SwaggerObject swaggerObject, SettingsCodeGen codeSettings)
            : base(codeSettings)
        {
            SwaggerObject = swaggerObject;
        }

        public SwaggerObject SwaggerObject { get; }

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

        protected override void GenerateCode(ICSharpRepository cSharpRepository)
        {
            // iterate all operations
            var cSharpMethods = Operations.Select(op =>
            {
                var swaggerOperation = new SwaggerOperation();
                swaggerOperation.Tags = GetTags(op);
                swaggerOperation.OperationId = op.OperationId;
                swaggerOperation.OperationDescription = SwaggerDescription.Create($"{op.Summary} {op.Description}", op.ExternalDocs?.Description, op.ExternalDocs?.Url);
                swaggerOperation.ReturnType = GetReturnType(swaggerOperation, op);
                swaggerOperation.Parameters = CreateParameters(swaggerOperation, op.Parameters);
                return OperationMapper(CodeSettings, swaggerOperation);
            }).ToList();

            cSharpMethods.ForEach(o =>
            {
                var i = cSharpRepository.GetInterface(o.InterfaceName);
                i.ParseComment($"<summary>{o.ClassSummary?.Summary}</summary><a href=\"{o.ClassSummary?.ExternalUri}\">{o.ClassSummary?.ExternalDescription}</a>");
                var returnType = (ICSharpType)GetClass(cSharpRepository, o.ReturnType);
                if (CodeSettings.UseAsyncAwaitPattern)
                {
                    returnType = CreateTask(returnType);
                }

                var m = new Model.CSharp.Impl.CSharpMethod(i, o.MethodName, returnType);
                m.ParseComment($"<summary>{o.MethodSummary?.Summary}</summary><a href=\"{o.MethodSummary?.ExternalUri}\">{o.MethodSummary?.ExternalDescription}</a>");
                foreach (var p in o.Parameters)
                {
                    var parameterType = GetClass(cSharpRepository, p.ParameterType);
                    var mp = new Model.CSharp.Impl.CSharpMethodParameter(m, p.Name, parameterType, p.Optional);
                    mp.ParseComment($"<summary>{p.Description}</summary>");
                    //if(!p.Optional)
                    //{
                    //    csp.DefaultValue = $"default({csp.ParameterType.FullName})";
                    //}
                    m.AddMember(mp);
                }
                if (CodeSettings.UseAsyncAwaitPattern)
                {
                    var parameterType = cSharpRepository.GetClass("System.Threading.CancellationToken");
                    var mp = new Model.CSharp.Impl.CSharpMethodParameter(m, "cancellationToken", parameterType, true, $"default({parameterType.FullName})");
                    m.AddMember(mp);
                }
                i.AddMember(m);
                AddUsings(i);
            });

            SwaggerObject.Definitions.Values.ToList().ForEach(o =>
            {
                var swaggerDef = GetSwaggerDefinition(null, o);
                var cSharpObject = DefinitionMapper(CodeSettings, swaggerDef);
                GetClass(cSharpRepository, cSharpObject);
            });
          }

        private IEnumerable<SwaggerTag> GetTags(OperationObject op)
        {
            if (op.Tags == null)
            {
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

        private SwaggerDefinition GetReturnType(SwaggerOperation swaggerOperation, OperationObject op)
        {
            var responseDef = new SwaggerDefinition(null, SwaggerDefinition.TypeVoid);
            op.Responses.ToList().ForEach(ro => {
                var def = new SwaggerDefinition(null, SwaggerDefinition.TypeVoid);
                if (ro.Value.Schema != null)
                {
                    def = GetSwaggerDefinition(swaggerOperation, ro.Value.Schema);
                }
                def.Description = ro.Value.Description;
                if (ro.Key == "200")
                {
                    responseDef = def;
                }
            });
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
                var prefix = "#/definitions/";
                if (schema.Ref.StartsWith(prefix))
                {
                    refKey = schema.Ref.Substring(prefix.Length);
                    var d = SwaggerObject.Definitions[refKey];
                    var rd = GetSwaggerDefinition(null, d, refs, refKey);
                    return rd;
                }
                else
                {
                    throw new Exception("Cannot handle ref.");
                }
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
                                Description = o.Value.Description
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
                        case "date-time":
                            return new SwaggerDefinition(swaggerOperation, SwaggerDefinition.TypeDateTime);
                        case "uuid":
                            return new SwaggerDefinition(swaggerOperation, SwaggerDefinition.TypeGuid);
                        case "binary":
                            return new SwaggerDefinition(swaggerOperation, SwaggerDefinition.TypeStream);
                        default:
                            throw new Exception("Cannot handle schema format:" + schema.Format);
                    }
                case "integer":
                    switch (schema.Format)
                    {
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
