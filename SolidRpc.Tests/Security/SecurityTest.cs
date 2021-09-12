using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using SolidProxy.GeneratorCastle;
using System.Threading.Tasks;
using System.Linq;
using SolidRpc.Abstractions.OpenApi.OAuth2;

namespace SolidRpc.Tests.Security
{
    /// <summary>
    /// Tests security functionality.
    /// </summary>
    public class SecurityTest : TestBase
    {
        ///// <summary>
        ///// Tests that the petstore json file is processed correctly.
        ///// </summary>
        //[Test]
        //public async Task TestMicrosoftLogin()
        //{
        //    var sc = new ServiceCollection();
        //    sc.AddLogging(ConfigureLogging);
        //    sc.AddHttpClient();
        //    sc.AddSingleton<IConfiguration>(new ConfigurationBuilder().Build());
        //    sc.GetSolidConfigurationBuilder().SetGenerator<SolidProxyCastleGenerator>();
        //    sc.AddSolidRpcSecurityBackendMicrosoft((_, conf) => _.ConfigureOptions(conf));
        //    sc.GetSolidConfigurationBuilder().AddAdvice(typeof(SolidRpcOpenApiInitAdvice<,,>));

        //    var sp = sc.BuildServiceProvider();
        //    var httpInvoker = sp.GetRequiredService<IInvoker<IMicrosoftRemote>>();
        //    var clientId = Guid.Parse("615993a8-66b3-40ce-a165-96a81edd3677");
        //    var response_type = new[] { "code" };
        //    var nounce = "234324";
        //    var scopes = new[] { "openid" };
        //    var redirect_uri = new Uri("https://arr1-petstore.azurewebsites.net/front/microsoft/login");
        //    var response_mode = "query";
        //    var state = "234322";
        //    var url = await httpInvoker.GetUriAsync(o => o.Authorize("common", clientId, response_type, redirect_uri, response_mode, scopes, state, nounce, null, null, null, null, CancellationToken.None));
        //    //Process.Start(new ProcessStartInfo("C:\\Program Files (x86)\\Google\\Chrome\\Application\\chrome.exe", url.ToString()));
        //    Assert.AreEqual("https://login.microsoftonline.com/common/oauth2/authorize?client_id=615993a8-66b3-40ce-a165-96a81edd3677&response_type=code&redirect_uri=https%3a%2f%2farr1-petstore.azurewebsites.net%2ffront%2fmicrosoft%2flogin&response_mode=query&scope=openid&state=234322&nounce=234324", url.ToString());
        //    //await sp.GetRequiredService<IMicrosoftRemote>().Authorize("common", clientId, response_type, redirect_uri, null, nounce, response_mode, state);
        //}

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
            sc.AddSolidRpcOAuth2();

            var sp = sc.BuildServiceProvider();

            // root doc
            var auth = sp.GetRequiredService<IAuthorityFactory>().GetAuthority("https://login.microsoftonline.com/common/v2.0");
            var keys = await auth.GetSigningKeysAsync();
            Assert.IsTrue(keys.Count() > 0);
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
            sc.AddSolidRpcOAuth2();

            var sp = sc.BuildServiceProvider();

            // root doc
            var auth = sp.GetRequiredService<IAuthorityFactory>().GetAuthority("https://accounts.google.com/");
            var keys = await auth.GetSigningKeysAsync();
            Assert.IsTrue(keys.Count() > 0);

        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        //[Test, Ignore("requires secrets")]
        //public async Task TestFacebook()
        //{
        //    var sc = new ServiceCollection();
        //    sc.AddLogging(ConfigureLogging);
        //    sc.AddHttpClient();

        //    var cb = new ConfigurationBuilder();
        //    cb.AddJsonFile("appsettings_local.json", false);
        //    sc.AddSingleton<IConfiguration>(cb.Build());

        //    sc.GetSolidConfigurationBuilder().SetGenerator<SolidProxyCastleGenerator>();
        //    //sc.AddSolidRpcSecurityFrontend();
        //    sc.GetSolidConfigurationBuilder().AddAdvice(typeof(SolidRpcOpenApiInitAdvice<,,>));

        //    var sp = sc.BuildServiceProvider();

        //    // root doc
        //    var fbConf = sp.GetRequiredService<FacebookOptions>();
        //    var fb = sp.GetRequiredService<IFacebookRemote>();
        //    var at = "EAAjG2EhdEDgBACsHZBhizQDnxXshewlUHTWfjfXJs0khHFZBL7qH9o36XWQDzwLmZAW5dZAmOsx1Y3LUBSxCbbpU5CW9uXIXwjTT1yYqSvK72roN6I6nbvpgdeRhUBF4sXgdZAwVtJjFYlatfuA6j1O0zF8mCMUKQDMH7pnhLBkQrfrDudp0mesyKsrqYpUQEPlStR0eqDgZDZD";
        //    var accessToken = await fb.GetAccessToken(fbConf.AppId, fbConf.AppSecret, "client_credentials");
        //    var debugToken = await fb.GetDebugToken(at, accessToken.AccessToken);
        //    Assert.AreEqual(fbConf.AppId, debugToken.Data.AppId.ToString());
        //}
    }
}
