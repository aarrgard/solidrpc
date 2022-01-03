using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SolidRpc.OpenApi.Generator.Types;
using SolidRpc.OpenApi.Model.CSharp;
using SolidRpc.OpenApi.Model.CSharp.Impl;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SolidRpc.OpenApi.Generator.Impl
{
    /// <summary>
    /// Class that transforms a csharp project to a charp repo.
    /// </summary>
    public class CSharpParser
    {
        private enum NameScope { Namespace, Class, Interface, Enum };

        public static ICSharpRepository ParseProject(Project project)
        {
            var cSharpRepository = new CSharpRepository();
            var parser = new CSharpParser(cSharpRepository);
            parser.ParseProjectInternal(project);
            return cSharpRepository;
        }

        public CSharpParser(ICSharpRepository cSharpRepository)
        {
            CSharpRepository = cSharpRepository;
            CompilationUnits = new ConcurrentDictionary<string, CompilationUnitSyntax>();
        }

        private ConcurrentDictionary<string, CompilationUnitSyntax> CompilationUnits { get; }

        private ICSharpRepository CSharpRepository { get; }

        private ICSharpRepository ParseProjectInternal(Project project)
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

            return CSharpRepository;
        }

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
            if (member is EnumDeclarationSyntax eds)
            {
                WalkMembers(eds.Members, action);
                return;
            }
            if (member is EnumMemberDeclarationSyntax emds)
            {
                return;
            }

            // skip this member
            //throw new Exception("Cannot handle member:" + member.GetType().FullName);
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
            if (member is EnumDeclarationSyntax eds)
            {
                var (className, nameScope) = GetClassOrInterfaceName(eds);
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
            try
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
                if (member is EnumMemberDeclarationSyntax emds)
                {
                    CreateCSharpEnumValue(emds);
                    return;
                }
            } 
            catch(Exception e)
            {
                // eat it or throw it?
                // throw e
                Console.WriteLine(e.Message);
            }
        }

        private void CreateCSharpEnumValue(EnumMemberDeclarationSyntax emds)
        {
            var (className, nameScope) = GetClassOrInterfaceName(emds);
            var m = GetMember(className, nameScope);
            int? value = null;
            var strValue = emds.EqualsValue?.Value?.ToString();
            if(!string.IsNullOrEmpty(strValue))
            {
                value = int.Parse(strValue);
            }
            m.AddMember(new CSharpEnumValue((ICSharpEnum)m, emds.Identifier.ToString(), value));
        }

        private void CreateCSharpProperty(PropertyDeclarationSyntax pds)
        {
            var (className, nameScope) = GetClassOrInterfaceName(pds);
            var m = GetMember(className, nameScope);

            var propertyTypeName = GetFullName(pds, pds.Type.ToString());
            var propertyType = CSharpRepository.GetType(propertyTypeName);
            if (propertyType == null)
            {
                throw new Exception("Failed to find the property type " + propertyTypeName);
            }
            var propertyName = pds.Identifier.ToString();

            var property = new CSharpProperty(m, propertyName, propertyType);
            SetComment(pds, property);
            m.AddMember(property);
            AddAttributes(pds, pds.AttributeLists, property);
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
                throw new Exception("Failed to find method return type " + returnTypeName);
            }

            var methodName = mds.Identifier.ToString();

            var method = new CSharpMethod(m, methodName, returnType);
            mds.ParameterList.Parameters.ToList().ForEach(o =>
            {
                var parameterTypeName = GetFullName(o, o.Type.ToString());
                var parameterType = CSharpRepository.GetType(parameterTypeName);
                if (parameterType == null)
                {
                    throw new Exception("Failed to find the parameter type " + parameterTypeName);
                }

                var parameterName = o.Identifier.ToString();
                var optional = o.Default != null;
                var mp = new CSharpMethodParameter(method, parameterName, parameterType, optional);
                method.AddMember(mp);
                AddAttributes(o, o.AttributeLists, mp);
            });

            SetComment(mds, method);
            m.AddMember(method);
            AddAttributes(mds, mds.AttributeLists, method);
        }

        private void AddAttributes(SyntaxNode sn, SyntaxList<AttributeListSyntax> lst, ICSharpMember mp)
        {
            foreach (var a in lst.SelectMany(x => x.Attributes))
            {
                var attrName = a.Name.ToString();
                if (!attrName.EndsWith("Attribute"))
                {
                    attrName = $"{attrName}Attribute";
                }
                attrName = GetFullName(sn, attrName);

                var namedArgs = new Dictionary<string, object>();
                IEnumerable<object> args = new object[0];
                var ala = a.ArgumentList?.Arguments;
                if(ala != null)
                {
                    ala.Value.Where(x => x.NameEquals != null).ToList().ForEach(x => namedArgs[x.NameEquals.Name.ToString()] = CreateValue(x.Expression));
                    args = ala.Value.Where(x => x.NameEquals == null).Select(x => CreateValue(x.Expression)).ToList();
                }

                mp.AddMember(new CSharpAttribute(mp, attrName, args, namedArgs));
            }
        }

        private object CreateValue(SyntaxNode expression)
        {
            switch(expression.Kind())
            {
                case SyntaxKind.StringLiteralExpression:
                    var s = expression.ToString();
                    return s.Substring(1, s.Length - 2);
                case SyntaxKind.TrueLiteralExpression:
                case SyntaxKind.FalseLiteralExpression:
                    return expression.ToString();
                case SyntaxKind.ImplicitArrayCreationExpression:
                    var x = (ImplicitArrayCreationExpressionSyntax)expression;
                    return x.ChildNodes().Select(o => (string[])CreateValue(o)).SelectMany(o => o).ToArray();
                case SyntaxKind.ArrayInitializerExpression:
                    var i = (InitializerExpressionSyntax)expression;
                    return i.ChildNodes().Select(o => (string)CreateValue(o)).ToArray();
                case SyntaxKind.InvocationExpression:
                    var expr = (InvocationExpressionSyntax)expression;
                    if(expr.Expression.ToString() == "nameof")
                    {
                        return expr.ArgumentList.Arguments[0].ToString();
                    }
                    throw new ArgumentException("Cannot create value from:" + expression.ToString());
                default:
                    throw new ArgumentException("Cannot create value from:" + expression.ToString());
            }
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
            else if (nameScope == NameScope.Enum)
            {
                return CSharpRepository.GetEnum(className);
            }
            else
            {
                throw new Exception("Cannot handle scope:" + nameScope);
            }
        }

        private string GetFullName(SyntaxNode member, string typeName)
        {
            // handle nullable types
            if (typeName.EndsWith("?"))
            {
                typeName = $"System.Nullable<{typeName.Substring(0, typeName.Length-1)}>";
            }
            // handle arrays
            if(typeName.EndsWith("[]"))
            {
                return GetFullName(member, typeName.Substring(0, typeName.Length - 2)) + "[]";
            }
            // handle generic types
            var (genType, genArgs) = Model.CSharp.Impl.CSharpRepository.ReadType(typeName);
            if (genArgs != null)
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

            } while (ns.LastIndexOf('.') > -1);

            // use the "usings"
            foreach (var u in usings)
            {
                var alias = u.Value;
                if(typeName.StartsWith(alias))
                {
                    string fullName;
                    if(string.IsNullOrEmpty(alias))
                    {
                        fullName = MergeNames(u.Key, ".", typeName);
                    }
                    else
                    {
                        fullName = MergeNames(u.Key, ".", typeName.Substring(alias.Length+1));
                    }
                    type = CSharpRepository.GetType(fullName);
                    if (type != null)
                    {
                        return type.FullName;
                    }
                }
            }
            type = CSharpRepository.GetType(typeName);
            if (type != null)
            {
                return type.FullName;
            }
            throw new Exception($"Cannot find type {typeName}");
        }

        private (IDictionary<string, string>, string) GetPrefixes(SyntaxNode member)
        {
            if (member == null)
            {
                return (new Dictionary<string, string>(), "");
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
                var key = u.Name.ToString();
                var alias = u.Alias?.Name?.ToString() ?? "";
                if(usings.TryGetValue(key, out string oldAlias))
                {
                    if(!string.IsNullOrEmpty(oldAlias))
                    {
                        usings[key] = alias;
                    }
                }
                else
                {
                    usings[key] = alias;
                }
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
            else if (member is EnumDeclarationSyntax eds)
            {
                localName = eds.Identifier.ToString();
                nameScope = NameScope.Enum;
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
