using System;
using System.Collections.Generic;

namespace SolidRpc.OpenApi.Model.CSharp.Impl
{
    /// <summary>
    /// Implements a c# method parameter
    /// </summary>
    public class CSharpMethodParameter : CSharpMember, ICSharpMethodParameter
    {
        /// <summary>
        /// Constructs a new instance
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="name"></param>
        /// <param name="parameterType"></param>
        /// <param name="optional"></param>
        /// <param name="defaultValue"></param>
        public CSharpMethodParameter(ICSharpMember parent, string name, ICSharpType parameterType, bool optional, string defaultValue = null) : base(parent, name)
        {
            ParameterType = parameterType ?? throw new ArgumentNullException(nameof(parameterType));
            Optional = optional;
            DefaultValue = defaultValue;
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
        /// The parameter type
        /// </summary>
        public ICSharpType ParameterType { get; }

        /// <summary>
        /// Is this parameter options
        /// </summary>
        public bool Optional { get; }

        /// <summary>
        /// The default value
        /// </summary>
        public string DefaultValue { get; }

        /// <summary>
        /// Emits the code
        /// </summary>
        /// <param name="codeWriter"></param>
        public override void WriteCode(ICodeWriter codeWriter)
        {
            codeWriter.Emit($"{SimplifyName(ParameterType.FullName)} {Name}");
            if (!string.IsNullOrEmpty(DefaultValue))
            {
                codeWriter.Emit($" = {SimplifyName(DefaultValue)}");
            }
        }
        /// <summary>
        /// Populates the namespaces based on the parameter types.
        /// </summary>
        /// <param name="namespaces"></param>
        public override void GetNamespaces(IDictionary<string, HashSet<string>> namespaces)
        {
            AddNamespacesFromName(namespaces, ParameterType);
            base.GetNamespaces(namespaces);
        }
    }
}