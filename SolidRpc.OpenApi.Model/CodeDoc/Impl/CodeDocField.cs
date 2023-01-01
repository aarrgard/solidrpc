using System;
using System.Text;

namespace SolidRpc.OpenApi.Model.CodeDoc.Impl
{
    /// <summary>
    /// Represents a field in the documentation
    /// </summary>
    public class CodeDocField : CodeDocMember, ICodeDocField
    {
        /// <summary>
        /// Constructs a new instance
        /// </summary>
        /// <param name="classDocumentation"></param>
        /// <param name="sunmmary"></param>
        public CodeDocField(CodeDocClass classDocumentation, string sunmmary)
        {
            ClassDocumentation = classDocumentation ?? throw new ArgumentNullException(nameof(classDocumentation));
            Summary = sunmmary ?? throw new ArgumentNullException(nameof(sunmmary));
        }

        /// <summary>
        /// The class documentation
        /// </summary>
        public ICodeDocClass ClassDocumentation { get; }

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