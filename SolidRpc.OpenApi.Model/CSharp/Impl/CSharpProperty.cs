using System;
using System.Collections.Generic;

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
        public CSharpProperty(ICSharpMember parent, string name, ICSharpType propertyType) : base(parent, name)
        {
            PropertyType = propertyType;
        }

        /// <summary>
        /// The property type
        /// </summary>
        public ICSharpType PropertyType { get; }

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
            codeWriter.Emit($"public {SimplifyName(PropertyType.FullName)} {Name} {{ get; set; }}{codeWriter.NewLine}");
        }

        /// <summary>
        /// Returns the associated namespaces.
        /// </summary>
        /// <param name="namespaces"></param>
        public override void GetNamespaces(ICollection<string> namespaces)
        {
            AddNamespacesFromName(namespaces, PropertyType);
            base.GetNamespaces(namespaces);
        }
    }
}