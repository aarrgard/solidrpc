using System;

namespace SolidRpc.Core
{
    /// <summary>
    /// The permission represents a permission required to access a resource.
    /// </summary>
    public abstract class Permission
    {
        /// <summary>
        /// Returns the permission name. Defaults to full name of clas.
        /// </summary>
        public virtual string PermissionName => GetType().FullName;
    }
}
