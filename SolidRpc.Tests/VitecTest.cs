using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using SolidProxy.GeneratorCastle;
using SolidRpc.OpenApi.AzQueue.Services;
using System;
using System.Threading.Tasks;
using SolidRpc.Abstractions.OpenApi.Proxy;
using SolidRpc.OpenApi.AzQueue.Invoker;
using SolidRpc.Abstractions.Serialization;
using SolidRpc.Abstractions.Types;
using System.IO;
using System.Text.RegularExpressions;
using System.Net.Http;
using SolidRpc.OpenApi.Binder.Http;
using SolidRpc.Abstractions.OpenApi.Http;

namespace SolidRpc.Tests
{
    /// <summary>
    /// Tests the type store
    /// </summary>
    public class VitecTest : TestBase
    {
        /// <summary>
        /// Test class
        /// </summary>
        public class JsonInfo
        {
            /// <summary>
            /// The container
            /// </summary>
            public string Container { get; set; }

            /// <summary>
            /// The data type
            /// </summary>
            public string DataType { get; set; }

            /// <summary>
            /// The id
            /// </summary>
            public string Id { get; set; }

            /// <summary>
            /// The ETag
            /// </summary>
            public string ETag { get; set; }
        }
        /// <summary>
        /// Test class
        /// </summary>
        public class JsonBlob : JsonInfo
        {
            /// <summary>
            /// The md5 hash of the content
            /// </summary>
            public string MD5Base64 { get; set; }

            /// <summary>
            /// The time when the blob was created
            /// </summary>
            public DateTimeOffset VersionTimestamp { get; set; }

            /// <summary>
            /// The json.
            /// </summary>
            public string Json { get; set; }
        } 

        /// <summary>
        /// Tests the type store
        /// </summary>
        [Test, Ignore("Live")]
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

            sc.AddSolidRpcAzTableQueue("AzureWebJobsStorage", "none", config =>
            {
                config.SetQueueTransport<AzTableHandler>(Abstractions.OpenApi.Transport.InvocationStrategy.Invoke, "AzureWebJobsStorage", "rdbms");
                return true;
            });
            //sc.AddVitecBackendServiceProvider();

            var sp = sc.BuildServiceProvider();


            var cf = sp.GetRequiredService<IHttpClientFactory>();
            var client = cf.CreateClient();
            client.DefaultRequestHeaders.Add("securitykey", "4327a198-49e6-473b-a028-fc03a7b0bc83");
            var resp = await client.GetAsync("https://eo-prd-vitec-func.azurewebsites.net/front/EO/Vitec/Services/IImage/GetImageAsync/MED2CEF19EC02964E1886DD11172DAC9CAA/true");

            var etag = resp.Headers.GetValues("ETag");
            var r2 = new SolidHttpResponse();
            await r2.CopyFromAsync(resp);


            var tq = sp.GetRequiredService<IAzTableQueue>();
            var serFact = sp.GetRequiredService<ISerializerFactory>();
            var msgs = await tq.ListMessagesAsync();
            foreach(var msg in msgs)
            {
                try
                {
                    if (msg.Status != "Error") continue;
                    HttpRequest httpRequest;
                    serFact.DeserializeFromString(msg.Message, out httpRequest);

                    JsonBlob jsonBlob;
                    serFact.DeserializeFromStream(httpRequest.Body, out jsonBlob);

                    if (jsonBlob.Id != null)
                    {
                        continue;
                    }
                    if (jsonBlob.Json == null)
                    {
                        continue;
                    }
                    jsonBlob.Id = ExtractIdFromJson(jsonBlob.DataType, jsonBlob.Json);

                    var ms = new MemoryStream();
                    serFact.SerializeToStream(ms, jsonBlob);
                    httpRequest.Body = new MemoryStream(ms.ToArray());

                    string s;
                    serFact.SerializeToString(out s, httpRequest);

                    msg.Message = s;
                    msg.Status = "Pending";

                    await tq.UpdateMessageAsync(msg);
                } 
                catch(Exception)
                {
                    // continue;
                }
            }

            //var estateService = sp
            //    .GetRequiredService<IVitecBackendServiceProvider>()
            //    .GetRequiredService<IEstate>();
            //var house = await estateService.EstateGetHousingCooperative("OBJ20965_1767989848");

            //var statuses = await estateService.EstateGetStatuses();
            //var lst = await estateService.EstateGetEstateList(new Test.Vitec.Types.Criteria.Estate.EstateCriteria()
            //{
            //    Statuses = statuses.Where(o => o.Id == "3").ToList()
            //});
        }

        private string ExtractIdFromJson(string dataType, string json)
        {
            if(string.IsNullOrEmpty(json))
            {
                throw new Exception("Cannot extract id for type:" + dataType + ":" + json);
            }
            Regex re;
            switch(dataType)
            {
                case "EO.Vitec.Types.Contact.Models.Person":
                    re = new Regex("\"contactId\":\"([^\"]+)\"");
                    break;
                case "EO.Vitec.Types.Contact.Models.Estate":
                    re = new Regex("\"contactId\":\"([^\"]+)\"");
                    break;
                case "EO.Vitec.Types.Contact.Models.Company":
                    re = new Regex("\"contactId\":\"([^\"]+)\"");
                    break;
                case "EO.Vitec.Types.Bid.Models.Bid":
                    re = new Regex("\"id\":\"([^\"]+)\"");
                    break;
                case "EO.Vitec.Types.Estate.Models.Farm":
                    re = new Regex("\"estateId\":\"([^\"]+)\"");
                    break;
                case "EO.Vitec.Types.Estate.Models.House":
                    re = new Regex("\"estateId\":\"([^\"]+)\"");
                    break;
                case "EO.Vitec.Types.Estate.Models.HousingCooperative":
                    re = new Regex("\"estateId\":\\s*\"([^\"]+)\"");
                    break;
                case "EO.Vitec.Types.Estate.Models.Plot":
                    re = new Regex("\"estateId\":\\s*\"([^\"]+)\"");
                    break;
                case "EO.Vitec.Types.ImageInfo":
                    re = new Regex("\"Id\":\\s*\"([^\"]+)\"");
                    break;
                case "EO.Vitec.Types.Meeting.Models.Meeting":
                    re = new Regex("\"id\":\\s*\"([^\"]+)\"");
                    break;
                case "EO.Vitec.Types.Office.Models.Office":
                    re = new Regex("\"officeId\":\\s*\"([^\"]+)\"");
                    break;
                case "EO.Vitec.Types.User.Models.User":
                    re = new Regex("\"userId\":\\s*\"([^\"]+)\"");
                    break;
                default:
                    throw new Exception("Cannot extract id for type:" + dataType + ":" + json);
            }

            var match = re.Match(json);
            if(!match.Success)
            {
                throw new Exception("Cannot extract id for type:" + dataType + ":" + json);
            }
            var id = match.Groups[1].Value;
            return id;
        }

        private object CreateClient()
        {
            throw new NotImplementedException();
        }
    }
}
