using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using SolidProxy.GeneratorCastle;
using SolidRpc.Abstractions.OpenApi.Binder;
using System;
using System.Threading;
using System.Threading.Tasks;
using SolidRpc.OpenApi.Binder.Proxy;
using System.Linq;
using SolidRpc.Security.Services.Microsoft;
using SolidRpc.Security.Services.Google;
using SolidRpc.Security.Services.Facebook;
using SolidRpc.Security.Back.Services.Facebook;

namespace SolidRpc.Tests.Security
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
            sc.AddHttpClient();
            sc.AddSingleton<IConfiguration>(new ConfigurationBuilder().Build());
            sc.GetSolidConfigurationBuilder().SetGenerator<SolidProxyCastleGenerator>();
            sc.AddSolidRpcSecurityBackendMicrosoft((_, conf) => _.ConfigureOptions(conf));
            sc.GetSolidConfigurationBuilder().AddAdvice(typeof(SolidRpcOpenApiAdvice<,,>));

            var sp = sc.BuildServiceProvider();
            var binder = sp.GetRequiredService<IMethodBinderStore>();
            var clientId = Guid.Parse("615993a8-66b3-40ce-a165-96a81edd3677");
            var response_type = new[] { "code" };
            var nounce = "234324";
            var scopes = new[] { "openid" };
            var redirect_uri = new Uri("https://arr1-petstore.azurewebsites.net/front/microsoft/login");
            var response_mode = "query";
            var state = "234322";
            var url = await binder.GetUrlAsync<IMicrosoftRemote>(o => o.Authorize("common", clientId, response_type, redirect_uri, scopes, nounce, response_mode, state, null, null, null, null, CancellationToken.None));
            //Process.Start(new ProcessStartInfo("C:\\Program Files (x86)\\Google\\Chrome\\Application\\chrome.exe", url.ToString()));
            Assert.AreEqual("https://login.microsoftonline.com/common/oauth2/authorize?client_id=615993a8-66b3-40ce-a165-96a81edd3677&response_type=code&redirect_uri=https://arr1-petstore.azurewebsites.net/front/microsoft/login&scope=openid&nounce=234324&response_mode=query&state=234322", url.ToString());
            //await sp.GetRequiredService<IMicrosoftRemote>().Authorize("common", clientId, response_type, redirect_uri, null, nounce, response_mode, state);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task TestMicrosoftDiscovery()
        {
            var sc = new ServiceCollection();
            sc.AddLogging(ConfigureLogging);
            sc.AddHttpClient();
            sc.AddSingleton<IConfiguration>(new ConfigurationBuilder().Build());
            sc.GetSolidConfigurationBuilder().SetGenerator<SolidProxyCastleGenerator>();
            sc.AddSolidRpcSecurityBackendMicrosoft((_, conf) => _.ConfigureOptions(conf));
            sc.GetSolidConfigurationBuilder().AddAdvice(typeof(SolidRpcOpenApiAdvice<,,>));

            var sp = sc.BuildServiceProvider();

            // root doc
            var rootDoc = await sp.GetRequiredService<IMicrosoftRemote>().OpenIdConfiguration("common");
            Assert.AreEqual(new Uri("https://login.microsoftonline.com/{tenantid}/v2.0"), rootDoc.Issuer);

            var keys = await sp.GetRequiredService<IMicrosoftRemote>().OpenIdKeys("common");
            Assert.IsTrue(keys.Keys.Count() > 0);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task TestGoogleDiscovery()
        {
            var sc = new ServiceCollection();
            sc.AddLogging(ConfigureLogging);
            sc.AddHttpClient();
            sc.AddSingleton<IConfiguration>(new ConfigurationBuilder().Build());
            sc.GetSolidConfigurationBuilder().SetGenerator<SolidProxyCastleGenerator>();
            sc.AddSolidRpcSecurityBackendGoogle((_, conf) => _.ConfigureOptions(conf));
            sc.GetSolidConfigurationBuilder().AddAdvice(typeof(SolidRpcOpenApiAdvice<,,>));

            var sp = sc.BuildServiceProvider();

            // root doc
            var rootDoc = await sp.GetRequiredService<IGoogleRemote>().OpenIdConfiguration();
            Assert.AreEqual(new Uri("https://accounts.google.com/"), rootDoc.Issuer);

            var keys = await sp.GetRequiredService<IGoogleRemote>().OpenIdKeys();
            Assert.IsTrue(keys.Keys.Count() > 0);

        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Test, Ignore("requires secrets")]
        public async Task TestFacebook()
        {
            var sc = new ServiceCollection();
            sc.AddLogging(ConfigureLogging);
            sc.AddHttpClient();

            var cb = new ConfigurationBuilder();
            cb.AddJsonFile("appsettings_local.json", false);
            sc.AddSingleton<IConfiguration>(cb.Build());

            sc.GetSolidConfigurationBuilder().SetGenerator<SolidProxyCastleGenerator>();
            sc.AddSolidRpcSecurityBackendFacebook((_, conf) => _.ConfigureOptions(conf));
            sc.GetSolidConfigurationBuilder().AddAdvice(typeof(SolidRpcOpenApiAdvice<,,>));

            var sp = sc.BuildServiceProvider();

            // root doc
            var fbConf = sp.GetRequiredService<FacebookOptions>();
            var fb = sp.GetRequiredService<IFacebookRemote>();
            var at = "EAAjG2EhdEDgBACsHZBhizQDnxXshewlUHTWfjfXJs0khHFZBL7qH9o36XWQDzwLmZAW5dZAmOsx1Y3LUBSxCbbpU5CW9uXIXwjTT1yYqSvK72roN6I6nbvpgdeRhUBF4sXgdZAwVtJjFYlatfuA6j1O0zF8mCMUKQDMH7pnhLBkQrfrDudp0mesyKsrqYpUQEPlStR0eqDgZDZD";
            var accessToken = await fb.GetAccessToken(fbConf.AppId, fbConf.AppSecret, "client_credentials");
            var debugToken = await fb.GetDebugToken(at, accessToken.AccessToken);
            Assert.AreEqual(fbConf.AppId, debugToken.Data.AppId.ToString());
        }
    }
}
