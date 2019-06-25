using System;
using System.Collections.Concurrent;
using System.IO;
using System.Reflection;
using System.Xml;

namespace SolidRpc.OpenApi.Model.CodeDoc.Impl
{
    public class CodeDocRepository : ICodeDocRepository
    {
        private ConcurrentDictionary<Assembly, ICodeDocAssembly> s_AssemblyDocs = new ConcurrentDictionary<Assembly, ICodeDocAssembly>();

        public ICodeDocAssembly GetAssemblyDoc(Assembly assembly)
        {
            return s_AssemblyDocs.GetOrAdd(assembly, GetAsseblyDocInternal);
        }

        public ICodeDocMethod GetMethodDoc(MethodInfo methodInfo)
        {
            var assemblyDoc = GetAssemblyDoc(methodInfo.DeclaringType.Assembly);
            var classDoc = assemblyDoc?.GetClassDocumentation(methodInfo.DeclaringType);
            return classDoc?.GetMethodDocumentation(methodInfo);
        }

        private ICodeDocAssembly GetAsseblyDocInternal(Assembly arg)
        {
            return new CodeDocAssembly(GetXmlDoc(arg));
        }

        private XmlDocument GetXmlDoc(Assembly arg)
        {
            var xmlDocLocation = new FileInfo(Path.ChangeExtension(arg.Location, ".xml"));
            var resName = $"{arg.GetName().Name}.xml";
            Stream docStream = null;

            // look for the documentation where the assebly resides
            if (xmlDocLocation.Exists)
            {
                // load from file
                docStream = xmlDocLocation.OpenRead();
            }
            else
            {
                // look for resource
                foreach(var name in arg.GetManifestResourceNames())
                {
                    if(name.EndsWith(resName))
                    {
                        docStream = arg.GetManifestResourceStream(resName);
                        break;
                    }
                }
            }
            if(docStream == null)
            {
                throw new Exception($"Cannot find documentation for assembly @{xmlDocLocation.FullName} or as a resource {resName}");
            }
            using (docStream)
            {
                var xmlDocument = new XmlDocument();
                xmlDocument.Load(docStream);
                return xmlDocument;
            }
        }
    }
}
