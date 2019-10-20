using System.Collections.Generic;
using System.Linq;

namespace SolidRpc.OpenApi.Model.CSharp.Impl
{
    /// <summary>
    /// Implements a c# namespace
    /// </summary>
    public class CSharpNamespace : CSharpMember, ICSharpNamespace
    {
        /// <summary>
        /// Constructs a new instance
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="name"></param>
        public CSharpNamespace(ICSharpMember parent, string name) : base(parent, name)
        {
            Parts = FullName.Split('.');
        }

        /// <summary>
        /// Returns the namespace parts
        /// </summary>
        public IEnumerable<string> Parts { get; }

        /// <summary>
        /// REturns all the namespaces
        /// </summary>
        public IEnumerable<string> Namespaces
        {
            get
            {
                string currentNamespace = "";
                yield return currentNamespace;
                foreach(var part in Parts)
                {
                    currentNamespace = new QualifiedName(currentNamespace, part);
                    yield return currentNamespace;
                };
            }
        }

        /// <summary>
        /// Emits the namespace and the members to supplied writer.
        /// </summary>
        /// <param name="codeWriter"></param>
        public override void WriteCode(ICodeWriter codeWriter)
        {
            Members.ToList().ForEach(o => o.WriteCode(codeWriter));
        }
    }
}