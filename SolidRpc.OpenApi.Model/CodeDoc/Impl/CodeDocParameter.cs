using System;

namespace SolidRpc.OpenApi.Model.CodeDoc.Impl
{
    /// <summary>
    /// Implements the logic for a parameter documentation.
    /// </summary>
    public class CodeDocParameter : CodeDocMember, ICodeDocParameter
    {
        /// <summary>
        /// Constructs a new instance.
        /// </summary>
        /// <param name="methodDocumentation"></param>
        /// <param name="name"></param>
        /// <param name="summary"></param>
        public CodeDocParameter(CodeDocMethod methodDocumentation, string name, string summary)
        {
            MethodDocumentation = methodDocumentation ?? throw new ArgumentNullException(nameof(methodDocumentation));
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Summary = summary ?? throw new ArgumentNullException(nameof(summary));
        }

        /// <summary>
        /// The method that the parameter belongs to
        /// </summary>
        public ICodeDocMethod MethodDocumentation { get; }

        /// <summary>
        /// The name of the parameter
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// The summary
        /// </summary>
        public string Summary { get; }
    }
}