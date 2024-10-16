﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using SolidProxy.GeneratorCastle;
using SolidRpc.OpenApi.Binder;
using System.Collections.Generic;
using System.Linq;

namespace SolidRpc.Tests
{
    /// <summary>
    /// Tests the template types
    /// </summary>
    public class MethodAddressTransformerTest : TestBase
    {
 
        /// <summary>
        /// Tests the type store
        /// </summary>
        [Test]
        public void TestPathRewrie()
        {
            var cb = new ConfigurationBuilder();
            cb.AddInMemoryCollection(new Dictionary<string, string>()
            {
                { ConfigurationMethodAddressTransformer.ConfigPathRewrites.First(), "/front:, /test2: /test3 ,/test4:/test4"}
            });
            var sc = new ServiceCollection();
            sc.AddSingleton<IConfiguration>(cb.Build());
            sc.GetSolidConfigurationBuilder().SetGenerator<SolidProxyCastleGenerator>();
            sc.AddSolidRpcServices();
            var sp = sc.BuildServiceProvider();

            var mat = sp.GetRequiredService<ConfigurationMethodAddressTransformer>();

            Assert.AreEqual("/no/rewrite", mat.RewritePath("/no/rewrite"));
            Assert.AreEqual("/test", mat.RewritePath("/front/test"));
            Assert.AreEqual("/test3", mat.RewritePath("/test2"));
            Assert.AreEqual("/test22", mat.RewritePath("/test22"));
            Assert.AreEqual("/test3", mat.RewritePath("/front/test2"));
            Assert.AreEqual("/test4", mat.RewritePath("/test4"));
        }

    }
}
