using System;
using System.Collections.Generic;
using System.Text;

namespace SolidRpc.OpenApi.Model.CSharp.Impl
{
    /// <summary>
    /// Represents a c# property
    /// </summary>
    public class CSharpProperty : CSharpMember, ICSharpProperty
    {
        /// <summary>
        /// Constructs a new instance
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="name"></param>
        /// <param name="propertyType"></param>
        public CSharpProperty(ICSharpMember parent, string name, ICSharpType propertyType, bool canRead = true, bool canWrite = true) : base(parent, name)
        {
            PropertyType = propertyType;
            CanRead = canRead;
            CanWrite = canWrite;
            Getter = new StringBuilder();
            Setter = new StringBuilder();
        }

        /// <summary>
        /// The property type
        /// </summary>
        public ICSharpType PropertyType { get; }

        /// <summary>
        /// Specifies if we can read this property
        /// </summary>
        public bool CanRead { get; }

        /// <summary>
        /// Specifies if we can write to this property
        /// </summary>
        public bool CanWrite { get; }

        public StringBuilder Getter { get;}

        public StringBuilder Setter { get; }

        /// <summary>
        /// Adds a member to this method
        /// </summary>
        /// <param name="member"></param>
        public override void AddMember(ICSharpMember member)
        {
            ProtectedMembers.Add(member);
        }

        /// <summary>
        /// emits the code
        /// </summary>
        /// <param name="codeWriter"></param>
        public override void WriteCode(ICodeWriter codeWriter)
        {
            WriteSummary(codeWriter);
            WriteAttributes(codeWriter);
            codeWriter.Emit($"public {SimplifyName(PropertyType.FullName)} {Name} {{");
            if (CanRead)
            {
                EmitCode(codeWriter, "get", Getter);
            }
            if (CanWrite)
            {
                EmitCode(codeWriter, "set", Setter);
            }
            codeWriter.Emit($" }}{codeWriter.NewLine}");
        }

        private void EmitCode(ICodeWriter codeWriter, string type, StringBuilder code)
        {
            if (code.Length == 0)
            {
                codeWriter.Emit($" {type};");
            }
            else
            {
                codeWriter.Emit($"{codeWriter.NewLine}");
                codeWriter.Indent();
                codeWriter.Emit($"{type} {{{codeWriter.NewLine}");
                codeWriter.Indent();
                codeWriter.Emit($"{code.ToString().Trim()}{codeWriter.NewLine}");
                codeWriter.Unindent();
                codeWriter.Emit($"}}{codeWriter.NewLine}");
                codeWriter.Unindent();
            }
        }

        /// <summary>
        /// Populates the namespaces based on the property type.
        /// </summary>
        /// <param name="namespaces"></param>
        public override void GetNamespaces(IDictionary<string, HashSet<string>> namespaces)
        {
            AddNamespacesFromName(namespaces, PropertyType);
            base.GetNamespaces(namespaces);
        }
    }
}