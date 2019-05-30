using System;
using System.Linq;

namespace SolidRpc.Swagger.Generator.Model.CSharp.Impl
{
    public abstract class CSharpType : CSharpMember, ICSharpType
    {
        public CSharpType(ICSharpNamespace ns, string name, Type runtimeType) : base(ns, name)
        {
            RuntimeType = runtimeType;
        }

        public Type RuntimeType { get; }

        public override void AddMember(ICSharpMember member)
        {
            ProtectedMembers.Add(member);
        }

        public override void WriteCode(ICodeWriter codeWriter)
        {
            codeWriter.MoveToClassFile(FullName);
            Members.OfType<ICSharpUsing>().ToList().ForEach(o =>
            {
                o.WriteCode(codeWriter);
            });

            if (!string.IsNullOrEmpty(Namespace.FullName))
            {
                codeWriter.Emit($"namespace {Namespace.FullName} {{{codeWriter.NewLine}");
                codeWriter.Indent();
            }
            var structType = (this is CSharpInterface) ? "interface" : "class";

            codeWriter.Emit($"/// <summary>{codeWriter.NewLine}");
            codeWriter.Emit($"/// {Comment?.Summary}{codeWriter.NewLine}");
            codeWriter.Emit($"/// </summary>{codeWriter.NewLine}");
            codeWriter.Emit($"public {structType} {Name} {{{codeWriter.NewLine}");
            Members.ToList().ForEach(o =>
            {
                if (o is ICSharpMethod || o is ICSharpProperty)
                {
                    codeWriter.Indent();
                    o.WriteCode(codeWriter);
                    codeWriter.Unindent();
                    codeWriter.Emit(codeWriter.NewLine);
                }
            });
            codeWriter.Emit($"}}{codeWriter.NewLine}");

            if (!string.IsNullOrEmpty(Namespace.FullName))
            {
                codeWriter.Unindent();
                codeWriter.Emit($"}}{codeWriter.NewLine}");
            }
        }
    }
}