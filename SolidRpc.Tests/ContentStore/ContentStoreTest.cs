using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using SolidProxy.GeneratorCastle;
using SolidRpc.Abstractions.InternalServices;
using SolidRpc.Abstractions.Services;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace SolidRpc.Tests
{
    /// <summary>
    /// Tests the template types
    /// </summary>
    public class ContentStoreTest : TestBase
    {
 
        /// <summary>
        /// Tests the type store
        /// </summary>
        [Test]
        public async Task TestContentStore()
        {
            var cb = new ConfigurationBuilder();
            cb.AddInMemoryCollection(new Dictionary<string, string>()
            {
            });
            var sc = new ServiceCollection();
            sc.AddSingleton<IConfiguration>(cb.Build());
            sc.GetSolidConfigurationBuilder().SetGenerator<SolidProxyCastleGenerator>();
            sc.AddLogging();
            sc.AddSolidRpcServices();
            sc.GetSolidRpcService<ISolidRpcContentStore>().AddContent(GetType().Assembly, "ContentStore.www.images.test-override1", "/images/");
            sc.GetSolidRpcService<ISolidRpcContentStore>().AddContent(GetType().Assembly, "ContentStore.www.images.test-override2", "/images/");
            sc.GetSolidRpcService<ISolidRpcContentStore>().AddContent(GetType().Assembly, "ContentStore.www", "/");
            var sp = sc.BuildServiceProvider();

            var ch = sp.GetRequiredService<ISolidRpcContentHandler>();
            var c1 = await ch.GetContent("/images/test.html");
            var c2 = await ch.GetContent("/images/override.html");
            using(var sr = new StreamReader(c2.Content))
            {
                Assert.AreEqual("overridden", sr.ReadToEnd());
            }
        }

    }
}
