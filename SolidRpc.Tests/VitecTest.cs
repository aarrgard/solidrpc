using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using SolidProxy.GeneratorCastle;
using SolidRpc.Test.Vitec.Impl;
using SolidRpc.Test.Vitec.Services;
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
        [Test, Ignore("Requires passwords")]
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

            sc.AddVitecBackendServiceProvider();

            var sp = sc.BuildServiceProvider();
            var estateService = sp
                .GetRequiredService<IVitecBackendServiceProvider>()
                .GetRequiredService<IEstate>();
            //var house = await estateService.EstateGetHousingCooperative("OBJ20965_1767989848");

            var statuses = await estateService.EstateGetStatuses();
            //var lst = await estateService.EstateGetEstateList(new Test.Vitec.Types.Criteria.Estate.EstateCriteria()
            //{
            //    Statuses = statuses.Where(o => o.Id == "3").ToList()
            //});
        }

    }
}
