using System.Runtime.Serialization;
using System.Xml.Linq;

namespace SolidRpc.OpenApi.Generator.Types.Project
{
    /// <summary>
    /// 
    /// </summary>
    public class ProjectAssetsTargetRefRuntime
    {
        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name = "related", EmitDefaultValue = false)]
        public string Related { get; set; }
    }
}