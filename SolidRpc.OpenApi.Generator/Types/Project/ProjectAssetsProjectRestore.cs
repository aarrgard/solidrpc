using System.Runtime.Serialization;
using System.Xml.Linq;

namespace SolidRpc.OpenApi.Generator.Types.Project
{
    /// <summary>
    /// 
    /// </summary>
    public class ProjectAssetsProjectRestore
    {
        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name = "projectUniqueName", EmitDefaultValue = false)]
        public string ProjectUniqueName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name = "projectName", EmitDefaultValue = false)]
        public string ProjectName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name = "projectPath", EmitDefaultValue = false)]
        public string ProjectPath { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name = "packagesPath", EmitDefaultValue = false)]
        public string PackagesPath { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name = "outputPath", EmitDefaultValue = false)]
        public string OutputPath { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name = "projectStyle", EmitDefaultValue = false)]
        public string projectStyle { get; set; }
    }
}