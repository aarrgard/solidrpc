using System.Collections.Generic;
using System.Linq;

namespace SolidRpc.Swagger.Generator.Code.CSharp
{
    public abstract class ClassOrInterface : Member, IQualifiedMember
    {
        public ClassOrInterface(IQualifiedMember parent) : base(parent)
        {
        }

        public bool IsInterface => this is IInterface;

        public string FullName
        {
            get
            {
                return new QualifiedName(Namespace, Name).ToString();
            }
        }

        public string Namespace
        {
            get
            {
                return ((IQualifiedMember)Parent).FullName;
            }
        }

        public string Summary { get; set; }

        public override void WriteCode(ICodeWriter codeWriter)
        {
            codeWriter.MoveToClassFile(FullName);
            Members.OfType<IUsing>().ToList().ForEach(o =>
            {
                o.WriteCode(codeWriter);
            });

            if (!string.IsNullOrEmpty(Namespace))
            {
                codeWriter.Emit($"namespace {Namespace} {{{codeWriter.NewLine}");
                codeWriter.Indent();
            }
            var structType = IsInterface ? "interface" : "class";

            codeWriter.Emit($"/// <summary>{codeWriter.NewLine}");
            codeWriter.Emit($"/// {Summary}{codeWriter.NewLine}");
            codeWriter.Emit($"/// </summary>{codeWriter.NewLine}");
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

            if (!string.IsNullOrEmpty(Namespace))
            {
                codeWriter.Unindent();
                codeWriter.Emit($"}}{codeWriter.NewLine}");
            }
        }
    }
}