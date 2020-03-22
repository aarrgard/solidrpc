using NUnit.Framework;
using SolidRpc.Abstractions.Types;
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
        /// <summary>
        /// Tests the type store
        /// </summary>
        [Test]
        public async Task TestFileContentTemplateType()
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
        public async Task TestMyFileType()
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
        }
    }
}
