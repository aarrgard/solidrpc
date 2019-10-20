using System;
using System.Collections.Generic;
using System.Linq;

namespace SolidRpc.OpenApi.Model.CSharp.Impl
{
    /// <summary>
    /// Represnets a csharp attribute
    /// </summary>
    public class CSharpAttribute : CSharpMember, ICSharpAttribute
    {
        /// <summary>
        /// Constructs a new instance
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="name"></param>
        /// <param name="args">The constructor args</param>
        /// <param name="attributeData"></param>
        public CSharpAttribute(ICSharpMember parent, string name, object[] args, IDictionary<string, object> attributeData) : base(parent, name)
        {
            Args = args ?? new object[0];
            AttributeData = attributeData ?? new Dictionary<string, object>();
        }

        /// <summary>
        /// The constructor args
        /// </summary>
        public object[] Args { get; }

        /// <summary>
        /// The properties or the member
        /// </summary>
        public IDictionary<string, object> AttributeData { get; }

        /// <summary>
        /// Returns the namespaces.
        /// </summary>
        /// <param name="namespaces"></param>
        public override void GetNamespaces(IDictionary<string, HashSet<string>> namespaces)
        {
            var qName = new QualifiedName(Name);
            AddNamespacesFromName(namespaces, qName.Namespace, qName.Name);
            base.GetNamespaces(namespaces);
        }

        /// <summary>
        /// Emits the attribute
        /// </summary>
        /// <param name="codeWriter"></param>
        public override void WriteCode(ICodeWriter codeWriter)
        {
            var name = Name;
            if(name.EndsWith("Attribute"))
            {
                name = name.Substring(0, name.Length - "Attribute".Length);
            }
            codeWriter.Emit($"[{SimplifyName(name)}");
            if(AttributeData.Any() || Args.Any())
            {
                codeWriter.Emit("(");
                var dataWritten = false;
                foreach(var arg in Args)
                {
                    if (dataWritten)
                    {
                        codeWriter.Emit(",");
                    }
                    WriteAttributeData(codeWriter, arg);
                    dataWritten = true;
                }
                foreach (var data in AttributeData)
                {
                    if(dataWritten)
                    {
                        codeWriter.Emit(",");
                    }
                    codeWriter.Emit(data.Key);
                    codeWriter.Emit("=");
                    WriteAttributeData(codeWriter, data.Value);
                    dataWritten = true;
                }
                codeWriter.Emit(")");
            }
            codeWriter.Emit($"]{codeWriter.NewLine}");
        }

        private void WriteAttributeData(ICodeWriter codeWriter, object value)
        {
            if (value is string str)
            {
                codeWriter.Emit("\"");
                codeWriter.Emit(value.ToString());
                codeWriter.Emit("\"");
            }
            else if (value is IEnumerable<string> strArr)
            {
                codeWriter.Emit("new [] {");
                codeWriter.Emit(string.Join(",", strArr.Select(o => $"\"{o}\"")));
                codeWriter.Emit("}");
            }
            else if (value is bool b)
            {
                codeWriter.Emit(b ? "true" : "false");
            }
            else
            {
                codeWriter.Emit(value.ToString());
            }
        }
    }
}
