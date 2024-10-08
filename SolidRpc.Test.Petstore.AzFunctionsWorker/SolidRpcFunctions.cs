
using Microsoft.Azure.Functions.Worker;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
namespace SolidRpc.OpenApi.AzFunctions
{
    
    public class WildcardFunc
    {
        private ILogger _logger;
        private IServiceProvider _serviceProvider;
        public WildcardFunc(ILogger<WildcardFunc> logger, IServiceProvider serviceProvider) {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }
        [Function("WildcardFunc")]
        public Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "options", "post", Route = "{*restOfPath}")] HttpRequest req,
            CancellationToken cancellationToken)
        {
            return HttpFunction.Run(req, _logger, _serviceProvider, cancellationToken);
        }
    }

}
