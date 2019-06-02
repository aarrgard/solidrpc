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
        protected ModelBase(ModelBase parent)
        {
            Parent = parent;
        }
        /// <summary>
        /// Returns the parent structure - null for SwaggerObject
        /// </summary>
        public ModelBase Parent { get; set; }

        /// <summary>
        /// Returns the parent of supplied type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetParent<T>() where T:ModelBase
        {
            var work = Parent;
            while(work != null)
            {
                if (work is T t)
                {
                    return t;
                }
                work = work.Parent;
            }
            return default(T);
        }
    }
}
