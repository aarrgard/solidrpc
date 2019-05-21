using System.Collections.Generic;
using System.Linq;

namespace SolidRpc.Swagger.Generator.Code.CSharp
{
    public abstract class ClassOrInterface : IMember, IQualifiedMember
    {
        public abstract string Name { get; }
        public abstract IEnumerable<IMember> Members { get; }
        public abstract INamespace Namespace { get; }
        public bool IsInterface => this is IInterface;

        public string FullName
        {
            get
            {
                return new QualifiedName(Namespace.FullName, Name).ToString();
            }
        }

        public void WriteCode(ICodeWriter codeWriter)
        {
            var fileName = new QualifiedName(Namespace.Name, Name).QName.Replace('.', '/') + ".cs";
            codeWriter.MoveToFile(fileName);
            if (!string.IsNullOrEmpty(Namespace.Name))
            {
                codeWriter.Emit($"namespace {Namespace.Name} {{{codeWriter.NewLine}");
                codeWriter.Indent();
            }
            var structType = IsInterface ? "interface" : "class";
            codeWriter.Emit($"public {structType} {Name} {{{codeWriter.NewLine}");
            Members.ToList().ForEach(o =>
            {
                codeWriter.Indent();
                o.WriteCode(codeWriter);
                codeWriter.Unindent();
            });
            codeWriter.Emit($"}}{codeWriter.NewLine}");

            if (!string.IsNullOrEmpty(Namespace.Name))
            {
                codeWriter.Unindent();
                codeWriter.Emit($"}}{codeWriter.NewLine}");
            }
        }
    }
}