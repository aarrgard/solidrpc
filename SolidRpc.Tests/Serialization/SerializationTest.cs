using NUnit.Framework;
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

namespace SolidRpc.Tests.Serialization
{
    /// <summary>
    /// Tests the type store
    /// </summary>
    public class SerializationTest : TestBase
    {
        private enum TestEnum
        {
            TestVal1 = 10,
            TestVal2 = 20
        }

        /// <summary>
        /// Test class
        /// </summary>
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

            public override int GetHashCode()
            {
                return 0;
            }
        }

        public class StructBase { 
            public virtual string Type { get; set; }
            [DataMember(Name ="Common", EmitDefaultValue = false)]
            public string Common { get; set; }
        }
        public class Struct1 : StructBase {
            public override string Type { get; set; } = "Type1";
            public int[] Arr { get; set; }
        }
        public class Struct2 : StructBase
        {
            public override string Type { get; set; } = "Type2";
            public int[][] Arr { get; set; }
        }
        public class Struct3 : StructBase
        {
            public override string Type { get; set; } = "Type3";
            public StructBase Arr { get; set; }
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

            // test min value
            ComplexType ct;
            serFact.DeserializeFromString("{\"DtOffset\":\"0001-01-01T00:00:00\"}", out ct);
            Assert.AreEqual(DateTimeOffset.MinValue, ct.DtOffset);

            // test now
            var now = DateTimeOffset.Now;
            serFact.DeserializeFromString($"{{\"DtOffset\":\"{now.ToString("yyyy-MM-ddTHH:mm:ss")}\"}}", out ct);
            Assert.AreEqual(now.Ticks / 10000000, ct.DtOffset.Value.Ticks / 10000000);

            // test specific date
            TestParseDate(serFact, 2021, 01, 02, 19, 03, 31, "Stockholm");
            TestParseDate(serFact, 2021, 04, 16, 19, 03, 31, "Stockholm");

            TestParseDate(serFact, 2021, 01, 02, 19, 03, 31, "London");
            TestParseDate(serFact, 2021, 04, 16, 19, 03, 31, "London");

            //var dt = ((DateTimeOffset)DateTime.Parse("2021-01-02T19:03:31")).ToString();
            //var dt2 = DateTimeOffset.Parse("2021-04-16 19:03:31+02:00");
            //var dt3 = DateTimeOffset.Parse("2021-01-02 19:03:31+01:00");

        }

        private void TestParseDate(ISerializerFactory serFact, int year, int month, int day, int hour, int minute, int second, string capitol)
        {
            var tz = TimeZoneInfo.GetSystemTimeZones().Where(o => o.Id.Contains(capitol) || o.DisplayName.Contains(capitol)).FirstOrDefault();
            serFact.DefaultSerializerSettings = serFact.DefaultSerializerSettings.SetDefaultTimeZone(tz);
            DateTimeOffset dt;
            var dtStr = $"\"{year.ToString("#0000")}-{month.ToString("#00")}-{day.ToString("#00")}T{hour.ToString("#00")}:{minute.ToString("#00")}:{second.ToString("#00")}\"";
            serFact.DeserializeFromString(dtStr, out dt);
            var d = new DateTime(year, month, day, hour, minute, second, DateTimeKind.Unspecified);
            Assert.AreEqual(new DateTimeOffset(d, tz.GetUtcOffset(d)), dt);
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
        /// Tests the complex type
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

        /// <summary>
        /// Tests the decimal type
        /// </summary>
        [Test]
        public void TestDecimal()
        {
            var sp = GetServiceProvider();
            var serFact = sp.GetRequiredService<ISerializerFactory>();

            TestSerializeDeserialize(serFact, 1.43m, str => Assert.AreEqual("1.43", str));
        }

        /// <summary>
        /// Tests the uri type
        /// </summary>
        [Test]
        public void TestUri()
        {
            var sp = GetServiceProvider();
            var serFact = sp.GetRequiredService<ISerializerFactory>();

            TestSerializeDeserialize(serFact, new Uri("ws://test.ws/ws"), str => Assert.AreEqual("\"ws://test.ws/ws\"", str));
        }

        /// <summary>
        /// Tests the enum
        /// </summary>
        [Test]
        public void TestEnumValue()
        {
            var sp = GetServiceProvider();
            var serFact = sp.GetRequiredService<ISerializerFactory>();

            TestSerializeDeserialize(serFact, TestEnum.TestVal2, str => Assert.AreEqual("\"TestVal2\"", str));
        }

        /// <summary>
        /// Tests the uri type
        /// </summary>
        [Test]
        public void TestNoJson()
        {
            try
            {
                var s = new MemoryStream(Encoding.UTF8.GetBytes("[Not json]"));
                var i = JsonHelper.Deserialize<int>(s);
                Assert.Fail();
            }
            catch (Exception e)
            {
                Assert.AreEqual("Cannot read json stream:[Not json]", e.Message);
            }
        }

        /// <summary>
        /// Tests the uri type
        /// </summary>
        [Test]
        public void TestNominator()
        {
            var serFact = GetServiceProvider().GetRequiredService<ISerializerFactory>();

            var s1 = new Struct1() { Arr = new [] { 1, 2 } };
            Assert.AreEqual("Type1", s1.Type);
            serFact.SerializeToString(out string s, s1);
            Assert.AreEqual(@"{""Type"":""Type1"",""Arr"":[1,2]}", s);

            var s2 = new Struct2() { Arr = new [] { new [] { 1, 2 } } };
            Assert.AreEqual("Type2", s2.Type);
            serFact.SerializeToString(out s, s2);
            Assert.AreEqual(@"{""Type"":""Type2"",""Arr"":[[1,2]]}", s);

            serFact.DeserializeFromString(@"{""Common"": ""a"",""Type"":""Type1"",""Arr"":[1,2]}", out StructBase sb);
            Assert.AreEqual("Type1", sb.Type);
            Assert.AreEqual("a", sb.Common);
            Assert.AreEqual(new[] { 1, 2 }, ((Struct1)sb).Arr);
            Assert.AreEqual(typeof(Struct1), sb.GetType());

            serFact.DeserializeFromString(@"{""Common"": ""a"",""Arr"":[1,2],""Type"":""Type1""}", out sb);
            Assert.AreEqual("Type1", sb.Type);
            Assert.AreEqual("a", sb.Common);
            Assert.AreEqual(new[] { 1, 2 }, ((Struct1)sb).Arr);
            Assert.AreEqual(typeof(Struct1), sb.GetType());

            serFact.DeserializeFromString(@"{""Common"": ""a"",""Type"":""Type2"",""Arr"":[[1,2]]}", out sb);
            Assert.AreEqual("Type2", sb.Type);
            Assert.AreEqual("a", sb.Common);
            Assert.AreEqual(new[] { new [] { 1, 2 } }, ((Struct2)sb).Arr);
            Assert.AreEqual(typeof(Struct2), sb.GetType());

            serFact.DeserializeFromString(@"{""Arr"":[[1,2]], ""Common"": ""a"",""Type"":""Type2""}", out sb);
            Assert.AreEqual("Type2", sb.Type);
            Assert.AreEqual("a", sb.Common);
            Assert.AreEqual(typeof(Struct2), sb.GetType());

            serFact.DeserializeFromString(@"{""Common"": ""a"",""Arr"":{""Type"":""Type2"" },""Type"":""Type3""}", out sb);
            Assert.AreEqual("Type3", sb.Type);
            Assert.AreEqual("a", sb.Common);
            Assert.AreEqual("Type2", ((Struct3)sb).Arr.Type);
            Assert.AreEqual(typeof(Struct3), sb.GetType());

            serFact.DeserializeFromString(@"{""Common"": ""a"",""Type"":""Type4"",""Arr"":[[1,2]]}", out sb);
            Assert.AreEqual("Type4", sb.Type);
            Assert.AreEqual("a", sb.Common);
            Assert.AreEqual(typeof(StructBase), sb.GetType());

            serFact.DeserializeFromString(@"{""Type"":""Type4"",""Arr"":[[1,2]],""Common"": ""a""}", out sb);
            Assert.AreEqual("Type4", sb.Type);
            Assert.AreEqual("a", sb.Common);
            Assert.AreEqual(typeof(StructBase), sb.GetType());

        }

        /// <summary>
        /// Tests the file serialization type
        /// </summary>
        [Test]
        public void TestSerializeFileTypeClass()
        {
            var serFact = GetServiceProvider().GetRequiredService<ISerializerFactory>();

            var fc = new FileContent();
            var x = new ComplexType() { MyData = "test" };
            serFact.SerializeToFileType(fc, x);
            Assert.IsNotNull(fc.Content);
            Assert.AreEqual("application/json", fc.ContentType);
            Assert.AreEqual("SolidRpc.Tests.Serialization.SerializationTest+ComplexType", fc.FileName);

            ComplexType x2;
            serFact.DeserializeFromFileType(fc, out x2);
            Assert.AreEqual(x.MyData, x2.MyData);
        }

        /// <summary>
        /// Tests the file serialization type
        /// </summary>
        [Test]
        public void TestSerializeFileTypeArray()
        {
            var serFact = GetServiceProvider().GetRequiredService<ISerializerFactory>();

            var fc = new FileContent();
            IEnumerable<ComplexType> x = new[] { new ComplexType() { MyData = "test" } };
            serFact.SerializeToFileType(fc, x);
            Assert.IsNotNull(fc.Content);
            Assert.AreEqual("application/json", fc.ContentType);
            Assert.AreEqual("SolidRpc.Tests.Serialization.SerializationTest+ComplexType[]", fc.FileName);

            IEnumerable<ComplexType> x2;
            serFact.DeserializeFromFileType(fc, out x2);
            Assert.AreEqual(x.First().MyData, x2.First().MyData);
        }
    }
}
