using NUnit.Framework;
using SolidRpc.OpenApi.DotNetTool;
using System;
using System.IO;

namespace SolidRpc.Tests.Swagger.ServerGen
{
    /// <summary>
    /// Tests swagger functionality.
    /// </summary>
    public class GenTest : WebHostMvcTest
    {

        /// <summary>
        /// Tests generating swagger file from code
        /// </summary>
        [Test]
        public void TestServerGen()
        {
            var path = GetProjectFolder(GetType().Assembly.GetName().Name).FullName;
            path = Path.Combine(path, "Swagger");
            path = Path.Combine(path, "ServerGen");
            var dir = new DirectoryInfo(path);
            Assert.IsTrue(dir.Exists);
            foreach (var subDir in dir.GetDirectories())
            {
                CreateSpec(subDir.Name, false);
            }
        }

        /// <summary>
        /// Returns the spec folder
        /// </summary>
        /// <param name="folderName"></param>
        /// <returns></returns>
        protected DirectoryInfo GetServerGenFolder(string folderName)
        {
            var path = GetProjectFolder(GetType().Assembly.GetName().Name).FullName;
            path = Path.Combine(path, "Swagger");
            path = Path.Combine(path, "ServerGen");
            path = Path.Combine(path, folderName);
            return new DirectoryInfo(path);
        }

        private void CreateSpec(string folderName, bool onlyCompare)
        {
            try
            {
                //
                // locate assets file
                //
                var dir = GetProjectFolder(GetType().Assembly.GetName().Name);
                var obj = new DirectoryInfo(Path.Combine(dir.FullName, "obj"));
                var assetsFile = new FileInfo(Path.Combine(obj.FullName, "project.assets.json"));
                if(!assetsFile.Exists)
                {
                    throw new Exception("Cannot find the assets file");
                }

                // make sure that the obj folder exists
                dir = GetServerGenFolder(folderName);
                obj = new DirectoryInfo(Path.Combine(dir.FullName, "obj"));
                obj.Create();
                assetsFile.CopyTo(Path.Combine(obj.FullName, assetsFile.Name), true);

                var bindingsFile = $"{dir.Name}.cs";

                Program.MainWithExeptions(new[] {
                "-serverbindings",
                "-d", dir.FullName,
                "-only-compare", onlyCompare.ToString(),
                "-BasePath", $".{GetType().Assembly.GetName().Name}.Swagger.ServerGen.{dir.Name}".Replace('.','/'),
                "-ProjectNamespace", $"{GetType().Assembly.GetName().Name}.Swagger.ServerGen.{dir.Name}",
                "-ProjectBaseFix", "../../..",
                bindingsFile}).Wait();
            }
            catch (Exception e)
            {
                Console.Out.WriteLine(e);
                throw;
            }
        }

    }
}
