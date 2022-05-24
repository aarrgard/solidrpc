using SolidRpc.Abstractions.InternalServices;
using SolidRpc.Abstractions.OpenApi.Invoker;
using SolidRpc.Abstractions.Services;
using SolidRpc.OpenApi.OAuth2.InternalServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SolidRpc.Test.Petstore.AzFunctionsV4
{

    public interface IProtectedResource
    {
        public Task<Uri> GetProtectedResourceUriAsync(string resource, CancellationToken cancellationToken);
    }

    public class ProtectedResourceTest : IProtectedResource
    {
        public ProtectedResourceTest(
            ISolidRpcProtectedResource solidRpcProtectedResource,
            IInvoker<ISolidRpcContentHandler> contentHandler)
        {
            SolidRpcProtectedResource = solidRpcProtectedResource;
            ContentHandler = contentHandler;
        }

        private ISolidRpcProtectedResource SolidRpcProtectedResource { get; }
        private IInvoker<ISolidRpcContentHandler> ContentHandler { get; }

        public async Task<Uri> GetProtectedResourceUriAsync(string resource, CancellationToken cancellationToken)
        {
            var barr = await SolidRpcProtectedResource.ProtectAsync(resource, DateTime.Now.AddDays(1), cancellationToken);
            return await ContentHandler.GetUriAsync(o => o.GetProtectedContentAsync(barr, null, cancellationToken));

        }
    }
}
