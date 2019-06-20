using NUnit.Framework;
using SolidRpc.OpenApi.DotNetTool;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace SolidRpc.Tests.Swagger.CodeGen
{
    /// <summary>
    /// Tests swagger functionality.
    /// </summary>
    public class CodeGenTest : SwaggerTestBase
    {
        /// <summary>
        /// Returns the spec folder
        /// </summary>
        /// <param name="folderName"></param>
        /// <returns></returns>
        protected override DirectoryInfo GetSpecFolder(string folderName)
        {
            var path = GetProjectFolder(GetType().Assembly.GetName().Name).FullName;
            path = Path.Combine(path, "Swagger");
            path = Path.Combine(path, "CodeGen");
            path = Path.Combine(path, folderName);
            return new DirectoryInfo(path);
        }

        /// <summary>
        /// Tests generating code from a swagger file
        /// </summary>
        [Test]
        public async Task TestCodeGenLocal()
        {
            string swaggerSpec;
            using (var ctx = await StartTestHostContextAsync())
            {
                var resp = await ctx.GetResponse("/swagger/v1/swagger.json");
                swaggerSpec = await AssertOk(resp);
           }

            var path = GetProjectFolder(GetType().Assembly.GetName().Name).FullName;
            path = Path.Combine(path, "Swagger");
            path = Path.Combine(path, "CodeGen");
            path = Path.Combine(path, "Local");
            path = Path.Combine(path, "swagger.json");
            var fi = new FileInfo(path);
            using (var tw = fi.CreateText())
            {
                await tw.WriteAsync(swaggerSpec);
            }           
        }

        /// <summary>
        /// Tests generating code from a swagger file
        /// </summary>
        [Test]
        public void TestCodeGen()
        {
            var path = GetProjectFolder(GetType().Assembly.GetName().Name).FullName;
            path = Path.Combine(path, "Swagger");
            path = Path.Combine(path, "CodeGen");
            var dir = new DirectoryInfo(path);
            Assert.IsTrue(dir.Exists);
            foreach(var subDir in dir.GetDirectories())
            {
                CreateCode(subDir, false);
            }
        }

        private void CreateCode(DirectoryInfo dir, bool onlyCompare)
        {
            var openApiFiles = dir.GetFiles("*.json");
            if(openApiFiles.Length != 1)
            {
                Assert.Fail("Cannot find json file in folder:" + dir.FullName);
            }
            try
            {
                Program.MainWithExeptions(new[] {
                "-openapi2code",
                "-d", dir.FullName,
                "-only-compare", onlyCompare.ToString(),
                "-ProjectNamespace", $"{GetType().Assembly.GetName().Name}.Swagger.CodeGen.{dir.Name}",
                openApiFiles[0].Name }).Wait();
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
       }

        /// <summary>
        /// Tests invoking the generated proxy.
        /// </summary>
        [Test]
        public async Task TestPetStore()
        {
            using (var ctx = CreateTestHostContext())
            {
                var config = ReadOpenApiConfiguration(nameof(TestPetStore).Substring(4));
                var api_key = "sfasdfoewr0sdf";
                var pet = new Petstore.Types.Pet()
                {
                    Status = "test",
                    Id = 22314,
                    Category = new Petstore.Types.Category()
                    {
                        Id = 334,
                        Name = "test"
                    },
                    Name = "Kalle",
                    Tags = new Petstore.Types.Tag[] {
                        new Petstore.Types.Tag()
                        {
                            Id = 23123,
                            Name = "Tag1"
                        }
                    },
                    PhotoUrls = new string[] { "url1", "url2" }
                };

                // await proxy.AddPet(pet);
                ctx.CreateServerInterceptor<Petstore.Services.IPet>(
                    o => o.AddPet(null, CancellationToken.None),
                    config,
                    args =>
                    {
                        Assert.AreEqual(2, args.Length);
                        CompareStructs(pet, args[0]);
                        Assert.AreEqual(CancellationToken.None, args[1]);
                        return Task.CompletedTask;
                    });

                // await proxy.DeletePet(api_key, pet.Id);
                ctx.CreateServerInterceptor<Petstore.Services.IPet>(
                    o => o.DeletePet(null, 0, CancellationToken.None),
                    config,
                    args =>
                    {
                        Assert.AreEqual(3, args.Length);
                        CompareStructs(api_key, args[0]);
                        CompareStructs(pet, args[1]);
                        CompareStructs(CancellationToken.None, args[2]);
                        return Task.CompletedTask;
                    });


                await ctx.StartAsync();
                var proxy = CreateProxy<Petstore.Services.IPet>(ctx.BaseAddress, config);

                await proxy.AddPet(pet);
                await proxy.DeletePet(api_key, pet.Id);
            }
        }

        /// <summary>
        /// Tests invoking the generated proxy.
        /// </summary>
        [Test]
        public async Task TestUrlParam()
        {
            using (var ctx = CreateTestHostContext())
            {
                var config = ReadOpenApiConfiguration(nameof(TestUrlParam).Substring(4));

                // await proxy.DeletePet(api_key, pet.Id);
                ctx.CreateServerInterceptor<UrlParam.Services.IUrlParam>(
                    o => o.ProxyIntegerInPath(1, CancellationToken.None),
                    config,
                    args =>
                    {
                        CompareStructs(3, args[0]);
                        CompareStructs(CancellationToken.None, args[1]);
                        return Task.CompletedTask;
                    });


                await ctx.StartAsync();
                var proxy = CreateProxy<UrlParam.Services.IUrlParam>(ctx.BaseAddress, config);

                await proxy.ProxyIntegerInPath(3);
            }
        }
    }
}
