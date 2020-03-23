﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using SolidProxy.GeneratorCastle;
using SolidRpc.Abstractions.OpenApi.Invoker;
using SolidRpc.Abstractions.Types;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SolidRpc.Tests.Invoker
{
    /// <summary>
    /// Tests the invokers
    /// </summary>
    public class LocalInvokerTest : TestBase
    {
        /// <summary>
        /// 
        /// </summary>
        public interface ITestInterface
        {
            /// <summary>
            /// 
            /// </summary>
            /// <param name="cancellation"></param>
            /// <returns></returns>
            Task<int> DoXAsync(CancellationToken cancellation = default(CancellationToken));
        }
        /// <summary>
        /// 
        /// </summary>
        public class TestImplementation : ITestInterface
        {
            /// <summary>
            /// 
            /// </summary>
            /// <param name="cancellation"></param>
            /// <returns></returns>
            public Task<int> DoXAsync(CancellationToken cancellation = default(CancellationToken))
            {
                return Task.FromResult(4711);
            }
        }

        /// <summary>
        /// Tests the type store
        /// </summary>
        [Test]
        public async Task TestLocalInvokerInternalService()
        {
            var sc = new ServiceCollection();
            sc.AddLogging(ConfigureLogging);
            sc.AddSolidRpcSingletonServices();
            sc.AddTransient<ITestInterface, TestImplementation>();
            var sp = sc.BuildServiceProvider();
            var invoker = sp.GetRequiredService<ILocalInvoker<ITestInterface>>();
            var result = await invoker.InvokeAsync(o => o.DoXAsync(CancellationToken.None));
            Assert.AreEqual(4711, result);
        }

        /// <summary>
        /// Tests the type store
        /// </summary>
        [Test]
        public async Task TestLocalInvokerExternalService()
        {
            var sb = new ConfigurationBuilder();
            var sc = new ServiceCollection();
            sc.AddSingleton<IConfiguration>(sb.Build());
            sc.AddLogging(ConfigureLogging);
            sc.GetSolidConfigurationBuilder().SetGenerator<SolidProxyCastleGenerator>();
            sc.AddSolidRpcSingletonServices();

            var openApiSpec = sc.GetSolidRpcOpenApiParser().CreateSpecification(typeof(ITestInterface)).WriteAsJsonString();
            sc.AddSolidRpcBindings(typeof(ITestInterface), typeof(TestImplementation), conf =>
            {
                conf.OpenApiSpec = openApiSpec;
                return true;
            });

            var sp = sc.BuildServiceProvider();
            var invoker = sp.GetRequiredService<ILocalInvoker<ITestInterface>>();
            var result = await invoker.InvokeAsync(o => o.DoXAsync(CancellationToken.None));
            Assert.AreEqual(4711, result);
        }

        /// <summary>
        /// Tests the type store
        /// </summary>
        [Test]
        public async Task TestLocalInvokerExternalServiceWithSecurityKey()
        {
            var sb = new ConfigurationBuilder();
            var sc = new ServiceCollection();
            sc.AddSingleton<IConfiguration>(sb.Build());
            sc.AddLogging(ConfigureLogging);
            sc.GetSolidConfigurationBuilder().SetGenerator<SolidProxyCastleGenerator>();
            sc.AddSolidRpcSingletonServices();

            var openApiSpec = sc.GetSolidRpcOpenApiParser().CreateSpecification(typeof(ITestInterface)).WriteAsJsonString();
            sc.AddSolidRpcBindings(typeof(ITestInterface), typeof(TestImplementation), conf =>
            {
                conf.OpenApiSpec = openApiSpec;
                conf.SecurityKey = new KeyValuePair<string, string>(Guid.NewGuid().ToString(), Guid.NewGuid().ToString());
                return true;
            });

            var sp = sc.BuildServiceProvider();

            // we should not be able to invoke the service directly
            try
            {
                await sp.GetRequiredService<ITestInterface>().DoXAsync();
                Assert.Fail();
            } 
            catch(UnauthorizedException e)
            {
                // this is the way we want it.
            }

            var invoker = sp.GetRequiredService<ILocalInvoker<ITestInterface>>();
            var result = await invoker.InvokeAsync(o => o.DoXAsync(CancellationToken.None));
            Assert.AreEqual(4711, result);
        }
    }
}
