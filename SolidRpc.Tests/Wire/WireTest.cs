using NUnit.Framework;
using SolidRpc.Wire;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Reflection;
using System.Linq;

namespace SolidRpc.Tests.Wire
{
    /// <summary>
    /// Tests the type store
    /// </summary>
    public class WireTest : TestBase
    {
        /// <summary>
        /// 
        /// </summary>
        public interface ITestInterface
        {
            /// <summary>
            /// 
            /// </summary>
            void DoX();

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            Task DoY();

            /// <summary>
            /// 
            /// </summary>
            /// <param name="cancellationToken"></param>
            /// <returns></returns>
            Task<int> DoZ(CancellationToken cancellationToken = default(CancellationToken));

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
        }

        private void TestMapBackAndForth<T>(string typeName)
        {
            Assert.AreEqual(typeName, SolidRpcTypeStore.GetTypeName(typeof(T)));
            Assert.AreEqual(typeof(T), SolidRpcTypeStore.GetType(typeName));

        }



        /// <summary>
        /// Tests the type store
        /// </summary>
        [Test]
        public void TestMethodInfoTestInterface()
        {
            foreach(var mi in typeof(ITestInterface).GetMethods())
            {
                TestMapBackAndForth(mi);
            }
        }
        private void TestMapBackAndForth(MethodInfo mi)
        {
            var solidMI = mi.GetSolidRpcMethodInfo();
            var mi2 = solidMI.GetMethodInfo();
            Assert.AreSame(mi, mi2);
        }

        /// <summary>
        /// Tests the type store
        /// </summary>
        [Test]
        public void TestMethodInfoDoX()
        {
            var mi = typeof(ITestInterface).GetMethod(nameof(ITestInterface.DoX)).GetSolidRpcMethodInfo();
            Assert.AreEqual(typeof(ITestInterface).Assembly.GetName().Name, mi.ApiName);
            Assert.AreEqual(typeof(ITestInterface).Assembly.GetName().Version.ToString(), mi.ApiVersion);
            Assert.AreEqual(typeof(ITestInterface).FullName, mi.InterfaceName);
            Assert.AreEqual(nameof(ITestInterface.DoX), mi.MethodName);
            Assert.AreEqual("System.Void", mi.ReturnType);
            Assert.AreEqual(0, mi.Arguments.Count());
        }

        /// <summary>
        /// Tests the type store
        /// </summary>
        [Test]
        public void TestMethodInfoDoY()
        {
            var mi = typeof(ITestInterface).GetMethod(nameof(ITestInterface.DoY)).GetSolidRpcMethodInfo();
            Assert.AreEqual(typeof(ITestInterface).Assembly.GetName().Name, mi.ApiName);
            Assert.AreEqual(typeof(ITestInterface).Assembly.GetName().Version.ToString(), mi.ApiVersion);
            Assert.AreEqual(typeof(ITestInterface).FullName, mi.InterfaceName);
            Assert.AreEqual(nameof(ITestInterface.DoY), mi.MethodName);
            Assert.AreEqual("System.Threading.Tasks.Task", mi.ReturnType);
            Assert.AreEqual(0, mi.Arguments.Count());
        }

        /// <summary>
        /// Tests the type store
        /// </summary>
        [Test]
        public void TestMethodInfoDoZ()
        {
            var mi = typeof(ITestInterface).GetMethod(nameof(ITestInterface.DoZ)).GetSolidRpcMethodInfo();
            Assert.AreEqual(typeof(ITestInterface).Assembly.GetName().Name, mi.ApiName);
            Assert.AreEqual(typeof(ITestInterface).Assembly.GetName().Version.ToString(), mi.ApiVersion);
            Assert.AreEqual(typeof(ITestInterface).FullName, mi.InterfaceName);
            Assert.AreEqual(nameof(ITestInterface.DoZ), mi.MethodName);
            Assert.AreEqual("System.Threading.Tasks.Task<System.Int32>", mi.ReturnType);
            Assert.AreEqual(1, mi.Arguments.Count());
            Assert.AreEqual("cancellationToken", mi.Arguments.First().Name);
            Assert.AreEqual("System.Threading.CancellationToken", mi.Arguments.First().ValueType);
        }

    }
}
