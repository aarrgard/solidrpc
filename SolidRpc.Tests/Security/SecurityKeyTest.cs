﻿using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using SolidRpc.Abstractions.OpenApi.Proxy;
using SolidRpc.Abstractions.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SolidRpc.Tests.Security
{
    /// <summary>
    /// Tests security functionality.
    /// </summary>
    public class SecurityKeyTest : WebHostTest
    {
        /// <summary>
        /// 
        /// </summary>
        public SecurityKeyTest()
        {
            SecurityKey = new KeyValuePair<string, string>(Guid.NewGuid().ToString(), Guid.NewGuid().ToString());
        }
        /// <summary>
        /// 
        /// </summary>
        public KeyValuePair<string, string> SecurityKey { get; }

        /// <summary>
        /// 
        /// </summary>
        public interface ITestInterface
        {
            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            Task MethodWithClientKeySpecified();

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            Task MethodWithoutClientKeySpecified();
        }
        /// <summary>
        /// 
        /// </summary>
        public class TestImplementation : ITestInterface
        {
            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public Task MethodWithClientKeySpecified()
            {
                return Task.CompletedTask;
            }

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public Task MethodWithoutClientKeySpecified()
            {
                return Task.CompletedTask;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="clientServices"></param>
        /// <param name="baseAddress"></param>
        public override void ConfigureClientServices(IServiceCollection clientServices, Uri baseAddress)
        {
            var apiSpec = clientServices.GetSolidRpcOpenApiParser()
                .CreateSpecification(typeof(ITestInterface))
                .SetBaseAddress(new Uri(baseAddress, typeof(ITestInterface).Assembly.GetName().Name.Replace('.','/')))
                .WriteAsJsonString();
            clientServices.AddSolidRpcSingletonBindings<ITestInterface>(null, conf =>
            {
                conf.OpenApiSpec = apiSpec;
                if(conf.Methods.First().Name == nameof(ITestInterface.MethodWithClientKeySpecified))
                {
                    conf.SetSecurityKey(SecurityKey);
                }
                return true;
            });
            base.ConfigureClientServices(clientServices, baseAddress);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public override void ConfigureServerServices(IServiceCollection services)
        {
            base.ConfigureServerServices(services);
            var apiSpec = services.GetSolidRpcOpenApiParser().CreateSpecification(typeof(ITestInterface)).WriteAsJsonString();
            services.AddSolidRpcBindings(typeof(ITestInterface), typeof(TestImplementation), conf =>
            {
                conf.OpenApiSpec = apiSpec;
                conf.SetSecurityKey(SecurityKey);
                return true;
            });
        }

        /// <summary>
        /// Tests the web host
        /// </summary>
        [Test]
        public async Task TestSecurityKey()
        {
            using (var ctx = CreateKestrelHostContext())
            {
                await ctx.StartAsync();

                var testInterface = ctx.ClientServiceProvider.GetRequiredService<ITestInterface>();
                await testInterface.MethodWithClientKeySpecified();
                try
                {
                    await testInterface.MethodWithoutClientKeySpecified();
                    Assert.Fail("Should be 401");
                }
                catch(UnauthorizedException)
                {
                    // this is ok.
                }
            }
        }
    }
}

