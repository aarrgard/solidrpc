using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using SolidRpc.Swagger.Model.V2;

namespace SolidRpc.Swagger.Binder.V2
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
            var prospects = Operations;
            binderStatus.Append($"->#{prospects.Count()}");
            prospects = prospects.Where(o => o.OperationId.StartsWith(mi.Name, StringComparison.InvariantCultureIgnoreCase));
            binderStatus.Append($"->{mi.Name}->#{prospects.Count()}");
            if(prospects.Count() != 1)
            {
                throw new NotImplementedException(binderStatus.ToString());
            }
            return new MethodInfoV2(prospects.Single(), mi);
        }
    }
}
