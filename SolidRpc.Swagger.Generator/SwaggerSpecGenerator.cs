using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SolidRpc.Swagger.Generator.Model.CSharp;
using SolidRpc.Swagger.Generator.Model.CSharp.Impl;
using SolidRpc.Swagger.Generator.V2;
using System;
using System.Collections.Generic;
using System.IO;

namespace SolidRpc.Swagger.Generator
{
    /// <summary>
    /// Base class for generating a swagger specification.
    /// </summary>
    public abstract class SwaggerSpecGenerator
    {
        private enum NameScope { Namespace, Class, Interface };
        public static void GenerateCode(SwaggerSpecSettings settings)
        {
            SwaggerSpecGenerator generator;
            switch (settings.SwaggerVersion)
            {
                case "2.0":
                    generator = new SwaggerSpecGeneratorV2(settings);
                    break;
                default:
                    throw new Exception("Cannot handle swagger version:" + settings.SwaggerVersion);
            }
            generator.GenerateSpec();
        }

        public SwaggerSpecGenerator(SwaggerSpecSettings settings)
        {
            Settings = settings;
            CSharpRepository = new CSharpRepository();
        }

        public SwaggerSpecSettings Settings { get; }

        public ICSharpRepository CSharpRepository { get; }

        protected virtual void GenerateSpec()
        {
            // find all .cs file
            var di = new DirectoryInfo(Settings.CodePath);
            foreach(var csFile in di.EnumerateFiles("*.cs", SearchOption.AllDirectories))
            {
                ProcessCsFile(csFile);
            }
        }

        private void ProcessCsFile(FileInfo csFile)
        {
            using (var tr = csFile.OpenText())
            {
                var syntaxTree = CSharpSyntaxTree.ParseText(tr.ReadToEnd());
                var cu = (CompilationUnitSyntax)syntaxTree.GetRoot();
                WalkMembers(cu.Members);
            }
        }

        private void WalkMembers(IEnumerable<MemberDeclarationSyntax> members)
        {
            foreach (var member in members)
            {
                WalkMember(member);
            }
        }

        private void WalkMember(MemberDeclarationSyntax member)
        {
            if (member is NamespaceDeclarationSyntax nds)
            {
                WalkMembers(nds.Members);
                return;
            }
            if (member is InterfaceDeclarationSyntax ids)
            {
                WalkMembers(ids.Members);
                return;
            }
            if (member is ClassDeclarationSyntax cds)
            {
                // handle the class
                WalkMembers(cds.Members);
                return;
            }
            if (member is MethodDeclarationSyntax mds)
            {
                CreateCSharpMethod(mds);
                return;
            }
            if (member is PropertyDeclarationSyntax pds)
            {
                var (className, nameScope) = GetClassOrInterfaceName(pds);
                var m = GetMember(className, nameScope);
                return;
            }

            throw new Exception("Cannot handle member:" + member.GetType().FullName);
        }

        private void CreateCSharpMethod(MethodDeclarationSyntax mds)
        {
            var (className, nameScope) = GetClassOrInterfaceName(mds);
            var m = GetMember(className, nameScope);

            var methodName = mds.Identifier.ToString();
            var returnType = mds.ReturnType.ToString();

            var method = new CSharpMethod(m, methodName);
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
                nameScope = NameScope.Interface;
            }
            if (string.IsNullOrEmpty(parentName))
            {
                return (localName, nameScope);
            }
            else if(string.IsNullOrEmpty(localName))
            {
                return (parentName, nameScope);
            }
            else
            {
                return ($"{parentName}{nameSeparator}{localName}", nameScope);
            }
        }
    }
}
