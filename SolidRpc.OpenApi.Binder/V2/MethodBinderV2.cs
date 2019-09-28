using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SolidRpc.Abstractions.OpenApi.Binder;
using SolidRpc.OpenApi.Model.CodeDoc.Impl;
using SolidRpc.OpenApi.Model.V2;

namespace SolidRpc.OpenApi.Binder.V2
{
    public class MethodBinderV2 : MethodBinderBase
    {
        public MethodBinderV2(IServiceProvider serviceProvider, SwaggerObject schemaObject, Assembly assembly) : base(schemaObject, assembly)
        {
            ServiceProvider = serviceProvider;
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

        public IServiceProvider ServiceProvider { get; }
        private SwaggerObject SchemaObject { get; }
        private CodeDocRepository CodeDocRepo { get; }

        private IList<OperationObject> Operations { get; }

        protected override IMethodBinding CreateBinding(MethodInfo mi, MethodAddressTransformer methodAddressTransformer, bool mustExist)
        {
            if (mi == null) throw new ArgumentNullException(nameof(mi));
            if (mi.DeclaringType.Assembly != Assembly)
            {
                throw new ArgumentException("Method does not belong to assembly.");
            }
            if (methodAddressTransformer == null)
            {
                methodAddressTransformer = (_, uri, __) => Task.FromResult(uri);
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

            prospects = prospects.Where(o => {
                ResponseObject resp;
                o.GetResponses().TryGetValue("200", out resp);
                return TypeMatches(mi.ReturnType, resp?.Schema);        
            }).ToList();

            binderStatus.Append($"->returntype->#{prospects.Count}");


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
            return new MethodBindingV2(this, prospects.Single(), mi, CodeDocRepo.GetMethodDoc(mi), methodAddressTransformer);

        }

        private bool FindParameter(IEnumerable<ParameterObject> parameters, ParameterInfo parameter)
        {
            var prospect = parameters.FirstOrDefault(o => MethodBindingV2.NameMatches(o.Name, parameter.Name));
            if(prospect != null)
            {
                return TypeMatches(parameter.ParameterType, prospect);
            }
            if(parameter.IsOptional || parameter.ParameterType == typeof(CancellationToken))
            {
                return true;
            }
            if(parameters.Any(o => o.IsBinaryType()))
            {
                if(TypeExtensions.FileTypeProperties.Keys.Any(o => parameter.Name.Equals(o, StringComparison.InvariantCultureIgnoreCase)))
                {
                    return true;
                }
            }
            var bodyParam = parameters.FirstOrDefault(o => o.In == "body");
            if (bodyParam != null)
            {
                var schema = bodyParam.Schema.GetRefSchema() ?? bodyParam.Schema;
                if(schema != null)
                {
                    return schema.Properties.ContainsKey(parameter.Name);
                }
            }
            return false;
        }

        private bool TypeMatches(Type type, ItemBase item)
        {
            if(item == null)
            {
                return type == typeof(void) || type == typeof(Task);
            }
            item = item.GetRefSchema() ?? item;
            var clrType = item.GetClrType();
            if(type.IsTaskType(out Type taskType))
            {
                type = taskType;
            }
            if (type.IsAssignableFrom(clrType))
            {
                return true;
            }
            if(clrType == typeof(object))
            {
                return true;
            }
            return false;
        }
    }
}
