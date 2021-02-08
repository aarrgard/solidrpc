﻿using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System.Threading.Tasks;
using SolidRpc.Abstractions.OpenApi.OAuth2;
using System.Security.Claims;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.IO;
using System;

namespace SolidRpc.Tests.Security
{
    /// <summary>
    /// Tests security functionality.
    /// </summary>
    public class SecurityAuthorityTest : WebHostTest
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public override void ConfigureServerServices(IServiceCollection services)
        {
            base.ConfigureServerServices(services);
            services.AddSolidRpcOAuth2();
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

                var af = ctx.ServerServiceProvider.GetRequiredService<IAuthorityFactory>();
                var a = af.GetLocalAuthority(ctx.BaseAddress.ToString());
                a.CreateSigningKey();

                var claimsIdentity = new ClaimsIdentity(new[] {
                    new Claim("test", "test")
                });

                var jwt = await a.CreateAccessTokenAsync(claimsIdentity);

                var prin = (ClaimsPrincipal)(await a.GetPrincipalAsync(jwt.AccessToken));
                Assert.AreEqual("test", prin.Claims.Single(o => o.Type == "test").Value);
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


                var af = ctx.ServerServiceProvider.GetRequiredService<IAuthorityFactory>();
                var a = af.GetLocalAuthority(ctx.BaseAddress.ToString());

                using (var cert = GetManifestResource(nameof(TestLocalAuthority2) + ".pfx"))
                {
                    var ms = new MemoryStream();
                    await cert.CopyToAsync(ms);
                    a.SetSigningKey(new X509Certificate2(ms.ToArray()));
                }

                var claimsIdentity = new ClaimsIdentity(new[] {
                    new Claim("test", "test")
                });

                var jwt = await a.CreateAccessTokenAsync(claimsIdentity);

                var prin = (ClaimsPrincipal)(await a.GetPrincipalAsync(jwt.AccessToken));
                Assert.AreEqual("test", prin.Claims.Single(o => o.Type == "test").Value);
                Assert.IsTrue(int.Parse(jwt.ExpiresIn) > 3550);
                Assert.IsTrue(int.Parse(jwt.ExpiresIn) <= 3600);
            }
        }
    }
}
