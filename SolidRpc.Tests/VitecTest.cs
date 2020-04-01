using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using SolidProxy.GeneratorCastle;
using SolidRpc.Test.Vitec.Impl;
using SolidRpc.Test.Vitec.Services;
using System;
using System.Threading.Tasks;

namespace SolidRpc.Tests
{
    /// <summary>
    /// Tests the type store
    /// </summary>
    public class VitecTest : TestBase
    {

        //public (HttpClient, CookieContainer) CreateHttpClient()
        //{
        //    var affin = "6516714284e2943ad8567792ce493a6239a7682752e69301a9c2147e7cb0a161";
        //    var cookieContainer = new CookieContainer();
        //    cookieContainer.Add(new Cookie("ARRAffinity", affin) { 
        //        Domain = "eo-prd-ratelimit-func.azurewebsites.net",
        //        Path = "/",
        //        HttpOnly = true
        //    });
        //    var httpClient = new HttpClient(new HttpClientHandler()
        //    {
        //        CookieContainer = cookieContainer,
        //        UseCookies = true
        //    });
        //    httpClient.DefaultRequestHeaders.Add("SolidRpcSecurityKey".ToLower(), "3a8f9eec-32b5-4c1d-ba16-8e8fa84ec92d");
        //    //httpClient.DefaultRequestHeaders.Add("Cookie", $"ARRAffinity={affin}");
        //    return (httpClient, cookieContainer);
        //}

        /// <summary>
        /// Tests the type store
        /// </summary>
        [Test, Ignore("Requires password")]
        public async Task TestStartProject()
        {
            //var (httpClient,cookieContainer) = CreateHttpClient();
            //var httpClients = new HttpClient[10];
            //var tasks = new Task<HttpResponseMessage>[httpClients.Length];
            //var cookieContainers = new CookieContainer[httpClients.Length];

            //var uri = new Uri("https://eo-prd-ratelimit-func.azurewebsites.net/front/SolidRpc/Abstractions/Services/ISolidRpcHost/GetHostInstance");
            //for (int i = 0; i < httpClients.Length; i++)
            //{
            //    httpClients[i] = httpClient;
            //    cookieContainers[i] = cookieContainer;

            //    tasks[i] = httpClient.GetAsync(uri);
            //    (httpClient, cookieContainer)  = CreateHttpClient();
            //}

            //for (int i = 0; i < httpClients.Length; i++)
            //{
            //    var resp = await tasks[i];
            //    Console.WriteLine(resp.StatusCode + "-" + cookieContainers[i].GetCookieHeader(uri));
            //    Console.WriteLine(await resp.Content.ReadAsStringAsync());
            //}
            //var cookies = cookieContainer.GetCookies(new Uri("https://eo-prd-ratelimit-func.azurewebsites.net/"));
            //Assert.Fail();


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

        private object CreateClient()
        {
            throw new NotImplementedException();
        }
    }
}
