using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using SolidProxy.GeneratorCastle;
using SolidRpc.Abstractions.OpenApi.Binder;
using System;
using System.Threading;
using System.Threading.Tasks;
using SolidRpc.Security.Services;

namespace SolidRpc.Tests.Swagger
{
    /// <summary>
    /// Tests security functionality.
    /// </summary>
    public class SecurityTest : TestBase
    {
        /// <summary>
        /// Tests that the petstore json file is processed correctly.
        /// </summary>
        [Test,Ignore("fix array bindings")]
        public async Task TestMicrosoft()
        {
            var sc = new ServiceCollection();
            sc.AddSingleton<IConfiguration>(new ConfigurationBuilder().Build());
            sc.GetSolidConfigurationBuilder().SetGenerator<SolidProxyCastleGenerator>();
            sc.AddSolidRpcSecurity();

            var sp = sc.BuildServiceProvider();
            var binder = sp.GetRequiredService<IMethodBinderStore>();
            var clientId = Guid.Parse("615993a8-66b3-40ce-a165-96a81edd3677");
            var response_type = new[] { "id_token" };
            var nounce = "nounce";
            var scopes = new[] { "test" };
            var redirect_uri = new Uri("http://test.com/test");
            var url = await binder.GetUrlAsync<IOAuth2Microsoft>(o => o.Authorize("common", clientId, response_type, scopes, nounce, redirect_uri, null, null, null, null, null, CancellationToken.None));
            Assert.AreEqual("", url);
        }
    }
}
