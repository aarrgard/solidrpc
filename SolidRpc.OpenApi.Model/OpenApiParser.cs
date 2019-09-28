﻿using Newtonsoft.Json;
using SolidRpc.Abstractions;
using SolidRpc.Abstractions.OpenApi.Model;
using SolidRpc.OpenApi.Model;
using SolidRpc.OpenApi.Model.CSharp.Impl;
using SolidRpc.OpenApi.Model.Generator;
using SolidRpc.OpenApi.Model.Generator.V2;
using SolidRpc.OpenApi.Model.V2;
using SolidRpc.OpenApi.Model.V3;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

[assembly: SolidRpcAbstractionProvider(typeof(IOpenApiParser), typeof(OpenApiParser))]
namespace SolidRpc.OpenApi.Model
{
    /// <summary>
    /// A parser that parses open api specifications.
    /// </summary>
    public class OpenApiParser : IOpenApiParser
    {
        private static OpenApiParserV2 v2Parser = new OpenApiParserV2();
        private static OpenApiParserV3 v3Parser = new OpenApiParserV3();

        IOpenApiSpec IOpenApiParser.CreateSpecification(params Type[] types)
        {
            return CreateSpecification(types.SelectMany(o => o.GetMethods()));
        }

        IOpenApiSpec IOpenApiParser.CreateSpecification(params MethodInfo[] methods)
        {
            return CreateSpecification(methods);
        }

        private IOpenApiSpec CreateSpecification(IEnumerable<MethodInfo> methods)
        {
            var cSharpRepository = new CSharpRepository();
            methods.ToList().ForEach(o => CSharpReflectionParser.AddMethod(cSharpRepository, o));
            return new OpenApiSpecGeneratorV2(new SettingsSpecGen()
            {
                BasePath = methods.Select(o => "/" + o.DeclaringType.Assembly.GetName().Name.Replace('.', '/')).FirstOrDefault() ?? null,
                Version = methods.Select(o => o.DeclaringType.Assembly.GetName().Version.ToString()).FirstOrDefault() ?? "0.0.0.0",
                Title = methods.Select(o => o.DeclaringType.Assembly.GetName().Name).FirstOrDefault() ?? "OpenApi",
                Description = $"This OpenApi specification was generated from compiled code on {Environment.MachineName} {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}"
            }).CreateSwaggerSpec(OpenApiSpecResolverDummy.Instance, cSharpRepository);
        }

        IOpenApiSpec IOpenApiParser.ParseSpec(IOpenApiSpecResolver specResolver, string address, string json)
        {
            var res = (IOpenApiSpec)v2Parser.ParseSwaggerDoc(json);
            if (res == null)
            {
                res = v3Parser.ParseSwaggerDoc(json);
            }
            res.SetOpenApiSpecResolver(specResolver, address);
            return res;
        }
    }

    /// <summary>
    /// Base class that we can use to parse and write swagger specs.
    /// </summary>
    public abstract class OpenApiParser<T> : OpenApiParser where T : IOpenApiSpec
    {
        private static readonly JsonSerializerSettings s_settingsCompact = new JsonSerializerSettings()
        {
            Formatting = Formatting.None,
            ContractResolver = NewtonsoftContractResolver.Instance
        };
        private static readonly JsonSerializerSettings s_settingsFormatted = new JsonSerializerSettings()
        {
            Formatting = Formatting.Indented,
            ContractResolver = NewtonsoftContractResolver.Instance
        };

        /// <summary>
        /// Writes the supplied specification to string
        /// </summary>
        /// <param name="swaggerSpec"></param>
        /// <param name="formatted"></param>
        /// <returns></returns>
        public static string WriteSwaggerDoc(T swaggerSpec, bool formatted = false)
        {
            var sw = new StringWriter();
            WriteSwaggerDoc(sw, swaggerSpec, formatted);
            return sw.ToString();
        }

        /// <summary>
        /// Writes the supplied specification to the writer
        /// </summary>
        /// <param name="sw"></param>
        /// <param name="swaggerSpec"></param>
        /// <param name="formatted"></param>
        private static void WriteSwaggerDoc(TextWriter sw, T swaggerSpec, bool formatted)
        {
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                var serializer = JsonSerializer.Create(formatted ? s_settingsFormatted : s_settingsCompact);
                serializer.Serialize(writer, swaggerSpec);
            }
        }

        /// <summary>
        /// Parses the supplied stream into a swagger doc.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public T ParseSwaggerDoc(string s)
        {
            using (StringReader sr = new StringReader(s))
            {
                return ParseSwaggerDoc(sr);
            }
        }

        /// <summary>
        /// Parses the supplied stream into a swagger doc.
        /// </summary>
        /// <param name="sr"></param>
        /// <returns></returns>
        private T ParseSwaggerDoc(TextReader sr)
        {
            using (JsonReader reader = new JsonTextReader(sr))
            {
                var serializer = JsonSerializer.Create(s_settingsCompact);
                var res = serializer.Deserialize<T>(reader);
                if (!CheckVersion(res)) return default(T);
                return res;
            }
        }

        /// <summary>
        /// Checks the version
        /// </summary>
        /// <param name="res"></param>
        /// <returns></returns>
        protected abstract bool CheckVersion(T res);
    }
}
