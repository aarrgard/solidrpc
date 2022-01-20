using SolidRpc.Abstractions.OpenApi.Invoker;
using SolidRpc.Abstractions.Types;
using SolidRpc.Node.Services;
using SolidRpc.Node.Types;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SolidRpc.Test.Petstore.AzFunctionsV3
{
    public interface ITestInterface 
    {
        Task<string> RunNodeService(CancellationToken cancellationToke = default);
        Task<string> MyFunc(string arg1);
        Task<string> MyFunc(string arg1, string arg2);
        Task<FileContent> UploadFile(string container, FileContent fileContent);
        Task<FileContent> TestSetCookie(CancellationToken cancellation = default(CancellationToken));
    }

    public class TestImplementation : ITestInterface
    {
        public TestImplementation(INodeService nodeService, IInvoker<ITestInterface> invoker)
        {
            NodeService = nodeService;
            Invoker = invoker;
        }

        private IInvoker<ITestInterface> Invoker { get; }

        private INodeService NodeService { get; }

        public Task<string> MyFunc(string arg)
        {
            throw new NotImplementedException();
        }

        public Task<string> MyFunc(string arg1, string arg2)
        {
            throw new NotImplementedException();
        }

        public async Task<string> RunNodeService(CancellationToken cancellationToken)
        {
            var res = await NodeService.ExecuteScriptAsync(new NodeExecutionInput()
            {
                ModuleId = Guid.Empty,
                Js = "console.log('test');"
            }, cancellationToken);
            if (res.ExitCode == 0) return res.Out;
            throw new Exception(res.Err);
        }

        public Task<FileContent> UploadFile(string container, FileContent fileContent)
        {
            string strContent;
            using (var sr = new StreamReader(fileContent.Content))
            {
                strContent = sr.ReadToEnd();
            }
            fileContent.ETag = $"{Convert.ToBase64String(new byte[] { 1, 2, 3, 4, 5 })}";
            fileContent.Content = new MemoryStream(Encoding.UTF8.GetBytes(strContent));
            fileContent.CharSet = Encoding.UTF8.HeaderName;
            return Task.FromResult(fileContent);
        }

        public async Task<FileContent> TestSetCookie(CancellationToken cancellation = default)
        {
            var uri = await Invoker.GetUriAsync(o => o.TestSetCookie(cancellation));

            string cookieName = "TestCookie135xyx";
            var invoc = SolidProxy.Core.Proxy.SolidProxyInvocationImplAdvice.CurrentInvocation;
            var cookieValue = ParseCookies(invoc.GetValue<IEnumerable<string>>("http_req_cookie"))
                .Where(o => o.Name == cookieName)
                .Select(o => o.Value)
                .FirstOrDefault();
            cookieValue = cookieValue ?? "0";
            cookieValue = (int.Parse(cookieValue) + 1).ToString();

            var enc = Encoding.UTF8;
            var strContent = $"Cookie value:{cookieValue}";

            var secure = string.Equals(uri.Scheme, "https", StringComparison.InvariantCultureIgnoreCase) ? " Secure;" : "";
            return new FileContent()
            {
                CharSet = enc.HeaderName,
                ContentType = "text/plain",
                Content = new MemoryStream(enc.GetBytes(strContent)),
                //SetCookie = $"{cookieName}={cookieValue}; Path={uri.AbsolutePath}; Domain={uri.Host}; HttpOnly; SameSite=Strict; {secure}"
                SetCookie = $"{cookieName}={cookieValue}; Path={uri.AbsolutePath}; HttpOnly; SameSite=Strict;{secure}"
            };
        }

        private IEnumerable<NameValuePair> ParseCookies(IEnumerable<string> cookies)
        {
            if (cookies == null) return new NameValuePair[0];
            return cookies.Select(o => {
                var vals = o.Split(';').First().Split('=');
                return new NameValuePair() { Name = vals[0], Value = vals[1] };
            });
        }
    }
}
