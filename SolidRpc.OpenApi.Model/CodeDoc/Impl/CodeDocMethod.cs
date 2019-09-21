using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        public CodeDocMethod(CodeDocClass classDocumentation, string nameAttr)
        {
            ClassDocumentation = classDocumentation ?? throw new ArgumentNullException(nameof(classDocumentation));
            MethodName = GetMethodName(nameAttr) ?? throw new ArgumentNullException(nameof(nameAttr));
            var methodNode = XmlDocument.SelectSingleNode($"/doc/members/member[@name='{nameAttr}']");
            if(methodNode == null)
            {
                Summary = "";
                ParameterDocumentation = new ICodeDocParameter[0];
                ExceptionDocumentation = new ICodeDocException[0];
                return;
            }
            Summary = SelectSingleNode(methodNode, "summary");
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
        public string Summary { get; }

        /// <summary>
        /// The parameter definitions
        /// </summary>
        public IEnumerable<ICodeDocParameter> ParameterDocumentation { get; }

        /// <summary>
        /// The exceptions
        /// </summary>
        public IEnumerable<ICodeDocException> ExceptionDocumentation { get; }

        /// <summary>
        /// Returns the code comments
        /// </summary>'
        public string CodeComments
        {
            get
            {
                var sb = new StringBuilder();
                sb.AppendLine("<summary>");
                sb.AppendLine(Summary);
                sb.AppendLine("</summary>");
                ParameterDocumentation.ToList().ForEach(o => sb.Append("<param name=\"").Append(o.Name).Append("\">").Append(o.Summary).Append("</param>"));
                ExceptionDocumentation.ToList().ForEach(o => sb.Append("<exception cref=\"").Append(o.ExceptionType).Append("\">").Append(o.Comment).Append("</exception>"));
                return sb.ToString();
            }
        }

        public ICodeDocParameter GetParameterDocumentation(string name)
        {
            return ParameterDocumentation.FirstOrDefault(o => o.Name == name);
        }
    }
}