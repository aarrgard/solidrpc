using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

namespace SolidRpc.OpenApi.Generator.Impl.Csproj
{
    /// <summary>
    /// Container for csproj info.
    /// </summary>
    public class CsprojInfo
    {

        /// <summary>
        /// Parses the supplied content
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="filecontent"></param>
        /// <returns></returns>
        public static CsprojInfo GetCsprojInfo(string filename, Stream filecontent)
        {
            if(!filename.EndsWith(".csproj", StringComparison.InvariantCultureIgnoreCase))
            {
                throw new ArgumentException("File name is not a .csproj file:" + filename);
            }
            // load xml
            var projectDocument = new XmlDocument();
            projectDocument.Load(filecontent);

            var projInfo = new CsprojInfo();
            foreach(var prop in projInfo.GetType().GetProperties())
            {
                if (!prop.CanRead) continue;
                if (!prop.CanWrite) continue;
                prop.SetValue(projInfo, GetProjectSetting(projectDocument, prop.Name));
            }
            if (string.IsNullOrEmpty(projInfo.AssemblyName))
            {
                projInfo.AssemblyName = filename.Substring(0, filename.Length - ".csproj".Length);
            }
            if (string.IsNullOrEmpty(projInfo.OpenApiBasePath))
            {
                projInfo.OpenApiBasePath = $"/{projInfo.AssemblyName.Replace(".", "/")}";
            }
            if (string.IsNullOrEmpty(projInfo.RootNamespace))
            {
                projInfo.RootNamespace = projInfo.AssemblyName;
            }
            return projInfo;
        }


        private static string GetProjectSetting(XmlDocument doc, params string[] settings)
        {
            var nsmgr = new XmlNamespaceManager(doc.NameTable);
            foreach (var setting in settings)
            {
                var xpath = $"/Project/PropertyGroup/{setting}";
                var node = doc.SelectSingleNode(xpath, nsmgr);
                if (node != null)
                {
                    return node.InnerText;
                }
            }
            return null;
        }

        public string RootNamespace { get; set; }
        public string AssemblyName { get; set; }
        public string Version { get; set; }
        public string Description { get; set; }
        public string Authors { get; set; }
        public string PackageProjectUrl { get; set; }
        public string PackageLicenseUrl { get; set; }
        public string OpenApiVersion { get; set; }
        public string OpenApiDescription { get; set; }
        public string OpenApiLicenseName { get; set; }
        public string OpenApiLicenseUrl { get; set; }
        public string OpenApiContactEmail { get; set; }
        public string OpenApiContactName { get; set; }
        public string OpenApiContactUrl { get; set; }
        public string OpenApiBasePath { get; set; }
    }
}
