using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using SolidRpc.Abstractions.OpenApi.OAuth2;
using System;
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
        /// <summary>
        /// A test interface
        /// </summary>
        public interface ITestInterface
        {
            /// <summary>
            /// A test method
            /// </summary>
            /// <param name="cancellationToken"></param>
            /// <returns></returns>
            Task<string> DoXAsync(CancellationToken cancellationToken);

            /// <summary>
            /// A test method
            /// </summary>
            /// <param name="cancellationToken"></param>
            /// <returns></returns>
            Task<string> DoYAsync(CancellationToken cancellationToken);
        }

        /// <summary>
        /// 
        /// </summary>
        [Test]
        public async Task TestAddSecurityDefs()
        {
            var services = new ServiceCollection();
            services.AddLogging(ConfigureLogging);
            services.AddSingleton<IConfiguration>(new ConfigurationBuilder().Build());
            services.GetSolidConfigurationBuilder().SetGenerator<SolidProxy.GeneratorCastle.SolidProxyCastleGenerator>();
            services.AddSolidRpcOAuth2();

            AddHttpClient(services, uri =>
            {
                switch(uri.ToString())
                {
                    case "http://localhost/.well-known/openid-configuration":
                        return GetManifestResourceAsString(nameof(TestAddSecurityDefs) + ".well-known.json");
                    default:
                        return null;
                }
            });
            var sp = services.BuildServiceProvider();
            var authority = sp.GetRequiredService<IAuthorityFactory>().GetAuthority("http://localhost/");
            var docs = await authority.GetDiscoveryDocumentAsync();
            var spec = services.GetSolidRpcOpenApiParser().CreateSpecification(typeof(ITestInterface));
            spec.Operations.ToList().ForEach(o =>
            {
                if(o.OperationId.EndsWith("#DoXAsync"))
                {
                    o.AddApiKeyAuth("security-definition1", "security-header1");
                    o.AddOAuth2Auth(docs, "accessCode", new[] { "scope1" });
                }
                else
                {
                    o.AddOAuth2Auth(docs, "accessCode", new[] { "scope2" });
                    o.AddApiKeyAuth("security-definition2", "security-header2");
                }
            });
            var template = GetManifestResourceAsString(nameof(TestAddSecurityDefs) + ".json");
            var strSpec = spec.WriteAsJsonString(true);

            var re = new Regex(@"This OpenApi specification was generated from compiled code on (.*) \d\d\d\d-\d\d-\d\d \d\d:\d\d:\d\d");
            strSpec = re.Replace(strSpec, "");

            re = new Regex(@"""version"": ""(\d+).(\d+).(\d+).(\d+)""");
            strSpec = re.Replace(strSpec, @"""version"": """"");

            Assert.AreEqual(template, strSpec);
        }
    }
}
