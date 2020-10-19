using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SolidProxy.Core.Configuration.Builder;
using SolidProxy.Core.Proxy;
using SolidProxy.MicrosoftDI;
using SolidRpc.Abstractions.OpenApi.Proxy;
using SolidRpc.Test.Vitec.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SolidRpc.Test.Vitec.Impl
{
    /// <summary>
    /// Interface that defines the logic for accessing the remote Vitec services
    /// </summary>
    public class VitecBackendServiceProvider : IVitecBackendServiceProvider
    {
        public VitecBackendServiceProvider(IServiceProvider frontend)
        {
            Frontend = frontend;
            var conf = frontend.GetRequiredService<IConfiguration>();
            var sc = new Microsoft.Extensions.DependencyInjection.ServiceCollection();
            sc.AddSingleton(conf);
            var proxyGenerator = frontend.GetRequiredService<ISolidProxyGenerator>();
            typeof(ISolidConfigurationBuilder).GetMethod(nameof(ISolidConfigurationBuilder.SetGenerator))
                .MakeGenericMethod(proxyGenerator.GetType()).Invoke(sc.GetSolidConfigurationBuilder(), null);
            AddDynamicBinding(sc, "System.Net.Http.IHttpClientFactory");
            sc.AddLogging();
            sc.AddSolidRpcBindings(VitecAssembly);
            sc.GetSolidConfigurationBuilder()
                .ConfigureInterfaceAssembly(VitecAssembly)
                .ConfigureAdvice<ISecurityKeyConfig>()
                .SecurityKey = new KeyValuePair<string, string>("Authorization", conf["VitecConnectAuthorization"]);


            Backend = sc.BuildServiceProvider();
        }

        private Assembly VitecAssembly => typeof(IEstate).Assembly;
        private IServiceProvider Frontend { get; }
        private IServiceProvider Backend { get; }

        private void AddDynamicBinding(IServiceCollection sc, string typeName)
        {
            var type = FindType(typeName);
            sc.AddTransient(type, sp => Frontend.GetService(type));
        }

        private Type FindType(string typeName)
        {
            var types = AppDomain.CurrentDomain.GetAssemblies().SelectMany(o =>
            {
                try
                {
                    return o.GetTypes();
                }
                catch (Exception e)
                {
                    return new Type[0];
                }
            }).ToList();
            return types.FirstOrDefault(o => o.FullName == typeName);
        }

        public object GetService(Type serviceType)
        {
            if(serviceType.Assembly == typeof(IEstate).Assembly)
            {
                return Backend.GetService(serviceType);
            }
            else
            {
                return Frontend.GetService(serviceType);
            }
        }
    }
}
