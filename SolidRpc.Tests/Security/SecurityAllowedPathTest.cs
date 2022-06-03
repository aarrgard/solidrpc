using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using SolidRpc.Abstractions.OpenApi.Model;
using SolidRpc.OpenApi.Binder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SolidRpc.Abstractions.OpenApi.Proxy;
using SolidRpc.OpenApi.OAuth2.Services;

namespace SolidRpc.Tests.Security
{
    /// <summary>
    /// Tests security functionality.
    /// </summary>
    public class SecurityAllowedPathTest : WebHostTest
    {

        public interface ITestInterface
        {
            Task DoXAsync(CancellationToken cancellationToken = default);
        }

        public class TestImpl : ITestInterface
        {
            public Task DoXAsync(CancellationToken cancellationToken = default)
            {
                return Task.CompletedTask;
            }
        }

        protected override void ConfigureServerConfiguration(IConfigurationBuilder cb, Uri baseAddress)
        {
            base.ConfigureServerConfiguration(cb, baseAddress);
            cb.AddInMemoryCollection(new Dictionary<string, string>()
            {
                { ConfigurationMethodAddressTransformer.ConfigPathRewrites.First(), "/X:/SolidRpc"}
            });
        }
        public override void ConfigureServerServices(IServiceCollection serverServices)
        {
            base.ConfigureServerServices(serverServices);
            serverServices.AddSolidRpcOAuth2Local(conf => conf.CreateSigningKey());
            serverServices.AddSolidRpcOidcTestImpl(new[] { "/X/*" });
            serverServices.AddSolidRpcServices();

            var openApiSpec = serverServices.GetSolidRpcService<IOpenApiParser>().CreateSpecification(typeof(ITestInterface)).WriteAsJsonString();
            serverServices.AddSolidRpcBindings(typeof(ITestInterface), typeof(TestImpl), conf =>
            {
                conf.OpenApiSpec = openApiSpec;
                conf.SetOAuth2ClientSecurity(serverServices.GetSolidRpcOAuth2LocalIssuer(), SolidRpcOidcTestImpl.ClientId, SolidRpcOidcTestImpl.ClientSecret);
                return true;
            });
        }

        public override void ConfigureClientServices(IServiceCollection clientServices, Uri baseAddress)
        {
            base.ConfigureClientServices(clientServices, baseAddress);
            clientServices.AddSolidRpcOAuth2();

            var openApiSpec = clientServices.GetSolidRpcService<IOpenApiParser>().CreateSpecification(typeof(ITestInterface)).WriteAsJsonString();
            clientServices.AddSolidRpcRemoteBindings<ITestInterface>(conf =>
            {
                conf.OpenApiSpec = openApiSpec;
                conf.SetOAuth2ClientSecurity(clientServices.GetSolidRpcOAuth2LocalIssuer(), SolidRpcOidcTestImpl.ClientId, SolidRpcOidcTestImpl.ClientSecret);
                return true;
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task TestAllowedPaths()
        {
            using (var ctx = CreateKestrelHostContext())
            {
                await ctx.StartAsync();

                var ti = ctx.ClientServiceProvider.GetRequiredService<ITestInterface>();
                await ti.DoXAsync();
            }
       }
    }
}
