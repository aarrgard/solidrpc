using SolidRpc.Security.Services.OAuth2.Microsoft;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SolidRpc.Security.Impl.Services.OAuth2.Microsoft
{
    public class OAuth2MicrosoftCallback : IOAuth2MicrosoftCallback
    {
        public Task Login(CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task Logout(CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }
    }
}
