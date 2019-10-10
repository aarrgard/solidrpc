using NUnit.Framework;
using SolidRpc.OpenApi.Model.Json;
using SolidRpc.OpenApi.Model.Json.Impl;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SolidRpc.Tests.Json
{
    /// <summary>
    /// Tests parsing some json structures
    /// </summary>
    public class JsonStrutureTest
    {
        [Test]
        public void TestParseString()
        {
            Assert.AreEqual("test", new JsonParser().Parse<IJsonString>("\"test\"").String);
        }

        [Test]
        public void TestParseBoolean()
        {
            Assert.AreEqual(true, new JsonParser().Parse<IJsonBoolean>("true").Boolean);
            Assert.AreEqual(false, new JsonParser().Parse<IJsonBoolean>("false").Boolean);
        }

        [Test]
        public void TestParseInteger()
        {
            Assert.AreEqual(13L, new JsonParser().Parse<IJsonInteger>("13").Integer);
        }

        [Test]
        public void TestParseFloat()
        {
            Assert.AreEqual(13.3d, new JsonParser().Parse<IJsonFloat>("13.3").Float);
        }

        [Test]
        public void TestParseNull()
        {
            Assert.IsNull(new JsonParser().Parse<IJsonStruct>("null"));
        }

        [Test]
        public void TestParseArray()
        {
            var arr = new JsonParser().Parse<IJsonArray>("[\"test\", 1, 1.1, true, false, null]");
            Assert.AreEqual("test", ((IJsonString)arr[0]).String);
            Assert.AreEqual(1L, ((IJsonInteger)arr[1]).Integer);
            Assert.AreEqual(1.1d, ((IJsonFloat)arr[2]).Float);
            Assert.AreEqual(true, ((IJsonBoolean)arr[3]).Boolean);
            Assert.AreEqual(false, ((IJsonBoolean)arr[4]).Boolean);
            Assert.AreEqual(null, arr[5]);
            Assert.AreSame(arr, ((IJsonString)arr[0]).Parent);
        }

        [Test]
        public void TestParseObject()
        {
            var obj = new JsonParser().Parse<IJsonObject>("{Integer:1,Float:1.1}");
            Assert.AreEqual(1, ((IJsonInteger)obj["Integer"]).Integer);
            Assert.AreEqual(1.1d, ((IJsonFloat)obj["Float"]).Float);
        }

        [Test]
        public void TestProxyObject()
        {
            for (int i = 0; i < 1000; i++)
            {
                var obj = new JsonParser().Parse<IJsonObject>("{Integer:1,Float:1.1}");
                var proxy = obj.AsObject<IJsonObject>();
                Assert.IsNotNull(proxy["Integer"]);
            }
        }
    }
}
