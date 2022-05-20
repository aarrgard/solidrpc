using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SolidRpc.Abstractions;
using SolidRpc.Abstractions.Services.Code;
using SolidRpc.Abstractions.Types.Code;
using SolidRpc.OpenApi.Binder.Services;

[assembly: SolidRpcService(typeof(ITypescriptGenerator), typeof(TypeScriptGenerator), SolidRpcServiceLifetime.Transient)]
namespace SolidRpc.OpenApi.Binder.Services
{
    /// <summary>
    /// Creates typescript code from supplied code namespace.
    /// </summary>
    public class TypeScriptGenerator : ITypescriptGenerator
    {
        private class RootNamespace
        {
            public RootNamespace(IEnumerable<string> name, CodeNamespace codeNamespace)

            {
                Name = name;
                CodeNamespace = codeNamespace;
            }
            public IEnumerable<string> Name { get; }
            public CodeNamespace CodeNamespace { get; }
        }

        public TypeScriptGenerator(ICodeNamespaceGenerator codeGenerator)
        {
            CodeGenerator = codeGenerator;
        }
        private ICodeNamespaceGenerator CodeGenerator { get; }

        public async Task<string> CreateTypesTsForAssemblyAsync(string assemblyName, CancellationToken cancellationToken = default(CancellationToken))
        {
            //
            // create static package
            //
            var resName = GetType().Assembly.GetManifestResourceNames().FirstOrDefault(o => o.EndsWith($".{assemblyName}.ts", StringComparison.InvariantCultureIgnoreCase));
            if (resName != null)
            {
                using (var s = GetType().Assembly.GetManifestResourceStream(resName))
                using (var sr = new StreamReader(s))
                {
                    return sr.ReadToEnd();
                }
            }

            var codeNamepace = await CodeGenerator.CreateCodeNamespace(assemblyName, cancellationToken);
            return await CreateTypesTsForCodeNamespaceAsync(codeNamepace, cancellationToken);
        }
        
        /// <summary>
        /// Creates a types.ts file
        /// </summary>
        /// <param name="rootNamespace"></param>
        /// <returns></returns>
        public Task<string> CreateTypesTsForCodeNamespaceAsync(CodeNamespace codeNamespace, CancellationToken cancellationToken = default(CancellationToken))
        {
            var sb = new StringBuilder();
            sb.AppendLine("import { default as CancellationToken } from 'cancellationtoken';");
            sb.AppendLine("import { Observable, Subject } from 'rxjs';");
            sb.AppendLine("import { share } from 'rxjs/operators'");
            sb.AppendLine("import { SolidRpcJs } from 'solidrpcjs';");

            if(string.IsNullOrEmpty(codeNamespace.Name) && codeNamespace.Namespaces == null)
            {
                return Task.FromResult(sb.ToString());
            }
            var rootNamespace = FindRootNamespace(new string[0], codeNamespace);
            CreateTypesTs(rootNamespace, new [] { rootNamespace.CodeNamespace.Name }, sb, "");
            return Task.FromResult(sb.ToString());
        }

        private RootNamespace FindRootNamespace(IEnumerable<string> parent, CodeNamespace codeNamespace)
        {
            if (codeNamespace.Types != null && codeNamespace.Types.Any())
            {
                return new RootNamespace(parent, codeNamespace);
            }
            if (codeNamespace.Interfaces != null && codeNamespace.Interfaces.Any())
            {
                return new RootNamespace(parent, codeNamespace);
            }
            if (codeNamespace.Namespaces != null && codeNamespace.Namespaces.Count() == 1)
            {
                return FindRootNamespace(parent.Concat(new[] { codeNamespace.Name }).Where(o => o != null), codeNamespace.Namespaces.First());
            }
            return new RootNamespace(parent, codeNamespace);
        }

        private CodeNamespace GetCodeNamespace(RootNamespace rootNamespace, IEnumerable<string> codeNamespaceName)
        {
            var codeNamespace = rootNamespace.CodeNamespace;
            if(codeNamespace.Name != codeNamespaceName.First())
            {
                throw new Exception("Code namespace does not begin with root namespace name");
            }
            foreach (var ns in codeNamespaceName.Skip(1))
            {
                codeNamespace = codeNamespace.Namespaces.Where(o => o.Name == ns).Single();
            }
            return codeNamespace;
        }

        private void CreateTypesTs(RootNamespace rootNamespace, IEnumerable<string> codeNamespaceName, StringBuilder code, string indentation)
        {
            var codeNamespace = GetCodeNamespace(rootNamespace, codeNamespaceName);
            code.Append(indentation).AppendLine($"export namespace {codeNamespace.Name} {{");
            var nsIndentation = CreateIndentation(indentation);
            (codeNamespace.Namespaces ?? new CodeNamespace[0]).OrderBy(o => o.Name).ToList().ForEach(o =>
            {
                CreateTypesTs(rootNamespace, codeNamespaceName.Concat(new[] { o.Name }).ToList(), code, nsIndentation); ;
            });
            (codeNamespace.Types ?? new CodeType[0]).OrderBy(o => o.Name).ToList().ForEach(o =>
            {
                CreateTypesTsClass(rootNamespace, codeNamespaceName, o, code, nsIndentation);
            });
            (codeNamespace.Interfaces ?? new CodeInterface[0]).OrderBy(o => o.Name).ToList().ForEach(o =>
            {
                CreateTypesTsFunctions(rootNamespace, codeNamespaceName, o, code, nsIndentation);
            });
            code.Append(indentation).AppendLine($"}}");
        }

        private void CreateTypesTsFunctions(RootNamespace rootNamespace, IEnumerable<string> codeNamespaceName, CodeInterface interfaze, StringBuilder code, string indentation)
        {
            CreteJsComment(code, indentation, interfaze.Description);
            code.Append(indentation).AppendLine($"export namespace {interfaze.Name} {{");
            var interfazeIndentation = CreateIndentation(indentation);
            var methodNames = new List<string>();
            (interfaze.Methods ?? new CodeMethod[0]).ToList().ForEach(m => {
                
                //
                // construct unique method name
                //
                var methodName = m.Name;
                int idx = 1;
                while(methodNames.Contains(methodName))
                {
                    methodName = $"{m.Name}{idx}";
                    idx++;
                }
                methodNames.Add(methodName);

                //
                // Add the method
                //
                var tsReturnType = CreateTypescriptType(rootNamespace, codeNamespaceName, m.ReturnType);
                code.Append(interfazeIndentation).AppendLine($"let {methodName}Subject = new Subject<{tsReturnType}>();");
                CreteJsComment(code, interfazeIndentation, $"This observable is hot and monitors all the responses from the {m.Name} invocations.");
                code.Append(interfazeIndentation).AppendLine($"export var {methodName}Observable = {methodName}Subject.asObservable().pipe(share());");

                CreteJsComment(code, interfazeIndentation, m.Description, m.Arguments.ToDictionary(o => o.Name, o => o.Description));
                code.Append(interfazeIndentation).Append($"export function {methodName}(");
                bool firstArg = true;
                string cancellationTokenArgName = "null";
                (m.Arguments ?? new CodeMethodArg[0]).ToList().ForEach(a =>
                {
                    var argumentIndentation = CreateIndentation(interfazeIndentation);
                    code.AppendLine(firstArg ? "" : ",");
                    firstArg = false;
                    code.Append(argumentIndentation).Append($"{a.Name}{(a.Optional ? "?" : "")} : {CreateTypescriptType(rootNamespace, codeNamespaceName, a.ArgType)}");
                    var tsArgType = CreateTypescriptType(rootNamespace, codeNamespaceName, a.ArgType);
                    if (tsArgType == "CancellationToken")
                    {
                        cancellationTokenArgName = a.Name;
                    }
                });
                if (!firstArg)
                {
                    code.AppendLine().Append(interfazeIndentation);
                }
                code.AppendLine($"): SolidRpcJs.RpcServiceRequestTyped<{tsReturnType}> {{");

                {
                    var codeIndentation = CreateIndentation(interfazeIndentation);
                    code.Append(codeIndentation).AppendLine($"let __ns = SolidRpcJs.rootNamespace.declareNamespace('{string.Join(".", rootNamespace.Name.Concat(codeNamespaceName).Concat(new[] { interfaze.Name }))}');");
                    code.Append(codeIndentation).AppendLine($"let __uri = __ns.getStringValue('baseUrl','{m.HttpBaseAddress}') + '{m.HttpPath}';");
                    m.Arguments.Where(o => o.HttpLocation == "path").ToList().ForEach(o =>
                    {
                        code.Append(codeIndentation).AppendLine($"SolidRpcJs.ifnull({o.Name}, () => {{ __uri = __uri.replace('{{{o.Name}}}', ''); }}, nn =>  {{ __uri = __uri.replace('{{{o.Name}}}', SolidRpcJs.encodeUriValue(nn.toString())); }});");
                    });

                    code.Append(codeIndentation).AppendLine($"let query: {{ [index: string]: any }} = {{}};");
                    var queryArgs = m.Arguments.Where(o => o.HttpLocation == "query").ToList();
                    queryArgs.ForEach(o =>
                    {
                        code.Append(codeIndentation).AppendLine($"SolidRpcJs.ifnotnull({o.Name}, x => {{ query['{o.HttpName}'] = x; }});");
                    });

                    code.Append(codeIndentation).AppendLine($"let headers: {{ [index: string]: any }} = {{}};");
                    var headerArgs = m.Arguments.Where(o => o.HttpLocation == "header").ToList();
                    headerArgs.ForEach(o =>
                    {
                        code.Append(codeIndentation).AppendLine($"SolidRpcJs.ifnotnull({o.Name}, x => {{ headers['{o.HttpName}'] = x; }});");
                    });

                    var strBodyArgs = new StringBuilder();
                    var bodyInlineArgs = m.Arguments.Where(o => o.HttpLocation == "body-inline").ToList();
                    var bodyArgs = m.Arguments.Where(o => o.HttpLocation == "body").ToList();
                    if (bodyInlineArgs.Any())
                    {
                        strBodyArgs.Append("{");
                        bodyInlineArgs.ForEach(o =>
                        {
                            strBodyArgs.Append(CreateIndentation(codeIndentation)).AppendLine($"'{o.HttpName}': {o.Name},");
                        });
                        strBodyArgs.Append("}");
                    }
                    else if (bodyArgs.Any())
                    {
                        code.Append(codeIndentation).AppendLine($"headers['Content-Type']='application/json';");
                        strBodyArgs.Append($"SolidRpcJs.toJson({bodyArgs.First().Name})");
                    }
                    else
                    {
                        strBodyArgs.Append("null");
                    }
                    m.Arguments.Where(o => o.HttpLocation == "body-inline").ToList().ForEach(o =>
                    {
                        code.Append(CreateIndentation(codeIndentation)).AppendLine($"'{o.HttpName}': {o.Name},");
                    });

                    code.Append(codeIndentation).AppendLine($"return new SolidRpcJs.RpcServiceRequestTyped<{tsReturnType}>('{m.HttpMethod.ToLower()}', __uri, query, headers, {strBodyArgs}, {cancellationTokenArgName}, function(code : number, data : any) {{");
                    {
                        var respIndentation = CreateIndentation(codeIndentation);
                        code.Append(respIndentation).AppendLine($"if(code == 200) {{");

                        code.Append(CreateIndentation(respIndentation)).AppendLine($"return {CreateJson2JsConverter(rootNamespace, codeNamespaceName, m.ReturnType, "data")};");
                        code.Append(respIndentation).AppendLine($"}} else {{");
                        code.Append(CreateIndentation(respIndentation)).AppendLine($"throw 'Response code != 200('+code+')';");
                        code.Append(respIndentation).AppendLine($"}}");

                    }
                    code.Append(codeIndentation).AppendLine($"}}, {methodName}Subject);");
                    code.Append(interfazeIndentation).AppendLine($"}}");
                }
            });

            code.Append(interfazeIndentation).AppendLine($"}}");
        }

        private string CreateJs2JsonConverter(RootNamespace rootNamespace, IEnumerable<string> codeNamespaceName, IEnumerable<string> type, string varName)
        {
            if (type.LastOrDefault() == "[]")
            {
                return $"arr.push('['); for (let i = 0; i < {varName}.length; i++) {{ {CreateJs2JsonConverter(rootNamespace, codeNamespaceName, type.Reverse().Skip(1).Reverse(), $"{varName}[i]")}; arr.push(','); }} arr.push(']');";
            }
            if (type.LastOrDefault() == "?")
            {
                return CreateJs2JsonConverter(rootNamespace, codeNamespaceName, type.Reverse().Skip(1).Reverse(), varName);
            }
            var jsType = CreateTypescriptType(rootNamespace, codeNamespaceName, type);
            switch (jsType)
            {
                case "void":
                    return "";
                case "string":
                case "boolean":
                case "number":
                case "bigint":
                case "Uint8Array":
                case "Date":
                case "Record<string,string>":
                    return $"arr.push(JSON.stringify({varName}))";
                default:
                    return $"if({varName}) {{{varName}.toJson(arr)}}";
            }
            throw new NotImplementedException();

        }

        private string CreateJson2JsConverter(RootNamespace rootNamespace, IEnumerable<string> codeNamespaceName, IEnumerable<string> type, string varName)
        {
            if (type.LastOrDefault() == "[]")
            {
                return $"Array.from({varName}).map(o => {CreateJson2JsConverter(rootNamespace, codeNamespaceName, type.Reverse().Skip(1).Reverse(), "o")})";
            }
            if (type.LastOrDefault() == "?")
            {
                var notNullType = CreateTypescriptType(rootNamespace, codeNamespaceName, type.Reverse().Skip(1).Reverse());
                return $"SolidRpcJs.ifnotnull<{notNullType}>({varName}, (notnull) => {CreateJson2JsConverter(rootNamespace, codeNamespaceName, type.Reverse().Skip(1).Reverse(), "notnull")})";
            }
            var jsType = CreateTypescriptType(rootNamespace, codeNamespaceName, type);
            switch (jsType)
            {
                case "void":
                    return "null";
                case "string":
                    return $"{varName} as string";
                case "boolean":
                    return $"[true, 'true', 1].some(o => o === {varName})";
                case "number":
                    return $"Number({varName})";
                case "Record<string,string>":
                    return $"{varName} as {jsType}";
                default:
                    return $"new {jsType}({varName})";
            }
            throw new NotImplementedException();
        }

        private string CreateIndentation(string indentation)
        {
            return $"{indentation}    ";
        }

        private void CreateTypesTsClass(RootNamespace rootNamespace, IEnumerable<string> codeNamespaceName, CodeType type, StringBuilder code, string indentation)
        {
            CreteJsComment(code, indentation, type.Description);
            code.Append(indentation).AppendLine($"export class {type.Name} {{");
            {
                var classIndentation = CreateIndentation(indentation);
                code.Append(classIndentation).AppendLine($"constructor(obj?: any) {{");
                {
                    var ctorIndentation = CreateIndentation(classIndentation);
                    (type.Properties ?? new CodeTypeProperty[0]).ToList().ForEach(o => {
                        var conv = CreateJson2JsConverter(rootNamespace, codeNamespaceName, o.PropertyType, "val");
                        code.Append(ctorIndentation).AppendLine($"SolidRpcJs.ifnotnull(obj.{o.HttpName}, val => {{ this.{o.Name} = {conv}; }});");
                    });
                }
                code.Append(classIndentation).AppendLine($"}}");

                // asJson
                code.Append(classIndentation).AppendLine($"toJson(arr: string[]): void {{");
                {
                    var asIndentation = CreateIndentation(classIndentation);
                    code.Append(asIndentation).AppendLine($"arr.push('{{');");
                    (type.Properties ?? new CodeTypeProperty[0]).ToList().ForEach(o => {
                        var jsonify = CreateJs2JsonConverter(rootNamespace, codeNamespaceName, o.PropertyType, $"this.{o.Name}");
                        code.Append(asIndentation).AppendLine($"if(this.{o.Name}) {{ arr.push('\"{o.HttpName}\": '); {jsonify}; arr.push(','); }} ");
                    });
                    code.Append(asIndentation).AppendLine($"if(arr[arr.length-1] == ',') arr[arr.length-1] = '}}'; else arr.push('}}');");
                }
                code.Append(classIndentation).AppendLine($"}}");

                // params
                (type.Properties ?? new CodeTypeProperty[0]).ToList().ForEach(o => {
                    CreteJsComment(code, classIndentation, o.Description);
                    code.Append(classIndentation).AppendLine($"{o.Name}: {CreateTypescriptType(rootNamespace, codeNamespaceName, o.PropertyType)} | null = null;");
                });
            }
            code.Append(indentation).AppendLine($"}}");
        }

        private void CreteJsComment(StringBuilder code, string indentation, string description, Dictionary<string, string> argDescs = null)
        {
            var descr = (description ?? "")
                .Replace('\r', '\n')
                .Replace("\n\n", "\n")
                .Replace("\n", "\n" + indentation + " * ")
                .Trim();
            code.Append(indentation).AppendLine("/**");
            code.Append(indentation).Append(" * ").AppendLine(descr);
            if(argDescs != null)
            {
                argDescs.ToList().ForEach(o =>
                {
                    code.Append(indentation).Append(" * @param ").Append(o.Key).Append(" ").AppendLine(o.Value);
                });
            }
            code.Append(indentation).AppendLine(" */");
        }

        private string CreateTypescriptType(RootNamespace rootNamespace, IEnumerable<string> codeNamespaceName, IEnumerable<string> type)
        {
            if(!type.Any())
            {
                return "void";
            }
            var typeName = type.Last();
            if (typeName == "[]")
            {
                return CreateTypescriptType(rootNamespace, codeNamespaceName, type.Reverse().Skip(1).Reverse()) + "[]";
            }
            if (typeName == "?")
            {
                return CreateTypescriptType(rootNamespace, codeNamespaceName, type.Reverse().Skip(1).Reverse()) + "|null";
            }
            if (type.Count() == 1)
            {
                if(typeName == "Uri")
                {
                    return "string";
                }
                return type.First();
            }

            var retVal = string.Join(".", FindType(rootNamespace, codeNamespaceName, type));
            return retVal;
        }

        private IEnumerable<string> FindType(RootNamespace rns, IEnumerable<string> codeNamespaceName, IEnumerable<string> type)
        {
            // remove root namespace from type name
            foreach(var name in rns.Name)
            {
                if(type.FirstOrDefault() == name)
                {
                    type = type.Skip(1);
                }
                else
                {
                    throw new Exception("Type does not belong to package.");
                }
            }

            var codeNamespace = rns.CodeNamespace;
            if (codeNamespace.Name != type.First())
            {
                throw new Exception("code namespace is not correct.");
            }

            // remove namespaces that the type and calling type have in common
            while (codeNamespaceName.FirstOrDefault() == type.FirstOrDefault())
            {
                codeNamespaceName = codeNamespaceName.Skip(1);
                type = type.Skip(1);
                if(codeNamespaceName.Any())
                {
                    codeNamespace = GetCodeNamespace(codeNamespace.Namespaces, type.First());
                }
            }
            return FindType(codeNamespace, type);
        }

        private CodeNamespace GetCodeNamespace(IEnumerable<CodeNamespace> cns, string name)
        {
            var namespaces = cns ?? new CodeNamespace[0];
            var subCns = namespaces.FirstOrDefault(o => o.Name == name);
            if (subCns == null)
            {
                throw new Exception($"Cannot find namespace: {name} among namespaces {string.Join(",", namespaces.Select(o => o.Name))}");
            }
            return subCns;
        }

        private IEnumerable<string> FindType(CodeNamespace cns, IEnumerable<string> type)
        {
            if (type.Count() == 1)
            {
                var codeType = (cns.Types ?? new CodeType[0]).FirstOrDefault(o => o.Name == type.First());
                if (codeType == null)
                {
                    throw new Exception("cannot find code type:" + type.First());
                }
                return type;
            } 
            else
            {
                if (cns.Name != type.First())
                {
                    throw new Exception($"Namespace name({cns.Name}) is not first type name({type.First()}).");
                }
                type = type.Skip(1);

                if(type.Count() == 1)
                {
                    return new[] { cns.Name }.Concat(FindType(cns, type));
                }
                else
                {
                    var subCns = GetCodeNamespace(cns.Namespaces, type.First());
                    return new[] { cns.Name }.Concat(FindType(subCns, type));
                }
            }
        }
    }
}
