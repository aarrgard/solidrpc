using System;

namespace SolidRpc.OpenApi.Generator.Model.CSharp.Impl
{
    internal class CSharpCommentExternalDoc : ICSharpCommentExternalDoc
    {
        public CSharpCommentExternalDoc(string url, string description)
        {
            if(!string.IsNullOrEmpty(url))
            {
                Url = new Uri(url);
            }
            Description = description;
        }

        public string Description { get; }

        public Uri Url { get; }
    }
}