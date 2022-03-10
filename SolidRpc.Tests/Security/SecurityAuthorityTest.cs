using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System.Threading.Tasks;
using SolidRpc.Abstractions.OpenApi.OAuth2;
using System.Security.Claims;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.IO;
using System;
using SolidRpc.Abstractions.Services;
using SolidRpc.Abstractions.InternalServices;
using System.Threading;
using SolidRpc.Abstractions.Types;
using Microsoft.IdentityModel.Logging;

namespace SolidRpc.Tests.Security
{
    /// <summary>
    /// Tests security functionality.
    /// </summary>
    public class SecurityAuthorityTest : WebHostTest
    {
        private class SolidRpcProtectedContentImpl : ISolidRpcProtectedContent
        {
            public SolidRpcProtectedContentImpl()
            {
            }

            public async Task<FileContent> GetProtectedContentAsync(string resource, CancellationToken cancellationToken)
            {
                var enc = System.Text.Encoding.UTF8;
                switch (resource)
                {
                    case "Vitec/a3c9c279-b1f2-46ca-aa0a-db909c0a3e76":
                        return new FileContent()
                        {
                            CharSet = enc.HeaderName,
                            Content = new MemoryStream(enc.GetBytes("Test"))
                        };
                }
                throw new FileContentNotFoundException();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public override void ConfigureServerServices(IServiceCollection services)
        {
            IdentityModelEventSource.ShowPII = true;
            base.ConfigureServerServices(services);
            services.AddSingleton<ISolidRpcProtectedContent, SolidRpcProtectedContentImpl>();
            var issuer = new Uri(services.GetSolidRpcService<Uri>(), "SolidRpc/Abstractions");
            services.AddSolidRpcOAuth2Local(issuer.ToString());
            services.AddSolidRpcOidcTestImpl();
            services.AddSolidRpcServices(o => true);
        }


        public override void ConfigureClientServices(IServiceCollection clientServices, Uri baseAddress)
        {
            base.ConfigureClientServices(clientServices, baseAddress);
            clientServices.AddSolidRpcRemoteBindings<ISolidRpcContentHandler>();
        }

        /// <summary>
        /// Tests the signing of a jwt token
        /// </summary>
        [Test]
        public async Task TestLocalAuthority()
        {
            using (var ctx = CreateKestrelHostContext())
            {
                await ctx.StartAsync();

                var a = ctx.ServerServiceProvider.GetRequiredService<IAuthorityLocal>();
                a.CreateSigningKey();

                var claimsIdentity = new ClaimsIdentity(new[] {
                    new Claim("test", "test")
                });

                var jwt = await a.CreateAccessTokenAsync(claimsIdentity);

                var prin = await a.GetPrincipalAsync(jwt.AccessToken);
                Assert.AreEqual("test", prin.Claims.Single(o => o.Type == "test").Value);

                await TestEncDecData(a);
            }
        }

        private async Task TestEncDecData(IAuthorityLocal a)
        {
            var a1 = new byte[] { 1, 2, 3 };
            var a2 = await a.SignHash(a1);
            Assert.IsTrue(await a.VerifyData(a1, a2));

            a.CreateSigningKey();

            Assert.IsTrue(await a.VerifyData(a1, a2));
        }

        /// <summary>
        /// Tests that the petstore json file is processed correctly.
        /// </summary>
        [Test]
        public async Task TestLocalAuthority2()
        {
            using (var ctx = CreateKestrelHostContext())
            {
                await ctx.StartAsync();


                var a = ctx.ServerServiceProvider.GetRequiredService<IAuthorityLocal>();

                X509Certificate2 cert;
                using (var certStream = GetManifestResource(nameof(TestLocalAuthority2) + ".pfx"))
                {
                    var ms = new MemoryStream();
                    await certStream.CopyToAsync(ms);
                    cert = new X509Certificate2(ms.ToArray());
                    a.SetSigningKey(cert, c => $"{c.Thumbprint}{c.SignatureAlgorithm}");
                }

                Assert.IsTrue((await a.GetSigningKeysAsync()).Any(o => o.Kid == $"{cert.Thumbprint}{cert.SignatureAlgorithm}"));

                var claimsIdentity = new ClaimsIdentity(new[] {
                    new Claim("test", "test")
                });

                var jwt = await a.CreateAccessTokenAsync(claimsIdentity);

                var prin = await a.GetPrincipalAsync(jwt.AccessToken);
                Assert.AreEqual("test", prin.Claims.Single(o => o.Type == "test").Value);
                Assert.IsTrue(int.Parse(jwt.ExpiresIn) > 3550);
                Assert.IsTrue(int.Parse(jwt.ExpiresIn) <= 3600);

                TestEncDecData(a);
            }
        }

        /// <summary>
        /// Tests that the petstore json file is processed correctly.
        /// </summary>
        [Test]
        public async Task TestProtectedResource()
        {
            using (var ctx = CreateKestrelHostContext())
            {
                await ctx.StartAsync();


                var a = ctx.ServerServiceProvider.GetRequiredService<IAuthorityLocal>();
                a.CreateSigningKey();


                var pc = ctx.ServerServiceProvider.GetRequiredService<ISolidRpcProtectedResource>();
                var rs = await pc.ProtectAsync("Vitec/a3c9c279-b1f2-46ca-aa0a-db909c0a3e76", DateTime.Now.AddHours(1));


                var h = ctx.ClientServiceProvider.GetRequiredService<ISolidRpcContentHandler>();
                var r = await h.GetProtectedContentAsync(rs);

                Assert.AreEqual("Test", await r.AsStringAsync());

                //
                // test expired
                //
                try
                {
                    rs = await pc.ProtectAsync("Vitec/a3c9c279-b1f2-46ca-aa0a-db909c0a3e76", DateTime.Now.AddHours(-1));
                    r = await h.GetProtectedContentAsync(rs);
                }
                catch(Exception e)
                {
                    //Assert.AreEqual("Test", e.Message);
                }
            }
        }
    }
}
