namespace SolidRpc.OpenApi.Model.V2
{
    /// <summary>
    /// Lists the headers that can be sent as part of a response.
    /// <a href="https://swagger.io/specification/v2/#headersObject"/>
    /// </summary>
    public class HeadersObject : ModelBaseDynamic<HeaderObject>
    {
        /// <summary>
        /// Constructs a new instance
        /// </summary>
        /// <param name="parent"></param>
        public HeadersObject(ModelBase parent) : base(parent) { }
    }
}