using System.Collections.Generic;
using System.Linq;
using SolidRpc.Swagger.Generator.Code.Binder;
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

        protected override void GenerateCode()
        {
            // iterate all operations
            foreach(var op in Operations)
            {
                var swaggerOperation = new SwaggerOperation()
                {
                    Tags = op.Tags,
                    OperationId = op.OperationId,
                    Parameters = CreateParameters(op.Parameters)
                };

                var cSharpMethod = CodeSettings.OperationMapper(CodeSettings, swaggerOperation);
            }
        }

        private IEnumerable<SwaggerOperationParameter> CreateParameters(IEnumerable<ParameterObject> parameters)
        {
            return parameters.Select(CreateParameter).ToList();
        }

        private SwaggerOperationParameter CreateParameter(ParameterObject arg)
        {
            var sop =  new SwaggerOperationParameter();
            sop.Name = arg.Name;
            return sop;
        }
    }
}
