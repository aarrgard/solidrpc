using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using SolidProxy.GeneratorCastle;
using SolidRpc.Abstractions.OpenApi.Proxy;
using SolidRpc.Test.Vitec.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SolidRpc.Tests
{
    /// <summary>
    /// Tests the type store
    /// </summary>
    public class VitecTest : TestBase
    {

        /// <summary>
        /// Tests the type store
        /// </summary>
        [Test, Ignore("Needs settings")]
        public async Task TestStartProject()
        {
            var cb = new ConfigurationBuilder();
            cb.AddJsonFile("appsettings.local.json", false);
            var conf = cb.Build();

            //
            // configure the client
            //
            var sc = new ServiceCollection();

            // copy the "urls" setting
            sc.AddSingleton<IConfiguration>(cb.Build());

            sc.AddHttpClient();
            sc.AddLogging(ConfigureLogging);
            sc.GetSolidConfigurationBuilder().SetGenerator<SolidProxyCastleGenerator>();
            sc.AddSolidRpcBindings(typeof(IEstate).Assembly);
            sc.GetSolidConfigurationBuilder()
                .ConfigureInterfaceAssembly(typeof(IEstate).Assembly)
                .ConfigureAdvice<ISolidRpcOpenApiConfig>()
                .SecurityKey = new KeyValuePair<string, string>("Authorization", conf["VitecConnectAuthorization"]);

            var sp = sc.BuildServiceProvider();
            var estateService = sp.GetRequiredService<IEstate>();
            var house = await estateService.EstateGetHousingCooperative("OBJ20965_1767989848");
        }

    }
}
