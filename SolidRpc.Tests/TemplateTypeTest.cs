using Microsoft.Extensions.Primitives;
using NUnit.Framework;
using SolidRpc.Abstractions.Types;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace SolidRpc.Tests
{
    /// <summary>
    /// Tests the template types
    /// </summary>
    public class TemplateTypeTest : TestBase
    {
        private class MyFileType
        {
            public Stream Content { get; set; }
        }
        private class MyHttpRequest
        {
            public Uri Uri { get; set; }
        }
        private class NotARequest
        {
            public Uri Uri { get; set; }
        }

        /// <summary>
        /// Tests the type store
        /// </summary>
        [Test]
        public void TestFileContentTemplateType()
        {
            var x = (object)new FileContent();
            var template = FileContentTemplate.GetTemplate(x.GetType());
            Assert.IsTrue(template.IsTemplateType);

            Assert.IsNull(template.GetContent(x));
            template.SetContent(x, new MemoryStream());
            Assert.IsNotNull(template.GetContent(x));

            Assert.IsNull(template.GetContentType(x));
            template.SetContentType(x, "application/json");
            Assert.IsNotNull(template.GetContentType(x));
        }

        /// <summary>
        /// Tests the type store
        /// </summary>
        [Test]
        public void TestHttpRequestTemplateType()
        {
            var x = (object)new HttpRequest();
            var template = HttpRequestTemplate.GetTemplate(x.GetType());
            Assert.IsTrue(template.IsTemplateType);

            Assert.IsNull(template.GetUri(x));
            template.SetUri(x, new Uri("http://test.com/testing"));
            Assert.IsNotNull(template.GetUri(x));

            Assert.IsNull(template.GetMethod(x));
            template.SetMethod(x, "GET");
            Assert.IsNotNull(template.GetMethod(x));
        }

        /// <summary>
        /// Tests the type store
        /// </summary>
        [Test]
        public void TestMyFileType()
        {
            var x = (object)new MyHttpRequest();
            var template = HttpRequestTemplate.GetTemplate(x.GetType());
            Assert.IsTrue(template.IsTemplateType);

            Assert.IsNull(template.GetUri(x));
            template.SetUri(x, new Uri("http://test.com/testing"));
            Assert.IsNotNull(template.GetUri(x));

            Assert.IsNull(template.GetHeaders(x));
            template.SetHeaders(x, new Dictionary<string, string[]>());
            Assert.IsNull(template.GetHeaders(x));
        }

        /// <summary>
        /// Tests the type store
        /// </summary>
        [Test]
        public void TestMyHttpRequest()
        {
            var x = (object)new MyFileType();
            var template = FileContentTemplate.GetTemplate(x.GetType());
            Assert.IsTrue(template.IsTemplateType);

            Assert.IsNull(template.GetContent(x));
            template.SetContent(x, new MemoryStream());
            Assert.IsNotNull(template.GetContent(x));

            Assert.IsNull(template.GetContentType(x));
            template.SetContentType(x, "application/json");
            Assert.IsNull(template.GetContentType(x));

            var myFileType = (MyFileType)template.CopyToTemplatedInstance(new FileContent()
            {
                Content = new MemoryStream(new byte[4])
            });
            Assert.AreEqual(new MemoryStream(new byte[4]), myFileType.Content);
        }

        /// <summary>
        /// Tests the type store
        /// </summary>
        [Test]
        public void TestNotARequest()
        {
            var x = (object)new NotARequest();
            var template = HttpRequestTemplate.GetTemplate(x.GetType());
            Assert.IsFalse(template.IsTemplateType);

        }
    }
}
