using System;
using System.Collections.Generic;
using System.Linq;

namespace SolidRpc.OpenApi.Model.CSharp.Impl
{
    /// <summary>
    /// Represents a C# method
    /// </summary>
    public class CSharpField : CSharpMember, ICSharpField
    {
        /// <summary>
        /// Constructs a new instance
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="name"></param>
        /// <param name="fieldType"></param>
        public CSharpField(ICSharpMember parent, string name, ICSharpType fieldType, string fieldValue) : base(parent, name)
        {
            FieldType = fieldType ?? throw new ArgumentNullException(nameof(fieldType));
            Value = fieldValue;
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
        /// The field type
        /// </summary>
        public ICSharpType FieldType { get; }

        /// <summary>
        /// The field value;
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Writes the code.
        /// </summary>
        /// <param name="codeWriter"></param>
        public override void WriteCode(ICodeWriter codeWriter)
        {
            var parameters = Members.OfType<ICSharpMethodParameter>().ToList();
            WriteSummary(codeWriter);
            codeWriter.Emit($"<<<field>>>");
        }

        /// <summary>
        /// Adds the namespaces.
        /// </summary>
        /// <param name="namespaces"></param>
        public override void GetNamespaces(IDictionary<string, HashSet<string>> namespaces)
        {
            AddNamespacesFromName(namespaces, FieldType);
            base.GetNamespaces(namespaces);
        }
    }
}