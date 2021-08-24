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

[assembly: SolidRpcServiceAttribute(typeof(ITypescriptGenerator), typeof(TypeScriptGenerator), SolidRpcServiceLifetime.Transient)]
namespace SolidRpc.OpenApi.Binder.Services
{
    /// <summary>
    /// Creates typescript code from supplied code namespace.
    /// </summary>
    public class TypeScriptGenerator : ITypescriptGenerator
    {
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
            codeNamespace = FindRootNamespace(codeNamespace);
            var sb = new StringBuilder();
            sb.AppendLine("import { default as CancellationToken } from 'cancellationtoken';");
            sb.AppendLine("import { Observable, Subject } from 'rxjs';");
            sb.AppendLine("import { share } from 'rxjs/operators'");
            sb.AppendLine("import { SolidRpcJs } from 'solidrpc';");


            CreateTypesTs(codeNamespace, sb, "", codeNamespace);
            return Task.FromResult(sb.ToString());
        }

        private CodeNamespace FindRootNamespace(CodeNamespace codeNamespace)
        {
            if (codeNamespace.Types != null && codeNamespace.Types.Any())
            {
                return codeNamespace;
            }
            if (codeNamespace.Interfaces != null && codeNamespace.Interfaces.Any())
            {
                return codeNamespace;
            }
            if (codeNamespace.Namespaces != null && codeNamespace.Namespaces.Count() == 1)
            {
                return FindRootNamespace(codeNamespace.Namespaces.First());
            }
            return codeNamespace;
        }

        private void CreateTypesTs(CodeNamespace rootNamespace, StringBuilder code, string indentation, CodeNamespace codeNamespace)
        {
            code.Append(indentation).AppendLine($"export namespace {codeNamespace.Name} {{");
            var nsIndentation = CreateIndentation(indentation);
            (codeNamespace.Namespaces ?? new CodeNamespace[0]).OrderBy(o => o.Name).ToList().ForEach(o =>
            {
                CreateTypesTs(rootNamespace, code, nsIndentation, o);
            });
            (codeNamespace.Types ?? new CodeType[0]).OrderBy(o => o.Name).ToList().ForEach(o =>
            {
                CreateTypesTsClass(rootNamespace, code, nsIndentation, o);
            });
            (codeNamespace.Interfaces ?? new CodeInterface[0]).OrderBy(o => o.Name).ToList().ForEach(o =>
            {
                CreateTypesTsInterface(rootNamespace, code, nsIndentation, o);
                CreateTypesTsClass(rootNamespace, code, nsIndentation, o);
                CreateTypesTsInstance(rootNamespace, code, nsIndentation, o);
            });
            code.Append(indentation).AppendLine($"}}");
        }

        private void CreateTypesTsInstance(CodeNamespace rootNamespace, StringBuilder code, string indentation, CodeInterface interfaze)
        {
            var className = CreateClassName(interfaze.Name);
            var instanceName = className;
            if(instanceName.EndsWith("Impl"))
            {
                instanceName = instanceName.Substring(0, instanceName.Length - "Impl".Length);
            }
            instanceName = instanceName + "Instance";
            CreteJsComment(code, indentation, $"Instance for the {interfaze.Name} type. Implemented by the {className}");
            code.Append(indentation).AppendLine($"export var {instanceName} : {interfaze.Name} = new {className}();");
        }

        private string CreateClassName(string typeName)
        {
            if (typeName.StartsWith("I"))
            {
                typeName = typeName.Substring(1);
            }
            return $"{typeName}Impl";
        }

        private void CreateTypesTsInterface(CodeNamespace rootNamespace, StringBuilder code, string indentation, CodeInterface interfaze)
        {
            CreteJsComment(code, indentation, interfaze.Description);
            code.Append(indentation).AppendLine($"export interface {interfaze.Name} {{");
            var interfazeIndentation = CreateIndentation(indentation);
            (interfaze.Methods ?? new CodeMethod[0]).ToList().ForEach(m => {
                var tsReturnType = CreateTypescriptType(rootNamespace, m.ReturnType);
                CreteJsComment(code, interfazeIndentation, m.Description, m.Arguments.ToDictionary(o => o.Name, o => o.Description));
                code.Append(interfazeIndentation).Append($"{m.Name}(");
                bool firstArg = true;
                (m.Arguments ?? new CodeMethodArg[0]).ToList().ForEach(a =>
                {
                    var argumentIndentation = CreateIndentation(interfazeIndentation);
                    code.AppendLine(firstArg ? "" : ",");
                    firstArg = false;
                    code.Append(argumentIndentation).Append($"{a.Name}{(a.Optional ? "?" : "")} : {CreateTypescriptType(rootNamespace, a.ArgType)}");
                });
                if (!firstArg)
                {
                    code.AppendLine().Append(interfazeIndentation);
                }
                code.AppendLine($"): Observable<{tsReturnType}>;");

                //
                // hot observable
                //
                CreteJsComment(code, interfazeIndentation, $"This observable is hot and monitors all the responses from the {m.Name} invocations.");
                code.Append(interfazeIndentation).AppendLine($"{m.Name}Observable : Observable<{tsReturnType}>;");
            });
            code.Append(indentation).AppendLine($"}}");
        }

        private void CreateTypesTsClass(CodeNamespace rootNamespace, StringBuilder code, string indentation, CodeInterface interfaze)
        {
            var className = interfaze.Name;
            if(className.StartsWith("I"))
            {
                className = className.Substring(1);
            }
            CreteJsComment(code, indentation, interfaze.Description);
            className = $"{className}Impl";
            code.Append(indentation).AppendLine($"export class {className}  extends SolidRpcJs.RpcServiceImpl implements {interfaze.Name} {{");
            {
                var interfazeIndentation = CreateIndentation(indentation);
                //
                // constructor
                //
                code.Append(interfazeIndentation).AppendLine($"constructor() {{");
                {
                    var ctorIndentation = CreateIndentation(interfazeIndentation);
                    code.Append(ctorIndentation).AppendLine($"super();");
                    (interfaze.Methods ?? new CodeMethod[0]).ToList().ForEach(m =>
                    {
                        var tsReturnType = CreateTypescriptType(rootNamespace, m.ReturnType);
                        code.Append(ctorIndentation).AppendLine($"this.{m.Name}Subject = new Subject<{tsReturnType}>();");
                        code.Append(ctorIndentation).AppendLine($"this.{m.Name}Observable = this.{m.Name}Subject.asObservable().pipe(share());");
                    });
                }
                code.Append(interfazeIndentation).AppendLine($"}}");

                //
                // methods
                //
                (interfaze.Methods ?? new CodeMethod[0]).ToList().ForEach(m => {
                    var tsReturnType = CreateTypescriptType(rootNamespace, m.ReturnType);
                    CreteJsComment(code, interfazeIndentation, m.Description, m.Arguments.ToDictionary(o => o.Name, o => o.Description));
                    code.Append(interfazeIndentation).Append($"{m.Name}(");
                    string cancellationTokenArgName = "null";
                    bool firstArg = true;
                    (m.Arguments ?? new CodeMethodArg[0]).ToList().ForEach(a =>
                    {
                        var argumentIndentation = CreateIndentation(interfazeIndentation);
                        code.AppendLine(firstArg ? "" : ",");
                        firstArg = false;
                        var tsArgType = CreateTypescriptType(rootNamespace, a.ArgType);
                        if(tsArgType == "CancellationToken")
                        {
                            cancellationTokenArgName = a.Name;
                        }
                        code.Append(argumentIndentation).Append($"{a.Name}{(a.Optional ? "?" : "")} : {tsArgType}");
                    });
                    if (!firstArg)
                    {
                        code.AppendLine().Append(interfazeIndentation);
                    }
                    code.AppendLine($"): Observable<{tsReturnType}> {{");
                    {
                        var codeIndentation = CreateIndentation(interfazeIndentation);
                        code.Append(codeIndentation).AppendLine($"let uri = '{m.HttpBaseAddress}{m.HttpPath}';");
                        m.Arguments.Where(o => o.HttpLocation == "path").ToList().ForEach(o =>
                        {
                            code.Append(codeIndentation).AppendLine($"uri = uri.replace('{{{o.Name}}}', this.enocodeUriValue({o.Name}.toString()));");
                        });

                        var strQueryArgs = new StringBuilder();
                        var queryArgs = m.Arguments.Where(o => o.HttpLocation == "query").ToList();
                        if(queryArgs.Any())
                        {
                            strQueryArgs.AppendLine("{");
                            queryArgs.ForEach(o =>
                            {
                                strQueryArgs.Append(CreateIndentation(codeIndentation)).AppendLine($"'{o.HttpName}': {o.Name},");
                            });
                            strQueryArgs.Append("}");
                        }
                        else
                        {
                            strQueryArgs.Append("null");
                        }
                        var strHeaderArgs = new StringBuilder();
                        var headerArgs = m.Arguments.Where(o => o.HttpLocation == "header").ToList();
                        if (headerArgs.Any())
                        {
                            strHeaderArgs.Append("}");
                            headerArgs.ForEach(o =>
                            {
                                strHeaderArgs.Append(CreateIndentation(codeIndentation)).AppendLine($"'{o.HttpName}': {o.Name},");
                            });
                            strHeaderArgs.Append("}");
                        }
                        else
                        {
                            strHeaderArgs.Append("null");
                        }
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
                            strHeaderArgs.Clear();
                            strHeaderArgs.Append("{'Content-Type': 'application/json'}");
                            strBodyArgs.Append($"this.toJson({bodyArgs.First().Name})");
                        }
                        else
                        {
                            strBodyArgs.Append("null");
                        }
                        m.Arguments.Where(o => o.HttpLocation == "body-inline").ToList().ForEach(o =>
                        {
                            code.Append(CreateIndentation(codeIndentation)).AppendLine($"'{o.HttpName}': {o.Name},");
                        });

                        code.Append(codeIndentation).AppendLine($"return this.request<{tsReturnType}>('{m.HttpMethod.ToLower()}', uri, {strQueryArgs}, {strHeaderArgs}, {strBodyArgs}, {cancellationTokenArgName}, function(code : number, data : any) {{");
                        {
                            var respIndentation = CreateIndentation(codeIndentation);
                            code.Append(respIndentation).AppendLine($"if(code == 200) {{");
                            
                            code.Append(CreateIndentation(respIndentation)).AppendLine($"return {CreateJson2JsConverter(rootNamespace, m.ReturnType, "data")};");
                            code.Append(respIndentation).AppendLine($"}} else {{");
                            code.Append(CreateIndentation(respIndentation)).AppendLine($"throw 'Response code != 200('+code+')';");
                            code.Append(respIndentation).AppendLine($"}}");

                        }
                        code.Append(codeIndentation).AppendLine($"}}, this.{m.Name}Subject);");

                        code.Append(interfazeIndentation).AppendLine($"}}");
                    }

                    //
                    // hot observable
                    //
                    CreteJsComment(code, interfazeIndentation, $"This observable is hot and monitors all the responses from the {m.Name} invocations.");
                    code.Append(interfazeIndentation).AppendLine($"{m.Name}Observable : Observable<{tsReturnType}>;");
                    code.Append(interfazeIndentation).AppendLine($"private {m.Name}Subject : Subject<{tsReturnType}>;");

                });
            }
            code.Append(indentation).AppendLine($"}}");
        }

        private string CreateJs2JsonConverter(CodeNamespace rootNamespace, IEnumerable<string> type, string varName)
        {
            if (type.LastOrDefault() == "[]")
            {
                return $"for (let i = 0; i < {varName}.length; i++) {CreateJs2JsonConverter(rootNamespace, type.Reverse().Skip(1).Reverse(), $"{varName}[i]")}; arr.push(',');";
            }
            var jsType = CreateTypescriptType(rootNamespace, type);
            switch (jsType)
            {
                case "void":
                    return "";
                case "string":
                case "boolean":
                case "number":
                case "bigint":
                case "Uint8Array":
                    return $"arr.push(JSON.stringify({varName}))";
                default:
                    return $"{varName}.toJson(arr)";
            }
            throw new NotImplementedException();

        }

        private string CreateJson2JsConverter(CodeNamespace rootNamespace, IEnumerable<string> type, string varName)
        {
            if(type.LastOrDefault() == "[]")
            {
                return $"Array.from({varName}).map(o => {CreateJson2JsConverter(rootNamespace, type.Reverse().Skip(1).Reverse(), "o")})";
            }
            var jsType = CreateTypescriptType(rootNamespace, type);
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
                default:
                    return $"new {jsType}({varName})";
            }
            throw new NotImplementedException();
        }

        private string CreateIndentation(string indentation)
        {
            return $"{indentation}    ";
        }

        private void CreateTypesTsClass(CodeNamespace rootNamespace, StringBuilder code, string indentation, CodeType type)
        {
            CreteJsComment(code, indentation, type.Description);
            code.Append(indentation).AppendLine($"export class {type.Name} {{");
            {
                var classIndentation = CreateIndentation(indentation);
                code.Append(classIndentation).AppendLine($"constructor(obj?: any) {{");
                {
                    var ctorIndentation = CreateIndentation(classIndentation);
                    code.Append(ctorIndentation).AppendLine($"for(let prop in obj) {{");
                    {
                        var forIndentation = CreateIndentation(ctorIndentation);
                        code.Append(forIndentation).AppendLine($"switch(prop) {{");
                        {
                            var switchIndentation = CreateIndentation(forIndentation);
                            (type.Properties ?? new CodeTypeProperty[0]).ToList().ForEach(o => {
                                code.Append(switchIndentation).AppendLine($"case \"{o.HttpName}\":");
                                var src = $"obj.{o.HttpName}";
                                src = CreateJson2JsConverter(rootNamespace, o.PropertyType, src);
                                var caseIndentation = CreateIndentation(switchIndentation);
                                code.Append(caseIndentation).AppendLine($"if (obj.{o.HttpName}) {{ this.{o.Name} = {src}; }}");
                                code.Append(caseIndentation).AppendLine($"break;");
                            });
                        }
                        code.Append(forIndentation).AppendLine($"}}");
                    }
                    code.Append(ctorIndentation).AppendLine($"}}");
                }
                code.Append(classIndentation).AppendLine($"}}");

                // asJson
                code.Append(classIndentation).AppendLine($"toJson(arr: string[] | null): string | null {{");
                {
                    var asIndentation = CreateIndentation(classIndentation);
                    code.Append(asIndentation).AppendLine($"let returnString = false");
                    code.Append(asIndentation).AppendLine($"if(arr == null) {{");
                    code.Append(CreateIndentation(asIndentation)).AppendLine($"arr = [];");
                    code.Append(CreateIndentation(asIndentation)).AppendLine($"returnString = true;");
                    code.Append(asIndentation).AppendLine($"}}");
                    code.Append(asIndentation).AppendLine($"arr.push('{{');");
                    (type.Properties ?? new CodeTypeProperty[0]).ToList().ForEach(o => {
                        var jsonify = CreateJs2JsonConverter(rootNamespace, o.PropertyType, $"this.{o.Name}");
                        code.Append(asIndentation).AppendLine($"if(this.{o.Name}) {{ arr.push('\"{o.HttpName}\": '); {jsonify}; arr.push(','); }} ");
                    });
                    code.Append(asIndentation).AppendLine($"if(arr[arr.length-1] == ',') arr[arr.length-1] = '}}'; else arr.push('}}');");
                    code.Append(asIndentation).AppendLine($"if(returnString) return arr.join(\"\");");
                    code.Append(asIndentation).AppendLine($"return null;");
                }
                code.Append(classIndentation).AppendLine($"}}");

                // params
                (type.Properties ?? new CodeTypeProperty[0]).ToList().ForEach(o => {
                    CreteJsComment(code, classIndentation, o.Description);
                    code.Append(classIndentation).AppendLine($"{o.Name}: {CreateTypescriptType(rootNamespace, o.PropertyType)} | null = null;");
                });
            }
            code.Append(indentation).AppendLine($"}}");
        }

        private void CreteJsComment(StringBuilder code, string indentation, string description, Dictionary<string, string> argDescs = null)
        {
            var descr = (description ?? "")
                .Replace('\r', '\n')
                .Replace("\n\n", "\n")
                .Replace("\n", indentation + " * ")
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

        private string CreateTypescriptType(CodeNamespace rootNamespace, IEnumerable<string> type)
        {
            if(!type.Any())
            {
                return "void";
            }
            if (type.Last() == "[]")
            {
                return CreateTypescriptType(rootNamespace, type.Reverse().Skip(1).Reverse()) + "[]";
            }
            if (type.Last() == "?")
            {
                return CreateTypescriptType(rootNamespace, type.Reverse().Skip(1).Reverse());
            }
            if (type.Count() == 1)
            {
                if(type.First() == "Uri")
                {
                    return "string";
                }
            }
            // locate type from root namespace
            var newType = type;
            while(newType.Any() && FindType(rootNamespace, newType) == null)
            {
                newType = newType.Skip(1);
            }
            if(FindType(rootNamespace, newType) != null)
            {
                type = newType.ToArray();
            }
            
            return string.Join(".", type);
        }

        private CodeType FindType(CodeNamespace cns, IEnumerable<string> type)
        {
            if(cns.Name != type.FirstOrDefault())
            {
                return null;
            }
            type = type.Skip(1);
            if(type.Count() == 1)
            {
                return (cns.Types ?? new CodeType[0]).FirstOrDefault(o => o.Name == type.First());
            }
            var subCns = (cns.Namespaces ?? new CodeNamespace[0]).FirstOrDefault(o => o.Name == type.FirstOrDefault());
            if(subCns == null)
            {
                return null;
            }
            return FindType(subCns, type);
        }

    }
}
