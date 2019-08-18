using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using SolidProxy.GeneratorCastle;
using SolidRpc.Abstractions.OpenApi.Binder;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;
using SolidRpc.Security.Services.OAuth2.Microsoft;
using SolidRpc.OpenApi.Binder.Proxy;
using System.Linq;
using SolidRpc.Security.Services.OAuth2.Google;

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
        [Test]
        public async Task TestMicrosoftLogin()
        {
            var sc = new ServiceCollection();
            sc.AddLogging(ConfigureLogging);
            sc.AddSingleton<IConfiguration>(new ConfigurationBuilder().Build());
            sc.GetSolidConfigurationBuilder().SetGenerator<SolidProxyCastleGenerator>();
            sc.AddSolidRpcSecurity();

            var sp = sc.BuildServiceProvider();
            var binder = sp.GetRequiredService<IMethodBinderStore>();
            var clientId = Guid.Parse("615993a8-66b3-40ce-a165-96a81edd3677");
            var response_type = new[] { "id_token" };
            var nounce = "234324";
            var scopes = new[] { "openid" };
            var redirect_uri = new Uri("https://arr1-petstore.azurewebsites.net/front/microsoft/login");
            var response_mode = "form_post";
            var state = "234322";
            var url = await binder.GetUrlAsync<IOAuth2Microsoft>(o => o.Authorize("common", clientId, response_type, scopes, nounce, redirect_uri, response_mode, state, null, null, null, CancellationToken.None));
            //Process.Start(new ProcessStartInfo("C:\\Program Files (x86)\\Google\\Chrome\\Application\\chrome.exe", url.ToString()));
            Assert.AreEqual("https://login.microsoftonline.com/common/oauth2/authorize?client_id=615993a8-66b3-40ce-a165-96a81edd3677&response_type=id_token&scope=openid&nounce=234324&redirect_uri=https://arr1-petstore.azurewebsites.net/front/microsoft/login&response_mode=form_post&state=234322", url.ToString());
        }
        [Test]
        public async Task TestMicrosoftDiscovery()
        {
            var sc = new ServiceCollection();
            sc.AddLogging(ConfigureLogging);
            sc.AddHttpClient();
            sc.AddSingleton<IConfiguration>(new ConfigurationBuilder().Build());
            sc.GetSolidConfigurationBuilder().SetGenerator<SolidProxyCastleGenerator>();
            sc.AddSolidRpcSecurity();
            sc.GetSolidConfigurationBuilder().AddAdvice(typeof(SolidRpcOpenApiAdvice<,,>));

            var sp = sc.BuildServiceProvider();

            // root doc
            var rootDoc = await sp.GetRequiredService<IOAuth2Microsoft>().OpenIdConfiguration("common");
            Assert.AreEqual(new Uri("https://login.microsoftonline.com/{tenantid}/v2.0"), rootDoc.Issuer);

            var keys = await sp.GetRequiredService<IOAuth2Microsoft>().OpenIdKeys("common");
            Assert.IsTrue(keys.Keys.Count() > 0);
        }
        [Test]
        public async Task TestGoogleDiscovery()
        {
            var sc = new ServiceCollection();
            sc.AddLogging(ConfigureLogging);
            sc.AddHttpClient();
            sc.AddSingleton<IConfiguration>(new ConfigurationBuilder().Build());
            sc.GetSolidConfigurationBuilder().SetGenerator<SolidProxyCastleGenerator>();
            sc.AddSolidRpcSecurity();
            sc.GetSolidConfigurationBuilder().AddAdvice(typeof(SolidRpcOpenApiAdvice<,,>));

            var sp = sc.BuildServiceProvider();

            // root doc
            var rootDoc = await sp.GetRequiredService<IOAuth2Google>().OpenIdConfiguration();
            Assert.AreEqual(new Uri("https://accounts.google.com/"), rootDoc.Issuer);

            var keysBinding = sp.GetRequiredService<IMethodBinderStore>().GetMethodBinding<IOAuth2Google>(o => o.OpenIdKeys(CancellationToken.None));
            keysBinding.Address = rootDoc.JwksUri;
            //rootDoc.Jwks_uri

            var keys = await sp.GetRequiredService<IOAuth2Google>().OpenIdKeys();
            Assert.IsTrue(keys.Keys.Count() > 0);
        }
    }
}
