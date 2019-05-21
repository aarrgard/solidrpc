using System;
using System.Collections.Generic;
using System.Linq;
using SolidRpc.Swagger.Generator.Code.Binder;
using SolidRpc.Swagger.Generator.Code.CSharp;
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

        protected override void GenerateCode(ICodeGenerator codeGenerator)
        {
            // iterate all operations
            var cSharpMethods = Operations.Select(op =>
            {
                var swaggerOperation = new SwaggerOperation();
                swaggerOperation.Tags = op.Tags;
                swaggerOperation.OperationId = op.OperationId;
                swaggerOperation.ReturnType = GetSwaggerDefinition(swaggerOperation, op);
                swaggerOperation.Parameters = CreateParameters(swaggerOperation, op.Parameters);
                return CodeSettings.OperationMapper(CodeSettings, swaggerOperation);
            }).ToList();

            cSharpMethods.ForEach(o =>
            {
                var ns = codeGenerator.GetNamespace(o.InterfaceName.Namespace);
                var i = ns.GetInterface(o.InterfaceName.Name);
                var m = i.AddMethod(o.MethodName);
                m.ReturnType = GetClass(codeGenerator, o.ReturnType);
                foreach(var p in o.Parameters)
                {
                    m.AddParameter(p.Name).ParameterType = GetClass(codeGenerator, p.ParameterType);
                }
            });

            SwaggerObject.Definitions.Values.ToList().ForEach(o =>
            {
                var swaggerDef = GetSwaggerDefinition(null, o);
                var cSharpObject = CodeSettings.ItemMapper(CodeSettings, swaggerDef);
                GetClass(codeGenerator, cSharpObject);
            });
          }

        private SwaggerDefinition GetSwaggerDefinition(SwaggerOperation swaggerOperation, OperationObject op)
        {
            ResponseObject ro;
            if(!op.Responses.TryGetValue("200", out ro))
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
                ParameterType = GetSwaggerDefinition(swaggerOperation, arg)
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
                    var sd = new SwaggerDefinition(swaggerOperation, schema.Name);
                    if(schema is SchemaObject so && so.Properties != null)
                    {
                        sd.Properties = so.Properties.Select(o => new SwaggerProperty()
                        {
                            Name = o.Key,
                            Type = GetSwaggerDefinition(swaggerOperation, o.Value)
                        }).ToList();
                    }
                    return sd;
                case "array":
                    var arrayType = GetSwaggerDefinition(swaggerOperation, schema.Items);
                    return new SwaggerDefinition(arrayType.SwaggerOperation, arrayType.Name)
                    {
                        IsArray = true
                    };
                case "string":
                    switch (schema.Format)
                    {
                        case null:
                            return new SwaggerDefinition(swaggerOperation, SwaggerDefinition.TypeString);
                        case "date-time":
                            return new SwaggerDefinition(swaggerOperation, SwaggerDefinition.TypeDateTime);
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
                default:
                    throw new Exception("Cannot handle schema type:"+schema.Type);
            }
        }
    }
}
