using System.Collections.Generic;
using System.Runtime.Serialization;


namespace SolidRpc.OpenApi.Model.V2
{
    /// <summary>
    /// Holds the relative paths to the individual endpoints. The path is appended to the basePath in order to construct the full URL. The Paths may be empty, due to ACL constraints.
    /// </summary>
    /// <see cref="https://swagger.io/specification/v2/#pathsObject"/>
    public class PathsObject : ModelBaseDynamic<PathItemObject>
    {
        public PathsObject(ModelBase parent) : base(parent) { }

    }
}