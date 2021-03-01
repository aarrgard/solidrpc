using SolidRpc.Abstractions.Serialization;
using SolidRpc.NpmGenerator.Debugger;
using System;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SolidRpc.Node.Debugger
{
    /// <summary>
    /// The debugger for the node service
    /// </summary>
    public class NodeDebugger
    {
        private static ConcurrentDictionary<string, Type> s_EventTypes = new ConcurrentDictionary<string, Type>();
        private object _mutex = new object();
        private int _id = 0;

        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="host"></param>
        /// <param name="port"></param>
        /// <param name="uid"></param>
        /// <param name="cancellationToken"></param>
        public NodeDebugger(ISerializerFactory serializerFactory, string host, int port, string uid, CancellationToken cancellationToken)
        {
            SerializerFactory = serializerFactory;
            Host = host;
            Port = port;
            Uid = uid;
            CancellationToken = cancellationToken;
        }

        private ISerializerFactory SerializerFactory { get; }
        private string Host { get; }
        private int Port { get; }
        private string Uid { get; }
        private Uri Uri => new Uri($"ws://{Host}:{Port}/{Uid}");
        private CancellationToken CancellationToken { get; }
        private ClientWebSocket ClientWebSocket { get; set; }

        /// <summary>
        /// Connects the debugger
        /// </summary>
        public void Connect()
        {
            ClientWebSocket = new ClientWebSocket();
            ClientWebSocket.ConnectAsync(Uri, CancellationToken).Wait();
            ReceiveAsync();
            SendCommand("Runtime.enable");
            SendCommand("Debugger.enable");
            SendCommand("Runtime.runIfWaitingForDebugger");
        }

        private async Task ReceiveAsync()
        {
            var ms = new MemoryStream();
            var buffer = new ArraySegment<byte>(new byte[1024]);
            while(!CancellationToken.IsCancellationRequested)
            {
                var res = await ClientWebSocket.ReceiveAsync(buffer, CancellationToken);

                switch(res.MessageType)
                {
                    case WebSocketMessageType.Close:
                        await ClientWebSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "server closing", CancellationToken);
                        return;
                    case WebSocketMessageType.Binary:
                    case WebSocketMessageType.Text:
                        await ms.WriteAsync(buffer.Array, buffer.Offset, res.Count);
                        break;
                }

                if(res.EndOfMessage)
                {
                    switch (res.MessageType)
                    {
                        case WebSocketMessageType.Binary:
                            await HandleMessage(ms.ToArray(), CancellationToken);
                            break;
                        case WebSocketMessageType.Text:
                            await HandleMessage(Encoding.UTF8.GetString(ms.ToArray()), CancellationToken);
                            break;
                    }
                    ms = new MemoryStream();
                }
            }
        }

        private Task HandleMessage(string text, CancellationToken cancellationToken)
        {
            try
            {
                V8DebuggerResponse resp;
                SerializerFactory.DeserializeFromString(text, out resp);
                if (!string.IsNullOrEmpty(resp.MethodName))
                {
                    var eventType = s_EventTypes.GetOrAdd(resp.MethodName, CreateEventType);
                    //var evt = ((JObject)resp.Parameters).ToObject(eventType);
                    object evt = null;
                    throw new Exception("Implement!");
                    return HandleEvent(evt);
                }
                else if (resp.Id != null)
                {
                    return HandleResult(resp.Id.Value, resp.Result);
                }
                else
                {
                    throw new Exception("Cannot handle json:"+text);
                }
            }
            catch(Exception e)
            {
                return Task.CompletedTask;
            }
        }

        private Task HandleResult(int value, object result)
        {
            return Task.CompletedTask;
        }

        private Type CreateEventType(string methodName)
        {
            var type = GetType().Assembly.GetTypes()
                .Where(o => o.FullName.EndsWith($".{methodName}", StringComparison.InvariantCultureIgnoreCase))
                .FirstOrDefault();
            if(type != null)
            {
                return type;
            }
            return typeof(object);
        }

        private Task HandleEvent(object evt)
        {
            if (evt is Events.Runtime.ExecutionContextCreated ecc)
            {
                return Task.CompletedTask;
            }
            else if (evt is Events.Debugger.ScriptParsed sp)
            {
                Console.WriteLine("ScriptParsed:" + sp.Url);
                return Task.CompletedTask;
            }
            else if (evt is Events.Debugger.Paused p)
            {
                return Task.CompletedTask;
            }
            else
            {
                return Task.CompletedTask;
            }
        }

        private Task HandleMessage(byte[] binary, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public Task SendCommand(string methodName, object parameters = null)
        {
            var cmd = new V8DebuggerCommand()
            {
                Id = GetCommandId(),
                MethodName = methodName,
                Parameters = parameters
            };

            string str;
            SerializerFactory.SerializeToString(out str, cmd);
            var buffer = new ArraySegment<byte>(Encoding.UTF8.GetBytes(str));
            return ClientWebSocket.SendAsync(buffer, WebSocketMessageType.Text, true, CancellationToken);
        }

        private int GetCommandId()
        {
            lock(_mutex)
            {
                return _id++;
            }
        }
    }
}
