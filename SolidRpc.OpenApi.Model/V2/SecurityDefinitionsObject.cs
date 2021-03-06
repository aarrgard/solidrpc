﻿namespace SolidRpc.OpenApi.Model.V2
{
    /// <summary>
    /// A declaration of the security schemes available to be used in the specification. This does not enforce the security schemes on the operations and only serves to provide the relevant details for each scheme.  
    /// </summary>
    /// <a href="https://swagger.io/specification/v2/#securityDefinitionsObject"/>
    public class SecurityDefinitionsObject : ModelBaseDynamic<SecuritySchemeObject>
    {
        public SecurityDefinitionsObject(ModelBase parent) : base(parent) { }
    }
}