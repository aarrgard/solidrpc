using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace SolidRpc.OpenApi.Model.CodeDoc.Impl
{
    /// <summary>
    /// Represents the method comment.
    /// </summary>
    public class CodeDocMethod : CodeDocMember, ICodeDocMethod
    {
        /// <summary>
        /// Constructs a new instance
        /// </summary>
        /// <param name="classDocumentation"></param>
        /// <param name="nameAttr"></param>
        /// <param name="xmlDocument"></param>
        public CodeDocMethod(CodeDocClass classDocumentation, string nameAttr)
        {
            ClassDocumentation = classDocumentation ?? throw new ArgumentNullException(nameof(classDocumentation));
            MethodName = GetMethodName(nameAttr) ?? throw new ArgumentNullException(nameof(nameAttr));
            var methodNode = XmlDocument.SelectSingleNode($"/doc/members/member[@name='{nameAttr}']");
            if(methodNode == null)
            {
                Comment = "";
                ParameterDocumentation = new ICodeDocParameter[0];
                ExceptionDocumentation = new ICodeDocException[0];
                return;
            }
            Comment = SelectSingleNode(methodNode, "summary");
            ParameterDocumentation = SelectXmlElements(methodNode, "param")
                .Select(o => new CodeDocParameter(this, o.Attributes["name"]?.InnerText, o.InnerText))
                .ToList();
            ExceptionDocumentation = SelectXmlElements(methodNode, "exception")
                .Select(o => new CodeDocException(this, o.Attributes["cref"]?.InnerText, o.InnerText))
                .ToList();
        }

        private CodeDocClass ClassDocumentation { get; }
        ICodeDocClass ICodeDocMethod.ClassDocumentation => ClassDocumentation;

        /// <summary>
        /// The underlying xml document
        /// </summary>
        public XmlDocument XmlDocument => ClassDocumentation.XmlDocument;

        /// <summary>
        /// The method name
        /// </summary>
        public string MethodName { get; }

        /// <summary>
        /// The comment
        /// </summary>
        public string Comment { get; }

        /// <summary>
        /// The parameter definitions
        /// </summary>
        public IEnumerable<ICodeDocParameter> ParameterDocumentation { get; }

        /// <summary>
        /// The exceptions
        /// </summary>
        public IEnumerable<ICodeDocException> ExceptionDocumentation { get; }
    }
}