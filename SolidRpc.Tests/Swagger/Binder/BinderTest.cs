using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using SolidRpc.Abstractions.OpenApi.Binder;
using SolidRpc.OpenApi.Binder.V2;
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
            services.AddHttpClient();
            services.AddSingleton<IConfiguration>(new ConfigurationBuilder().Build());
            services.GetSolidConfigurationBuilder().SetGenerator<SolidProxy.GeneratorCastle.SolidProxyCastleGenerator>();
            var spec = services.GetSolidRpcOpenApiParser().CreateSpecification(typeof(IHelloWorld));
            services.AddSolidRpcBindings(
                typeof(IHelloWorld),
                typeof(IHelloWorld),
                (c) =>
                {
                    c.OpenApiSpec = spec.WriteAsJsonString();
                    return true;
                });

            var sp = services.BuildServiceProvider();
            var store = sp.GetRequiredService<IMethodBinderStore>();
            var methods = store.MethodBinders.SelectMany(o => o.MethodBindings).Select(o => o.MethodInfo.Name).ToList();
            Assert.AreEqual(1, methods.Count);
        }

        /// <summary>
        /// 
        /// </summary>
        [Test]
        public void TestNameMatches()
        {
            Assert.IsFalse(MethodBindingV2.NameMatches("Test", "TestX"));
            Assert.IsTrue(MethodBindingV2.NameMatches("Test", "Test"));
            Assert.IsTrue(MethodBindingV2.NameMatches("X#Test", "Test"));
            Assert.IsTrue(MethodBindingV2.NameMatches("X#Test#Y", "Test"));
            Assert.IsTrue(MethodBindingV2.NameMatches("X#Test#Y", "Test"));

            Assert.IsFalse(MethodBindingV2.NameMatches("XTest", "Test"));
            Assert.IsFalse(MethodBindingV2.NameMatches("TestX", "Test"));
            Assert.IsFalse(MethodBindingV2.NameMatches("#TestX", "Test"));
            Assert.IsFalse(MethodBindingV2.NameMatches("#TestX#", "Test"));
            Assert.IsFalse(MethodBindingV2.NameMatches("TestX#", "Test"));

            Assert.IsTrue(MethodBindingV2.NameMatches("Test", "Test#"));
            Assert.IsTrue(MethodBindingV2.NameMatches("Test", "#Test"));
        }
    }
}
