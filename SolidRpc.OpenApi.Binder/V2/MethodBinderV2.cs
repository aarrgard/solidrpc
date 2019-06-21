using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using SolidRpc.OpenApi.Model.V2;

namespace SolidRpc.OpenApi.Binder.V2
{
    public class MethodBinderV2 : MethodBinderBase
    {
        public MethodBinderV2(SwaggerObject schemaObject)
        {
            SchemaObject = schemaObject;
        }

        private SwaggerObject SchemaObject { get; }

        private IEnumerable<OperationObject> Operations => SchemaObject.Paths.Values.SelectMany(o => new[] {
            o.Delete,
            o.Get,
            o.Head,
            o.Options,
            o.Patch,
            o.Post,
            o.Put
        }).Where(o => o != null);

        protected override IMethodInfo FindBinding(MethodInfo mi)
        {
            var binderStatus = new StringBuilder();
            binderStatus.Append($"->#{Operations.Count()}");

            // operation id must start with method name
            var prospects = Operations.Where(o => o.OperationId.Equals(mi.Name, StringComparison.InvariantCultureIgnoreCase));
            if(prospects.Count() == 0)
            {
                prospects = Operations.Where(o => o.OperationId.StartsWith(mi.Name, StringComparison.InvariantCultureIgnoreCase));
            }
            binderStatus.Append($"->method({mi.Name})->#{prospects.Count()}");

            // find all parameters 
            foreach(var param in mi.GetParameters())
            {
                prospects = prospects.Where(o => FindParameter(o.Parameters, param));
                binderStatus.Append($"->param({param.Name})->#{prospects.Count()}");
            }

            if (prospects.Count() != 1)
            {
                throw new NotImplementedException(binderStatus.ToString());
            }
            return new MethodInfoV2(prospects.Single(), mi);
        }

        private bool FindParameter(IEnumerable<ParameterObject> parameters, ParameterInfo parameter)
        {
            var prospect = parameters.FirstOrDefault(o => o.Name == parameter.Name);
            if(prospect != null)
            {
                return true;
            }
            if(parameter.IsOptional)
            {
                return true;
            }
            if(parameters.Any(o => o.IsBinaryType()))
            {
                if (parameter.Name.Equals("filename", StringComparison.InvariantCultureIgnoreCase))
                {
                    return true;
                }
                if (parameter.Name.Equals("contenttype", StringComparison.InvariantCultureIgnoreCase))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
