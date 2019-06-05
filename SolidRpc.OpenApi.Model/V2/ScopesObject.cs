namespace SolidRpc.OpenApi.Model.V2
{
    /// <summary>
    /// Lists the available scopes for an OAuth2 security scheme.
    ///</summary>
    /// <see cref="https://swagger.io/specification/v2/#scopesObject"/>
    public class ScopesObject : ModelBaseDynamic<string>
    {
        public ScopesObject(ModelBase parent) : base(parent) { }
    }
}