using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml;

namespace SolidRpc.OpenApi.Model.CodeDoc.Impl
{
    public class CodeDocClass : CodeDocMember, ICodeDocClass
    {

        public CodeDocClass(ICodeDocAssembly assemblyDocumentation, string className, XmlDocument xmlDocument)
        {
            AssemblyDocumentation = assemblyDocumentation;
            ClassName = className;
            MethodDocumentation = SelectXmlElements(xmlDocument, "/doc/members/member")
                .Where(o => GetClassName(o.Attributes["name"].InnerText) == ClassName)
                .Where(o => GetMethodName(o.Attributes["name"].InnerText) != null)
                .Select(o => new CodeDocMethod(this, o.Attributes["name"].InnerText, xmlDocument))
                .ToList();
            PropertyDocumentation = SelectXmlElements(xmlDocument, "/doc/members/member")
                .Where(o => GetClassName(o.Attributes["name"].InnerText) == ClassName)
                .Where(o => GetPropertyName(o.Attributes["name"].InnerText) != null)
                .Select(o => new CodeDocProperty(this, o.Attributes["name"].InnerText, xmlDocument))
                .ToList();
        }

        public ICodeDocAssembly AssemblyDocumentation { get; }

        public string ClassName { get; }

        public IEnumerable<ICodeDocMethod> MethodDocumentation { get; }

        public IEnumerable<ICodeDocProperty> PropertyDocumentation { get; }

        public ICodeDocMethod GetMethodDocumentation(MethodInfo methodInfo)
        {
            return MethodDocumentation.Where(o => o.MethodName == methodInfo.Name)
                .FirstOrDefault();
        }
    }
}