﻿using NUnit.Framework;
using System;
using Microsoft.Extensions.DependencyInjection;
using SolidRpc.Abstractions.Serialization;
using System.IO;
using SolidRpc.Abstractions.Types;
using Microsoft.Extensions.Primitives;
using System.Runtime.Serialization;
using System.Linq;
using SolidRpc.OpenApi.Binder;
using System.Text;
using System.Collections.Generic;
using RA.Mspecs.Types.Event;
using static System.Runtime.InteropServices.JavaScript.JSType;
using SolidRpc.OpenApi.Model.Serialization;

namespace SolidRpc.Tests.Serialization
{
    /// <summary>
    /// Tests the type store
    /// </summary>
    public class JsonNodeTest : TestBase
    {
        /// <summary>
        /// Represents a json node
        /// </summary>
        public class JsonNode
        {
            private readonly string _json;

            private JsonNode(string json) { _json = json; }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="json"></param>
            public static implicit operator JsonNode(string json)
            {
                return new JsonNode(json);
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="jn"></param>
            public static implicit operator string(JsonNode jn)
            {
                return jn._json;
            }
        }
        public class StructWithJsonNode
        {
            public string Prologue { get; set; }    
            public JsonNode JsonNode { get; set; }
            public string Epilogue { get; set; }

        }


        private IServiceProvider GetServiceProvider()
        {
            var sc = new ServiceCollection();
            sc.AddSolidRpcSingletonServices();
            return sc.BuildServiceProvider();
        }


        /// <summary>
        /// 
        /// </summary>
        [Test]
        public void TestSerializeJsonNode()
        {
            var serFact = GetServiceProvider().GetRequiredService<SerializerFactory>();
            var json = "{\"test\":\"test\"}";
            serFact.SerializeToString(out string s, (JsonNode)json);
            Assert.AreEqual(json, s);

            serFact.DeserializeFromString(s, out JsonNode jsonNode);
            Assert.AreEqual(json, (string)jsonNode);
        }

        /// <summary>
        /// 
        /// </summary>
        [Test]
        public void TestSerializeStructWithJsonNode()
        {
            var serFact = GetServiceProvider().GetRequiredService<SerializerFactory>();
            var json = "{\"test\":\"test\"}";
            var structWithJsonNode = new StructWithJsonNode()
            {
                Prologue = "prologue",
                JsonNode = json,
                Epilogue = "epilogue"
            };
            serFact.SerializeToString(out string s, structWithJsonNode);
            serFact.DeserializeFromString(s, out structWithJsonNode);
            Assert.AreEqual("epilogue", structWithJsonNode.Epilogue);
            Assert.AreEqual(json, (string)structWithJsonNode.JsonNode);
            Assert.AreEqual("prologue", structWithJsonNode.Prologue);

            // test error json
            structWithJsonNode.JsonNode = "{{";
            try
            {
                serFact.SerializeToString(out s, structWithJsonNode);
                Assert.Fail();
            }
            catch (Exception e)
            {
                Assert.AreEqual("Json node is not well formed.", e.InnerException?.Message);
            }

        }

    }
}
