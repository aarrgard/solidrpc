using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SolidRpc.OpenApi.SwaggerUI.Types;

namespace SolidRpc.OpenApi.SwaggerUI.Services
{
    /// <summary>
    /// The swagger config
    /// </summary>
    public class SwaggerUI : ISwaggerUI
    {
        /// <summary>
        /// Returns the index.hmtl file.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<FileContent> GetIndexHtml(CancellationToken cancellationToken = default(CancellationToken))
        {
            var html = @"<!-- HTML for static distribution bundle build -->
<!DOCTYPE html>
<html lang=""en"">
  <head>
    <meta charset=""UTF-8"">
    <title>Swagger UI</title>
    <link rel=""stylesheet"" type=""text/css"" href=""./swagger-ui.css"" >
    <link rel=""icon"" type=""image/png"" href=""./favicon-32x32.png"" sizes=""32x32"" />
    <link rel=""icon"" type=""image/png"" href=""./favicon-16x16.png"" sizes=""16x16"" />
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

    <script src=""../swagger-ui-bundle.js""> </script>
    <script src=""../swagger-ui-standalone-preset.js""> </script>
    <script>
    window.onload = function() {{
      // Begin Swagger UI call region
      const ui = SwaggerUIBundle({{
        url: ""https://petstore.swagger.io/v2/swagger.json"",
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
            var urls = await GetSwaggerUrls(cancellationToken);
            var encoding = Encoding.UTF8;
            return new FileContent()
            {
                ContentType = $"text/html; charset=''{encoding.EncodingName}",
                Body = new MemoryStream(encoding.GetBytes(html))
            };
        }

        /// <summary>
        /// Returns the swagger urls.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<IEnumerable<SwaggerUrl>> GetSwaggerUrls(CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }
    }
}
