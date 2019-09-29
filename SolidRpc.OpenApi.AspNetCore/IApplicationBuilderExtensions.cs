using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using SolidRpc.Abstractions.OpenApi.Binder;
using SolidRpc.Abstractions.OpenApi.Http;
using SolidRpc.Abstractions.Services;
using SolidRpc.Abstractions.Types;
using SolidRpc.OpenApi.Binder.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Builder
{
    /// <summary>
    /// Extension methods for the application builder
    /// </summary>
    public static class IApplicationBuilderExtensions
    {
        private class PathHandler
        {
            public PathHandler(params string[] allowedCorsOrigins)
            {
                AllowedCorsOrigins = allowedCorsOrigins;
            }

            /// <summary>
            /// The method mapped to the path
            /// </summary>
            public IMethodBinding MethodInfo { get; set; }

            public ISolidRpcContentHandler ContentHandler { get; set; }
            public IEnumerable<string> AllowedCorsOrigins { get; }

            public override string ToString()
            {
                if(MethodInfo != null)
                {
                    return $"Operation:{MethodInfo.OperationId}:{MethodInfo.MethodInfo.DeclaringType.FullName}:{MethodInfo.MethodInfo}";
                }
                else
                {
                    return "Static";
                }
            }
        }

        /// <summary>
        /// Binds all the solid rpc proxies that has an implementation on this server.
        /// </summary>
        /// <param name="applicationBuilder"></param>
        /// <param name="allowedCorsOrigins"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseSolidRpcProxies(this IApplicationBuilder applicationBuilder, params string[] allowedCorsOrigins)
        {
            // if no allowed CORS origins supplied - add the wildcard
            if(!allowedCorsOrigins.Any())
            {
                allowedCorsOrigins = new[] { "*" };
            }
            var dict = new Dictionary<string, PathHandler>();

            //
            // map all static paths
            //
            var contentHandler = applicationBuilder.ApplicationServices.GetService<ISolidRpcContentHandler>();
            if (contentHandler == null)
            {
                throw new Exception("No content handler registered - have you configured the solid rpc services?.");
            }
            foreach (var path in contentHandler.PathPrefixes)
            {
                dict[$"GET{path}"] = new PathHandler("*") { ContentHandler = contentHandler };
                dict[$"HEAD{path}"] = new PathHandler("*") { ContentHandler = contentHandler };
            }
            foreach (var path in contentHandler.PathMappings)
            {
                dict[$"GET{path.Name}"] = new PathHandler("*") { ContentHandler = contentHandler };
                dict[$"HEAD{path.Name}"] = new PathHandler("*") { ContentHandler = contentHandler };
            }

            var bindingStore = applicationBuilder.ApplicationServices.GetService<IMethodBinderStore>();
            if (bindingStore == null)
            {
                throw new Exception("No method binding store registered - please configure during startup.");
            }

            //
            // Extract all paths and map them accordingly.
            //
            bindingStore.MethodBinders
                .SelectMany(o => o.MethodBindings)
                .Where(o => o.IsLocal)
                .ToList()
                .ForEach(o =>
                {
                    var path = $"{o.Method}{o.Address.LocalPath}";
                    if(!dict.TryGetValue(path, out PathHandler binding))
                    {
                        dict[path] = binding = new PathHandler(allowedCorsOrigins);
                    }
                    binding.MethodInfo = o;

                    //register an "options" handler
                    path = $"OPTIONS{o.Address.LocalPath}";
                    if (!dict.TryGetValue(path, out binding))
                    {
                        dict[path] = binding = new PathHandler(allowedCorsOrigins);
                    }
                    binding.MethodInfo = o;
                });

            //
            // start mapping paths
            //
            foreach(var method in dict.Keys.Select(o => o.Split('/')[0]).Distinct())
            {
                applicationBuilder.MapWhen(
                    ctx => string.Equals(ctx.Request.Method, method, StringComparison.InvariantCultureIgnoreCase),
                    (ab) => BindPath(ab, method, dict));
            }
            return applicationBuilder;
        }

        private static void BindPath(IApplicationBuilder ab, string pathPrefix, Dictionary<string, PathHandler> paths)
        {
            //ab.ApplicationServices.LogInformation<IApplicationBuilder>($"Handling path {pathPrefix}");

            if(paths.TryGetValue($"{pathPrefix}/", out PathHandler staticHandler))
            {
                if(staticHandler.ContentHandler != null)
                {
                    ConnectStaticContent(staticHandler.AllowedCorsOrigins, ab, staticHandler.ContentHandler);
                }
            }

            // drill down in sub paths
            var parts = paths.Select(o => o.Key)
                .Where(o => o.StartsWith($"{pathPrefix}/"))
                .Select(o => o.Substring(pathPrefix.Length+1).Split('/')[0])
                .Distinct()
                .ToList();

            var fixedPaths = parts.Where(o => !o.StartsWith("{")).Select(o => $"/{o}").ToList();
            foreach (var part in parts)
            {
                ab.MapWhen(ctx => IsMatch(ctx, $"/{part}", fixedPaths), (sab) => BindPath(sab, $"{pathPrefix}/{part}", paths));
            }

            // add handler for this path
            if (paths.TryGetValue(pathPrefix, out PathHandler pathHandler))
            {
                ab.ApplicationServices.LogInformation<IApplicationBuilder>($"Binding path {pathPrefix} to {pathHandler}");
                if(pathHandler.MethodInfo != null)
                {
                    ab.Run((ctx) => HandleInvocation(pathHandler.AllowedCorsOrigins, pathHandler.MethodInfo, ctx));
                }
                else if (pathHandler.ContentHandler != null)
                {
                    ConnectStaticContent(pathHandler.AllowedCorsOrigins, ab, pathHandler.ContentHandler);
                }
            }
        }

        private static void ConnectStaticContent(IEnumerable<string> allowedCorsOrigins, IApplicationBuilder ab, ISolidRpcContentHandler contentHandler)
        {
            ab.Use(async (ctx, next) =>
            {
                await next();
                await HandleInvocation(allowedCorsOrigins, contentHandler, ctx);
            });
        }

        private static bool IsMatch(HttpContext ctx, string segment, IEnumerable<string> fixedPaths)
        {
            var path = ctx.Request.Path.Value;
            if (!path.StartsWith("/"))
            {
                return false;
            }
            var nextSlashIdx = path.IndexOf('/', 1);
            if(nextSlashIdx > -1)
            {
                path = path.Substring(0, nextSlashIdx);
            }
            if(path == segment)
            {
                ctx.Request.Path = ctx.Request.Path.Value.Substring(path.Length);
                ctx.Request.PathBase = ctx.Request.PathBase.Add(path);
                return true;
            }
            if(fixedPaths.Contains(path))
            {
                return false;
            }
            if(segment.StartsWith("/{"))
            {
                ctx.Request.Path = ctx.Request.Path.Value.Substring(path.Length);
                ctx.Request.PathBase = ctx.Request.PathBase.Add(path);
                return true;
            }
            return false;
        }

        private static async Task HandleInvocation(IEnumerable<string> allowedCorsOrigins, ISolidRpcContentHandler contentHandler, HttpContext ctx)
        {
            if(ctx.Response.StatusCode != 404)
            {
                return;
            }
            try
            {
                // get content
                var path = $"{ctx.Request.PathBase}{ctx.Request.Path}";
                var content = await contentHandler.GetContent(path, ctx.RequestAborted);

                // send response
                ctx.Response.StatusCode = 200;
                var charset = string.IsNullOrEmpty(content.CharSet) ? "" : $"; charset=\"{content.CharSet}\"";
                ctx.Response.ContentType = $"{content.ContentType}{charset}";
                await content.Content.CopyToAsync(ctx.Response.Body);
            } 
            catch(FileContentNotFoundException)
            {
                ctx.Response.StatusCode = 404;
            }
        }

        private static async Task HandleInvocation(IEnumerable<string> allowedCorsOrigins, IMethodBinding methodInfo, HttpContext context)
        {
            try
            {
                //
                // check if the request is intended for this path.
                //
                if (context.Request.Path != "")
                {
                    return;
                }

                //
                // handle the access-control(CORS) request for this invocation
                //
                if(!CheckCorsIsValid(allowedCorsOrigins, methodInfo, context))
                {
                    return;
                }

                //
                // if the supplied method does not match the handler - return.
                // This is the case with "OPTIONS" in cors preflight
                //
                if(methodInfo.Method != context.Request.Method)
                {
                    context.Response.StatusCode = 204; // No content
                    AddAllowedCorsHeaders(context);
                    return;
                }

                context.RequestServices.LogTrace<IApplicationBuilder>($"Letting {methodInfo.OperationId}:{methodInfo.MethodInfo} handle invocation to {context.Request.Method}:{context.Request.PathBase}{context.Request.Path}");

                // extract information from http context.
                var request = new SolidRpc.OpenApi.Binder.Http.SolidHttpRequest();
                await request.CopyFromAsync(context.Request);

                var methodInvoker = context.RequestServices.GetRequiredService<IMethodInvoker>();
                var response = await methodInvoker.InvokeAsync(request, methodInfo, context.RequestAborted);

                // send data back
                AddAllowedCorsHeaders(context);
                await response.CopyToAsync(context.Response);
            }
            catch (Exception e)
            {
                context.RequestServices.LogError<IApplicationBuilder>(e, "Failed to invoke service");
                throw;
            }
        }

        private static void AddAllowedCorsHeaders(HttpContext context)
        {
            var origin = context.Request.Headers["origin"];
            if(string.IsNullOrEmpty(origin))
            {
                return;
            }
            if (!string.IsNullOrEmpty(origin))
            {
                context.Response.Headers["Access-Control-Allow-Origin"] = origin;
            }
            var accessControlRequestHeaders = context.Request.Headers["Access-Control-Request-Headers"];
            if (!string.IsNullOrEmpty(accessControlRequestHeaders))
            {
                context.Response.Headers["Access-Control-Allow-Headers"] = accessControlRequestHeaders;
            }
            var accessControlRequestMethod = context.Request.Headers["Access-Control-Request-Method"];
            if (!string.IsNullOrEmpty(accessControlRequestMethod))
            {
                context.Response.Headers["Access-Control-Allow-Method"] = accessControlRequestMethod;
            }
            context.Response.Headers["Access-Control-Max-Age"] = "86400";
        }

        private static bool CheckCorsIsValid(IEnumerable<string> allowedCorsOrigins, IMethodBinding methodInfo, HttpContext context)
        {
            var origin = context.Request.Headers["origin"];
            if(!allowedCorsOrigins.Contains("*") && !allowedCorsOrigins.Any(o => origin.ToString().StartsWith(o)))
            {
                // request not allowed
                context.Response.StatusCode = 401;
                context.RequestServices.LogInformation<IApplicationBuilder>($"Rejecting request. {origin} not part of allowed origins {string.Join(",", allowedCorsOrigins)}");
                return false;
            }
            return true;
        }
    }
}
