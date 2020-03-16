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
        /// <param name="propertyName"></param>
        public CodeDocProperty(CodeDocClass classDocumentation, string propertyName)
        {
            ClassDocumentation = classDocumentation ?? throw new ArgumentNullException(nameof(classDocumentation));
            Name = propertyName ?? throw new ArgumentNullException(nameof(propertyName));
            Summary = "";
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

        /// <summary>
        /// The summary
        /// </summary>
        public string Summary { get; private set; }
    }
}