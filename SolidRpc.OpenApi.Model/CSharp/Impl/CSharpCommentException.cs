using System;

namespace SolidRpc.OpenApi.Model.CSharp.Impl
{
    internal class CSharpCommentException : ICSharpCommentException
    {
        public CSharpCommentException(string cref, string description)
        {
            Cref = cref;
            Description = description;
        }

        public string Description { get; }

        public string Cref { get; }
    }
}