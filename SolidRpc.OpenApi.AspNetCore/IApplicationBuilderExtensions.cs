using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using SolidRpc.Abstractions.OpenApi.Binder;
using SolidRpc.Abstractions.OpenApi.Http;
using SolidRpc.Abstractions.Services;
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
            /// <summary>
            /// The method mapped to the path
            /// </summary>
            public IMethodInfo MethodInfo { get; set; }

            public ISolidRpcStaticContent StaticContent { get; set; }

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
        /// Exposes the open api config for supplied method binder.
        /// </summary>
        /// <param name="applicationBuilder"></param>
        /// <param name="methodBinder"></param>
        /// <returns></returns>

        public static string UseOpenApiConfig(this IApplicationBuilder applicationBuilder, IMethodBinder methodBinder)
        {
            var name = methodBinder.Assembly.GetName();
            var spec = methodBinder.OpenApiSpec;
            var path = $"{spec.BaseAddress.AbsolutePath}/{name.Name}-v{spec.OpenApiVersion}-{name.Version}.json";
            var paths = path.Split('/').Where(o => !string.IsNullOrEmpty(o));
            MapPath(applicationBuilder, paths, methodBinder);
            return path;
        }

        private static void MapPath(IApplicationBuilder applicationBuilder, IEnumerable<string> paths, IMethodBinder methodBinder)
        {
            if(paths.Count() == 1)
            {
                applicationBuilder.Run(async ctx =>
                {
                    var openApiSpec = methodBinder.OpenApiSpec;
                    openApiSpec.SetSchemeAndHostAndPort(ctx.Request.GetUri());
                    ctx.Response.StatusCode = 200;
                    using (var sw = new StreamWriter(ctx.Response.Body))
                    {
                        ctx.Response.Headers["Content-Type"] = $"application/json; charset=\"{sw.Encoding.EncodingName}\"";
                        await sw.WriteAsync(openApiSpec.WriteAsJsonString());
                    }
                });
            }
            else
            {
                applicationBuilder.Map($"/{paths.First()}", o => MapPath(o, paths.Skip(1), methodBinder));
            }
        }

        /// <summary>
        /// Binds all the solid rpc proxies that has an implementation on this server.
        /// </summary>
        /// <param name="applicationBuilder"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseSolidRpcProxies(this IApplicationBuilder applicationBuilder)
        {
            var dict = new Dictionary<string, PathHandler>();

            //
            // map all static paths
            //
            var staticContent = applicationBuilder.ApplicationServices.GetService<ISolidRpcStaticContent>();
            if (staticContent != null)
            {
                foreach(var path in staticContent.PathPrefixes)
                {
                    dict[$"GET{path}"] = new PathHandler() { StaticContent = staticContent };
                    dict[$"HEAD{path}"] = new PathHandler() { StaticContent = staticContent };
                }
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
                .SelectMany(o => o.MethodInfos)
                .ToList()
                .ForEach(o =>
                {
                    var path = $"{o.Method}{o.Path}";
                    if(!dict.TryGetValue(path, out PathHandler binding))
                    {
                        dict[path] = binding = new PathHandler();
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
                if(staticHandler.StaticContent != null)
                {
                    ConnectStaticContent(ab);
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
                    ab.Run((ctx) => HandleInvocation(pathHandler.MethodInfo, ctx));
                }
                else if (pathHandler.StaticContent != null)
                {
                    ConnectStaticContent(ab);
                }
            }
        }

        private static void ConnectStaticContent(IApplicationBuilder ab)
        {
            ab.Use(async (ctx, next) =>
            {
                await next();
                await HandleInvocation(ctx.RequestServices.GetRequiredService<ISolidRpcStaticContent>(), ctx);
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

        private static async Task HandleInvocation(ISolidRpcStaticContent staticContent, HttpContext ctx)
        {
            if(ctx.Response.StatusCode != 404)
            {
                return;
            }
            // get content
            var path = $"{ctx.Request.PathBase}{ctx.Request.Path}";
            var content = await staticContent.GetStaticContent(path, ctx.RequestAborted);

            // send response
            ctx.Response.StatusCode = 200;
            var charset = string.IsNullOrEmpty(content.CharSet) ? "" : $"; charset=\"{content.CharSet}\"";
            ctx.Response.ContentType = $"{content.ContentType}{charset}";
            await content.Content.CopyToAsync(ctx.Response.Body);
        }

        private static async Task HandleInvocation(IMethodInfo methodInfo, HttpContext context)
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

                context.RequestServices.LogTrace<IApplicationBuilder>($"Letting {methodInfo.OperationId}:{methodInfo.MethodInfo} handle invocation to {context.Request.Method}:{context.Request.PathBase}{context.Request.Path}");

                // extract information from http context.
                var request = new SolidRpc.OpenApi.Binder.Http.SolidHttpRequest();
                await request.CopyFromAsync(context.Request);

                var methodInvoker = context.RequestServices.GetRequiredService<IMethodInvoker>();
                var response = await methodInvoker.InvokeAsync(request, methodInfo, context.RequestAborted);

                // send data back
                await response.CopyToAsync(context.Response);
            }
            catch (Exception e)
            {
                context.RequestServices.LogError<IApplicationBuilder>(e, "Failed to invoke service");
                throw;
            }
        }
    }
}
