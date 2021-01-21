using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Primitives;
using SolidProxy.Core.Configuration;
using SolidRpc.Abstractions.OpenApi.Binder;
using SolidRpc.Abstractions.OpenApi.Transport;
using SolidRpc.Abstractions.Types;
using SolidRpc.OpenApi.Model.CodeDoc.Impl;
using SolidRpc.OpenApi.Model.V2;

namespace SolidRpc.OpenApi.Binder.V2
{
    public class MethodBinderV2 : MethodBinderBase
    {
        public MethodBinderV2(IServiceProvider serviceProvider, SwaggerObject schemaObject, Assembly assembly) : base(serviceProvider, schemaObject, assembly)
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

        protected override IEnumerable<IMethodBinding> DoCreateMethodBinding(
            MethodInfo mi,
            IEnumerable<ITransport> transports)
        {
            if (mi == null) throw new ArgumentNullException(nameof(mi));
            if (mi.DeclaringType.Assembly != Assembly)
            {
                throw new ArgumentException("Method does not belong to assembly.");
            }
            var prospects = Operations;
            var binderStatus = new StringBuilder();
            binderStatus.Append($"->#{prospects.Count}");

            // operation id must start with method name
            prospects = Operations.Where(o => MethodBindingV2.NameMatches(o.OperationId, mi.Name)).ToList();
            binderStatus.Append($"->method({mi.Name})->#{prospects.Count}");

            // find all parameters for clr method
            foreach(var param in mi.GetParameters())
            {
                prospects = prospects.Where(o => FindParameter(o.GetParameters(), param)).ToList();
                binderStatus.Append($"->param({param.Name})->#{prospects.Count}");
            }

            // all the open api parameters must match a method argument
            prospects = prospects.Where(o =>
            {
                return o.GetParameters().All(p => FindParameter(p, mi.GetParameters())); ;
            }).ToList();
            binderStatus.Append($"->prospectargs->#{prospects.Count}");

            prospects = prospects.Where(o => {
                ResponseObject resp;
                o.GetResponses().TryGetValue("200", out resp);
                return TypeMatches(mi.ReturnType, resp?.Schema);        
            }).ToList();

            binderStatus.Append($"->returntype->#{prospects.Count}");


            if (prospects.Count < 1)
            {
                throw new NotImplementedException(binderStatus.ToString());
            }
            var methodDoc = CodeDocRepo.GetMethodDoc(mi);
            return prospects.Select(op => new MethodBindingV2(
                this,
                op,
                mi,
                methodDoc,
                transports)).ToList();
        }

        private bool FindParameter(ParameterObject p, ParameterInfo[] parameterInfos)
        {
            return parameterInfos.Any(o => MethodBindingV2.NameMatches(o.Name, p.Name));
        }

        private bool FindParameter(IEnumerable<ParameterObject> parameters, ParameterInfo parameter)
        {
            var prospect = parameters.FirstOrDefault(o => MethodBindingV2.NameMatches(o.Name, parameter.Name));
            if(prospect != null)
            {
                //
                // All the form data can be converted to the specified type - supplied data must be on correct format...
                //
                if(prospect.In == "formData")
                {
                    return true;
                }
                return TypeMatches(parameter.ParameterType, prospect);
            }

            //
            // cancellation tokens, http requests and IPrincipal objects can be implicitly bound
            //
            if (parameter.ParameterType == typeof(CancellationToken))
            {
                return true;
            }
            if (parameter.ParameterType == typeof(IPrincipal))
            {
                return true;
            }
            if(HttpRequestTemplate.GetTemplate(parameter.ParameterType).IsTemplateType)
            {
                return true;
            }

            //
            // find parameter as a binary type argument(if there is a binary argument...)
            //
            if(parameters.Any(o => o.IsBinaryType()))
            {
                if(FileContentTemplate.PropertyTypes.Keys.Any(o => parameter.Name.Equals(o, StringComparison.InvariantCultureIgnoreCase)))
                {
                    return true;
                }
            }

            //
            // find parameter in the body
            //
            var bodyParam = parameters.FirstOrDefault(o => o.In == "body");
            if (bodyParam != null)
            {
                var schema = bodyParam.Schema.GetRefSchema() ?? bodyParam.Schema;
                if(schema != null)
                {
                    return schema.Properties.ContainsKey(parameter.Name);
                }
            }
            if (parameter.IsOptional)
            {
                return true;
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
            if (type.IsNullableType(out Type nullableType))
            {
                type = nullableType;
            }
            if (type.IsTaskType(out Type taskType))
            {
                type = taskType;
            }
            if (type.IsAssignableFrom(clrType))
            {
                return true;
            }
            if (type.IsEnum && clrType == typeof(string))
            {
                return true;
            }
            if (clrType == typeof(object))
            {
                return true;
            }
            // this test can be removed when we register converters in the IoC container.
            if (type == typeof(DateTime) && clrType == typeof(DateTimeOffset))
            {
                return true;
            }
            if(clrType == typeof(string[]) && type == typeof(StringValues))
            {
                return true;
            }
            return false;
        }

        public T GetSolidProxyConfig<T>(MethodInfo methodInfo) where T : class, ISolidProxyInvocationAdviceConfig
        {
            var invocConf = ConfigStore.ProxyConfigurations
                .SelectMany(o => o.InvocationConfigurations).Where(o => o.MethodInfo == methodInfo)
                .FirstOrDefault();
            if (invocConf == null) return default(T);
            if (!invocConf.IsAdviceConfigured<T>()) return default(T);
            return invocConf.ConfigureAdvice<T>();
        }
    }
}
