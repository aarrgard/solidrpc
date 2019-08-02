using System;
using System.Xml;

namespace SolidRpc.OpenApi.Model.CodeDoc.Impl
{
    public class CodeDocProperty : CodeDocMember, ICodeDocProperty
    {

        public CodeDocProperty(CodeDocClass codeDocClass, string nameAttr)
        {
            CodeDocClass = codeDocClass;
            Name = GetPropertyName(nameAttr);
        }

        private CodeDocClass CodeDocClass { get; }

        /// <summary>
        /// The name of the property
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// The comment
        /// </summary>
        public string Comment => throw new System.NotImplementedException();
    }
}