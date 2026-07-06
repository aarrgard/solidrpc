using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SolidRpc.Abstractions.InternalServices;
using SolidRpc.Abstractions.OpenApi.Binder;
using SolidRpc.Abstractions.OpenApi.Http;
using SolidRpc.Abstractions.OpenApi.Invoker;
using SolidRpc.Abstractions.OpenApi.Transport;
using SolidRpc.Abstractions.Services;
using SolidRpc.Abstractions.Types;
using SolidRpc.OpenApi.AspNetCore;
using SolidRpc.OpenApi.Binder.Http;
using SolidRpc.OpenApi.Binder.Invoker;
using SolidRpc.OpenApi.Binder.Proxy;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolidRpc.OpenApi.AspNetCore {
    public class IApplicationBuilderExtensionsLogging { }
}

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
        ///
        private class PathHandler
        {
            public PathHandler(string path)
            {
                Path = path;
            }

            /// <summary>
            /// The path
            /// </summary>
            public string Path { get;  }

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

        private class SegmentHandler
        {
            private IDictionary<string, SegmentHandler> segmentHandlers = new Dictionary<string, SegmentHandler>();

            public SegmentHandler(SegmentHandler parent, string segment, PathHandler pathHandler, Func<HttpContext, Task> preInvoke, Func<HttpContext, Task> postInvoke)
            {
                Parent = parent;
                Segment = segment;
                PathHandler = pathHandler;
                PreInvoke = preInvoke;
                PostInvoke = postInvoke;
            }

            public SegmentHandler Parent { get; }
            public string Segment { get; }
            public PathHandler PathHandler { get; }
            public Func<HttpContext, Task> PreInvoke { get; }
            public Func<HttpContext, Task> PostInvoke { get; }

            public void Initialize(Dictionary<string, PathHandler> dict)
            {
                // grab all the first segments
                foreach (var baseSegment in dict.Keys.Select(o => o.Split('/')[0]).Distinct())
                {
                    dict.TryGetValue(baseSegment, out PathHandler pathHandler);
                    var segmentHandler = new SegmentHandler(this, baseSegment, pathHandler, PreInvoke, PostInvoke);
                    var subSegments = dict.Where(o => o.Key.StartsWith(baseSegment + "/"))
                        .ToDictionary(o => o.Key.Substring(baseSegment.Length + 1), o => o.Value);
                    segmentHandler.Initialize(subSegments);
                    segmentHandlers[baseSegment] = segmentHandler;
                }
            }

            public Task<bool> HandleRequest(HttpContext ctx)
            {
                if(segmentHandlers.TryGetValue(ctx.Request.Method, out SegmentHandler segmentHandler)) 
                {
                    return segmentHandler.HandlePath(ctx, "", ctx.Request.PathBase + ctx.Request.Path, false);
                }
                return Task.FromResult(false);
            }

            public async Task<bool> HandlePath(HttpContext ctx, string matched, string rest, bool lastMatchIsVariable)
            {
                if(string.IsNullOrEmpty(rest))
                {
                    if (PathHandler == null)
                    {
                        return false;
                    }
                    return await HandleInvocation(PathHandler, ctx);
                }
                if (!rest.StartsWith("/"))
                {
                    throw new ArgumentException();
                }
                var segment = rest.Substring(1);
                var segmentIdx = segment.IndexOf('/');
                if(segmentIdx > -1)
                {
                    segment = segment.Substring(0, segmentIdx);
                }
                if(segmentHandlers.TryGetValue(segment, out SegmentHandler segmentHandler))
                {
                    return await segmentHandler.HandlePath(ctx, $"{matched}/{segment}", rest.Substring(segment.Length + 1), false);
                }
                foreach(var varSegment in segmentHandlers.Where(o => o.Key.StartsWith("{")))
                {
                    // we need to use the "raw" url to get correct data 
                    var reqFeat = (IHttpRequestFeature)ctx.Features[typeof(IHttpRequestFeature)];
                    var addrTrans = ctx.RequestServices.GetRequiredService<IMethodAddressTransformer>();
                    var rawPath = addrTrans.RewritePath(reqFeat.RawTarget);

                    var nextRawSegment = GetNextRawSegment(matched, rawPath);
                    nextRawSegment = DecodeHex(nextRawSegment, '/');

                    if (!rest.StartsWith(nextRawSegment))
                    {
                        nextRawSegment = DecodeHex(nextRawSegment);
                        rest = DecodeHex(rest);
                        if (!rest.StartsWith(nextRawSegment))
                        {
                            throw new Exception("Path does not start with extracted next segement");
                        }
                    }

                    var handled = await varSegment.Value.HandlePath(ctx, $"{matched}{nextRawSegment}", rest.Substring(nextRawSegment.Length), true);
                    if(handled)
                    {
                        return true;
                    }
                }

                if(segmentHandlers.TryGetValue("*", out segmentHandler))
                {
                    return await segmentHandler.HandlePath(ctx, $"{matched}{rest}", "", true);
                }

                if(lastMatchIsVariable)
                {
                    return await HandlePath(ctx, $"{matched}{rest}", "", false);
                }
                else
                {
                    return false;
                }
            }

            private async Task<bool> HandleInvocation(PathHandler pathHandler, HttpContext ctx)
            {
                var logger = ctx.RequestServices.GetRequiredService<ILogger<IApplicationBuilderExtensionsLogging>>();
                // bind path
                if (pathHandler.MethodBinding != null)
                {
                    logger.LogTrace($"{pathHandler.Path} using method binding to handle invocation");
                    try
                    {
                        await PreInvoke.Invoke(ctx);
                        await HandleInvocation(pathHandler.HttpTransport, pathHandler.MethodBinding, ctx);
                        return true;
                    }
                    finally
                    {
                        await PostInvoke.Invoke(ctx);
                    }
                }
                else if (pathHandler.ContentHandler != null)
                {
                    logger.LogTrace($"{pathHandler.Path} using content handler handle invocation");
                    return await HandleInvocation(pathHandler.ContentHandler, ctx);
                }
                else
                {
                    logger.LogTrace($"{pathHandler.Path} no handler for path");
                    return false;
                }
            }

            private async Task<bool> HandleInvocation(ISolidRpcContentHandler contentHandler, HttpContext ctx)
            {
                if (ctx.IsProcessed())
                {
                    return true;
                }
                try
                {
                    // send response
                    var request = new SolidHttpRequest();
                    await request.CopyFromAsync(ctx.Request);

                    // get content
                    var path = $"{ctx.Request.PathBase}{ctx.Request.Path}";
                    using (InvocationOptions.Current.SetKeyValues(MethodInvoker.GetRequestHeadersAndQueryString(request)).Attach())
                    {
                        var content = await contentHandler.GetContent(path, ctx.RequestAborted);

                        var resp = new SolidHttpResponse();
                        resp.StatusCode = 200;
                        resp.CharSet = content.CharSet;
                        resp.MediaType = content.ContentType;
                        resp.ResponseStream = content.Content;
                        resp.Location = content.Location;
                        resp.AddAllowedCorsHeaders(request);

                        await resp.CopyToAsync(ctx.Response);
                        ctx.SetProcessed();
                    }
                }
                catch (FileContentNotFoundException)
                {
                    ctx.Response.StatusCode = FileContentNotFoundException.HttpStatusCode;
                }
                catch (UnauthorizedException)
                {
                    ctx.Response.StatusCode = UnauthorizedException.HttpStatusCode;
                }
                return true;
            }

            private async Task HandleInvocation(IHttpTransport httpTransport, IMethodBinding methodBinding, HttpContext context)
            {
                try
                {
                    // extract information from http context.
                    var request = new SolidHttpRequest();
                    await request.CopyFromAsync(context.Request);

                    context.RequestServices.LogTrace<IApplicationBuilderExtensionsLogging>($"Letting {methodBinding.OperationId}:{methodBinding.MethodInfo} handle invocation to {context.Request.Method}:{context.Request.PathBase}{context.Request.Path}");

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
                    context.RequestServices.LogError<IApplicationBuilderExtensionsLogging>(e, "Failed to invoke service");
                    throw;
                }
            }
        }

        /// <summary>
        /// Binds all the solid rpc proxies that has an implementation on this server.
        /// </summary>
        /// <param name="applicationBuilder"></param>
        /// <param name="preInvoke"></param>
        /// <param name="postInvoke"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseSolidRpcProxies(
            this IApplicationBuilder applicationBuilder,
            Func<HttpContext, Task> preInvoke = null,
            Func<HttpContext, Task> postInvoke = null)
        {
            if (preInvoke is null)
            {
                preInvoke = (ctx) => Task.CompletedTask;
            }
            if (postInvoke is null)
            {
                postInvoke = (ctx) => Task.CompletedTask;
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
                dict[$"GET{path}"] = new PathHandler(path) { ContentHandler = contentHandler };
                dict[$"HEAD{path}"] = new PathHandler(path) { ContentHandler = contentHandler };
            }
            foreach (var path in contentHandler.GetPathMappingsAsync(false).Result)
            {
                dict[$"GET{path.Name}"] = new PathHandler(path.Name) { ContentHandler = contentHandler };
                dict[$"HEAD{path.Name}"] = new PathHandler(path.Name) { ContentHandler = contentHandler };
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
                    applicationBuilder.ApplicationServices.LogInformation<IApplicationBuilderExtensionsLogging>($"No http transport configured for binding {o.OperationId} - will not map path.");
                    continue;
                } 
                var path = $"{o.Method}{httpTransport.OperationAddress.LocalPath}";
                if(!dict.TryGetValue(path, out PathHandler binding))
                {
                    dict[path] = binding = new PathHandler(path);
                }
                binding.MethodBinding = o;
                binding.HttpTransport = httpTransport;

                //register an "options" handler
                path = $"OPTIONS{httpTransport.OperationAddress.LocalPath}";
                if (!dict.TryGetValue(path, out binding))
                {
                    dict[path] = binding = new PathHandler(path);
                }
                binding.MethodBinding = o;
                binding.HttpTransport = httpTransport;
            }

            //
            // map all the paths to segment handlers
            //
            var rootSegment = new SegmentHandler(null, null, null, preInvoke, postInvoke);
            rootSegment.Initialize(dict);
            applicationBuilder.Use((ctx, next) => HandleRequest(ctx, next, rootSegment));

            return applicationBuilder;
        }

        private static async Task HandleRequest(HttpContext ctx, Func<Task> next, SegmentHandler rootSegment)
        {
            if(await rootSegment.HandleRequest(ctx))
            {
                return;
            }
            else
            {
                await next();
            }
        }

        private static async Task RewriteUrl(HttpContext ctx, Func<Task> next)
        {
            var trans = ctx.RequestServices.GetRequiredService<IMethodAddressTransformer>();
            var oldPath = ctx.Request.Path;
            var newPath = trans.RewritePath(oldPath);

            var logger = ctx.RequestServices.GetRequiredService<ILogger<IApplicationBuilderExtensionsLogging>>();
            if(oldPath == newPath)
            {
                logger.LogTrace($"Path not rewritten");
            }
            else
            {
                logger.LogTrace($"Path rewritten from {oldPath} to {newPath}");
                ctx.Request.Path = newPath;
            }

            await next();
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
            if (string.IsNullOrEmpty(str)) return str;

            // Simple percent-decoder that collects consecutive %XX sequences,
            // decodes them as UTF-8 bytes and appends decoded chars unless they are in skipChars.
            // Use a pooled byte buffer to avoid allocating for common small sequences.

            var skipSet = (skipChars == null || skipChars.Length == 0) ? null : new HashSet<char>(skipChars);
            var sb = new StringBuilder(str.Length);
            var pool = System.Buffers.ArrayPool<byte>.Shared;
            byte[] rented = null;
            try
            {
                int len = str.Length;
                for (int i = 0; i < len; i++)
                {
                    char c = str[i];
                    if (c != '%')
                    {
                        sb.Append(c);
                        continue;
                    }

                    // collect consecutive %XX sequences
                    int j = i;
                    int byteCount = 0;
                    while (j + 2 < len && str[j] == '%')
                    {
                        int hi = HexValue(str[j + 1]);
                        int lo = HexValue(str[j + 2]);
                        if (hi < 0 || lo < 0) break;
                        if (rented == null)
                        {
                            rented = pool.Rent(8);
                        }
                        if (byteCount >= rented.Length)
                        {
                            // grow buffer
                            var newBuf = pool.Rent(rented.Length * 2);
                            Array.Copy(rented, newBuf, rented.Length);
                            pool.Return(rented);
                            rented = newBuf;
                        }
                        rented[byteCount++] = (byte)((hi << 4) | lo);
                        j += 3;
                    }

                    if (byteCount == 0)
                    {
                        // not a valid escape sequence, copy the '%' char
                        sb.Append('%');
                        continue;
                    }

                    // Decode collected bytes as UTF8
                    string decoded = Encoding.UTF8.GetString(rented, 0, byteCount);

                    // If decoded result is a single char and it's in skipSet, then append the original percent-encoded text
                    if (decoded.Length == 1 && skipSet != null && skipSet.Contains(decoded[0]))
                    {
                        sb.Append(str.Substring(i, (j - i)));
                    }
                    else
                    {
                        sb.Append(decoded);
                    }

                    i = j - 1; // advance outer loop
                }

                return sb.ToString();
            }
            finally
            {
                if (rented != null) pool.Return(rented);
            }
        }

        private static int HexValue(char c)
        {
            if (c >= '0' && c <= '9') return c - '0';
            if (c >= 'a' && c <= 'f') return c - 'a' + 10;
            if (c >= 'A' && c <= 'F') return c - 'A' + 10;
            return -1;
        }
    }
}
