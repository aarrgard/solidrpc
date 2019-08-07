using System;
using System.Collections.Generic;

namespace SolidRpc.Security.Security
{
    /// <summary>
    /// The security attribute defines what permissions that are required to
    /// accesss the data in these interfaces
    /// </summary>
    public class SecurityAttribute : Attribute
    {
        /// <summary>
        /// The permissions required to access this resource
        /// </summary>
        /// <param name="permissions"></param>
        public SecurityAttribute(params string[] permissions)
        {
            Permissions = permissions;
            Public = false;
        }

        /// <summary>
        /// Specifies if this resource is public(ie no permissions required)
        /// </summary>
        public bool Public { get; set; }

        /// <summary>
        /// The permissions required to access this resource
        /// </summary>
        public IEnumerable<string> Permissions { get; }
    }
}
