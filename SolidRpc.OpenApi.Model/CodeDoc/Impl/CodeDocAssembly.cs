using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace SolidRpc.OpenApi.Model.CodeDoc.Impl
{
    /// <summary>
    /// Creates a wrapper for supplied documentation
    /// </summary>
    public class CodeDocAssembly : CodeDocMember, ICodeDocAssembly
    {
        /// <summary>
        /// Constrcuts a new instance
        /// </summary>
        /// <param name="xmlDocument"></param>
        public CodeDocAssembly(XmlDocument xmlDocument)
        {
            if(xmlDocument.SelectSingleNode("/doc/assembly/name") == null)
            {
                Name = "Unknown";
            }
            else
            {
                Name = SelectSingleNode(xmlDocument, "/doc/assembly/name");
            }
            XmlDocument = xmlDocument;
            var classNames = SelectXmlElements(xmlDocument, "/doc/members/member")
                .Select(o => GetClassName(o.Attributes["name"].InnerText))
                .Distinct()
                .ToList();
            ClassDoumentation = classNames.Select(o => new CodeDocClass(this, o)).ToList();
        }

        ICodeDocClass ICodeDocAssembly.GetClassDocumentation(Type type)
        {
            var className = type.FullName.Replace('+', '.');
            var doc = ClassDoumentation
                .Where(o => o.ClassName == className)
                .FirstOrDefault();

            if(doc == null)
            {
                doc = new CodeDocClass(this, className);
            }

            return doc;
        }

        /// <summary>
        /// The name of the assembly.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// The underlying xml document
        /// </summary>
        public XmlDocument XmlDocument { get; }

        /// <summary>
        /// The class documentation
        /// </summary>
        public IEnumerable<ICodeDocClass> ClassDoumentation { get; }
    }
}