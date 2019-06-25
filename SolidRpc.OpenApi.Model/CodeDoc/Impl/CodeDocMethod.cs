using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace SolidRpc.OpenApi.Model.CodeDoc.Impl
{
    public class CodeDocMethod : CodeDocMember, ICodeDocMethod
    {
        public CodeDocMethod(CodeDocClass classDocumentation, string nameAttr, XmlDocument xmlDocument)
        {
            ClassDocumentation = classDocumentation ?? throw new ArgumentNullException(nameof(classDocumentation));
            MethodName = GetMethodName(nameAttr) ?? throw new ArgumentNullException(nameof(nameAttr));
            var methodNode = xmlDocument.SelectSingleNode($"/doc/members/member[@name='{nameAttr}']");
            Comment = SelectSingleNode(methodNode, "summary");
            ParameterDocumentation = SelectXmlElements(methodNode, "param")
                .Select(o => new CodeDocParameter(this, o.Attributes["name"]?.InnerText, o.InnerText))
                .ToList();
            ExceptionDocumentation = SelectXmlElements(methodNode, "exception")
                .Select(o => new CodeDocException(this, o.Attributes["cref"]?.InnerText, o.InnerText))
                .ToList();
        }

        public ICodeDocClass ClassDocumentation { get; }

        public string MethodName { get; }

        public string Comment { get; }

        public IEnumerable<ICodeDocParameter> ParameterDocumentation { get; }

        public IEnumerable<ICodeDocException> ExceptionDocumentation { get; }
    }
}