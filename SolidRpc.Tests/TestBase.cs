using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using SolidRpc.OpenApi.Binder.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SolidRpc.Tests
{
    /// <summary>
    /// 
    /// </summary>
    public class TestBase
    {
        /// <summary>
        /// The connect handler
        /// </summary>
        public class ConnectHandler : DelegatingHandler
        {
            /// <summary>
            /// The callbacks
            /// </summary>
            public ICollection<Func<Uri, string>> Callbacks = new List<Func<Uri, string>>();

            /// <summary>
            /// Constructs a new instance
            /// </summary>
            public ConnectHandler()
            {
            }

            /// <summary>
            /// Returns the data
            /// </summary>
            /// <param name="request"></param>
            /// <param name="cancellationToken"></param>
            /// <returns></returns>
            protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                var resp = new HttpResponseMessage();
                foreach (var callback in Callbacks)
                {
                    var strContent = callback(request.RequestUri);
                    if (!string.IsNullOrEmpty(strContent))
                    {
                        resp.StatusCode = System.Net.HttpStatusCode.OK;
                        resp.Content = new StringContent(strContent, Encoding.UTF8, "application/json");
                        return Task.FromResult(resp);
                    }
                }
                resp.StatusCode = System.Net.HttpStatusCode.NotFound;
                return Task.FromResult(resp);
            }
        }

        public class CustomHttpMessageHandlerBuilder : HttpMessageHandlerBuilder
        {
            public CustomHttpMessageHandlerBuilder(ConnectHandler connectHandler)
            {
                ConnectHandler = connectHandler;
            }
            public override string Name { get; set; }
            public override HttpMessageHandler PrimaryHandler { get; set; }
            public override IList<DelegatingHandler> AdditionalHandlers => new List<DelegatingHandler>();
            private ConnectHandler ConnectHandler { get; }

            // Our custom builder doesn't care about any of the above.
            public override HttpMessageHandler Build()
            {
                return ConnectHandler;
            }
        }

        /// <summary>
        /// The logger
        /// </summary>
        public static TaskConsoleLoggerClass TaskConsoleLogger = new TaskConsoleLoggerClass();

        /// <summary>
        /// Console logger implementation
        /// </summary>
        public class TaskConsoleLoggerClass
        {
            SemaphoreSlim logsemaphore = new SemaphoreSlim(0);

            /// <summary>
            /// Constructs a new instance
            /// </summary>
            public TaskConsoleLoggerClass()
            {
                StartLogger();
            }

            private async Task StartLogger()
            {
                while(true)
                {
                    await logsemaphore.WaitAsync();
                    lock (LogItems)
                    {
                        var log = LogItems.Dequeue();
                        Console.WriteLine(log);
                    }
                }
            }

            private Queue<string> LogItems = new Queue<string>();

            /// <summary>
            /// Logs to the console
            /// </summary>
            /// <param name="log"></param>
            public void Log(string log)
            {
                lock(LogItems)
                {
                    LogItems.Enqueue(log);
                }
                logsemaphore.Release();
            }

            /// <summary>
            /// Fluses the data to console
            /// </summary>
            public void Flush()
            {
                lock (LogItems)
                {
                    while(LogItems.TryDequeue(out string log))
                    {
                        Console.WriteLine(log);
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected Stream GetManifestResource(string resourceName)
        {
            var resName = GetType().Assembly.GetManifestResourceNames().FirstOrDefault(o => o.EndsWith(resourceName));
            if(resName == null)
            {
                throw new Exception("Cannot find resource with ending:" + resourceName);
            }
            return GetType().Assembly.GetManifestResourceStream(resName);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="resourceName"></param>
        /// <returns></returns>
        protected string GetManifestResourceAsString(string resourceName)
        {
            using (var sr = new StreamReader(GetManifestResource(resourceName)))
            {
                return sr.ReadToEnd();
            }
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
        /// Configures the logging.
        /// </summary>
        protected void ConfigureLogging(ILoggingBuilder builder)
        {
            builder.SetMinimumLevel(LogLevel.Trace);
            builder.AddConsole(o =>
            {
                o.IncludeScopes = true;
                o.TimestampFormat = "HH:mm:ss.ffff";
            });
            builder.AddProvider(new InvocationLoggingProvider("RequestId"));
            //builder.ClearProviders();
            //builder.AddProvider(new ConsoleProvider(Log));
        }

        protected void AddHttpClient(IServiceCollection services, Func<Uri, string> callback)
        {
            var connectHandler = services.GetSolidRpcService<ConnectHandler>(false);
            if(connectHandler == null)
            {
                connectHandler = new ConnectHandler();
                services.AddSingleton(connectHandler);
            }
            connectHandler.Callbacks.Add(callback);
            
            services.AddHttpClient();
            services.AddSingleton<HttpMessageHandlerBuilder, CustomHttpMessageHandlerBuilder>();
        }

        /// <summary>
        /// Logs a message.
        /// </summary>
        /// <param name="msg"></param>
        protected void Log(string msg)
        {
            TaskConsoleLogger.Log($"[{DateTime.Now.ToString("HH:mm:ss.ffff")}]{msg}");
        }

        /// <summary>
        /// Compares the two structures
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="o1"></param>
        /// <param name="o2"></param>
        /// <returns></returns>
        public static bool CompareTypedStructs<T>(T o1, T o2)
        {
            return CompareStructs(typeof(T), o1, o2);
        }
        /// <summary>
        /// Compares the structs
        /// </summary>
        /// <param name="o1"></param>
        /// <param name="o2"></param>
        public static bool CompareStructs(object o1, object o2)
        {
            Assert.AreEqual(o1?.GetType(), o2?.GetType());
            if(ReferenceEquals(o1, o2))
            {
                return true;
            }
            return CompareStructs(o1.GetType(), o1, o2);
        }

        /// <summary>
        /// Compares the structs
        /// </summary>
        /// <param name="t"></param>
        /// <param name="o1"></param>
        /// <param name="o2"></param>
        public static bool CompareStructs(Type t, object o1, object o2)
        {
            var path = new Stack<string>();
            return CompareStructs(path, t, o1, o2);
        }
        private static bool CompareStructs(Stack<string> path, Type t, object o1, object o2)
        {
            if (ReferenceEquals(o1, o2))
            {
                return true;
            }
            if (o1 == null || o2 == null)
            {
                throw new Exception("One of the objects is null.");
            }
            if (typeof(DateTime).IsAssignableFrom(t))
            {
                Assert.AreEqual(o1, o2);
                return true;
            }
            if (typeof(DateTimeOffset).IsAssignableFrom(t))
            {
                var dto1 = (DateTimeOffset)o1;
                var dto2 = (DateTimeOffset)o2;
                Assert.AreEqual(dto1.ToUniversalTime().Ticks, dto2.ToUniversalTime().Ticks);
                return true;
            }
            if (typeof(string).IsAssignableFrom(t))
            {
                Assert.AreEqual(o1, o2);
                return true;
            }
            if (typeof(CancellationToken).IsAssignableFrom(t))
            {
                return true;
            }
            if (typeof(Stream).IsAssignableFrom(t))
            {
                CompareStreams(path, o1, o2);
                return true;
            }
            if (t.IsArray)
            {
                CompareArrays(path, o1, o2);
                return true;
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
                return true;
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
            return true;
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
