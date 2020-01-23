using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using SolidRpc.Abstractions.OpenApi.Binder;
using SolidRpc.Abstractions.OpenApi.Proxy;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SolidRpc.Tests.Swagger.Binder
{
    /// <summary>
    /// Tests swagger functionality.
    /// </summary>
    public class BinderTest : TestBase
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public interface IHelloWorld
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
            Task<string> WriteHello(CancellationToken cancellationToken);
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        }

        /// <summary>
        /// 
        /// </summary>
        [Test]
        public void TestBindHelloWorldWithoutComments()
        {
            var services = new ServiceCollection();
            services.AddLogging(ConfigureLogging);
            services.AddSingleton<IConfiguration>(new ConfigurationBuilder().Build());
            services.GetSolidConfigurationBuilder().SetGenerator<SolidProxy.GeneratorCastle.SolidProxyCastleGenerator>();
            var spec = services.GetSolidRpcOpenApiParser().CreateSpecification(typeof(IHelloWorld));
            services.AddSolidRpcBindings(
                typeof(IHelloWorld), 
                typeof(IHelloWorld),
                (c) =>
                {
                    c.ConfigureAdvice<ISolidRpcOpenApiConfig>().OpenApiSpec = spec.WriteAsJsonString();
                });

            var sp = services.BuildServiceProvider();
            var store = sp.GetRequiredService<IMethodBinderStore>();
            var methods = store.MethodBinders.SelectMany(o => o.MethodBindings).Select(o => o.MethodInfo.Name).ToList();
            Assert.AreEqual(1, methods.Count);
        }
    }
}
