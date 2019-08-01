using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using SolidRpc.Abstractions.OpenApi.Binder;
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

        /// <summary>
        /// 
        /// </summary>
        public interface IHelloWorld
        {
            /// <summary>
            /// 
            /// </summary>
            /// <param name="cancellationToken"></param>
            /// <returns></returns>
            Task<string> WriteHello(CancellationToken cancellationToken);
        }

        /// <summary>
        /// 
        /// </summary>
        [Test]
        public void TestBindHelloWorld()
        {
            var services = new ServiceCollection();
            services.AddSingleton<IConfiguration>(new ConfigurationBuilder().Build());
            services.GetSolidConfigurationBuilder().SetGenerator<SolidProxy.GeneratorCastle.SolidProxyCastleGenerator>();
            var spec = services.GetSolidRpcOpenApiParser().CreateSpecification(typeof(IHelloWorld));
            services.AddSolidRpcBindings(typeof(IHelloWorld), typeof(IHelloWorld), spec.WriteAsJsonString());

            var sp = services.BuildServiceProvider();
            var store = sp.GetRequiredService<IMethodBinderStore>();
            var methods = store.MethodBinders.SelectMany(o => o.MethodInfos).Select(o => o.MethodInfo.Name).ToList();
            Assert.AreEqual(1, methods.Count);
        }
    }
}
