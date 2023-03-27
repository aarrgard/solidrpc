using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolidRpc.OpenApi.Model.CSharp.Impl
{
    /// <summary>
    /// Represents a C# method
    /// </summary>
    public class CSharpMethod : CSharpMember, ICSharpMethod
    {
        /// <summary>
        /// Constructs a new instance
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="name"></param>
        /// <param name="returnType"></param>
        public CSharpMethod(ICSharpMember parent, string name, ICSharpType returnType) : base(parent, name)
        {
            ReturnType = returnType ?? throw new ArgumentNullException(nameof(returnType));
        }

        /// <summary>
        /// Adds a member to this method
        /// </summary>
        /// <param name="member"></param>
        public override void AddMember(ICSharpMember member)
        {
            ProtectedMembers.Add(member);
        }

        /// <summary>
        /// The return type
        /// </summary>
        public ICSharpType ReturnType { get; }

        /// <summary>
        /// The parameters
        /// </summary>
        public IEnumerable<ICSharpMethodParameter> Parameters => ProtectedMembers.OfType<ICSharpMethodParameter>();

        /// <summary>
        /// Sets the comment parameter
        /// </summary>
        /// <param name="parameterName"></param>
        /// <param name="comment"></param>
        protected override void SetCommentParameter(string parameterName, string comment)
        {
            //  find the parameter
            Parameters.Where(o => o.Name == parameterName).ToList().ForEach(o =>
            {
                o.ParseComment($"<summary>{comment}</summary>");
            });
        }

        /// <summary>
        /// Writes the code.
        /// </summary>
        /// <param name="codeWriter"></param>
        public override void WriteCode(ICodeWriter codeWriter)
        {
            var parameters = Members.OfType<ICSharpMethodParameter>().ToList();
            WriteSummary(codeWriter);
            parameters.ForEach(p =>
            {
                codeWriter.Emit($"/// <param name=\"{p.Name}\">");
                codeWriter.Indent("///");
                codeWriter.Emit(p.Comment?.Summary);
                codeWriter.Unindent();
                codeWriter.Emit($"</param>{codeWriter.NewLine}");
            });
            Comment?.Exceptions.ToList().ForEach(e =>
            {
                codeWriter.Emit($"/// <exception cref=\"{e.Cref}\">{e.Description}</exception>{codeWriter.NewLine}");
            });
            Members.OfType<ICSharpAttribute>().ToList().ForEach(o => o.WriteCode(codeWriter));
            Members.OfType<ICSharpModifier>().ToList().ForEach(o => o.WriteCode(codeWriter));
            codeWriter.Emit($"{SimplifyName(ReturnType.FullName)} {Name}(");
            codeWriter.Emit($"");
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
            codeWriter.Emit($")");

            if(Body.Length == 0)
            {
                codeWriter.Emit($";{codeWriter.NewLine}");
            }
            else
            {
                codeWriter.Emit($" {{{codeWriter.NewLine}");
                codeWriter.Indent();
                codeWriter.Emit(Body.ToString().Trim());
                codeWriter.Emit($"{codeWriter.NewLine}");
                codeWriter.Unindent();
                codeWriter.Emit($"}}{codeWriter.NewLine}");
            }
        }

        /// <summary>
        /// Adds the namespaces.
        /// </summary>
        /// <param name="namespaces"></param>
        public override void GetNamespaces(IDictionary<string, HashSet<string>> namespaces)
        {
            AddNamespacesFromName(namespaces, ReturnType);
            base.GetNamespaces(namespaces);
        }

        /// <summary>
        /// The body
        /// </summary>
        public StringBuilder Body { get; set; } = new StringBuilder();
    }
}