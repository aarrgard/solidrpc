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
        /// <param name="methodName"></param>
        public CodeDocMethod(
            CodeDocClass classDocumentation,
            string methodName
            )
        {
            ClassDocumentation = classDocumentation ?? throw new ArgumentNullException(nameof(classDocumentation));
            MethodName = methodName ?? throw new ArgumentNullException(nameof(methodName));
            Summary = "";
            ParameterDocumentation = new List<ICodeDocParameter>();
            ExceptionDocumentation = new List<ICodeDocException>();
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
        public string Summary { get; private set;  }

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