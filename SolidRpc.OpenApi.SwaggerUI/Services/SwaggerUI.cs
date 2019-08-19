using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using SolidRpc.Abstractions.OpenApi.Binder;
using SolidRpc.Abstractions.Types;
using SolidRpc.OpenApi.SwaggerUI.Types;

namespace SolidRpc.OpenApi.SwaggerUI.Services
{
    /// <summary>
    /// The swagger config
    /// </summary>
    public class SwaggerUI : ISwaggerUI
    {
        /// <summary>
        /// Constructs a new instance
        /// </summary>
        /// <param name="methodBinderStore"></param>
        public SwaggerUI(IMethodBinderStore methodBinderStore)
        {
            MethodBinderStore = methodBinderStore;
        }

        private IMethodBinderStore MethodBinderStore { get; }
        private IConfiguration Configuration { get; }

        /// <summary>
        /// Returns the index.hmtl file.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<FileContent> GetIndexHtml(CancellationToken cancellationToken = default(CancellationToken))
        {
            var strBase = "../..";
            var html = $@"<!-- HTML for static distribution bundle build -->
<!DOCTYPE html>
<html lang=""en"">
  <head>
    <meta charset=""UTF-8"">
    <title>Swagger UI</title>
    <link rel=""stylesheet"" type=""text/css"" href=""../../swagger-ui.css"" >
    <link rel=""icon"" type=""image/png"" href=""{strBase}/favicon-32x32.png"" sizes=""32x32"" />
    <link rel=""icon"" type=""image/png"" href=""{strBase}/favicon-16x16.png"" sizes=""16x16"" />
    <style>
      html
      {{
        box-sizing: border-box;
        overflow: -moz-scrollbars-vertical;
        overflow-y: scroll;
      }}

      *,
      *:before,
      *:after
      {{
        box-sizing: inherit;
      }}

      body
      {{
        margin:0;
        background: #fafafa;
      }}
    </style>
  </head>

  <body>
    <div id=""swagger-ui""></div>

    <script src=""{strBase}/swagger-ui-bundle.js""> </script>
    <script src=""{strBase}/swagger-ui-standalone-preset.js""> </script>
    <script>
    window.onload = function() {{
      // Begin Swagger UI call region
      const ui = SwaggerUIBundle({{
        urls: { AsJson(await GetSwaggerUrls(cancellationToken)) },
        dom_id: '#swagger-ui',
        deepLinking: true,
        presets: [
          SwaggerUIBundle.presets.apis,
          SwaggerUIStandalonePreset
        ],
        plugins: [
          SwaggerUIBundle.plugins.DownloadUrl
        ],
        layout: ""StandaloneLayout""
      }})
      // End Swagger UI call region

      window.ui = ui
    }}
  </script>
  </body>
</html>
";
            var encoding = Encoding.UTF8;
            return new FileContent()
            {
                CharSet = encoding.HeaderName,
                ContentType = $"text/html",
                Content = new MemoryStream(encoding.GetBytes(html))
            };
        }

        private object AsJson(IEnumerable<SwaggerUrl> urls)
        {
            var sb = new StringBuilder();
            sb.Append('[');
            foreach(var url in urls)
            {
                sb.Append('{');
                sb.Append("name:").Append('"').Append(url.Name).Append('"').Append(',');
                sb.Append("url:").Append('"').Append(url.Url).Append('"');
                sb.Append('}').Append(',');
            }
            sb.Append(']');
            return sb.ToString();
        }

        /// <summary>
        /// Returns the openapi spec for specified assembly and openapi spec
        /// </summary>
        /// <param name="assemblyName"></param>
        /// <param name="openApiTitle"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<FileContent> GetOpenApiSpec(string assemblyName, string openApiTitle, CancellationToken cancellationToken = default(CancellationToken))
        {
            var binders = MethodBinderStore.MethodBinders
                .Where(o => o.Assembly.GetName().Name == assemblyName)
                .Where(o => o.OpenApiSpec.Title == openApiTitle);
            if(!binders.Any())
            {
                throw new FileContentNotFoundException();
            }
            var json = binders.First().OpenApiSpec.WriteAsJsonString();
            var encoding = Encoding.UTF8;
            return Task.FromResult(new FileContent()
            {
                CharSet = encoding.HeaderName,
                Content = new MemoryStream(encoding.GetBytes(json)),
                ContentType = "application/json"
            });
        }

        /// <summary>
        /// Returns the swagger urls.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<IEnumerable<SwaggerUrl>> GetSwaggerUrls(CancellationToken cancellationToken = default(CancellationToken))
        {
            var swaggerUrls = new List<SwaggerUrl>();
            foreach (var mb in MethodBinderStore.MethodBinders)
            {
                var assemblyName = mb.Assembly.GetName().Name;
                var openApiTitle = mb.OpenApiSpec.Title;
                var name = openApiTitle;
                if(!name.StartsWith(assemblyName))
                {
                    name = $"{assemblyName}.{name}";
                }
                swaggerUrls.Add(new SwaggerUrl()
                {
                    Name = name,
                    Url = await MethodBinderStore.GetUrlAsync<ISwaggerUI>(o => o.GetOpenApiSpec(assemblyName, openApiTitle, cancellationToken))
                });
            }
            return swaggerUrls;
        }
    }
}
