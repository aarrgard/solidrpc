using System;
using System.Collections.Generic;
using System.Linq;

namespace SolidRpc.OpenApi.Model.CSharp.Impl
{
    public class CSharpMethod : CSharpMember, ICSharpMethod
    {
        public CSharpMethod(ICSharpMember parent, string name, ICSharpType returnType) : base(parent, name)
        {
            ReturnType = returnType ?? throw new ArgumentNullException(nameof(returnType));
        }

        public override void AddMember(ICSharpMember member)
        {
            ProtectedMembers.Add(member);
        }

        public ICSharpType ReturnType { get; }

        public IEnumerable<ICSharpMethodParameter> Parameters { get; set; }

        protected override void SetCommentParameter(string parameterName, string comment)
        {
            //  find the parameter
            Parameters.Where(o => o.Name == parameterName).ToList().ForEach(o =>
            {
                o.ParseComment($"<summary>{comment}</summary>");
            });
        }

        public override void WriteCode(ICodeWriter codeWriter)
        {
            var parameters = Members.OfType<ICSharpMethodParameter>().ToList();
            WriteSummary(codeWriter);
            parameters.ForEach(p =>
            {
                codeWriter.Emit($"/// <param name=\"{p.Name}\">{p.Comment?.Summary}</param>{codeWriter.NewLine}");
            });
            Comment.Exceptions.ToList().ForEach(e =>
            {
                codeWriter.Emit($"/// <exception cref=\"{SimplifyName(e.Cref)}\">{e.Description}</exception>{codeWriter.NewLine}");
            });
            codeWriter.Emit($"{SimplifyName(ReturnType.FullName)} {Name}(");
            for (int i = 0; i < parameters.Count; i++)
            {
                codeWriter.Emit(codeWriter.NewLine);
                codeWriter.Indent();
                parameters[i].WriteCode(codeWriter);
                if (i < parameters.Count - 1)
                {
                    codeWriter.Emit(",");
                }
                codeWriter.Unindent();
            }
            codeWriter.Emit($");{codeWriter.NewLine}");
        }

        public override void GetNamespaces(ICollection<string> namespaces)
        {
            namespaces.Add(ReturnType.Namespace.FullName);
            base.GetNamespaces(namespaces);
        }
    }
}