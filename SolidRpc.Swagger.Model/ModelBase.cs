using System;
using System.Collections.Generic;
using System.Text;

namespace SolidRpc.Swagger.Model
{
    /// <summary>
    /// Base class implemented by all the model instances.
    /// </summary>
    public abstract class ModelBase
    {
        /// <summary>
        /// Returns the parent structure - null for SwaggerObject
        /// </summary>
        public ModelBase Parent { get; set; }
    }
}
