﻿using SolidRpc.Abstractions.OpenApi.Binder;
using SolidRpc.Security.Services.Microsoft;
using SolidRpc.Security.Types;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SolidRpc.Security.Back.Services.Microsoft
{
    public class MicrosoftLocal : LoginProviderBase, IMicrosoftLocal
    {
        public MicrosoftLocal(MicrosoftOptions microsoftOptions,  IMethodBinderStore methodBinderStore)
        {
            MicrosoftOptions = microsoftOptions;
            MethodBinderStore = methodBinderStore;
        }
        public override string ProviderName => "Microsoft";
        private MicrosoftOptions MicrosoftOptions { get; }
        private IMethodBinderStore MethodBinderStore { get; }
        public string ButtonHtml => $"<img src=\"{ProviderName}\" alt=\"{ProviderName}\"/>";

        public override async Task<LoginProvider> LoginProvider(CancellationToken cancellationToken = default(CancellationToken))
        {
            return new LoginProvider()
            {
                Name = ProviderName,
                Meta = new LoginProviderMeta[0],
                Script = new[] { await MethodBinderStore.GetUrlAsync<IMicrosoftLocal>(o => o.LoginScript(cancellationToken)) },
                Status = "NotLoggedIn",
                ButtonHtml = ButtonHtml
            };
        }

        public Task<string> LoggedIn(string accessToken, CancellationToken cancellationToken = default(CancellationToken))
        {
                throw new NotImplementedException();
        }

        public Task<string> LoggedOut(string accessToken, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public async Task<WebContent> LoginScript(CancellationToken cancellationToken = default(CancellationToken))
        {
            var scope = new[] { "openid" };
            var nounce = Guid.NewGuid().ToString();
            var responseType = new[] { "id_token" };
            var responseMode = "query";
            var state = "12345";
            var redirectUri = await MethodBinderStore.GetUrlAsync<IMicrosoftLocal>(o => o.LoggedIn(null, cancellationToken), false);
            var authorizeEndpoint = await MethodBinderStore.GetUrlAsync<IMicrosoftRemote>(o => o.Authorize(MicrosoftOptions.Tenant, MicrosoftOptions.ClientID, responseType, redirectUri, responseMode, scope, state, nounce, null, null, null, null, cancellationToken));
            var replace = new Dictionary<string, string>()
            {
                { "{authorizeEndpoint}", authorizeEndpoint?.ToString()}
            };
            return await GetManifestResourceAsWebContent("MicrosoftLocal.LoginScript.js", replace);
        }
    }
}
