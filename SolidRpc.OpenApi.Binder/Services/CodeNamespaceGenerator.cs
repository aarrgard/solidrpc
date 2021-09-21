using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Threading;
using System.Threading.Tasks;
using SolidRpc.Abstractions;
using SolidRpc.Abstractions.OpenApi.Binder;
using SolidRpc.Abstractions.OpenApi.Transport;
using SolidRpc.Abstractions.Services.Code;
using SolidRpc.Abstractions.Types.Code;
using SolidRpc.OpenApi.Binder.Services;
using SolidRpc.OpenApi.Model.CodeDoc;

[assembly: SolidRpcServiceAttribute(typeof(ICodeNamespaceGenerator), typeof(CodeNamespaceGenerator), SolidRpcServiceLifetime.Transient)]
namespace SolidRpc.OpenApi.Binder.Services
{
    /// <summary>
    /// Creates code namespaces from the reggistered bindings.
    /// </summary>
    public class CodeNamespaceGenerator : ICodeNamespaceGenerator
    {
        /// <summary>
        /// Creates an instance
        /// </summary>
        /// <param name="methodBinderStore"></param>
        /// <param name="codeDocRepository"></param>
        public CodeNamespaceGenerator(IMethodBinderStore methodBinderStore, ICodeDocRepository codeDocRepository)
        {
            MethodBinderStore = methodBinderStore;
            CodeDocRepository = codeDocRepository;
        }

        private IMethodBinderStore MethodBinderStore { get; }
        private ICodeDocRepository CodeDocRepository { get; }

        /// <summary>
        /// Creates code for an assembly
        /// </summary>
        /// <param name="assemblyName"></param>
        /// <returns></returns>
        public Task<CodeNamespace> CreateCodeNamespace(string assemblyName, CancellationToken cancellationToken = default(CancellationToken))
        {
            var bindings = MethodBinderStore.MethodBinders
                .Where(o => o.Assembly.GetName().Name == assemblyName)
                .SelectMany(o => o.MethodBindings);
            return Task.FromResult(CreateCodeNamespace(bindings));
        }

        private CodeNamespace CreateCodeNamespace(IEnumerable<IMethodBinding> bindings)
        {
            var rootNs = new CodeNamespace();
            bindings.Select(o => o.MethodInfo.DeclaringType)
                .Distinct().ToList()
                .ForEach(o => CreateCodeInterface(rootNs, o));
            return rootNs;
        }


        private bool CreateCodeType(CodeNamespace rootNamespace, Type type)
        {
            var ns = GetNamespace(rootNamespace, type.FullName);
            var nsTypes = ns.Types ?? new CodeType[0];
            var codeType = nsTypes.FirstOrDefault(o => o.Name == type.Name);
            if (codeType != null)
            {
                return false;
            }
            codeType = new CodeType()
            {
                Name = type.Name,
                Description = CodeDocRepository.GetClassDoc(type).Summary
            };
            ns.Types = nsTypes.Concat(new[] { codeType }).ToArray();

            codeType.Properties = type.GetProperties().Select(o => CreateCodeTypeProperty(rootNamespace, o)).ToArray();

            return true;
        }

        private CodeTypeProperty CreateCodeTypeProperty(CodeNamespace rootNamespace, PropertyInfo pi)
        {
            return new CodeTypeProperty()
            {
                Description = CodeDocRepository.GetPropertyDoc(pi).Summary,
                Name = pi.Name,
                PropertyType = ResolveCodeType(rootNamespace, pi.PropertyType),
                HttpName = pi.GetCustomAttribute<DataMemberAttribute>()?.Name ?? pi.Name
            };
        }

        private CodeNamespace GetNamespace(CodeNamespace rootNamespace, string fullName)
        {
            var ns = rootNamespace;
            fullName.Split('.').SelectMany(o => o.Split('+')).Reverse().Skip(1).Reverse().ToList().ForEach(subNsName =>
            {
                var nsNamespaces = ns.Namespaces ?? new CodeNamespace[0];
                var subNs = nsNamespaces.SingleOrDefault(o => o.Name == subNsName);
                if(subNs == null)
                {
                    subNs = new CodeNamespace()
                    {
                        Name = subNsName
                    };
                    ns.Namespaces = nsNamespaces.Concat(new[] { subNs }).ToArray();
                }
                ns = subNs;
            });
            return ns;
        }

        private void CreateCodeInterface(CodeNamespace rootNamespace, Type interfaze)
        {
            var ci = new CodeInterface()
            {
                Description = CodeDocRepository.GetClassDoc(interfaze).Summary,
                Name = interfaze.Name,
                Methods = interfaze.GetMethods().Select(o => CreateCodeMethod(rootNamespace, o)).Where(o => o != null).ToList()
            };

            var ns = GetNamespace(rootNamespace, interfaze.FullName);
            var nsInterfaces = ns.Interfaces ?? new CodeInterface[0];
            ns.Interfaces = nsInterfaces.Concat(new[] { ci }).ToArray();
        }

        private CodeMethod CreateCodeMethod(CodeNamespace rootNamespace, MethodInfo mi)
        {
            var methodBinding = MethodBinderStore.GetMethodBinding(mi);
            if (methodBinding == null) return null;
            var httpTransport = methodBinding.Transports.OfType<IHttpTransport>().FirstOrDefault();
            if(httpTransport == null)
            {
                throw new Exception("No http transport configured for method - cannot create");
            }
            
            var hostUrl = httpTransport.OperationAddress.ToString();
            if(!hostUrl.EndsWith(methodBinding.LocalPath))
            {
                throw new Exception("Http transport address does not end with openapi adress");
            }

            var cm = new CodeMethod()
            {
                Description = CodeDocRepository.GetMethodDoc(mi).Summary,
                Name = mi.Name,
                Arguments = methodBinding.Arguments.Select(o => CreateCodeMethodArg(rootNamespace, o)).ToList(),
                ReturnType = ResolveCodeType(rootNamespace, mi.ReturnType),
                HttpMethod = methodBinding.Method,
                HttpBaseAddress = new Uri(hostUrl.Substring(0, hostUrl.Length - methodBinding.LocalPath.Length)+1),
                HttpPath = methodBinding.LocalPath.Substring(1)
            };
            return cm;
        }

        private CodeMethodArg CreateCodeMethodArg(CodeNamespace rootNamespace, IMethodArgument arg)
        {
            var mArg = new CodeMethodArg()
            {
                Description = CodeDocRepository?
                    .GetMethodDoc((MethodInfo)arg.ParameterInfo.Member)?
                    .GetParameterDocumentation(arg.Name)?.Summary,
                Name = arg.ParameterInfo.Name,
                ArgType = ResolveCodeType(rootNamespace, arg.ParameterInfo.ParameterType),
                Optional = arg.Optional,
                HttpName = arg.Name,
                HttpLocation = arg.Location
            };
            return mArg;
        }

        private IEnumerable<string> ResolveCodeType(CodeNamespace rootNamespace, Type type)
        {
            if(type == null)
            {
                return new string[0];
            }
            if(type.IsTaskType(out Type taskType))
            {
                return ResolveCodeType(rootNamespace, taskType);
            }
            if (type.IsNullableType(out Type nullType))
            {
                return ResolveCodeType(rootNamespace, nullType).Concat(new string[] { "?" }).ToArray();
            }
            if (type.IsGenericType)
            {
                if(type.GetGenericTypeDefinition() == typeof(IDictionary<,>))
                {
                    var args = type.GetGenericArguments().Select(o => ResolveCodeType(rootNamespace, o)).ToList();
                    return new string[] { $"Record<{string.Join(",", args.SelectMany(o => o))}>"};
                }
            }
            if (type.IsEnumType(out Type enumType))
            {
                return ResolveCodeType(rootNamespace, enumType).Concat(new string[] { "[]" }).ToArray();
            }
            if (type.IsGenericType)
            {
                throw new Exception("Cannot handle generic type:" + type.GetGenericTypeDefinition());
            }
            switch (type.FullName)
            {
                case "System.Boolean":
                    return new string[] { "boolean" };
                case "System.Object":
                    return new string[] { "object" };
                case "System.Guid":
                case "System.String":
                    return new string[] { "string" };
                case "System.String[]":
                    return new string[] { "string" , "[]"};
                case "System.Byte":
                case "System.Int16":
                case "System.Int32":
                case "System.Int64":
                case "System.Double":
                case "System.Single":
                case "System.Decimal":
                    return new string[] { "number" };
                case "System.IO.Stream":
                    return new string[] { "Uint8Array" };
                case "System.Threading.CancellationToken":
                    return new string[] { "CancellationToken" };
                case "System.Uri":
                    return new string[] { "Uri" };
                case "System.TimeSpan":
                case "System.DateTime":
                case "System.DateTimeOffset":
                    return new string[] { "Date" };
                default:
                    if(CreateCodeType(rootNamespace, type))
                    {

                    }
                    var codeType = type.FullName.Split('.').SelectMany(o => o.Split('+'));
                    return codeType;
            }
        }
    }
}
