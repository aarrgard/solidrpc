using System;
using System.Linq;

namespace SolidRpc.OpenApi.Model.CSharp.Impl
{
    public abstract class CSharpType : CSharpMember, ICSharpType
    {
        public CSharpType(ICSharpNamespace ns, string name, Type runtimeType) : base(ns, name)
        {
            RuntimeType = runtimeType;
        }

        public Type RuntimeType { get; }
        public bool Initialized { get; set; }

        public bool IsFileType
        {
            get
            {
                var runtimeProps = Properties.ToDictionary(o => o.Name, o => o.PropertyType.RuntimeType);
                return TypeExtensions.IsFileType(FullName, runtimeProps);
            }
        }

        public bool IsGenericType => Name.Contains('<');

        public void AddExtends(ICSharpType extType)
        {
            if(Members.OfType<ICSharpTypeExtends>().Where(o => o.Name == extType.FullName).Any())
            {
                return;
            }
            ProtectedMembers.Add(new CSharpTypeExtends(this, extType));
        }

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

            WriteSummary(codeWriter);
            codeWriter.Emit($"public {structType} {Name}");
            if(Members.OfType<ICSharpTypeExtends>().Any())
            {
                codeWriter.Emit($" : {string.Join(",", Members.OfType<ICSharpTypeExtends>().Select(o => o.Name))}");
            }
            codeWriter.Emit($" {{{codeWriter.NewLine}");
            Members.Where(o => o is ICSharpMethod ||
                        o is ICSharpProperty ||
                        o is ICSharpConstructor)
                .ToList()
                .ForEach(o =>
                {
                    codeWriter.Indent();
                    o.WriteCode(codeWriter);
                    codeWriter.Unindent();
                    codeWriter.Emit(codeWriter.NewLine);
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