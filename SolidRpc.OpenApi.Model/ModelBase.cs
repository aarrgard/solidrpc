using System;
using System.Collections.Generic;
using System.Text;

namespace SolidRpc.OpenApi.Model
{
    /// <summary>
    /// Base class implemented by all the model instances.
    /// </summary>
    public abstract class ModelBase
    {
        /// <summary>
        /// Constructs a new instance
        /// </summary>
        /// <param name="parent"></param>
        protected ModelBase(ModelBase parent)
        {
            Parent = parent;
        }
        /// <summary>
        /// Returns the parent structure - null for SwaggerObject
        /// </summary>
        public ModelBase Parent { get; private set; }

        /// <summary>
        /// Sets the parent.
        /// </summary>
        /// <param name="parent"></param>
        public void SetParent(ModelBase parent)
        {
            Parent = parent;
        }

        /// <summary>
        /// Returns the parent of supplied type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetParent<T>() where T:class
        {
            var work = Parent;
            while(work != null)
            {
                var t = work as T;
                if (t != null)
                {
                    return t;
                }
                work = work.Parent;
            }
            return default(T);
        }
    }
}
