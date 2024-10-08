using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SolidProxy.GeneratorCastle;
using SolidRpc.Abstractions.OpenApi.Proxy;
using SolidRpc.Abstractions.Services;
using SolidRpc.OpenApi.AzQueue.Services;
using SolidRpc.OpenApi.OAuth2.Services;
using SolidRpc.OpenApi.SwaggerUI.Services;

var host = new HostBuilder()
    .ConfigureFunctionsWebApplication()
    .ConfigureAppConfiguration(con =>
    {
        con.AddUserSecrets<Program>(optional: true, reloadOnChange: false);
    })
    .ConfigureServices(services =>
    {
        services.GetSolidRpcContentStore().AddPrefixRewrite("/front", "");
        services.GetSolidRpcContentStore().AddPrefixRewrite("/api", "");

        services.AddHttpClient();

        // solidrpc services
        services.AddSolidRpcOidcTestImpl();

        services.AddSolidRpcOAuth2(conf =>
        {
            conf.AddDefaultScopes("authorization_code", ["openid", "solidrpc", "offline_access"]);
        });
        services.AddSolidRpcOAuth2Local(conf => { conf.CreateSigningKey(); });

        var settings = services.GetSolidRpcService<IConfiguration>();
        services.GetAzFunctionHandler();
        services.GetSolidConfigurationBuilder().SetGenerator<SolidProxyCastleGenerator>();
        services.AddSolidRpcServices(conf => Configure(services, conf));
        services.AddSolidRpcSwaggerUI(o => {
            o.OAuthClientId = SolidRpcOidcTestImpl.ClientId;
            o.OAuthClientSecret = SolidRpcOidcTestImpl.ClientSecret;
        }, conf => Configure(services, conf));

        services.AddSolidRpcAzTableQueue("AzureWebJobsStorage", "azfunctions", c => Configure(services, c));
        services.AddAzFunctionTimer<IAzTableQueue>(o => o.DoScheduledScanAsync(CancellationToken.None), "0 * * * * *");
    })
    .Build();
bool Configure(IServiceCollection services, ISolidRpcOpenApiConfig conf)
{
    conf.SetOAuth2ClientSecurity(services.GetSolidRpcOAuth2LocalIssuer(), SolidRpcOidcTestImpl.ClientId, SolidRpcOidcTestImpl.ClientSecret);
    var method = conf.Methods.First();
    if (method.DeclaringType == typeof(ISwaggerUI))
    {
        //conf.DisableSecurity();
        //return true;
        switch (method.Name)
        {
            case nameof(ISwaggerUI.GetOauth2RedirectHtml):
                conf.DisableSecurity();
                break;
            default:
                conf.GetAdviceConfig<ISecurityOAuth2Config>().RedirectUnauthorizedIdentity = true;
                break;
        }
    }
    if (method.DeclaringType == typeof(ISolidRpcContentHandler))
    {
        conf.DisableSecurity();
        return true;
    }
    if (method.DeclaringType == typeof(ISolidRpcOAuth2))
    {
        conf.DisableSecurity();
        return true;
    }
    if (method.DeclaringType == typeof(ISolidRpcOidc))
    {
        conf.DisableSecurity();
        return true;
    }
    return true;
}

host.Run();
