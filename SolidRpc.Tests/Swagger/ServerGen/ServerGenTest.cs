using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NuGet.Frameworks;
using NUnit.Framework;
using RA.Mspecs.Services;
using RA.Mspecs.Types.Contact;
using SolidRpc.Abstractions.OpenApi.Invoker;
using SolidRpc.OpenApi.DotNetTool;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace SolidRpc.Tests.Swagger.ServerGen
{
    /// <summary>
    /// Tests swagger functionality.
    /// </summary>
    public class GenTest : WebHostMvcTest
    {
        public class ContactImpl : IContact
        {
            public Task<Contact> GetContactAsync(string id, bool? useCache = null, CancellationToken cancellationToken = default)
            {
                return Task.FromResult(new Contact() { 
                    Id = id, 
                    FirstName = InvocationOptions.Current.TransportType,
                    Identifier = InvocationOptions.Current.Priority.ToString() 
                });
            }

            public Task<Contact> UpsertContactAsync(Contact contact, CancellationToken cancellationToken = default)
            {
                return Task.FromResult(contact);
            }
        }

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
                //if(subDir.Name != "TestProject2") { continue; }
                CreateSpec(subDir.Name, false, subDir.Name == "TestProject1");
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

        private void CreateSpec(string folderName, bool onlyCompare, bool fixProjectBase)
        {
            try
            {
                var dir = GetProjectFolder(GetType().Assembly.GetName().Name);
                //
                // locate assets file
                //
                var obj = new DirectoryInfo(Path.Combine(dir.FullName, "obj"));
                var assetsFile = new FileInfo(Path.Combine(obj.FullName, "project.assets.json"));
                if (!assetsFile.Exists)
                {
                    throw new Exception("Cannot find the assets file");
                }

                // make sure that the obj folder exists
                dir = GetServerGenFolder(folderName);
                obj = new DirectoryInfo(Path.Combine(dir.FullName, "obj"));
                obj.Create();
                if (folderName == "TestProject1")
                {
                    assetsFile.CopyTo(Path.Combine(obj.FullName, assetsFile.Name), true);
                }

                var bindingsFile = $"{dir.Name}.cs";

                Program.MainWithExeptions(new[] {
                "-serverbindings",
                "-d", dir.FullName,
                "-only-compare", onlyCompare.ToString(),
                "-BasePath", $".{GetType().Assembly.GetName().Name}.Swagger.ServerGen.{dir.Name}".Replace('.','/'),
                "-ProjectNamespace", $"{GetType().Assembly.GetName().Name}.Swagger.ServerGen.{dir.Name}",
                "-ProjectBaseFix", fixProjectBase ? "../../.." : "",
                bindingsFile}).Wait();
            }
            catch (Exception e)
            {
                Console.Out.WriteLine(e);
                throw;
            }
        }

        /// <summary>
        /// Tests generating swagger file from code
        /// </summary>
        [Test]
        public async Task TestProject2NoImplementation()
        {
            var sc = new ServiceCollection();
            sc.AddRAMspecs(conf =>
            {
                return conf;
            });
            var sp = sc.BuildServiceProvider();
            var cs = sp.GetRequiredService<IContact>();
            try
            {
                var c = await cs.GetContactAsync("test");
                Assert.Fail();
            }
            catch (Exception e)
            {
                Assert.AreEqual("No implementation registered for service RA.Mspecs.Services.IContact", e.Message);
            }
        }

        /// <summary>
        /// Tests generating swagger file from code
        /// </summary>
        [Test]
        public async Task TestProject2Assembly()
        {
            var intercepted = new List<MethodInfo>();
            var sc = new ServiceCollection();
            sc.AddRAMspecs(conf =>
            {
                if (conf.ProxyType == typeof(IContact))
                {
                    return conf.SetAssemblyFactory(GetType().Assembly);
                }
                return conf;
            });
            sc.ConfigureInterface<IContact>(conf => conf.AddInterceptor((next, sp, impl, mi, args) =>
            {
                intercepted.Add(mi);
                return next();
            }));
            var sp = sc.BuildServiceProvider();
            var cs = sp.GetRequiredService<IContact>();
            var c = await cs.GetContactAsync("test");
            Assert.AreEqual("test", c.Id);
            Assert.AreEqual("Local", c.FirstName);
            Assert.AreEqual("5", c.Identifier);
            Assert.AreEqual(nameof(IContact.GetContactAsync), intercepted[0].Name);

            var ci = sp.GetRequiredService<IInvoker<IContact>>();
            c = await ci.InvokeAsync(o => o.GetContactAsync("test", false, CancellationToken.None), opts => opts.SetPriority(1).SetTransport("Queue"));
            Assert.AreEqual("test", c.Id);
            Assert.AreEqual("Queue", c.FirstName);
            Assert.AreEqual("1", c.Identifier);
            Assert.AreEqual(nameof(IContact.GetContactAsync), intercepted[1].Name);
        }
    }
}
