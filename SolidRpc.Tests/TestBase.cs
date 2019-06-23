using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace SolidRpc.Tests
{
    /// <summary>
    /// 
    /// </summary>
    public class TestBase
    {
        /// <summary>
        /// 
        /// </summary>
        protected Stream GetManifestResource(string resourceName)
        {
            var resName = GetType().Assembly.GetManifestResourceNames().Single(o => o.EndsWith(resourceName));
            return GetType().Assembly.GetManifestResourceStream(resName);
        }
        /// <summary>
        /// 
        /// </summary>
        protected DirectoryInfo GetProjectFolder(string projectName)
        {
            var dir = new DirectoryInfo(".");
            while (dir.Parent != null)
            {
                if (dir.Parent.Name == projectName)
                {
                    return dir.Parent;
                }
                dir = dir.Parent;
            }
            throw new Exception("Cannot find project folder:" + projectName);
        }

        /// <summary>
        /// Compares the structs
        /// </summary>
        /// <param name="o1"></param>
        /// <param name="o2"></param>
        public static void CompareStructs(object o1, object o2)
        {
            Assert.AreEqual(o1?.GetType(), o2?.GetType());
            if(ReferenceEquals(o1, o2))
            {
                return;
            }
            CompareStructs(o1.GetType(), o1, o2);
        }

        /// <summary>
        /// Compares the structs
        /// </summary>
        /// <param name="t"></param>
        /// <param name="o1"></param>
        /// <param name="o2"></param>
        public static void CompareStructs(Type t, object o1, object o2)
        {
            var path = new Stack<string>();
            CompareStructs(path, t, o1, o2);
        }
        private static void CompareStructs(Stack<string> path, Type t, object o1, object o2)
        {
            if (ReferenceEquals(o1, o2))
            {
                return;
            }
            if (o1 == null || o2 == null)
            {
                throw new Exception("One of the objects is null.");
            }
            if (typeof(DateTime).IsAssignableFrom(t))
            {
                Assert.AreEqual(o1, o2);
                return;
            }
            if (typeof(string).IsAssignableFrom(t))
            {
                Assert.AreEqual(o1, o2);
                return;
            }
            if (typeof(CancellationToken).IsAssignableFrom(t))
            {
                return;
            }
            if (typeof(Stream).IsAssignableFrom(t))
            {
                CompareStreams(path, o1, o2);
                return;
            }
            if (t.IsArray)
            {
                CompareArrays(path, o1, o2);
                return;
            }
            if (typeof(IEnumerable).IsAssignableFrom(t))
            {
                var enum1 = (IEnumerable)o1;
                var enum2 = (IEnumerable)o2;
                var lst1 = new List<object>();
                foreach (var e1 in enum1)
                {
                    lst1.Add(e1);
                }
                var lst2 = new List<object>();
                foreach (var e2 in enum2)
                {
                    lst2.Add(e2);
                }
                Assert.AreEqual(lst1.Count, lst2.Count);
                for (int i = 0; i < lst1.Count; i++)
                {
                    try
                    {
                        path.Push($"[{i}]");
                        CompareStructs(path, lst1[i].GetType(), lst1[i], lst2[i]);
                    }
                    finally
                    {
                        path.Pop();
                    }
                }
                return;
            }
            if (t.GetProperties().Length == 0)
            {
                try
                {
                    Assert.AreEqual(o1, o2, $"{string.Join(".", path)}");
                }
                catch
                {
                    throw;
                }
            }
            foreach (var p in t.GetProperties())
            {
                var p1 = p.GetValue(o1);
                var p2 = p.GetValue(o2);
                try
                {
                    path.Push($"{p.Name}[{p.PropertyType.Name}]");
                    var pt = p.PropertyType;
                    if (pt == typeof(object) && p1 != null)
                    {
                        pt = p1.GetType();
                    }
                    CompareStructs(path, pt, p1, p2);
                }
                finally
                {
                    path.Pop();
                }
            }
        }

        private static void CompareArrays(Stack<string> path, object input, object result)
        {
            var aInput = (Array)input;
            var aResult = (Array)result;
            Assert.AreEqual(aInput.Length, aResult.Length);
            for (int i = 0; i < aInput.Length; i++)
            {
                CompareStructs(path, aInput.GetType().GetElementType(), aInput.GetValue(i), aResult.GetValue(i));
            }
        }

        private static void CompareStreams(Stack<string> path, object input, object result)
        {
            var sInput = (Stream)input;
            sInput.Position = 0;
            var msInput = new MemoryStream();
            sInput.CopyTo(msInput);
            sInput.Position = 0;

            var sResult = (Stream)result;
            var msResult = new MemoryStream();
            sResult.CopyTo(msResult);
            CompareArrays(path, msInput.ToArray(), msResult.ToArray());
        }
    }
}
