using System;

namespace SolidRpc.OpenApi.Model.CodeDoc.Impl
{
    public class CodeDocParameter : CodeDocMember, ICodeDocParameter
    {
        public CodeDocParameter(CodeDocMethod methodDocumentation, string name, string comment)
        {
            MethodDocumentation = methodDocumentation ?? throw new ArgumentNullException(nameof(methodDocumentation));
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Comment = comment ?? throw new ArgumentNullException(nameof(comment));
        }

        public ICodeDocMethod MethodDocumentation { get; }

        public string Name { get; }

        public string Comment { get; }
    }
}