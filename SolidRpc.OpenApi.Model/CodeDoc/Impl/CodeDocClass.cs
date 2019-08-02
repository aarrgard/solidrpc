using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml;

namespace SolidRpc.OpenApi.Model.CodeDoc.Impl
{
    /// <summary>
    /// Represents the documentation for a class.
    /// </summary>
    public class CodeDocClass : CodeDocMember, ICodeDocClass
    {

        /// <summary>
        /// Constructs a new instance
        /// </summary>
        /// <param name="assemblyDocumentation"></param>
        /// <param name="className"></param>
        public CodeDocClass(CodeDocAssembly assemblyDocumentation, string className)
        {
            AssemblyDocumentation = assemblyDocumentation;
            ClassName = className;
            MethodDocumentation = SelectXmlElements(XmlDocument, "/doc/members/member")
                .Where(o => GetClassName(o.Attributes["name"].InnerText) == ClassName)
                .Where(o => GetMethodName(o.Attributes["name"].InnerText) != null)
                .Select(o => new CodeDocMethod(this, o.Attributes["name"].InnerText))
                .ToList();
            PropertyDocumentation = SelectXmlElements(XmlDocument, "/doc/members/member")
                .Where(o => GetClassName(o.Attributes["name"].InnerText) == ClassName)
                .Where(o => GetPropertyName(o.Attributes["name"].InnerText) != null)
                .Select(o => new CodeDocProperty(this, o.Attributes["name"].InnerText))
                .ToList();
        }

        /// <summary>
        /// The underlying xml document
        /// </summary>
        public XmlDocument XmlDocument => AssemblyDocumentation.XmlDocument;

        private CodeDocAssembly AssemblyDocumentation { get; }
        ICodeDocAssembly ICodeDocClass.AssemblyDocumentation => AssemblyDocumentation;

        /// <summary>
        /// The class name.
        /// </summary>
        public string ClassName { get; }

        /// <summary>
        /// All the method documentations.
        /// </summary>
        public IEnumerable<ICodeDocMethod> MethodDocumentation { get; }

        /// <summary>
        /// All the property documentations
        /// </summary>
        public IEnumerable<ICodeDocProperty> PropertyDocumentation { get; }


        /// <summary>
        /// Returns the documentation for supplied method
        /// </summary>
        /// <param name="methodInfo"></param>
        /// <returns></returns>
        public ICodeDocMethod GetMethodDocumentation(MethodInfo methodInfo)
        {
            var methodDoc = MethodDocumentation
                .Where(o => o.MethodName == methodInfo.Name)
                .FirstOrDefault();

            if(methodDoc == null)
            {
                methodDoc = new CodeDocMethod(this, $"M:{ClassName}.{methodInfo.Name}");
            }
            return methodDoc;
        }

    }
}