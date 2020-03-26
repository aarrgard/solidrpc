using Microsoft.Extensions.DependencyInjection;
using SolidRpc.Abstractions;
using SolidRpc.Abstractions.OpenApi.Model;
using SolidRpc.Abstractions.Serialization;
using SolidRpc.OpenApi.Model;
using SolidRpc.OpenApi.Model.CSharp.Impl;
using SolidRpc.OpenApi.Model.Generator;
using SolidRpc.OpenApi.Model.Generator.V2;
using SolidRpc.OpenApi.Model.V2;
using SolidRpc.OpenApi.Model.V3;
using System;
using System.Collections.Generic;
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
        /// <summary>
        /// Constructs a new instance
        /// </summary>
        public OpenApiParser(ISerializerFactory serializerFactory)
        {
            SerializerFactory = serializerFactory;
            V2Parser = new OpenApiParserV2(this);
            V3Parser = new OpenApiParserV3(this);
        }

        protected OpenApiParser(OpenApiParser openApiParser)
        {
            SerializerFactory = openApiParser.SerializerFactory;
        }

        public ISerializerFactory SerializerFactory { get; }
        public OpenApiParserV2 V2Parser { get; }
        public OpenApiParserV3 V3Parser { get; }

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
            }).CreateSwaggerSpec(new OpenApiSpecResolverDummy(this), cSharpRepository);
        }

        IOpenApiSpec IOpenApiParser.ParseSpec(IOpenApiSpecResolver specResolver, string address, string json)
        {
            var res = (IOpenApiSpec)V2Parser.ParseSwaggerSpec(json);
            if (res == null)
            {
                res = V3Parser.ParseSwaggerSpec(json);
            }
            res.SetOpenApiSpecResolver(specResolver, address);
            return res;
        }

        /// <summary>
        /// Writes the supplied specification to string
        /// </summary>
        /// <param name="openApiSpec"></param>
        /// <param name="formatted"></param>
        /// <returns></returns>
        public string WriteSwaggerSpec(IOpenApiSpec openApiSpec, bool formatted = true)
        {
            string s;
            SerializerFactory.SerializeToString(out s, openApiSpec.GetType(), openApiSpec, "application/json", null, formatted);
            return s;
        }

        /// <summary>
        /// Uses the serializer to clone the node.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="node"></param>
        /// <returns></returns>
        public T CloneNode<T>(T node)
        {
            string s;
            SerializerFactory.SerializeToString(out s, node);
            T newNode;
            SerializerFactory.DeserializeFromString(s, out newNode);
            return newNode;
        }
    }

    /// <summary>
    /// Base class that we can use to parse and write swagger specs.
    /// </summary>
    public abstract class OpenApiParser<T> : OpenApiParser where T : IOpenApiSpec
    {
        /// <summary>
        /// Constructs a new instance
        /// </summary>
        /// <param name="baseParser"></param>
        public OpenApiParser(OpenApiParser baseParser)
            : base(baseParser) 
        { 
        }

        /// <summary>
        /// Parses the supplied stream into a swagger doc.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public T ParseSwaggerSpec(string s)
        {
            T res;
            SerializerFactory.DeserializeFromString(s, out res, "application/json");
            if (!CheckVersion(res)) return default(T);
            return res;
        }

        /// <summary>
        /// Checks the version
        /// </summary>
        /// <param name="res"></param>
        /// <returns></returns>
        protected abstract bool CheckVersion(T res);
    }
}
