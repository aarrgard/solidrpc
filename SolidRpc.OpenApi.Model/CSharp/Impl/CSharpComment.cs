using System.Collections.Generic;

namespace SolidRpc.OpenApi.Model.CSharp.Impl
{
    /// <summary>
    /// Implemets the c# comment
    /// </summary>
    public class CSharpComment : ICSharpComment
    {
        /// <summary>
        /// Constructs a new instance
        /// </summary>
        /// <param name="summary"></param>
        /// <param name="exceptions"></param>
        /// <param name="externalDoc"></param>
        public CSharpComment(string summary, IEnumerable<ICSharpCommentException> exceptions, ICSharpCommentExternalDoc externalDoc = null)
        {
            Summary = summary;
            ExternalDoc = externalDoc;
            Exceptions = exceptions;
        }

        /// <summary>
        /// The summary
        /// </summary>
        public string Summary { get; }

        /// <summary>
        /// The external doc
        /// </summary>
        public ICSharpCommentExternalDoc ExternalDoc { get; }

        /// <summary>
        /// The exceptions
        /// </summary>
        public IEnumerable<ICSharpCommentException> Exceptions { get; }
    }
}