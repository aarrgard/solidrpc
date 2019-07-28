using System.Collections.Generic;

namespace SolidRpc.OpenApi.Model.CSharp.Impl
{
    public class CSharpComment : ICSharpComment
    {
        public CSharpComment(string summary, IEnumerable<ICSharpCommentException> exceptions, ICSharpCommentExternalDoc externalDoc = null)
        {
            Summary = summary;
            ExternalDoc = externalDoc;
            Exceptions = exceptions;
        }
        public string Summary { get; }

        public ICSharpCommentExternalDoc ExternalDoc { get; }

        public IEnumerable<ICSharpCommentException> Exceptions { get; }
    }
}