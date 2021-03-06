﻿using NUnit.Framework;
using System;
using Microsoft.Extensions.DependencyInjection;
using SolidRpc.Abstractions.Serialization;
using System.IO;
using SolidRpc.Abstractions.Types;
using Microsoft.Extensions.Primitives;
using System.Runtime.Serialization;

namespace SolidRpc.Tests.Serialization
{
    /// <summary>
    /// Tests the type store
    /// </summary>
    public class SerializationTest : TestBase
    {
        private class ComplexType
        {
            [DataMember(Name= "MyData", EmitDefaultValue = false)]
            public string MyData { get; set; }

            [DataMember(Name = "MyStream", EmitDefaultValue = false)]
            public Stream MyStream { get; set; }

            [DataMember(Name = "DtOffset", EmitDefaultValue = false)]
            public DateTimeOffset? DtOffset { get; set; }

            [DataMember(Name = "NullableInt", EmitDefaultValue = false)]
            public int? NullableInt { get; set; }

            public override bool Equals(object obj)
            {
                if(obj is ComplexType other)
                {
                    if (!Equals(other.MyData, MyData)) return false;
                    if (!Equals(other.MyStream, MyStream)) return false;
                    return true;
                }
                return false;
            }
        }


        private IServiceProvider GetServiceProvider()
        {
            var sc = new ServiceCollection();
            sc.AddSolidRpcSingletonServices();
            return sc.BuildServiceProvider();
        }

        private void TestSerializeDeserialize<T>(ISerializerFactory serFact, T val, Action<string> wireCheck = null)
        {
            string str;
            serFact.SerializeToString(out str, val, "application/json");

            wireCheck?.Invoke(str);

            T copy;
            serFact.DeserializeFromString(str, out copy, "application/json");

            Assert.AreEqual(val, copy);

            if(((object)val) is MemoryStream ms) 
            {
                ms.Position = 0;
            }

            var fileType = new FileContent();
            serFact.SerializeToFileType(fileType, val);

            Assert.AreEqual(typeof(T).FullName, fileType.FileName);
            Assert.AreEqual("application/json", fileType.ContentType);

            serFact.DeserializeFromFileType(fileType, out copy);
            Assert.AreEqual(val, copy);
        }

        /// <summary>
        /// Tests the type store
        /// </summary>
        [Test]
        public void TestSerializeSimpleType()
        {
            var sp = GetServiceProvider();
            var serFact = sp.GetRequiredService<ISerializerFactory>();

            TestSerializeDeserialize(serFact, "test", str => Assert.AreEqual("\"test\"", str));
            TestSerializeDeserialize(serFact, 12, str => Assert.AreEqual("12", str));
        }

        /// <summary>
        /// Tests the type store
        /// </summary>
        [Test]
        public void TestStringValues()
        {
            var sp = GetServiceProvider();
            var serFact = sp.GetRequiredService<ISerializerFactory>();

            TestSerializeDeserialize(serFact, new StringValues(new[] { "1", "2" }), str => Assert.AreEqual("[\"1\",\"2\"]", str));
        }

        /// <summary>
        /// Tests deserializing structure with additional properties
        /// </summary>
        [Test]
        public void TestAdditionalProperties()
        {
            var sp = GetServiceProvider();
            var serFact = sp.GetRequiredService<ISerializerFactory>();

            ComplexType ct;
            serFact.DeserializeFromString("{\"MyData\":\"test\", \"PropertyWithMissingDeclaration\": \"test\"}", out ct);
            Assert.AreEqual("test", ct.MyData);

            serFact.DeserializeFromString("{\"MyData\":\"test\", \"PropertyWithMissingDeclaration\": {\"scoped\": null}}", out ct);
            Assert.AreEqual("test", ct.MyData);

            serFact.DeserializeFromString("{\"PropertyWithMissingDeclaration\": {\"scoped\": null}, \"MyData\":\"test\"}", out ct);
            Assert.AreEqual("test", ct.MyData);

            serFact.DeserializeFromString("{\"PropertyWithMissingDeclaration\": [\"scoped\", null], \"MyData\":\"test\"}", out ct);
            Assert.AreEqual("test", ct.MyData);
        }

        /// <summary>
        /// Tests the type store
        /// </summary>
        [Test]
        public void TestDateTimeOffset()
        {
            var sp = GetServiceProvider();
            var serFact = sp.GetRequiredService<ISerializerFactory>();

            var dtOffsetNow = DateTimeOffset.Now;
            TestSerializeDeserialize(serFact, DateTimeOffset.MinValue, str => Assert.AreEqual("\"0001-01-01T00:00:00+00:00\"", str));
            TestSerializeDeserialize(serFact, new ComplexType() { DtOffset = dtOffsetNow }, str => Assert.AreEqual($"{{\"DtOffset\":\"{RemoveZeroes(dtOffsetNow.ToString($"yyyy-MM-ddTHH:mm:ss.fffffffzzz"))}\"}}", str));
            TestSerializeDeserialize(serFact, new ComplexType() { DtOffset = DateTimeOffset.MinValue }, str => Assert.AreEqual("{\"DtOffset\":\"0001-01-01T00:00:00+00:00\"}", str));

            ComplexType ct;
            serFact.DeserializeFromString("{\"DtOffset\":\"0001-01-01T00:00:00\"}", out ct);
        }

        /// <summary>
        /// Tests the type store
        /// </summary>
        [Test]
        public void TestNullableTypes()
        {
            var sp = GetServiceProvider();
            var serFact = sp.GetRequiredService<ISerializerFactory>();

            int? ni;
            serFact.DeserializeFromString("5", out ni);
            Assert.AreEqual(5, ni.Value);

            decimal? di;
            serFact.DeserializeFromString("5.67", out di);
            Assert.AreEqual(5.67, di.Value);

            ComplexType ct;
            serFact.DeserializeFromString("{\"NullableInt\": null}", out ct);
            Assert.IsNull(ct.NullableInt);
            serFact.DeserializeFromString("{\"NullableInt\": 5}", out ct);
            Assert.AreEqual(5, ct.NullableInt.Value);
        }

        private string RemoveZeroes(string str)
        {
            var newStr = str.Replace("0+", "+");
            if(newStr != str)
            {
                return RemoveZeroes(newStr);
            }
            return newStr;
        }

        /// <summary>
        /// Tests the type store
        /// </summary>
        [Test]
        public void TestComplexType()
        {
            var sp = GetServiceProvider();
            var serFact = sp.GetRequiredService<ISerializerFactory>();

            TestSerializeDeserialize(serFact, new ComplexType() { MyData = "test" }, str => Assert.AreEqual("{\"MyData\":\"test\"}", str));
        }

        /// <summary>
        /// Tests the type store
        /// </summary>
        [Test]
        public void TestStreamType()
        {
            var sp = GetServiceProvider();
            var serFact = sp.GetRequiredService<ISerializerFactory>();

            TestSerializeDeserialize(serFact, new MemoryStream(new byte[] { 1, 2, 3 }), str => Assert.AreEqual("\"AQID\"", str));
        }
    }
}
