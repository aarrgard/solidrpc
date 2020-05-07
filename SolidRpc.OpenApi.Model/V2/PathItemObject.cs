using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace SolidRpc.OpenApi.Model.V2
{
    /// <summary>
    /// Holds the relative paths to the individual endpoints. The path is appended to the basePath in order to construct the full URL. The Paths may be empty, due to ACL constraints.
    /// </summary>
    /// <a href="https://swagger.io/specification/v2/#pathItemObject"/>
    public class PathItemObject : ModelBase
    {
        /// <summary>
        /// Constructs a new instance
        /// </summary>
        /// <param name="parent"></param>
        public PathItemObject(ModelBase parent) : base(parent)
        {

        }
        /// <summary>
        /// A definition of a GET operation on this path.
        /// </summary>
        [DataMember(Name = "get", EmitDefaultValue = false)]
        public OperationObject Get { get; set; }

        /// <summary>
        /// A definition of a PUT operation on this path.
        /// </summary>
        [DataMember(Name = "put", EmitDefaultValue = false)]
        public OperationObject Put { get; set; }

        /// <summary>
        /// A definition of a POST operation on this path.
        /// </summary>
        [DataMember(Name = "post", EmitDefaultValue = false)]
        public OperationObject Post { get; set; }

        /// <summary>
        /// A definition of a DELETE operation on this path.
        /// </summary>
        [DataMember(Name = "delete", EmitDefaultValue = false)]
        public OperationObject Delete { get; set; }

        /// <summary>
        /// A definition of a OPTIONS operation on this path.
        /// </summary>
        [DataMember(Name = "options", EmitDefaultValue = false)]
        public OperationObject Options { get; set; }

        /// <summary>
        /// A definition of a HEAD operation on this path.
        /// </summary>
        [DataMember(Name = "head", EmitDefaultValue = false)]
        public OperationObject Head { get; set; }

        /// <summary>
        /// A definition of a HEAD operation on this path.
        /// </summary>
        [DataMember(Name = "patch", EmitDefaultValue = false)]
        public OperationObject Patch { get; set; }

        /// <summary>
        /// A list of parameters that are applicable for all the operations described under this path. These parameters can be overridden at the operation level, but cannot be removed there. The list MUST NOT include duplicated parameters. A unique parameter is defined by a combination of a name and location. The list can use the Reference Object to link to parameters that are defined at the Swagger Object's parameters. There can be one "body" parameter at most.
        /// </summary>
        [DataMember(Name = "parameters", EmitDefaultValue = false)]
        public IEnumerable<ParameterObject> Parameters { get; set; }

        /// <summary>
        /// The path to this operation.
        /// </summary>
        public string Path
        {
            get
            {
                if (Parent is PathsObject pathsItem)
                {
                    return pathsItem.Where(o => o.Value == this).Select(o => o.Key).Single();
                }
                throw new System.Exception("Cannot determine method.");
            }
        }

        /// <summary>
        /// Returns true if all the operations are null
        /// </summary>
        public bool IsEmpty { 
            get
            {
                return Head == null &&
                    Options == null &&
                    Get == null &&
                    Put == null &&
                    Delete == null &&
                    Post == null &&
                    Patch == null;
            }
        }

        /// <summary>
        /// Returns the parameters. Empty array if null.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ParameterObject> GetParameters()
        {
            return Parameters ?? new ParameterObject[0];
        }
    }
}