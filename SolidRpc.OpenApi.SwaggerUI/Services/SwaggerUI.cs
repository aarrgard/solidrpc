﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SolidRpc.Abstractions.OpenApi.Binder;
using SolidRpc.Abstractions.OpenApi.Invoker;
using SolidRpc.Abstractions.OpenApi.Proxy;
using SolidRpc.Abstractions.OpenApi.Transport;
using SolidRpc.Abstractions.Services;
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
        /// <param name="swaggerOptions"></param>
        /// <param name="methodBinderStore"></param>
        /// <param name="contentHandler"></param>
        /// <param name="invoker"></param>
        public SwaggerUI(
            SwaggerOptions swaggerOptions, 
            IMethodBinderStore methodBinderStore,
            ISolidRpcContentHandler contentHandler,
            IInvoker<ISwaggerUI> invoker)
        {
            SwaggerOptions = swaggerOptions;
            MethodBinderStore = methodBinderStore;
            ContentHandler = contentHandler;
            Invoker = invoker;
        }

        private SwaggerOptions SwaggerOptions { get; }
        private IMethodBinderStore MethodBinderStore { get; }
        private ISolidRpcContentHandler ContentHandler { get; }
        private IInvoker<ISwaggerUI> Invoker { get; }

        /// <summary>
        /// Returns the index.hmtl file.
        /// </summary>
        /// <param name="onlyImplemented"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<FileContent> GetIndexHtml(
            bool onlyImplemented, 
            CancellationToken cancellationToken = default(CancellationToken))
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
        urls: { AsJson(await GetSwaggerUrls(onlyImplemented, cancellationToken)) },
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
        /// <param name="openApiSpecResolverAddress">The address of the openapi spec</param>
        /// <param name="onlyImplemented"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<FileContent> GetOpenApiSpec(
            string assemblyName, 
            string openApiSpecResolverAddress, 
            bool onlyImplemented,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            //
            // search for the spec using existing bindings.
            //
            var binder = MethodBinderStore.MethodBinders
                .Where(o => o.Assembly.GetName().Name == assemblyName)
                .Where(o => o.OpenApiSpec.OpenApiSpecResolverAddress == openApiSpecResolverAddress)
                .FirstOrDefault();

            var hostedAddress = binder?.HostedAddress;
            var openApiSpec = binder?.OpenApiSpec;

            //
            // Spec may be imported from other bindings.
            //
            if (openApiSpec == null)
            {
                //
                // find the assembly - must be part of the bindings!
                //
                var assembly = MethodBinderStore.MethodBinders
                    .Where(o => o.Assembly.GetName().Name == assemblyName)
                    .Select(o => o.Assembly).FirstOrDefault();
                if (assembly == null)
                {
                    throw new FileContentNotFoundException("Assembly not part of any bindings.");
                }

                var openApiSpecResolver = MethodBinderStore.GetOpenApiSpecResolver(assembly);
                if (!openApiSpecResolver.TryResolveApiSpec(openApiSpecResolverAddress, out openApiSpec))
                {
                    throw new FileContentNotFoundException("Cannot find open api spec in assembly.");
                }
            }

            // clone spec and set base address
            if(hostedAddress != null)
            {
                openApiSpec = openApiSpec.SetBaseAddress(hostedAddress);
            }
            else
            {
                openApiSpec = openApiSpec.Clone();
            }

            //
            // check if there is any content associated
            //
            if (ContentHandler.PathPrefixes.Any(o => o == openApiSpec.BaseAddress.AbsolutePath))
            {
                try
                {
                    var indexHtmlPath = new Uri(openApiSpec.BaseAddress, "index.html");
                    var indexHtmlContent = await ContentHandler.GetContent(indexHtmlPath.AbsolutePath);
                    openApiSpec.SetExternalDoc("Navigate to the site", indexHtmlPath);
                }
                catch(FileContentNotFoundException)
                {
                    
                }
            }

            //
            // remove operations not in the binder
            //
            if(binder != null)
            {
                var allOperations = binder.MethodBindings;
                var httpOperations = allOperations.Where(o => o.Transports.OfType<IHttpTransport>().Any());
                var enabledOperations = httpOperations.Where(o => o.IsEnabled);
                var operations = enabledOperations.ToDictionary(o => o.OperationId, o => o);

                foreach(var op in openApiSpec.Operations)
                {
                    if (!operations.TryGetValue(op.OperationId, out IMethodBinding binding))
                    {
                        openApiSpec.RemoveOperation(op);
                        continue;
                    }
                    var opAddr = binding.Transports.OfType<IHttpTransport>().FirstOrDefault()?.OperationAddress;
                    if (binding.IsLocal)
                    {
                        if (binding.SecurityKey != null)
                        {
                            var definitionName = $"SolidRpcSecurity.{binding.SecurityKey.Value.Key}";
                            op.AddApiKeyAuth(definitionName, binding.SecurityKey.Value.Key.ToLower());
                        }
                        var azFuncAuth = binding.GetSolidProxyConfig<ISolidAzureFunctionConfig>()?.HttpAuthLevel;
                        if (!string.IsNullOrEmpty(azFuncAuth) && !azFuncAuth.Equals("anonymous", StringComparison.InvariantCultureIgnoreCase))
                        {
                            var definitionName = $"AzureFunctions";
                            var headerName = "x-functions-key";
                            op.AddApiKeyAuth(definitionName, headerName);
                        }
                    }
                    else
                    {
                        if (onlyImplemented)
                        {
                            openApiSpec.RemoveOperation(op);
                        }
                        else
                        {
                            op.Description = $"This method is implemented @{opAddr}<br/>{op.Description}";
                        }
                    }
                }
            }

            var json = openApiSpec.WriteAsJsonString();
            var encoding = Encoding.UTF8;
            return new FileContent()
            {
                CharSet = encoding.HeaderName,
                Content = new MemoryStream(encoding.GetBytes(json)),
                ContentType = "application/json"
            };
        }

        /// <summary>
        /// Returns the swagger urls.
        /// </summary>
        /// <param name="onlyImplemented"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<IEnumerable<SwaggerUrl>> GetSwaggerUrls(
            bool onlyImplemented,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var swaggerUrls = new List<SwaggerUrl>();
            foreach (var mb in MethodBinderStore.MethodBinders)
            {
                var assemblyName = mb.Assembly.GetName().Name;
                var openApiSpecResolverAddress = mb.OpenApiSpec.OpenApiSpecResolverAddress;
                var name = mb.OpenApiSpec.Title;
                if (!name.StartsWith(assemblyName))
                {
                    name = $"{assemblyName}.{name}";
                }
                swaggerUrls.Add(new SwaggerUrl()
                {
                    Name = name,
                    Url = await Invoker.GetUriAsync(o => o.GetOpenApiSpec(assemblyName, openApiSpecResolverAddress, onlyImplemented, cancellationToken))
                });
            }
            return swaggerUrls
                .OrderBy(o => o.Name == SwaggerOptions.DefaultOpenApiSpec ? 0 : 1)
                .ThenBy(o => o.Name);
        }
    }
}
