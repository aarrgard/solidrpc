namespace SolidRpc.OpenApi.Model.V2
{
    /// <summary>
    /// Lists the headers that can be sent as part of a response.
    /// </summary>
    /// <see cref="https://swagger.io/specification/v2/#headersObject"/>
    public class HeadersObject : ModelBaseDynamic<HeaderObject>
    {
        public HeadersObject(ModelBase parent) : base(parent) { }
    }
}