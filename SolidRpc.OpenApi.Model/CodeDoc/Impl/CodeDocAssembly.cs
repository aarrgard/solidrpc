using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace SolidRpc.OpenApi.Model.CodeDoc.Impl
{
    public class CodeDocAssembly : CodeDocMember, ICodeDocAssembly
    {
        public CodeDocAssembly(XmlDocument xmlDocument)
        {
            Name = SelectSingleNode(xmlDocument, "/doc/assembly/name");
            var classNames = SelectXmlElements(xmlDocument, "/doc/members/member")
                .Select(o => GetClassName(o.Attributes["name"].InnerText))
                .Distinct()
                .ToList();
            ClassDoumentation = classNames.Select(o => new CodeDocClass(this, o, xmlDocument)).ToList();
        }

        public ICodeDocClass GetClassDocumentation(Type type)
        {
            return ClassDoumentation.Where(o => o.ClassName == type.FullName)
                .FirstOrDefault();
        }

        public string Name { get; }

        public IEnumerable<ICodeDocClass> ClassDoumentation { get; }
    }
}