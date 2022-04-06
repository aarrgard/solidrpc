using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.DependencyInjection;
using SolidRpc.Abstractions.InternalServices;
using SolidRpc.Abstractions.OpenApi.Binder;
using SolidRpc.Abstractions.OpenApi.Http;
using SolidRpc.Abstractions.OpenApi.Transport;
using SolidRpc.Abstractions.Services;
using SolidRpc.Abstractions.Types;
using SolidRpc.OpenApi.Binder.Http;
using SolidRpc.OpenApi.Binder.Invoker;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Microsoft.AspNetCore.Builder
{
    public static class HttpContextExtensions
    {
        public static bool IsProcessed(this HttpContext httpContext)
        {
            return (httpContext.Items["__Processed__"] as bool?) ?? false;
        }
        public static void SetProcessed(this HttpContext httpContext)
        {
            httpContext.Items["__Processed__"] = true;
        }
    }

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
            public IHttpTransport HttpTransport { get; set; }

            /// <summary>
            /// The method binding
            /// </summary>
            public IMethodBinding MethodBinding { get; set; }

            /// <summary>
            /// The content handler
            /// </summary>
            public ISolidRpcContentHandler ContentHandler { get; set; }

            public override string ToString()
            {
                if(MethodBinding != null)
                {
                    return $"Operation:{MethodBinding.OperationId}:{MethodBinding.MethodInfo.DeclaringType.FullName}:{MethodBinding.MethodInfo}";
                }
                else
                {
                    return "Static content";
                }
            }
        }

        /// <summary>
        /// Binds all the solid rpc proxies that has an implementation on this server.
        /// </summary>
        /// <param name="applicationBuilder"></param>
        /// <param name="preInvoke"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseSolidRpcProxies(this IApplicationBuilder applicationBuilder, Func<HttpContext, Task> preInvoke = null)
        {
            if (preInvoke is null)
            {
                preInvoke = (ctx) => Task.CompletedTask;
            }

            applicationBuilder.Use(RewriteUrl);

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
                dict[$"GET{path}"] = new PathHandler() { ContentHandler = contentHandler };
                dict[$"HEAD{path}"] = new PathHandler() { ContentHandler = contentHandler };
            }
            foreach (var path in contentHandler.GetPathMappingsAsync(false).Result)
            {
                dict[$"GET{path.Name}"] = new PathHandler() { ContentHandler = contentHandler };
                dict[$"HEAD{path.Name}"] = new PathHandler() { ContentHandler = contentHandler };
            }

            var bindingStore = applicationBuilder.ApplicationServices.GetService<IMethodBinderStore>();
            if (bindingStore == null)
            {
                throw new Exception("No method binding store registered - please configure during startup.");
            }

            //
            // Extract all paths and map them accordingly.
            //
            foreach(var o in bindingStore.MethodBinders.SelectMany(o => o.MethodBindings))
            {
                if (!o.IsEnabled)
                {
                    continue;
                }
                var httpTransport = o.Transports.OfType<IHttpTransport>().FirstOrDefault();
                if(httpTransport == null)
                {
                    applicationBuilder.ApplicationServices.LogInformation<IApplicationBuilder>($"No http transport configured for binding {o.OperationId} - will not map path.");
                    continue;
                } 
                var path = $"{o.Method}{httpTransport.OperationAddress.LocalPath}";
                if(!dict.TryGetValue(path, out PathHandler binding))
                {
                    dict[path] = binding = new PathHandler();
                }
                binding.MethodBinding = o;
                binding.HttpTransport = httpTransport;

                //register an "options" handler
                path = $"OPTIONS{httpTransport.OperationAddress.LocalPath}";
                if (!dict.TryGetValue(path, out binding))
                {
                    dict[path] = binding = new PathHandler();
                }
                binding.MethodBinding = o;
                binding.HttpTransport = httpTransport;
            }

            //
            // start mapping paths
            //
            foreach(var method in dict.Keys.Select(o => o.Split('/')[0]).Distinct())
            {
                applicationBuilder.MapWhen(
                    ctx => string.Equals(ctx.Request.Method, method, StringComparison.InvariantCultureIgnoreCase),
                    (ab) => BindPath(ab, method, dict, preInvoke));
            }
            return applicationBuilder;
        }

        private static Task RewriteUrl(HttpContext ctx, Func<Task> next)
        {
            var trans = ctx.RequestServices.GetRequiredService<IMethodAddressTransformer>();
            ctx.Request.Path = trans.RewritePath(ctx.Request.Path);
            return next();
        }

        private static void BindPath(IApplicationBuilder ab, string pathPrefix, Dictionary<string, PathHandler> paths, Func<HttpContext, Task> preInvoke)
        {
            //ab.ApplicationServices.LogInformation<IApplicationBuilder>($"Handling path {pathPrefix}");

            if(paths.TryGetValue($"{pathPrefix}/", out PathHandler staticHandler))
            {
                if(staticHandler.ContentHandler != null)
                {
                    ConnectStaticContent(ab, staticHandler.ContentHandler);
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
                ab.MapWhen(ctx => IsMatch(ctx, $"/{part}", fixedPaths), (sab) => BindPath(sab, $"{pathPrefix}/{part}", paths, preInvoke));
            }

            // add handler for this path
            if (paths.TryGetValue(pathPrefix, out PathHandler pathHandler))
            {
                // emit to log
                ab.ApplicationServices.LogInformation<IApplicationBuilder>($"Binding path {pathPrefix} to {pathHandler}");
                
                // bind path
                if(pathHandler.MethodBinding != null)
                {
                    ab.Run(async (ctx) => {
                        await preInvoke.Invoke(ctx);
                        await HandleInvocation(pathHandler.HttpTransport, pathHandler.MethodBinding, ctx);
                    });
                }
                else if (pathHandler.ContentHandler != null)
                {
                    ConnectStaticContent(ab, pathHandler.ContentHandler);
                }
            }
        }

        private static void ConnectStaticContent(IApplicationBuilder ab, ISolidRpcContentHandler contentHandler)
        {
            ab.Use(async (ctx, next) =>
            {
                await next();
                await HandleInvocation(contentHandler, ctx);
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
                // we need to use the "raw" url to get correct data 
                var reqFeat = (IHttpRequestFeature)ctx.Features[typeof(IHttpRequestFeature)];
                var addrTrans = ctx.RequestServices.GetRequiredService<IMethodAddressTransformer>();
                var rawPath = addrTrans.RewritePath(reqFeat.RawTarget);

                var nextRawSegment = GetNextRawSegment(ctx.Request.PathBase, rawPath);
                nextRawSegment = DecodeHex(nextRawSegment, '/');
                
                path = ctx.Request.Path.Value;
                if (!path.StartsWith(nextRawSegment))
                {
                    nextRawSegment = DecodeHex(nextRawSegment);
                    if (!path.StartsWith(nextRawSegment))
                    {
                        throw new Exception("Path does not start with extracted next segement");
                    }
                }

                ctx.Request.Path = path.Substring(nextRawSegment.Length);
                ctx.Request.PathBase = ctx.Request.PathBase.Add(nextRawSegment);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Returns the next segment. We use strict matching against the decoded path
        /// so that we dont allow double escaped sequences. 
        /// https://owasp.org/www-community/Double_Encoding
        /// </summary>
        /// <param name="pathBase"></param>
        /// <param name="rawPath"></param>
        /// <returns></returns>
        private static string GetNextRawSegment(string pathBase, string rawPath)
        {
            //
            // for some reason the [] are not decoded?? - perhaps more will show up...
            //
            pathBase = DecodeHex(pathBase);

            if (!DecodeHex(rawPath).StartsWith(pathBase))
            {
                throw new Exception("raw path does not start with path base!");
            }

            // split raw path into segments and remove decoded version from path base
            var rawSegments = rawPath.Split('/').AsEnumerable().GetEnumerator();
            if(!rawSegments.MoveNext()) throw new Exception("Cannot move to next segment!");
            if(rawSegments.Current != "") throw new Exception("Paths does not start with slash!");
            while (pathBase.Length > 0)
            {
                if (!rawSegments.MoveNext()) throw new Exception("Cannot move to next segment!");
                var rawDecodedSegment = "/" + DecodeHex(rawSegments.Current);
                if (!pathBase.StartsWith(rawDecodedSegment))
                {
                    throw new Exception("Something is rotten in the state of denmark!");
                }
                pathBase = pathBase.Substring(rawDecodedSegment.Length);
            }

            if(!rawSegments.MoveNext())
            {
                return null;
            }

            // raw url might contain query parameters
            var currentRawSegment = rawSegments.Current;
            var queryStart = currentRawSegment.IndexOf('?');
            if (queryStart > -1)
            {
                currentRawSegment = currentRawSegment.Substring(0, queryStart);
            }

            return $"/{currentRawSegment}";
        }

        private static string DecodeHex(string str, params char[] skipChars)
        {
            var sb = new StringBuilder();
            int escapeSequence = 0;
            for(int i = 0; i < str.Length; i++)
            {
                switch (str[i])
                {
                    case '%':
                        escapeSequence ++;
                        sb.Append(str[i]);
                        break;
                    default:
                        if (escapeSequence > 0)
                        {
                            escapeSequence++;
                        }
                        sb.Append(str[i]);
                        if (escapeSequence == 3)
                        {
                            var hex = $"{sb[sb.Length - 2]}{sb[sb.Length - 1]}";
                            if(int.TryParse(hex, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out int nbr))
                            {
                                var c = (char)nbr;
                                if(!skipChars.Contains(c))
                                {
                                    sb.Length = sb.Length - 3;
                                    sb.Append(c);
                                }
                            }
                            escapeSequence = 0;
                        }
                        break;
                }
            }
            return sb.ToString();
        }

        private static async Task HandleInvocation(ISolidRpcContentHandler contentHandler, HttpContext ctx)
        {
            if(ctx.IsProcessed())
            {
                return;
            }
            try
            {
                // get content
                var path = $"{ctx.Request.PathBase}{ctx.Request.Path}";
                var content = await contentHandler.GetContent(path, ctx.RequestAborted);

                // send response
                var request = new SolidHttpRequest();
                await request.CopyFromAsync(ctx.Request);

                var resp = new SolidHttpResponse();
                resp.StatusCode = 200;
                resp.CharSet = content.CharSet;
                resp.MediaType = content.ContentType;
                resp.ResponseStream = content.Content;
                resp.AddAllowedCorsHeaders(request);

                await resp.CopyToAsync(ctx.Response);
                ctx.SetProcessed();
            }
            catch (FileContentNotFoundException)
            {
                ctx.Response.StatusCode = FileContentNotFoundException.HttpStatusCode;
            }
            catch (UnauthorizedException)
            {
                ctx.Response.StatusCode = UnauthorizedException.HttpStatusCode;
            }
        }

        private static async Task HandleInvocation(IHttpTransport httpTransport, IMethodBinding methodBinding, HttpContext context)
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

                // extract information from http context.
                var request = new SolidHttpRequest();
                await request.CopyFromAsync(context.Request);
                
                context.RequestServices.LogTrace<IApplicationBuilder>($"Letting {methodBinding.OperationId}:{methodBinding.MethodInfo} handle invocation to {context.Request.Method}:{context.Request.PathBase}{context.Request.Path}");

                context.RequestServices.GetRequiredService<ISolidRpcAuthorization>().CurrentPrincipal = context.User;
                var httpHandler = context.RequestServices.GetRequiredService<HttpHandler>();
                var methodInvoker = context.RequestServices.GetRequiredService<IMethodInvoker>();
                var response = await methodInvoker.InvokeAsync(context.RequestServices, httpHandler, request, new[] { methodBinding }, context.RequestAborted);

                // send data back
                await response.CopyToAsync(context.Response);

                context.SetProcessed();
            }
            catch (Exception e)
            {
                context.RequestServices.LogError<IApplicationBuilder>(e, "Failed to invoke service");
                throw;
            }
        }
    }
}
