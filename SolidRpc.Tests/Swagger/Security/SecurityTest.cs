using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using SolidRpc.Abstractions.OpenApi.Binder;
using SolidRpc.Abstractions.OpenApi.Proxy;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace SolidRpc.Tests.Swagger.Binder
{
    /// <summary>
    /// Tests swagger functionality.
    /// </summary>
    public class SecurityTest : TestBase
    {
        public interface ITestInterface
        {
            Task<string> DoXAsync(CancellationToken cancellationToken);
        }

        /// <summary>
        /// 
        /// </summary>
        [Test]
        public void TestAddSecurityKey()
        {
            var services = new ServiceCollection();
            services.AddLogging(ConfigureLogging);
            services.AddSingleton<IConfiguration>(new ConfigurationBuilder().Build());
            services.GetSolidConfigurationBuilder().SetGenerator<SolidProxy.GeneratorCastle.SolidProxyCastleGenerator>();
            var spec = services.GetSolidRpcOpenApiParser().CreateSpecification(typeof(ITestInterface));
            spec.Operations.ToList().ForEach(o => o.AddSolidRpcSecurityKey("test"));
            var template = GetManifestResourceAsString(nameof(TestAddSecurityKey) + ".json");
            var strSpec = spec.WriteAsJsonString(true);
            
            var re = new Regex(@"This OpenApi specification was generated from compiled code on (.*) \d\d\d\d-\d\d-\d\d \d\d:\d\d:\d\d");
            strSpec = re.Replace(strSpec, "");
            
            re = new Regex(@"""version"": ""(\d+).(\d+).(\d+).(\d+)""");
            strSpec = re.Replace(strSpec, @"""version"": """"");
            
            Assert.AreEqual(template, strSpec);
        }
    }
}
