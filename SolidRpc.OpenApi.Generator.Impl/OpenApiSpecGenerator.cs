using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Newtonsoft.Json;
using SolidRpc.OpenApi.Generator.Model.CSharp;
using SolidRpc.OpenApi.Generator.Model.CSharp.Impl;
using SolidRpc.OpenApi.Generator.Types;
using SolidRpc.OpenApi.Generator.V2;
using SolidRpc.OpenApi.Model;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SolidRpc.OpenApi.Generator
{
    /// <summary>
    /// Base class for generating a OpenApi specification.
    /// </summary>
    public abstract class OpenApiSpecGenerator
    {
        private enum NameScope { Namespace, Class, Interface };
        public static FileData GenerateOpenApiSpec(SettingsSpecGen settings, Project project)
        {
            OpenApiSpecGenerator generator;
            switch (settings.OpenApiVersion)
            {
                case "2.0":
                    generator = new OpenApiSpecGeneratorV2(settings);
                    break;
                default:
                    throw new Exception("Cannot handle swagger version:" + settings.OpenApiVersion);
            }
            return generator.GenerateSpec(project);
        }

        public OpenApiSpecGenerator(SettingsSpecGen settings)
        {
            Settings = settings;
            CSharpRepository = new CSharpRepository();
            CompilationUnits = new ConcurrentDictionary<string, CompilationUnitSyntax>();

            TypeDefinitionNameMapper = c => {
                var name = c.FullName;
                if (name.StartsWith($"{Settings.ProjectNamespace}."))
                {
                    name = name.Substring(Settings.ProjectNamespace.Length + 1);
                }
                if (name.StartsWith($"{Settings.CodeNamespace}."))
                {
                    name = name.Substring(Settings.CodeNamespace.Length + 1);
                }
                if (name.StartsWith($"{Settings.ServiceNamespace}."))
                {
                    name = name.Substring(Settings.ServiceNamespace.Length + 1);
                }
                else if (name.StartsWith($"{Settings.TypeNamespace}."))
                {
                    name = name.Substring(Settings.TypeNamespace.Length + 1);
                }

                return name;
            };
            MapPath = s => $"/{s.Replace('.', '/')}";
        }

        public SettingsSpecGen Settings { get; }

        public ConcurrentDictionary<string, CompilationUnitSyntax> CompilationUnits { get; }

        public ICSharpRepository CSharpRepository { get; }

        /// <summary>
        /// The function that maps a type definition on to a reference name.
        /// </summary>
        public Func<ICSharpType, string> TypeDefinitionNameMapper { get; set; }

        /// <summary>
        /// The function that maps the method name to a path.
        /// </summary>
        public Func<string, string> MapPath { get; set; }

        protected virtual FileData GenerateSpec(Project project)
        {
            var csFiles = project.ProjectFiles
                .Where(o => o.FileData.Filename.EndsWith(".cs", StringComparison.InvariantCultureIgnoreCase));
            foreach (var csFile in csFiles)
            {
                var cu = GetCompilationUnit(csFile);
                WalkMembers(cu.Members, Sweap1);
            }
            foreach (var csFile in csFiles)
            {
                var cu = GetCompilationUnit(csFile);
                WalkMembers(cu.Members, Sweap2);
            }

            var swaggerSpec = CreateSwaggerSpecFromTypesInRepo();
            var ms = new MemoryStream();
            using (var tw = new StreamWriter(ms))
            {
                var s = JsonSerializer.Create(new JsonSerializerSettings()
                {
                    ContractResolver = NewtonsoftContractResolver.Instance,
                    Formatting = Formatting.Indented
                });
                s.Serialize(tw, swaggerSpec);
            }
            return new FileData()
            {
                ContentType = "application/json",
                FileStream = new MemoryStream(ms.ToArray()),
                Filename = $"{Settings.ProjectNamespace}.json"
            };
        }

        protected abstract IOpenApiSpec CreateSwaggerSpecFromTypesInRepo();

        private CompilationUnitSyntax GetCompilationUnit(ProjectFile csFile)
        {
            var fullName = $"{csFile.Directory}/{csFile.FileData.Filename}";
            return CompilationUnits.GetOrAdd(fullName, _ =>
            {
                using (var sr = new StreamReader(csFile.FileData.FileStream))
                {
                    var syntaxTree = CSharpSyntaxTree.ParseText(sr.ReadToEnd());
                    return (CompilationUnitSyntax)syntaxTree.GetRoot();
                }
            });
        }

        private void WalkMembers(IEnumerable<MemberDeclarationSyntax> members, Action<MemberDeclarationSyntax> action)
        {
            foreach (var member in members)
            {
                WalkMember(member, action);
            }
        }

        private void WalkMember(MemberDeclarationSyntax member, Action<MemberDeclarationSyntax> action)
        {
            action(member);
            if (member is NamespaceDeclarationSyntax nds)
            {
                WalkMembers(nds.Members, action);
                return;
            }
            if (member is InterfaceDeclarationSyntax ids)
            {
                WalkMembers(ids.Members, action);
                return;
            }
            if (member is ClassDeclarationSyntax cds)
            {
                // handle the class
                WalkMembers(cds.Members, action);
                return;
            }
            if (member is MethodDeclarationSyntax mds)
            {
                return;
            }
            if (member is PropertyDeclarationSyntax pds)
            {
                return;
            }
            if (member is ConstructorDeclarationSyntax ctrds)
            {
                return;
            }

            throw new Exception("Cannot handle member:" + member.GetType().FullName);
        }

        private void Sweap1(MemberDeclarationSyntax member)
        {
            if (member is InterfaceDeclarationSyntax ids)
            {
                var (className, nameScope) = GetClassOrInterfaceName(ids);
                var m = GetMember(className, nameScope);
                SetComment(member, m);
             }
            if (member is ClassDeclarationSyntax cds)
            {
                var (className, nameScope) = GetClassOrInterfaceName(cds);
                var m = GetMember(className, nameScope);
                SetComment(member, m);
            }
        }

        private void SetComment(SyntaxNode node, ICSharpMember cSharpMember)
        {
            var comment = string.Join("", node.GetLeadingTrivia()
                 //.Where(o => o.Kind() == SyntaxKind.SingleLineDocumentationCommentTrivia)
                 .Select(o => o.ToString()));
            cSharpMember.ParseComment(comment);
        }

        private void Sweap2(MemberDeclarationSyntax member)
        {
            if (member is MethodDeclarationSyntax mds)
            {
                CreateCSharpMethod(mds);
                return;
            }
            if (member is PropertyDeclarationSyntax pds)
            {
                CreateCSharpProperty(pds);
                return;
            }
        }

        private void CreateCSharpProperty(PropertyDeclarationSyntax pds)
        {
            var (className, nameScope) = GetClassOrInterfaceName(pds);
            var m = GetMember(className, nameScope);

            var propertyTypeName = GetFullName(pds, pds.Type.ToString());
            var propertyType = CSharpRepository.GetType(propertyTypeName);
            if (propertyType == null)
            {
                throw new Exception("Failed to find the type for " + propertyTypeName);
            }
            var propertyName = pds.Identifier.ToString();

            var property = new CSharpProperty(m, propertyName, propertyType);
            SetComment(pds,property);
            m.AddMember(property);
        }

        private void CreateCSharpMethod(MethodDeclarationSyntax mds)
        {
            var (className, nameScope) = GetClassOrInterfaceName(mds);
            var m = GetMember(className, nameScope);

            // return type
            var returnTypeName = GetFullName(mds, mds.ReturnType.ToString());
            var returnType = CSharpRepository.GetType(returnTypeName);
            if (returnType == null)
            {
                throw new Exception("Failed to find the type for " + returnTypeName);
            }

            var methodName = mds.Identifier.ToString();

            var method = new CSharpMethod(m, methodName, returnType);
            method.Parameters = mds.ParameterList.Parameters.Select(o =>
            {
                var parameterTypeName = GetFullName(o, o.Type.ToString());
                var parameterType = CSharpRepository.GetType(parameterTypeName);
                if(parameterType == null)
                {
                    throw new Exception("Failed to find the type for " + parameterTypeName);
                }

                var parameterName = o.Identifier.ToString();
                var optional = o.Default == null;
                return new CSharpMethodParameter(method, parameterName, parameterType, optional);
            }).ToList();

            SetComment(mds, method);
            m.AddMember(method);
        }

        private ICSharpMember GetMember(string className, NameScope nameScope)
        {
            if (nameScope == NameScope.Class)
            {
                return CSharpRepository.GetClass(className);
            }
            else if (nameScope == NameScope.Interface)
            {
                return CSharpRepository.GetInterface(className);
            }
            else
            {
                throw new Exception("Cannot handle scope:" + nameScope);
            }
        }

        private string GetFullName(SyntaxNode member, string typeName)
        {
            // handle generic types
            var (genType, genArgs, rest) = Model.CSharp.Impl.CSharpRepository.ReadType(typeName);
            if(genArgs != null)
            {
                var suffix = $"`{genArgs.Count}";
                genType = $"{genType}{suffix}";
                genType = GetFullName(member, genType);
                genArgs = genArgs.Select(o => GetFullName(member, o)).ToList();
                return $"{genType.Substring(0, genType.Length - suffix.Length)}<{string.Join(",", genArgs)}>";
            }
            var genIdx = typeName.IndexOf("<");

            var (usings, ns) = GetPrefixes(member);

            // try to get the type from "ns"
            ICSharpType type;
            do
            {
                type = CSharpRepository.GetType(MergeNames(ns, ".", typeName));
                if (type != null)
                {
                    return type.FullName;
                }
                ns = ns.Substring(0, ns.LastIndexOf('.'));

            } while (ns.LastIndexOf('.') > -1) ;

            // use the "usings"
            foreach (var u in usings)
            {
                type = CSharpRepository.GetType(MergeNames(u, ".", typeName));
                if (type != null)
                {
                    return type.FullName;
                }
            }
            type = CSharpRepository.GetType(typeName);
            if(type != null)
            {
                return type.FullName;
            }
            throw new Exception($"Cannot find type {typeName}");
        }

        private (HashSet<string>, string) GetPrefixes(SyntaxNode member)
        {
            if(member == null)
            {
                return (new HashSet<string>(), "");
            }
            var (usings, ns) = GetPrefixes(member.Parent);
            if (member is ClassDeclarationSyntax cds)
            {
                ns = MergeNames(ns, ".", cds.Identifier.ToString());
            }
            if (member is NamespaceDeclarationSyntax nds)
            {
                ns = MergeNames(ns, ".", nds.Name.ToString());
            }
            foreach (var u in member.ChildNodes().OfType<UsingDirectiveSyntax>())
            {
                usings.Add(u.Name.ToString());
            }
            return (usings, ns);
        }

        private (string, NameScope) GetClassOrInterfaceName(SyntaxNode member)
        {
            if (member == null)
            {
                return ("", NameScope.Namespace);
            }
            var (parentName, nameScope) = GetClassOrInterfaceName(member.Parent);
            var localName = "";
            var nameSeparator = nameScope == NameScope.Namespace ? "." : "+";
            if (member is ClassDeclarationSyntax cds)
            {
                localName = cds.Identifier.ToString();
                nameScope = NameScope.Class;
            }
            else if (member is InterfaceDeclarationSyntax ids)
            {
                localName = ids.Identifier.ToString();
                nameScope = NameScope.Interface;
            }
            else if (member is NamespaceDeclarationSyntax nds)
            {
                localName = nds.Name.ToString();
                nameScope = NameScope.Namespace;
            }

            return (MergeNames(parentName, nameSeparator, localName), nameScope);
        }

        private string MergeNames(string parentName, string nameSeparator, string localName)
        {
            if (string.IsNullOrEmpty(parentName))
            {
                return localName;
            }
            else if (string.IsNullOrEmpty(localName))
            {
                return parentName;
            }
            else
            {
                return $"{parentName}{nameSeparator}{localName}";
            }
        }
    }
}
