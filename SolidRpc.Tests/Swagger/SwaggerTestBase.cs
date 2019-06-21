using Microsoft.Extensions.DependencyInjection;
using SolidProxy.GeneratorCastle;
using SolidRpc.OpenApi.Proxy;
using System;
using System.IO;
using System.Linq;

namespace SolidRpc.Tests.Swagger
{
    /// <summary>
    /// Base class for the swagger tests
    /// </summary>
    public abstract class SwaggerTestBase : WebHostMvcTest
    {
        /// <summary>
        /// Reads the api config
        /// </summary>
        /// <param name="folderName"></param>
        /// <returns></returns>
        protected string ReadOpenApiConfiguration(string folderName)
        {
            var folder = GetSpecFolder(folderName);
            var jsonFile = folder.GetFiles("*.json").Single();
            using (var sr = jsonFile.OpenText())
            {
                return sr.ReadToEnd();
            }
        }

        /// <summary>
        /// Returns the spec folder.
        /// </summary>
        /// <param name="folderName"></param>
        /// <returns></returns>
        protected abstract DirectoryInfo GetSpecFolder(string folderName);

        /// <summary>
        /// Creates a proxy
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="rootAddress"></param>
        /// <param name="openApiConfiguration"></param>
        /// <returns></returns>
        protected T CreateProxy<T>(Uri rootAddress, string openApiConfiguration) where T : class
        {
            var sc = new ServiceCollection();
            sc.AddLogging(ConfigureLogging);
            sc.AddTransient<T, T>();
            var conf = sc.GetSolidConfigurationBuilder()
                .SetGenerator<SolidProxyCastleGenerator>()
                .ConfigureInterface<T>()
                .ConfigureAdvice<ISolidRpcProxyConfig>();
            conf.OpenApiConfiguration = openApiConfiguration;
            conf.RootAddress = rootAddress;

            sc.GetSolidConfigurationBuilder().AddAdvice(typeof(LoggingAdvice<,,>), o => o.MethodInfo.DeclaringType == typeof(T));
            sc.GetSolidConfigurationBuilder().AddAdvice(typeof(SolidRpcProxyAdvice<,,>));

            var sp = sc.BuildServiceProvider();
            return sp.GetRequiredService<T>();
        }
    }
}
