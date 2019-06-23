using System;

namespace SolidRpc.OpenApi.Generator.Model.CSharp.Impl
{
    internal class CSharpCommentExternalDoc : ICSharpCommentExternalDoc
    {
        public CSharpCommentExternalDoc(string url, string description)
        {
            Url = url;
            Description = description;
        }

        public string Description { get; }

        public string Url { get; }
    }
}