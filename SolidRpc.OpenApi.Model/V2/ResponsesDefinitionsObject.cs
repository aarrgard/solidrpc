namespace SolidRpc.OpenApi.Model.V2
{
    /// <summary>
    /// An object to hold responses to be reused across operations. Response definitions can be referenced to the ones defined here.
    /// This does not define global operation responses.
    /// </summary>
    /// <a href="https://swagger.io/specification/v2/#responsesDefinitionsObject"/>
    public class ResponsesDefinitionsObject : ModelBaseDynamic<ResponseObject>
    {
        public ResponsesDefinitionsObject(ModelBase parent) : base(parent) { }
    }
}