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
using SolidRpc.OpenApi.OAuth2.InternalServices;
using Microsoft.Extensions.Logging;
using System.Threading;
using SolidRpc.Abstractions.Types;

namespace SolidRpc.Tests.Security
{
    /// <summary>
    /// Tests security functionality.
    /// </summary>
    public class SecurityAuthorityTest : WebHostTest
    {
        private class SolidRpcProtectedContentImpl : SolidRpcProtectedContent
        {
            public SolidRpcProtectedContentImpl(ILogger<SolidRpcProtectedContent> logger, IAuthorityLocal authority = null) : base(logger, authority)
            {
            }

            public override async Task<FileContent> GetProtectedContentAsync(string resource, CancellationToken cancellationToken)
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
                return await base.GetProtectedContentAsync(resource, cancellationToken);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public override void ConfigureServerServices(IServiceCollection services)
        {
            base.ConfigureServerServices(services);
            services.AddSingleton<ISolidRpcProtectedContent, SolidRpcProtectedContentImpl>();
            var issuer = new Uri(services.GetSolidRpcService<Uri>(), "SolidRpc/Abstractions");
            services.AddSolidRpcOAuth2Local(issuer.ToString());
            services.AddSolidRpcOidcImpl();
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

                TestEncDecData(a);
            }
        }

        private void TestEncDecData(IAuthorityLocal a)
        {
            var a1 = new byte[] { 1, 2, 3 };
            var a2 = a.Encrypt(a1);
            var a3 = a.Decrypt(a2);
            Assert.AreEqual(a1.Length, a3.Length);
            for (int i = 0; i < a1.Length; i++)
            {
                Assert.AreEqual(a1[i], a3[i]);
            }

            a.CreateSigningKey();

            a3 = a.Decrypt(a2);
            Assert.AreEqual(a1.Length, a3.Length);
            for (int i = 0; i < a1.Length; i++)
            {
                Assert.AreEqual(a1[i], a3[i]);
            }
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


                var pc = ctx.ServerServiceProvider.GetRequiredService<ISolidRpcProtectedContent>();
                var rs = (await pc.CreateProtectedResourceStringsAsync(new[] { "Vitec/a3c9c279-b1f2-46ca-aa0a-db909c0a3e76" }, DateTime.Now.AddHours(1))).Single();


                var h = ctx.ClientServiceProvider.GetRequiredService<ISolidRpcContentHandler>();
                var r = await h.GetProtectedContentAsync(rs);

                Assert.AreEqual("Test", await r.AsStringAsync());

                //
                // test expired
                //
                try
                {
                    rs = (await pc.CreateProtectedResourceStringsAsync(new[] { "Vitec/a3c9c279-b1f2-46ca-aa0a-db909c0a3e76" }, DateTime.Now.AddHours(-1))).Single();
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
