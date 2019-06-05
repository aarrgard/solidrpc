using System;
using System.IO;
using System.Linq;

namespace SolidRpc.Tests
{
    /// <summary>
    /// 
    /// </summary>
    public class TestBase
    {
        /// <summary>
        /// 
        /// </summary>
        protected Stream GetManifestResource(string resourceName)
        {
            var resName = GetType().Assembly.GetManifestResourceNames().Single(o => o.EndsWith(resourceName));
            return GetType().Assembly.GetManifestResourceStream(resName);
        }
        /// <summary>
        /// 
        /// </summary>
        protected DirectoryInfo GetProjectFolder(string projectName)
        {
            var dir = new DirectoryInfo(".");
            while (dir.Parent != null)
            {
                if (dir.Parent.Name == projectName)
                {
                    return dir.Parent;
                }
                dir = dir.Parent;
            }
            throw new Exception("Cannot find project folder:" + projectName);
        }
    }
}
