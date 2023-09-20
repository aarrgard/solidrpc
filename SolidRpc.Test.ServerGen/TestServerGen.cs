using SolidRpc.OpenApi.Binder;
using SolidRpc.OpenApi.DotNetTool;
using SolidRpc.OpenApi.Generator.Types.Project;
using System.Reflection;

namespace SolidRpc.Test.ServerGen
{
    public class TestServerGen
    {

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TestGenerateCode()
        {
            var dir = Program.GetProjectFolder(GetType().Assembly.GetName().Name);
            var bindingsFile = $"{dir.Name}.cs";
            var fixProjectBase = false;
            var onlyCompare = false;

            // copy the SwaggerUI dll to the bin folder
            var swaggerUIDir = Program.GetProjectFolder("SolidRpc.OpenApi.SwaggerUI");
            if (!(swaggerUIDir?.Exists ?? false)) throw new Exception();
            var swagggerUIDll = Program.FindFile(swaggerUIDir.GetDirectories("bin").First(), "SolidRpc.OpenApi.SwaggerUI.dll");
            if (!(swagggerUIDll?.Exists ?? false)) throw new Exception();
            var thisDll = Program.FindFile(dir.GetDirectories("bin").First(), $"{typeof(TestServerGen).Assembly.GetName().Name}.dll");
            if (!(thisDll?.Exists ?? false)) throw new Exception();
            swagggerUIDll = swagggerUIDll.CopyTo(Path.Combine(thisDll.DirectoryName, swagggerUIDll.Name), true);

            var assetsJson = Program.FindFile(dir.GetDirectories("obj").First(), $"project.assets.json");
            if (!(assetsJson?.Exists ?? false)) throw new Exception();
            AddDllToAssetsJson(assetsJson, swagggerUIDll);


            Program.MainWithExeptions(new[] {
                "-serverbindings",
                "-d", dir.FullName,
                "-only-compare", onlyCompare.ToString(),
                "-BasePath", $".{GetType().Assembly.GetName().Name}.Swagger.ServerGen.{dir.Name}".Replace('.','/'),
                "-ProjectNamespace", $"{GetType().Assembly.GetName().Name}.Swagger.ServerGen.{dir.Name}",
                "-ProjectBaseFix", fixProjectBase ? "../../.." : "",
                bindingsFile}).Wait();
        }

        private void AddDllToAssetsJson(FileInfo assetsJson, FileInfo dll)
        {
            var ass = Assembly.LoadFrom(dll.FullName);
            if (!assetsJson.Exists) throw new ArgumentException("Cannot locate assets file");
            ProjectAssets assets;
            using (var fs = assetsJson.OpenRead())
            {
                assets = JsonHelper.Deserialize<ProjectAssets>(fs);
            }
            var key = ass.GetName().Name + "/" + ass.GetName().Version;
            foreach(var target in assets.Targets.Keys.ToList())
            {
                var refs = assets.Targets[target];
                refs[key] = new ProjectAssetsTargetRef()
                {
                    Type = "project",
                    Compile = new ProjectAssetsTargetRefRuntimes()
                    {
                        { $"bin/placeholder/{dll.Name}", new ProjectAssetsTargetRefRuntime() }
                    },
                    Runtime = new ProjectAssetsTargetRefRuntimes()
                    {
                        { $"bin/placeholder/{dll.Name}", new ProjectAssetsTargetRefRuntime() }
                    }
                };
            }
        }
    }
}