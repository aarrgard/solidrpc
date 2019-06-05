namespace SolidRpc.OpenApi.Generator.Model.CSharp.Impl
{
    public class CSharpComment : ICSharpComment
    {
        public CSharpComment(string summary, ICSharpCommentExternalDoc externalDoc = null)
        {
            Summary = summary;
            ExternalDoc = externalDoc;
        }
        public string Summary { get; }

        public ICSharpCommentExternalDoc ExternalDoc { get; }
    }
}