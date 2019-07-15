using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using SolidRpc.OpenApi.Model.CodeDoc.Impl;
using SolidRpc.OpenApi.Model.V2;

namespace SolidRpc.OpenApi.Binder.V2
{
    public class MethodBinderV2 : MethodBinderBase
    {
        public MethodBinderV2(SwaggerObject schemaObject, Assembly assembly) : base(schemaObject, assembly)
        {
            SchemaObject = schemaObject;
            CodeDocRepo = new CodeDocRepository();

            Operations = SchemaObject.Paths.Values.SelectMany(o => new[] {
                o.Delete,
                o.Get,
                o.Head,
                o.Options,
                o.Patch,
                o.Post,
                o.Put
            }).Where(o => o != null).ToList();
        }

        private SwaggerObject SchemaObject { get; }
        private CodeDocRepository CodeDocRepo { get; }

        private IList<OperationObject> Operations { get; }

        protected override IMethodInfo FindBinding(MethodInfo mi, bool mustExist)
        {
            if(mi.DeclaringType.Assembly != Assembly)
            {
                throw new ArgumentException("Method does not belong to assembly.");
            }

            var prospects = Operations;
            var binderStatus = new StringBuilder();
            binderStatus.Append($"->#{prospects.Count}");

            // operation id must start with method name
            prospects = Operations.Where(o => o.OperationId.Equals(mi.Name, StringComparison.InvariantCultureIgnoreCase)).ToList();
            if(prospects.Count() == 0)
            {
                prospects = Operations.Where(o => o.OperationId.StartsWith(mi.Name, StringComparison.InvariantCultureIgnoreCase)).ToList();
            }
            binderStatus.Append($"->method({mi.Name})->#{prospects.Count}");

            // find all parameters 
            foreach(var param in mi.GetParameters())
            {
                prospects = prospects.Where(o => FindParameter(o.GetParameters(), param)).ToList();
                binderStatus.Append($"->param({param.Name})->#{prospects.Count}");
            }

            if (prospects.Count != 1)
            {
                if(mustExist)
                {
                    throw new NotImplementedException(binderStatus.ToString());
                }
                else
                {
                    return null;
                }
            }
            return new MethodInfoV2(this, prospects.Single(), mi, CodeDocRepo.GetMethodDoc(mi));

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
