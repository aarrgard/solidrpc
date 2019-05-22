using System.Collections.Generic;
using System.Linq;

namespace SolidRpc.Swagger.Generator.Code.CSharp
{
    public abstract class ClassOrInterface : Member, IQualifiedMember
    {
        public ClassOrInterface(IMember parent) : base(parent)
        {
        }

        public abstract INamespace Namespace { get; }
        public bool IsInterface => this is IInterface;

        public string FullName
        {
            get
            {
                return new QualifiedName(Namespace.FullName, Name).ToString();
            }
        }

        public override void WriteCode(ICodeWriter codeWriter)
        {
            var fileName = new QualifiedName(Namespace.FullName, Name).QName.Replace('.', '/') + ".cs";
            codeWriter.MoveToFile(fileName);
            Members.OfType<IUsing>().ToList().ForEach(o =>
            {
                o.WriteCode(codeWriter);
            });

            if (!string.IsNullOrEmpty(Namespace.Name))
            {
                codeWriter.Emit($"namespace {Namespace.FullName} {{{codeWriter.NewLine}");
                codeWriter.Indent();
            }
            var structType = IsInterface ? "interface" : "class";
            codeWriter.Emit($"public {structType} {Name} {{{codeWriter.NewLine}");
            Members.ToList().ForEach(o =>
            {
                if(o is IMethod || o is IProperty) 
                {
                    codeWriter.Indent();
                    o.WriteCode(codeWriter);
                    codeWriter.Unindent();
                    codeWriter.Emit(codeWriter.NewLine);
                }
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