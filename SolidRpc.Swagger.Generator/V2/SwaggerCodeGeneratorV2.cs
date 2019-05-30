using System;
using System.Collections.Generic;
using System.Linq;
using SolidRpc.Swagger.Generator.Code.Binder;
using SolidRpc.Swagger.Generator.Model.CSharp;
using SolidRpc.Swagger.Model.V2;

namespace SolidRpc.Swagger.Generator.V2
{
    public class SwaggerCodeGeneratorV2 : SwaggerCodeGenerator
    {
        public SwaggerCodeGeneratorV2(SwaggerObject swaggerObject, SwaggerCodeSettings codeSettings)
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
                swaggerOperation.Tags = op.Tags;
                swaggerOperation.OperationId = op.OperationId;
                swaggerOperation.OperationSummary = op.Summary;
                swaggerOperation.OperationDescription = op.Description;
                swaggerOperation.ReturnType = GetSwaggerDefinition(swaggerOperation, op);
                swaggerOperation.Parameters = CreateParameters(swaggerOperation, op.Parameters);
                return CodeSettings.OperationMapper(CodeSettings, swaggerOperation);
            }).ToList();

            cSharpMethods.ForEach(o =>
            {
                var i = cSharpRepository.GetInterface(o.InterfaceName);
                i.ParseComment($"<summary>{o.ClassSummary}</summary>");
                var returnType = (ICSharpType)GetClass(cSharpRepository, o.ReturnType);
                if (CodeSettings.UseAsyncAwaitPattern)
                {
                    returnType = CreateTask(returnType);
                }

                var m = new Model.CSharp.Impl.CSharpMethod(i, o.MethodName, returnType);
                m.ParseComment($"<summary>{o.MethodSummary}</summary>");
                foreach(var p in o.Parameters)
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
                var cSharpObject = CodeSettings.DefinitionMapper(CodeSettings, swaggerDef);
                GetClass(cSharpRepository, cSharpObject);
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

        private SwaggerDefinition GetSwaggerDefinition(SwaggerOperation swaggerOperation, OperationObject op)
        {
            if (!op.Responses.TryGetValue("200", out ResponseObject ro))
            {
                return SwaggerDefinition.Void;
            }
            if (ro.Schema == null)
            {
                return SwaggerDefinition.Void;
            }
            return GetSwaggerDefinition(swaggerOperation, ro.Schema);
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
        private SwaggerDefinition GetSwaggerDefinition(SwaggerOperation swaggerOperation, ItemBase schema)
        {
            if (schema == null)
            {
                throw new ArgumentNullException(nameof(schema));
            }
            if(!string.IsNullOrEmpty(schema.Ref))
            {
                var prefix = "#/definitions/";
                if(schema.Ref.StartsWith(prefix))
                {
                    var d = SwaggerObject.Definitions[schema.Ref.Substring(prefix.Length)];
                    return GetSwaggerDefinition(null, d);
                }
                else
                {
                    throw new Exception("Cannot handle ref.");
                }
            }
            switch(schema.Type)
            {
                case "object":
                    var sd = new SwaggerDefinition(swaggerOperation, schema.OperationName);
                    if(schema is SchemaObject so && so.Properties != null)
                    {
                        sd.Properties = so.Properties.Select(o => new SwaggerProperty()
                        {
                            Name = o.Key,
                            Type = GetSwaggerDefinition(swaggerOperation, o.Value),
                            Description = o.Value.Description
                        }).ToList();
                    }
                    return sd;
                case "array":
                    var arrayType = GetSwaggerDefinition(swaggerOperation, schema.Items);
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
