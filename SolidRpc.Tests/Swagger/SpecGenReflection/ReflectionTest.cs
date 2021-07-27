using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;
using NUnit.Framework;
using SolidRpc.Abstractions.OpenApi.Binder;
using SolidRpc.Abstractions.OpenApi.Model;
using SolidRpc.Abstractions.Types;
using SolidRpc.OpenApi.Model.CSharp.Impl;
using SolidRpc.OpenApi.Model.Generator;
using SolidRpc.OpenApi.Model.Generator.V2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;

namespace SolidRpc.Tests.Swagger.SpecGenReflection
{
    /// <summary>
    /// Tests swagger functionality.
    /// </summary>
    public class ReflectionTest : TestBase
    {
        /// <summary>
        /// A test interface
        /// </summary>
        public interface Interface1
        {
            /// <summary>
            /// Tests some stuff
            /// </summary>
            /// <param name="cancellationToken"></param>
            /// <returns></returns>
            Task TestStuff(CancellationToken cancellationToken);

            /// <summary>
            /// Some more testing
            /// </summary>
            /// <returns></returns>
            Task<int> TestStuff2(int i, double d, string s, Uri u, Guid g);

            /// <summary>
            /// Some more testing
            /// </summary>
            /// <returns></returns>
            Task<IEnumerable<TestStruct>> TestArray(TestStruct[] testStruct, Guid[] guids);

            /// <summary>
            /// Optional parameters should be placed in the query.
            /// </summary>
            /// <param name="s"></param>
            /// <returns></returns>
            Task TestOptionalParameter(string s = null);

            /// <summary>
            /// Nullable parameters should be optional
            /// </summary>
            /// <param name="d"></param>
            /// <returns></returns>
            Task TestNullableParameter(DateTime? d);

            /// <summary>
            /// Nullable parameters should be optional
            /// </summary>
            /// <param name="additionalData"></param>
            /// <param name="fileContent"></param>
            /// <returns></returns>
            Task TestFileTypeWithAdditionalData(string additionalData, FileContent fileContent);

            /// <summary>
            /// Tests a set of string values
            /// </summary>
            /// <param name="data"></param>
            /// <returns></returns>
            Task<StringValues> TestStringValues(StringValues data);

            /// <summary>
            /// Tests a dictionary
            /// </summary>
            /// <param name="data"></param>
            /// <returns></returns>
            Task TestDictionary(IDictionary<string, StringValues> data);

            /// <summary>
            /// Tests a http request argument
            /// </summary>
            /// <param name="req"></param>
            /// <returns></returns>
            Task TestHttpRequest(HttpRequest req);

            /// <summary>
            /// Tests a principal argument
            /// </summary>
            /// <param name="principal"></param>
            /// <returns></returns>
            Task TestIPrincipal(IPrincipal principal);

            /// <summary>
            /// Tests a TimeSpan argument
            /// </summary>
            /// <param name="ts"></param>
            /// <returns></returns>
            Task<TimeSpan> TestTimeSpan(TimeSpan ts);
        }

        /// <summary>
        /// A test interface
        /// </summary>
        public interface Interface2
        {
            /// <summary>
            /// Tests some stuff
            /// </summary>
            /// <param name="cancellationToken"></param>
            /// <returns></returns>
            Task TestStuff(CancellationToken cancellationToken);
        }

        /// <summary>
        /// The test struct
        /// </summary>
        public class TestStruct
        {
            /// <summary>
            /// The int property
            /// </summary>
            public int IntProp { get; set; }

            /// <summary>
            /// Recursive property
            /// </summary>
            public IEnumerable<TestStruct> Recurse { get; set; }
        }

        /// <summary>
        /// Represents a request
        /// </summary>
        public class HttpRequest
        {
            /// <summary>
            /// The uri
            /// </summary>
            public Uri Uri { get; set; }
        }

        private IServiceProvider ServiceProvider { 
            get
            {
                var cb = new ConfigurationBuilder();
                var sc = new ServiceCollection();
                sc.AddLogging(ConfigureLogging);
                sc.AddSingleton<IConfiguration>(cb.Build());
                sc.GetSolidConfigurationBuilder().SetGenerator<SolidProxy.GeneratorCastle.SolidProxyCastleGenerator>();
                sc.AddSolidRpcServices();
                return sc.BuildServiceProvider();
            } 
        }

        /// <summary>
        /// Tests the type parser
        /// </summary>
        [Test]
        public void TestTypeParser()
        {
            string type;
            IList<string> genArgs;

            (type, genArgs) = CSharpRepository.ReadType("int");
            Assert.AreEqual("int", type);
            Assert.IsNull(genArgs);

            (type, genArgs) = CSharpRepository.ReadType("System.Nullable<int>");
            Assert.AreEqual("System.Nullable", type);
            Assert.AreEqual(new[] { "int" }, genArgs);

            (type, genArgs) = CSharpRepository.ReadType("System.Task<System.Nullable<int>>");
            Assert.AreEqual("System.Task", type);
            Assert.AreEqual(new[] { "System.Nullable<int>" }, genArgs);

            (type, genArgs) = CSharpRepository.ReadType("System.Task<int,System.Nullable<int>,float>");
            Assert.AreEqual("System.Task", type);
            Assert.AreEqual(new[] { "int", "System.Nullable<int>", "float" }, genArgs);

        }

        /// <summary>
        /// Tests generating the swagger spec from compiled code
        /// </summary>
        [Test]
        public void TestEmptyRepo()
        {
            var cSharpRepository = new CSharpRepository();
            var specResolver = ServiceProvider.GetRequiredService<IOpenApiSpecResolver>();
            var swaggerSpec = new OpenApiSpecGeneratorV2(new SettingsSpecGen()).CreateSwaggerSpec(specResolver, cSharpRepository);
            var spec = swaggerSpec.WriteAsJsonString(true);
            Assert.AreEqual(GetManifestResourceAsString($"{nameof(TestEmptyRepo)}.json"), spec);
        }

        /// <summary>
        /// Tests generating the swagger spec from compiled code
        /// </summary>
        [Test]
        public void TestInterface1TestStuff()
        {
            var cSharpRepository = new CSharpRepository();
            var methodInfo = typeof(Interface1).GetMethod(nameof(Interface1.TestStuff));
            CSharpReflectionParser.AddMethod(cSharpRepository, methodInfo);
            var specResolver = ServiceProvider.GetRequiredService<IOpenApiSpecResolver>();
            var swaggerSpec = new OpenApiSpecGeneratorV2(new SettingsSpecGen()).CreateSwaggerSpec(specResolver, cSharpRepository);
            var spec = swaggerSpec.WriteAsJsonString(true);
            Assert.AreEqual(GetManifestResourceAsString($"{nameof(TestInterface1TestStuff)}.json"), spec);
        }

        /// <summary>
        /// Tests generating the swagger spec from compiled code
        /// </summary>
        [Test]
        public void TestInterface1AllMethods()
        {
            var cSharpRepository = new CSharpRepository();
            CSharpReflectionParser.AddType(cSharpRepository, typeof(Interface1));
            var specResolver = ServiceProvider.GetRequiredService<IOpenApiSpecResolver>();
            var swaggerSpec = new OpenApiSpecGeneratorV2(new SettingsSpecGen()).CreateSwaggerSpec(specResolver, cSharpRepository);
            var spec = swaggerSpec.WriteAsJsonString(true);
            Assert.AreEqual(GetManifestResourceAsString($"{nameof(TestInterface1AllMethods)}.json"), spec);
        }

        /// <summary>
        /// Tests generating the swagger spec from compiled code
        /// </summary>
        [Test]
        public void TestInterface1TestArray()
        {
            var cSharpRepository = new CSharpRepository();
            var methodInfo = typeof(Interface1).GetMethod(nameof(Interface1.TestArray));
            CSharpReflectionParser.AddMethod(cSharpRepository, methodInfo);
            var specResolver = ServiceProvider.GetRequiredService<IOpenApiSpecResolver>();
            var swaggerSpec = new OpenApiSpecGeneratorV2(new SettingsSpecGen()).CreateSwaggerSpec(specResolver, cSharpRepository);
            var spec = swaggerSpec.WriteAsJsonString(true);
            Assert.AreEqual(GetManifestResourceAsString($"{nameof(TestInterface1TestArray)}.json"), spec);

            var binding = ServiceProvider.GetRequiredService<IMethodBinderStore>().CreateMethodBindings(spec, methodInfo).First();
            Assert.AreEqual(2, binding.Arguments.Count());
        }

        /// <summary>
        /// Tests generating the swagger spec from compiled code
        /// </summary>
        [Test]
        public void TestInterface1OptionalParameter()
        {
            var cSharpRepository = new CSharpRepository();
            var methodInfo = typeof(Interface1).GetMethod(nameof(Interface1.TestOptionalParameter));
            CSharpReflectionParser.AddMethod(cSharpRepository, methodInfo);
            var specResolver = ServiceProvider.GetRequiredService<IOpenApiSpecResolver>();
            var swaggerSpec = new OpenApiSpecGeneratorV2(new SettingsSpecGen()).CreateSwaggerSpec(specResolver, cSharpRepository);
            var spec = swaggerSpec.WriteAsJsonString(true);
            Assert.AreEqual(GetManifestResourceAsString($"{nameof(TestInterface1OptionalParameter)}.json"), spec);

            var binding = ServiceProvider.GetRequiredService<IMethodBinderStore>().CreateMethodBindings(spec, methodInfo).First();
            Assert.AreEqual(1, binding.Arguments.Count());
        }

        /// <summary>
        /// Tests generating the swagger spec from compiled code
        /// </summary>
        [Test]
        public void TestInterface1NullableParameter()
        {
            var cSharpRepository = new CSharpRepository();
            var methodInfo = typeof(Interface1).GetMethod(nameof(Interface1.TestNullableParameter));
            CSharpReflectionParser.AddMethod(cSharpRepository, methodInfo);
            var specResolver = ServiceProvider.GetRequiredService<IOpenApiSpecResolver>();
            var swaggerSpec = new OpenApiSpecGeneratorV2(new SettingsSpecGen()).CreateSwaggerSpec(specResolver, cSharpRepository);
            var spec = swaggerSpec.WriteAsJsonString(true);
            Assert.AreEqual(GetManifestResourceAsString($"{nameof(TestInterface1NullableParameter)}.json"), spec);

            var binding = ServiceProvider.GetRequiredService<IMethodBinderStore>().CreateMethodBindings(spec, methodInfo).First();
            Assert.AreEqual(1, binding.Arguments.Count());
        }

        /// <summary>
        /// Tests generating the swagger spec from compiled code
        /// </summary>
        [Test]
        public void TestInterface1FileTypeWithAdditionalData()
        {
            var cSharpRepository = new CSharpRepository();
            var methodInfo = typeof(Interface1).GetMethod(nameof(Interface1.TestFileTypeWithAdditionalData));
            CSharpReflectionParser.AddMethod(cSharpRepository, methodInfo);
            var specResolver = ServiceProvider.GetRequiredService<IOpenApiSpecResolver>();
            var swaggerSpec = new OpenApiSpecGeneratorV2(new SettingsSpecGen()).CreateSwaggerSpec(specResolver, cSharpRepository);
            var spec = swaggerSpec.WriteAsJsonString(true);
            Assert.AreEqual(GetManifestResourceAsString($"{nameof(TestInterface1FileTypeWithAdditionalData)}.json"), spec);

            var binding = ServiceProvider.GetRequiredService<IMethodBinderStore>().CreateMethodBindings(spec, methodInfo).First();
            Assert.AreEqual(2, binding.Arguments.Count());
        }

        /// <summary>
        /// Tests generating the swagger spec from compiled code
        /// </summary>
        [Test]
        public void TestInterface1StringValues()
        {
            var cSharpRepository = new CSharpRepository();
            var methodInfo = typeof(Interface1).GetMethod(nameof(Interface1.TestStringValues));
            CSharpReflectionParser.AddMethod(cSharpRepository, methodInfo);
            var specResolver = ServiceProvider.GetRequiredService<IOpenApiSpecResolver>();
            var swaggerSpec = new OpenApiSpecGeneratorV2(new SettingsSpecGen()).CreateSwaggerSpec(specResolver, cSharpRepository);
            var spec = swaggerSpec.WriteAsJsonString(true);
            Assert.AreEqual(GetManifestResourceAsString($"{nameof(TestInterface1StringValues)}.json"), spec);

            var binding = ServiceProvider.GetRequiredService<IMethodBinderStore>().CreateMethodBindings(spec, methodInfo).First();
            Assert.AreEqual(1, binding.Arguments.Count());
        }

        /// <summary>
        /// Tests generating the swagger spec from compiled code
        /// </summary>
        [Test]
        public void TestInterface1Dictionary()
        {
            var cSharpRepository = new CSharpRepository();
            var methodInfo = typeof(Interface1).GetMethod(nameof(Interface1.TestDictionary));
            CSharpReflectionParser.AddMethod(cSharpRepository, methodInfo);
            var specResolver = ServiceProvider.GetRequiredService<IOpenApiSpecResolver>();
            var swaggerSpec = new OpenApiSpecGeneratorV2(new SettingsSpecGen()).CreateSwaggerSpec(specResolver, cSharpRepository);
            var spec = swaggerSpec.WriteAsJsonString(true);
            Assert.AreEqual(GetManifestResourceAsString($"{nameof(TestInterface1Dictionary)}.json"), spec);

            var binding = ServiceProvider.GetRequiredService<IMethodBinderStore>().CreateMethodBindings(spec, methodInfo).First();
            Assert.AreEqual(1, binding.Arguments.Count());
        }

        /// <summary>
        /// Tests generating the swagger spec from compiled code
        /// </summary>
        [Test]
        public void TestInterface1HttpRequest()
        {
            var cSharpRepository = new CSharpRepository();
            var methodInfo = typeof(Interface1).GetMethod(nameof(Interface1.TestHttpRequest));
            CSharpReflectionParser.AddMethod(cSharpRepository, methodInfo);
            var specResolver = ServiceProvider.GetRequiredService<IOpenApiSpecResolver>();
            var swaggerSpec = new OpenApiSpecGeneratorV2(new SettingsSpecGen()).CreateSwaggerSpec(specResolver, cSharpRepository);
            var spec = swaggerSpec.WriteAsJsonString(true);
            Assert.AreEqual(GetManifestResourceAsString($"{nameof(TestInterface1HttpRequest)}.json"), spec);

            var binding = ServiceProvider.GetRequiredService<IMethodBinderStore>().CreateMethodBindings(spec, methodInfo).First();
            Assert.AreEqual(1, binding.Arguments.Count());
        }

        /// <summary>
        /// Tests generating the swagger spec from compiled code
        /// </summary>
        [Test]
        public void TestInterface1Principal()
        {
            var cSharpRepository = new CSharpRepository();
            var methodInfo = typeof(Interface1).GetMethod(nameof(Interface1.TestIPrincipal));
            CSharpReflectionParser.AddMethod(cSharpRepository, methodInfo);
            var specResolver = ServiceProvider.GetRequiredService<IOpenApiSpecResolver>();
            var swaggerSpec = new OpenApiSpecGeneratorV2(new SettingsSpecGen()).CreateSwaggerSpec(specResolver, cSharpRepository);
            var spec = swaggerSpec.WriteAsJsonString(true);
            Assert.AreEqual(GetManifestResourceAsString($"{nameof(TestInterface1Principal)}.json"), spec);

            var binding = ServiceProvider.GetRequiredService<IMethodBinderStore>().CreateMethodBindings(spec, methodInfo).First();
            Assert.AreEqual(1, binding.Arguments.Count());
        }

        /// <summary>
        /// Tests generating the swagger spec from compiled code
        /// </summary>
        [Test]
        public void TestInterface1TimeSpan()
        {
            var cSharpRepository = new CSharpRepository();
            var methodInfo = typeof(Interface1).GetMethod(nameof(Interface1.TestTimeSpan));
            CSharpReflectionParser.AddMethod(cSharpRepository, methodInfo);
            var specResolver = ServiceProvider.GetRequiredService<IOpenApiSpecResolver>();
            var swaggerSpec = new OpenApiSpecGeneratorV2(new SettingsSpecGen()).CreateSwaggerSpec(specResolver, cSharpRepository);
            var spec = swaggerSpec.WriteAsJsonString(true);
            Assert.AreEqual(GetManifestResourceAsString($"{nameof(TestInterface1TimeSpan)}.json"), spec);

            var binding = ServiceProvider.GetRequiredService<IMethodBinderStore>().CreateMethodBindings(spec, methodInfo).First();
            Assert.AreEqual(1, binding.Arguments.Count());
        }

        /// <summary>
        /// Tests generating the swagger spec from compiled code
        /// </summary>
        [Test]
        public void TestSameMethodNameDifferentInterfaces()
        {
            var cSharpRepository = new CSharpRepository();
            var methodInfo1 = typeof(Interface1).GetMethod(nameof(Interface1.TestStuff));
            CSharpReflectionParser.AddMethod(cSharpRepository, methodInfo1);
            var methodInfo2 = typeof(Interface2).GetMethod(nameof(Interface2.TestStuff));
            CSharpReflectionParser.AddMethod(cSharpRepository, methodInfo2);
            var specResolver = ServiceProvider.GetRequiredService<IOpenApiSpecResolver>();
            var swaggerSpec = new OpenApiSpecGeneratorV2(new SettingsSpecGen()).CreateSwaggerSpec(specResolver, cSharpRepository);
            var spec = swaggerSpec.WriteAsJsonString(true);
            Assert.AreEqual(GetManifestResourceAsString($"{nameof(TestSameMethodNameDifferentInterfaces)}.json"), spec);

            var mbs = ServiceProvider.GetRequiredService<IMethodBinderStore>();
            var binding1 = mbs.CreateMethodBindings(spec, methodInfo1).Single();
            Assert.AreEqual("SolidRpc#Tests#Swagger#SpecGenReflection#ReflectionTest#Interface1#TestStuff", binding1.OperationId);
            Assert.AreEqual(methodInfo1, binding1.MethodInfo);

            var binding2 = mbs.CreateMethodBindings(spec, methodInfo2).Single();

            Assert.AreEqual("SolidRpc#Tests#Swagger#SpecGenReflection#ReflectionTest#Interface2#TestStuff", binding2.OperationId);
            Assert.AreEqual(methodInfo2, binding2.MethodInfo);

            var methodBinder = mbs.MethodBinders.Single(o => o.Assembly == typeof(Interface1).Assembly);
            Assert.AreEqual(2, methodBinder.MethodBindings.Count());

        }
    }
}
