using NUnit.Framework;
using SolidRpc.Wire;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Reflection;
using System.Linq;
using System;
using Microsoft.Extensions.DependencyInjection;
using SolidRpc.Abstractions.Serialization;
using System.IO;
using SolidRpc.Abstractions.Types;
using Microsoft.Extensions.Primitives;

namespace SolidRpc.Tests.Serialization
{
    /// <summary>
    /// Tests the type store
    /// </summary>
    public class SerializationTest : TestBase
    {
        private class ComplexType
        {
            public string MyData { get; set; }
            public override bool Equals(object obj)
            {
                if(obj is ComplexType other)
                {
                    if (!Equals(other.MyData, MyData)) return false;
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
        /// Tests the type store
        /// </summary>
        [Test]
        public void TestComplexType()
        {
            var sp = GetServiceProvider();
            var serFact = sp.GetRequiredService<ISerializerFactory>();

            TestSerializeDeserialize(serFact, new ComplexType() { MyData = "test" }, str => Assert.AreEqual("{\"MyData\":\"test\"}", str));
        }
    }
}
