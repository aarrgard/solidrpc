﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.DependencyInjection;
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
            public IHttpTransport HttpTransport { get; set; }

            /// <summary>
            /// The method binding
            /// </summary>
            public IMethodBinding MethodBinding { get; set; }

            /// <summary>
            /// The content handler
            /// </summary>
            public ISolidRpcContentHandler ContentHandler { get; set; }

            /// <summary>
            /// The allowed cors origins. 
            /// </summary>
            public IEnumerable<string> AllowedCorsOrigins { get; }

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
            foreach (var path in contentHandler.GetPathMappingsAsync(false).Result)
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
                .Where(o => o.IsEnabled)
                .ToList()
                .ForEach(o =>
                {
                    var httpTransport = o.Transports.OfType<IHttpTransport>().FirstOrDefault();
                    if(httpTransport == null)
                    {
                        applicationBuilder.ApplicationServices.LogInformation<IApplicationBuilder>($"No http transport configured for binding {o.OperationId} - will not map path.");
                        return;
                    } 
                    var path = $"{o.Method}{httpTransport.OperationAddress.LocalPath}";
                    if(!dict.TryGetValue(path, out PathHandler binding))
                    {
                        dict[path] = binding = new PathHandler(allowedCorsOrigins);
                    }
                    binding.MethodBinding = o;
                    binding.HttpTransport = httpTransport;

                    //register an "options" handler
                    path = $"OPTIONS{httpTransport.OperationAddress.LocalPath}";
                    if (!dict.TryGetValue(path, out binding))
                    {
                        dict[path] = binding = new PathHandler(allowedCorsOrigins);
                    }
                    binding.MethodBinding = o;
                    binding.HttpTransport = httpTransport;
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
                // emit to log
                var secKeyOutput = "";
                var secKey = pathHandler.MethodBinding?.SecurityKey;
                if (secKey != null)
                {
                    secKeyOutput = $", use {secKey.Value.Key}:{secKey.Value.Value} to access resources";
                }
                ab.ApplicationServices.LogInformation<IApplicationBuilder>($"Binding path {pathPrefix} to {pathHandler}{secKeyOutput}");
                
                // bind path
                if(pathHandler.MethodBinding != null)
                {
                    ab.Run((ctx) => HandleInvocation(pathHandler.AllowedCorsOrigins, pathHandler.HttpTransport, pathHandler.MethodBinding, ctx));
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
                // we need to use the "raw" url to get correct data 
                var reqFeat = (IHttpRequestFeature)ctx.Features[typeof(IHttpRequestFeature)];
                var rawPath = reqFeat.RawTarget;

                var nextSegment = GetNextSegment(ctx.Request.PathBase, rawPath);
                
                path = ctx.Request.Path.Value;
                if (!path.StartsWith(nextSegment))
                {
                    throw new Exception("Path does not start with extracted next segement");
                }

                ctx.Request.Path = path.Substring(nextSegment.Length);
                ctx.Request.PathBase = ctx.Request.PathBase.Add(nextSegment);
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
        private static string GetNextSegment(string pathBase, string rawPath)
        {
            var rawPathDecoded = HttpUtility.UrlDecode(rawPath);

            //
            // for some reason the [] are not decoded?? - perhaps more will show up...
            //
            pathBase = DecodeHex(pathBase);

            if (!rawPathDecoded.StartsWith(pathBase))
            {
                throw new Exception("raw path does not start with path base!");
            }

            // split raw path into segments and rremove decoded version from path base
            var segments = rawPath.Split('/').AsEnumerable().GetEnumerator();
            if(!segments.MoveNext()) throw new Exception("Cannot move to next segment!");
            if(segments.Current != "") throw new Exception("Paths does not start with slash!");
            while (pathBase.Length > 0)
            {
                if (!segments.MoveNext()) throw new Exception("Cannot move to next segment!");
                var urlDecdedSegment = "/" + DecodeHex(segments.Current);
                if (!pathBase.StartsWith(urlDecdedSegment))
                {
                    throw new Exception("Something is rotten in the state of denmark!");
                }
                pathBase = pathBase.Substring(urlDecdedSegment.Length);
            }

            if(!segments.MoveNext())
            {
                return null;
            }

            // raw url might contain query parameters
            var currentSegment = segments.Current;
            var queryStart = currentSegment.IndexOf('?');
            if (queryStart > -1)
            {
                currentSegment = currentSegment.Substring(0, queryStart);
            }

            return "/" + HttpUtility.UrlDecode(currentSegment);
        }

        private static string DecodeHex(string str)
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
                                sb.Length = sb.Length - 3;
                                sb.Append((char)nbr);
                            }
                            escapeSequence = 0;
                        }
                        break;
                }
            }
            return sb.ToString();
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
                AddAllowedCorsHeaders(ctx);
                await content.Content.CopyToAsync(ctx.Response.Body);
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

        private static async Task HandleInvocation(IEnumerable<string> allowedCorsOrigins, IHttpTransport httpTransport, IMethodBinding methodBinding, HttpContext context)
        {
            try
            {
                //
                // check if the request is intended for this path.
                //
                if (context.Request.Path != "")
                {
                    context.Response.StatusCode = 404;
                    return;
                }

                //
                // handle the access-control(CORS) request for this invocation
                //
                if(!CheckCorsIsValid(allowedCorsOrigins, methodBinding, context))
                {
                    return;
                }

                //
                // if the supplied method does not match the handler - return.
                // This is the case with "OPTIONS" in cors preflight
                //
                if(!string.Equals(methodBinding.Method, context.Request.Method, StringComparison.InvariantCultureIgnoreCase))
                {
                    context.Response.StatusCode = 204; // No content
                    AddAllowedCorsHeaders(context);
                    return;
                }

                context.RequestServices.LogTrace<IApplicationBuilder>($"Letting {methodBinding.OperationId}:{methodBinding.MethodInfo} handle invocation to {context.Request.Method}:{context.Request.PathBase}{context.Request.Path}");


                // extract information from http context.
                var request = new SolidRpc.OpenApi.Binder.Http.SolidHttpRequest();
                await request.CopyFromAsync(context.Request);

                var httpHandler = context.RequestServices.GetRequiredService<HttpHandler>();
                var methodInvoker = context.RequestServices.GetRequiredService<IMethodInvoker>();
                var response = await methodInvoker.InvokeAsync(context.RequestServices, httpHandler, request, new[] { methodBinding }, context.RequestAborted);

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
