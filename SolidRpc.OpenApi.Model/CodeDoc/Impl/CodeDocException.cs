using System;

namespace SolidRpc.OpenApi.Model.CodeDoc.Impl
{
    public class CodeDocException : CodeDocMember, ICodeDocException
    {
        public CodeDocException(CodeDocMethod methodDocumentation, string cref, string comment)
        {
            MethodDocumentation = methodDocumentation ?? throw new ArgumentNullException(nameof(methodDocumentation));
            ExceptionType = GetClassName(cref) ?? throw new ArgumentNullException(nameof(cref));
            Comment = comment ?? throw new ArgumentNullException(nameof(comment));
        }

        public ICodeDocMethod MethodDocumentation { get; }

        public string ExceptionType { get; }

        public string Comment { get; }

    }
}