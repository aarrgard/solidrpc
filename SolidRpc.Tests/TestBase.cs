using System.IO;
using System.Linq;

namespace SolidRpc.Tests
{
    public class TestBase
    {
        protected Stream GetManifestResource(string resourceName)
        {
            var resName = GetType().Assembly.GetManifestResourceNames().Single(o => o.EndsWith(resourceName));
            return GetType().Assembly.GetManifestResourceStream(resName);
        }
    }
}
