using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml;

namespace SolidRpc.OpenApi.Model.CodeDoc.Impl
{
    /// <summary>
    /// Represents the documentation for a class.
    /// </summary>
    public class CodeDocClass : CodeDocMember, ICodeDocClass
    {

        /// <summary>
        /// Constructs a new instance
        /// </summary>
        /// <param name="assemblyDocumentation"></param>
        /// <param name="className"></param>
        public CodeDocClass(
            CodeDocAssembly assemblyDocumentation, 
            string className)
        {
            AssemblyDocumentation = assemblyDocumentation;
            ClassName = className;
            Summary = "";
            MethodDocumentation = new Dictionary<string, ICodeDocMethod>();
            PropertyDocumentation = new Dictionary<string, ICodeDocProperty>();
        }

        /// <summary>
        /// The underlying xml document
        /// </summary>
        public XmlDocument XmlDocument => AssemblyDocumentation.XmlDocument;

        private CodeDocAssembly AssemblyDocumentation { get; }
        ICodeDocAssembly ICodeDocClass.AssemblyDocumentation => AssemblyDocumentation;

        /// <summary>
        /// The class name.
        /// </summary>
        public string ClassName { get; }

        /// <summary>
        /// The comment for this type.
        /// </summary>
        public string Summary { get; private set; }

        /// <summary>
        /// All the field documentation
        /// </summary>
        public IDictionary<string, ICodeDocField> FieldDocumentation { get; }
        IEnumerable<ICodeDocField> ICodeDocClass.FieldDocumentation => FieldDocumentation.Values;

        /// <summary>
        /// All the method documentations.
        /// </summary>
        public IDictionary<string, ICodeDocMethod> MethodDocumentation { get; }
        IEnumerable<ICodeDocMethod> ICodeDocClass.MethodDocumentation => MethodDocumentation.Values;

        /// <summary>
        /// All the property documentations
        /// </summary>
        public IDictionary<string, ICodeDocProperty> PropertyDocumentation { get; }
        IEnumerable<ICodeDocProperty> ICodeDocClass.PropertyDocumentation => PropertyDocumentation.Values;

        /// <summary>
        /// Returns the code comments.
        /// </summary>
        public string CodeComments
        {
            get
            {
                var sb = new StringBuilder();
                sb.AppendLine("<summary>");
                sb.AppendLine(Summary);
                sb.AppendLine("</summary>");
                //ParameterDocumentation.ToList().ForEach(o => sb.Append("<param name=\"").Append(o.Name).Append("\">").Append(o.Comment).Append("</param>"));
                //ExceptionDocumentation.ToList().ForEach(o => sb.Append("<exception cref=\"").Append(o.ExceptionType).Append("\">").Append(o.Comment).Append("</exception>"));
                return sb.ToString();

            }
        }

        /// <summary>
        /// Returns the documentation for supplied method
        /// </summary>
        /// <param name="methodInfo"></param>
        /// <returns></returns>
        public ICodeDocMethod GetMethodDocumentation(MethodInfo methodInfo)
        {
            if(!MethodDocumentation.TryGetValue(methodInfo.Name, out ICodeDocMethod doc))
            {
                doc = new CodeDocMethod(this, methodInfo.Name);
            }
            return doc;
        }

        /// <summary>
        /// Returns the documentation for supplied property.
        /// </summary>
        /// <param name="pi"></param>
        /// <returns></returns>
        public ICodeDocProperty GetPropertyDocumentation(PropertyInfo pi)
        {
            if (!PropertyDocumentation.TryGetValue(pi.Name, out ICodeDocProperty doc))
            {
                doc = new CodeDocProperty(this, pi.Name);
            }
            return doc;
        }

        /// <summary>
        /// Returns the field documentation
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        public ICodeDocField GetFieldDocumentation(FieldInfo field)
        {
            if (!FieldDocumentation.TryGetValue(field.Name, out ICodeDocField doc))
            {
                doc = new CodeDocField(this, field.Name);
            }
            return doc;
        }
    }
}