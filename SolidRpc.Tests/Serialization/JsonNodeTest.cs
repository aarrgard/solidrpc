using NUnit.Framework;
using System;
using Microsoft.Extensions.DependencyInjection;
using SolidRpc.OpenApi.Model.Serialization;
using System.Text;

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
        public void TestSerializeJsonNode1()
        {
            var serFact = GetServiceProvider().GetRequiredService<SerializerFactory>();
            var json = GetManifestResourceAsString(nameof(TestSerializeJsonNode1) + ".json");
            serFact.SerializeToString(out string s, (JsonNode)json);
            Assert.AreEqual(json, s);

            serFact.DeserializeFromString(s, out JsonNode jsonNode);
            json = CleanJson(json);
            Assert.AreEqual(json, (string)jsonNode);
        }
        /// <summary>
        /// 
        /// </summary>
        [Test]
        public void TestSerializeJsonNode2()
        {
            var serFact = GetServiceProvider().GetRequiredService<SerializerFactory>();
            var json = GetManifestResourceAsString(nameof(TestSerializeJsonNode2) + ".json");
            serFact.SerializeToString(out string s, (JsonNode)json);
            Assert.AreEqual(json, s);

            serFact.DeserializeFromString(s, out JsonNode jsonNode);
            Assert.AreEqual(CleanJson(json), (string)jsonNode);
        }

        private string CleanJson(string json)
        {
            var sb = new StringBuilder();
            var inString = false;
            var escaped = false;
            foreach (var c in json)
            { 
                var resetEscaped = escaped;
                switch (c)
                {
                    case '\\':
                        if(!inString)
                        {
                            escaped = true;
                        }
                        sb.Append(c);
                        break;
                    case '\r':
                    case '\n':
                    case ' ':
                        if (inString)
                        {
                            sb.Append(c);
                        }
                        break;
                    case '"':

                        if(!escaped)
                        {
                            inString = !inString;
                        }
                        sb.Append(c);
                        break;
                    default:
                        sb.Append(c);
                        break;
                }
                if(resetEscaped)
                {
                    resetEscaped = false;
                    escaped = false;
                }
            }
            return sb.ToString();
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
