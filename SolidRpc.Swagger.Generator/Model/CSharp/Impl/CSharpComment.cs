namespace SolidRpc.Swagger.Generator.Model.CSharp.Impl
{
    public class CSharpComment : ICSharpComment
    {
        public CSharpComment(string summary)
        {
            Summary = summary;
        }
        public string Summary { get; }
    }
}