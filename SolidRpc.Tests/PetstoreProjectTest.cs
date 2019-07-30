using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using SolidRpc.Abstractions.OpenApi.Binder;
using SolidRpc.OpenApi.Binder;
using SolidRpc.OpenApi.Proxy;
using SolidRpc.Test.Petstore.Services;
using SolidRpc.Test.Petstore.Types;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace SolidRpc.Tests
{
    /// <summary>
    /// Tests the type store
    /// </summary>
    public class PetstoreProjectTest : TestBase, IStartup
    {
        /// <summary>
        /// 
        /// </summary>
        public class PetImpl : IPet
        {
            /// <summary>
            /// 
            /// </summary>
            /// <param name="body"></param>
            /// <param name="cancellationToken"></param>
            /// <returns></returns>
            public Task AddPet(Pet body, CancellationToken cancellationToken = default(CancellationToken))
            {
                throw new NotImplementedException();
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="api_key"></param>
            /// <param name="petId"></param>
            /// <param name="cancellationToken"></param>
            /// <returns></returns>
            public Task DeletePet(string api_key, long petId, CancellationToken cancellationToken = default(CancellationToken))
            {
                throw new NotImplementedException();
            }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="status"></param>
            /// <param name="cancellationToken"></param>
            /// <returns></returns>
            public Task<IEnumerable<Pet>> FindPetsByStatus(IEnumerable<string> status, CancellationToken cancellationToken = default(CancellationToken))
            {
                throw new NotImplementedException();
            }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="tags"></param>
            /// <param name="cancellationToken"></param>
            /// <returns></returns>
            public Task<IEnumerable<Pet>> FindPetsByTags(IEnumerable<string> tags, CancellationToken cancellationToken = default(CancellationToken))
            {
                throw new NotImplementedException();
            }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="petId"></param>
            /// <param name="cancellationToken"></param>
            /// <returns></returns>
            public Task<Pet> GetPetById(long petId, CancellationToken cancellationToken = default(CancellationToken))
            {
                return Task.FromResult(new Pet()
                {
                    Id = petId
                });
            }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="body"></param>
            /// <param name="cancellationToken"></param>
            /// <returns></returns>
            public Task UpdatePet(Pet body, CancellationToken cancellationToken = default(CancellationToken))
            {
                throw new NotImplementedException();
            }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="petId"></param>
            /// <param name="name"></param>
            /// <param name="status"></param>
            /// <param name="cancellationToken"></param>
            /// <returns></returns>
            public Task UpdatePetWithForm(long petId, string name, string status, CancellationToken cancellationToken = default(CancellationToken))
            {
                throw new NotImplementedException();
            }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="petId"></param>
            /// <param name="additionalMetadata"></param>
            /// <param name="file"></param>
            /// <param name="cancellationToken"></param>
            /// <returns></returns>
            public Task<ApiResponse> UploadFile(long petId, string additionalMetadata, Stream file, CancellationToken cancellationToken = default(CancellationToken))
            {
                throw new NotImplementedException();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="app"></param>
        public void Configure(IApplicationBuilder app)
        {
            app.UseSolidRpcProxies();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddLogging(ConfigureLogging);
            services.GetSolidConfigurationBuilder()
                .SetGenerator<SolidProxy.GeneratorCastle.SolidProxyCastleGenerator>();
            services.AddSolidRpcSingletonServices();
            services.AddTransient<IPet, PetImpl>();
            services.AddSolidRpcBindings(typeof(IPet).Assembly, null, GetBaseUrl);

            return services.BuildServiceProvider();
        }

        /// <summary>
        /// Tests the type store
        /// </summary>
        [Test]
        public async Task TestStartProject()
        {
            //
            // start server
            //
            var builder = Microsoft.AspNetCore.WebHost.CreateDefaultBuilder(new string[0]);
            builder.ConfigureLogging(ConfigureLogging);
            builder.ConfigureServices(_ => {
                _.AddSingleton<IStartup>(this);
            });
            var host = builder.Build();
            await host.StartAsync();

            var feature = host.ServerFeatures.Get<IServerAddressesFeature>();
            foreach (var addr in feature.Addresses)
            {
                BaseUri = new Uri(addr);
            }

            //
            // configure the client
            //
            var sc = new ServiceCollection();
            sc.AddLogging(ConfigureLogging);
            sc.AddTransient<IPet, IPet>();
            sc.AddSolidRpcSingletonServices();
            sc.GetSolidConfigurationBuilder()
                .SetGenerator<SolidProxy.GeneratorCastle.SolidProxyCastleGenerator>()
                .ConfigureInterfaceAssembly(typeof(IPet).Assembly)
                .ConfigureAdvice<ISolidRpcProxyConfig>()
                .BaseUriTransformer = GetBaseUrl;

            sc.GetSolidConfigurationBuilder().AddAdvice(typeof(SolidRpcProxyAdvice<,,>));

            var petService = sc.BuildServiceProvider().GetRequiredService<IPet>();
            Assert.AreEqual(4711, (await petService.GetPetById(4711)).Id);

            await host.StopAsync();
        }

        private Uri BaseUri;
        private Uri GetBaseUrl(IServiceProvider serviceProvider, Uri baseUri)
        {
            return new Uri(BaseUri.ToString() + baseUri.AbsolutePath.Substring(1));
        }
    }
}
