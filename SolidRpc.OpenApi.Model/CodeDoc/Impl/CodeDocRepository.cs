using System;
using System.Collections.Concurrent;
using System.IO;
using System.Reflection;
using System.Xml;

namespace SolidRpc.OpenApi.Model.CodeDoc.Impl
{
    /// <summary>
    /// Implements the logic for the code comment repository.
    /// </summary>
    public class CodeDocRepository : ICodeDocRepository
    {
        private ConcurrentDictionary<Assembly, ICodeDocAssembly> s_AssemblyDocs = new ConcurrentDictionary<Assembly, ICodeDocAssembly>();

        /// <summary>
        /// Returns the assembly documentation
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        public ICodeDocAssembly GetAssemblyDoc(Assembly assembly)
        {
            return s_AssemblyDocs.GetOrAdd(assembly, GetAsseblyDocInternal);
        }

        /// <summary>
        /// Returns the class doc
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public ICodeDocClass GetClassDoc(Type type)
        {
            var assemblyDoc = GetAssemblyDoc(type.Assembly);
            return assemblyDoc.GetClassDocumentation(type);
        }

        /// <summary>
        /// Returns the method doc
        /// </summary>
        /// <param name="methodInfo"></param>
        /// <returns></returns>
        public ICodeDocMethod GetMethodDoc(MethodInfo methodInfo)
        {
            var classDoc = GetClassDoc(methodInfo.DeclaringType);
            return classDoc.GetMethodDocumentation(methodInfo);
        }

        public ICodeDocProperty GetPropertyDoc(PropertyInfo pi)
        {
            var classDoc = GetClassDoc(pi.DeclaringType);
            return classDoc.GetPropertyDocumentation(pi);
        }

        private ICodeDocAssembly GetAsseblyDocInternal(Assembly arg)
        {
            return new CodeDocAssembly(GetXmlDoc(arg));
        }

        private XmlDocument GetXmlDoc(Assembly arg)
        {
            Stream docStream = null;

            // look for resource
            var resName = $"{arg.GetName().Name}.xml";
            foreach (var name in arg.GetManifestResourceNames())
            {
                if (name.EndsWith(resName))
                {
                    docStream = arg.GetManifestResourceStream(resName);
                    break;
                }
            }

            // look for the documentation where the assebly resides
            var xmlDocLocation = new FileInfo(Path.ChangeExtension(arg.Location, ".xml"));
            if (docStream == null && xmlDocLocation.Exists)
            {
                // load from file
                docStream = xmlDocLocation.OpenRead();
            }

            var xmlDocument = new XmlDocument();
            if (docStream == null)
            {
                return xmlDocument;
                //throw new Exception($"Cannot find documentation for assembly @{xmlDocLocation.FullName} or as a resource {resName}");
            }
            using (docStream)
            {
                xmlDocument.Load(docStream);
                return xmlDocument;
            }
        }
    }
}
