using Microsoft.Extensions.Logging;
using SolidRpc.Abstractions;
using SolidRpc.Abstractions.InternalServices;
using SolidRpc.Abstractions.OpenApi.OAuth2;
using SolidRpc.Abstractions.Types;
using SolidRpc.OpenApi.OAuth2.InternalServices;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

[assembly: SolidRpcService(typeof(ISolidRpcProtectedContent), typeof(SolidRpcProtectedContent), SolidRpcServiceLifetime.Singleton)]
namespace SolidRpc.OpenApi.OAuth2.InternalServices
{
    /// <summary>
    /// Implements the solid application logic
    /// </summary>
    public class SolidRpcProtectedContent : ISolidRpcProtectedContent
    {
        private static readonly DateTimeOffset StartDate = new DateTimeOffset(2020,01,01,0,0,0,0,TimeSpan.Zero);

        public SolidRpcProtectedContent(
            ILogger<SolidRpcProtectedContent> logger,
            IAuthorityLocal authority = null)
        {
            Logger = logger;
            Authority = authority;
        }

        private ILogger Logger { get; }
        private IAuthorityLocal Authority { get; }

        public Task<IEnumerable<byte[]>> CreateProtectedResourceStringsAsync(IEnumerable<string> content, DateTimeOffset expiryTime, CancellationToken cancellationToken)
        {
            var expHours = (int)(expiryTime - StartDate).TotalHours;
            if (expHours < 0) throw new ArgumentException("Expiry date too early.");
            var expTime = BitConverter.GetBytes(expHours);
            var res = content.Select(o =>
            {
                var ms = new MemoryStream();
                WriteAll(ms, expTime);
                WriteAll(ms, Encoding.UTF8.GetBytes(o));
                return Authority.Encrypt(ms.ToArray());
            });
            return Task.FromResult(res);
        }

        private void WriteAll(MemoryStream ms, byte[] vs)
        {
            ms.Write(vs, 0, vs.Length);
        }

        public Task<FileContent> GetProtectedContentAsync(byte[] resource, CancellationToken cancellationToken)
        {
            byte[] decResource;
            try
            {
                decResource = Authority.Decrypt(resource);
            } 
            catch(Exception e)
            {
                throw new FileContentNotFoundException("Failed to decrypt resource identifier");
            }

            var expHours = BitConverter.ToInt32(decResource, 0);
            var expTime = StartDate.AddHours(expHours);
            if(expTime < DateTimeOffset.UtcNow)
            {
                throw new FileContentNotFoundException("Resource expired");
            }
            return GetProtectedContentAsync(Encoding.UTF8.GetString(decResource, 4, decResource.Length - 4), cancellationToken);
        }

        public virtual Task<FileContent> GetProtectedContentAsync(string resource, CancellationToken cancellationToken)
        {
            Logger.LogError("No protected content found:"+resource);
            throw new FileContentNotFoundException();
        }
    }
}
