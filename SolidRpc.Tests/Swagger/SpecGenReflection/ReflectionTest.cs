﻿using NUnit.Framework;
using SolidRpc.OpenApi.Model.CSharp.Impl;
using SolidRpc.OpenApi.Model.Generator;
using SolidRpc.OpenApi.Model.Generator.V2;
using System;
using System.Collections.Generic;
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
        /// Tests generating the swagger spec from compiled code
        /// </summary>
        [Test]
        public void TestEmptyRepo()
        {
            var cSharpRepository = new CSharpRepository();
            var swaggerSpec = new OpenApiSpecGeneratorV2(new SettingsSpecGen()).CreateSwaggerSpec(cSharpRepository);
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
            CSharpReflectionParser.AddMethod(cSharpRepository, typeof(Interface1).GetMethod(nameof(Interface1.TestStuff)));
            var swaggerSpec = new OpenApiSpecGeneratorV2(new SettingsSpecGen()).CreateSwaggerSpec(cSharpRepository);
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
            var swaggerSpec = new OpenApiSpecGeneratorV2(new SettingsSpecGen()).CreateSwaggerSpec(cSharpRepository);
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
            CSharpReflectionParser.AddMethod(cSharpRepository, typeof(Interface1).GetMethod(nameof(Interface1.TestArray)));
            var swaggerSpec = new OpenApiSpecGeneratorV2(new SettingsSpecGen()).CreateSwaggerSpec(cSharpRepository);
            var spec = swaggerSpec.WriteAsJsonString(true);
            Assert.AreEqual(GetManifestResourceAsString($"{nameof(TestInterface1TestArray)}.json"), spec);
        }
    }
}