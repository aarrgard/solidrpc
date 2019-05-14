﻿using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SolidRpc.Swagger.V2
{
    /// <summary>
    /// Lists the required security schemes to execute this operation. The object can have multiple security schemes declared in it which are all required (that is, there is a logical AND between the schemes).
    /// The name used for each property MUST correspond to a security scheme declared in the Security Definitions.
    /// </summary>
    /// <see cref="https://swagger.io/specification/v2/#securityRequirementObject"/>
    public class SecurityRequirementObject : DynamicBase<IEnumerable<string>>
    {
    }
}