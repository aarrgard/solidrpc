using NUnit.Framework;
using System.Collections.Generic;
using SolidRpc.OpenApi.Model.Serialization;

namespace SolidRpc.Tests.Serialization
{
    /// <summary>
    /// Tests the type store
    /// </summary>
    public class TypeStoreTest : TestBase
    {
        /// <summary>
        /// Test class
        /// </summary>
        public class TestClass
        {
        }

        /// <summary>
        /// Tests the type store
        /// </summary>
        [Test]
        public void TestTypeStoreTypes()
        {
            TestMapBackAndForth<string>("System.String");
            TestMapBackAndForth<string[]>("System.String[]");
            TestMapBackAndForth<IEnumerable<string>>("System.Collections.Generic.IEnumerable<System.String>");
            TestMapBackAndForth<TestClass>(typeof(TestClass).FullName);
            TestMapBackAndForth<TestClass[]>(typeof(TestClass).FullName+"[]");
        }

        private void TestMapBackAndForth<T>(string typeName)
        {
            Assert.AreEqual(typeof(T), SolidRpcTypeStore.GetType(typeName));
            Assert.AreEqual(typeName, SolidRpcTypeStore.GetTypeName(typeof(T)));
        }

    }
}
