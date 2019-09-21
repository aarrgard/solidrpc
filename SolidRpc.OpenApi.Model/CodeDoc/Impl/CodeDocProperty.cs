using System;
using System.Text;
using System.Xml;

namespace SolidRpc.OpenApi.Model.CodeDoc.Impl
{
    /// <summary>
    /// Represents a property description
    /// </summary>
    public class CodeDocProperty : CodeDocMember, ICodeDocProperty
    {
        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="classDocumentation"></param>
        /// <param name="nameAttr"></param>
        public CodeDocProperty(CodeDocClass classDocumentation, string nameAttr)
        {
            ClassDocumentation = classDocumentation ?? throw new ArgumentNullException(nameof(classDocumentation));
            Name = GetPropertyName(nameAttr);
            var methodNode = XmlDocument.SelectSingleNode($"/doc/members/member[@name='{nameAttr}']");
            if (methodNode == null)
            {
                Summary = "";
                return;
            }
            Summary = SelectSingleNode(methodNode, "summary");
        }

        private CodeDocClass ClassDocumentation { get; }
        ICodeDocClass ICodeDocProperty.ClassDocumentation => ClassDocumentation;

        /// <summary>
        /// The underlying xml document
        /// </summary>
        public XmlDocument XmlDocument => ClassDocumentation.XmlDocument;

        /// <summary>
        /// The name of the property
        /// </summary>
        public string Name { get; }

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
                return sb.ToString();
            }
        }

        public string Summary { get; private set; }
    }
}