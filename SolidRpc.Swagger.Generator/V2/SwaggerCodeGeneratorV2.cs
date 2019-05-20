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
            var methods = Operations.Select(op =>
            {
                var swaggerOperation = new SwaggerOperation()
                {
                    Tags = op.Tags,
                    OperationId = op.OperationId,
                    ReturnType = GetSwaggerDefinition(op),
                    Parameters = CreateParameters(op.Parameters)
                };

                return CodeSettings.OperationMapper(CodeSettings, swaggerOperation);
            }).ToList();

            methods.ForEach(o =>
            {
                var ns = codeGenerator.GetNamespace(o.InterfaceName.Namespace);
                var i = ns.GetInterface(o.InterfaceName.Name);
                var m = i.AddMethod(o.MethodName);
                m.ReturnType = codeGenerator.GetNamespace(o.ReturnType.Name.Namespace).GetClass(o.ReturnType.Name.Name);
                foreach(var p in o.Parameters)
                {
                    m.AddParameter(p.Name).ParameterType = codeGenerator.GetNamespace(p.ParameterType.Name.Namespace).GetClass(p.ParameterType.Name.Name);
                }
            });

          }

        private SwaggerDefinition GetSwaggerDefinition(OperationObject op)
        {
            ResponseObject ro;
            if(!op.Responses.TryGetValue("200", out ro))
            {
                return SwaggerDefinition.Void;
            }
            return GetSwaggerDefinition($"{op.OperationId}Args", ro.Schema);
        }

        private IEnumerable<SwaggerOperationParameter> CreateParameters(IEnumerable<ParameterObject> parameters)
        {
            return parameters.Select(CreateParameter).ToList();
        }

        private SwaggerOperationParameter CreateParameter(ParameterObject arg)
        {
            return new SwaggerOperationParameter()
            {
                Name = arg.Name,
                ParameterType = GetSwaggerDefinition(arg.Name, arg.Schema)
            };
        }

        private SwaggerDefinition GetSwaggerDefinition(string definitionName, ItemBase schema)
        {
            if(!string.IsNullOrEmpty(schema.Ref))
            {
                var prefix = "#/definitions/";
                if(schema.Ref.StartsWith(prefix))
                {
                    var d = SwaggerObject.Definitions[schema.Ref.Substring(prefix.Length)];
                    return GetSwaggerDefinition(definitionName, d);
                }
                else
                {
                    throw new Exception("Cannot handle ref.");
                }
            }
            switch(schema.Type)
            {
                case "object":
                    return new SwaggerDefinition()
                    {
                        Name = schema.Name
                    };
                case "array":
                    var arrayType = GetSwaggerDefinition(definitionName, schema.Items);
                    return new SwaggerDefinition()
                    {
                        Name = arrayType.Name,
                        IsArray = true
                    };
                default:
                    throw new Exception("Cannot handle schema type:"+schema.Type);
            }
        }
    }
}
