using System;
using System.Xml;

namespace SolidRpc.OpenApi.Model.CodeDoc.Impl
{
    public class CodeDocProperty : CodeDocMember, ICodeDocProperty
    {

        public CodeDocProperty(CodeDocClass codeDocClass, string nameAttr, XmlDocument xmlDocument)
        {
            CodeDocClass = codeDocClass;
            Name = GetPropertyName(nameAttr);
        }

        public CodeDocClass CodeDocClass { get; }

        public string Name { get; }

        public string Comment => throw new System.NotImplementedException();
    }
}