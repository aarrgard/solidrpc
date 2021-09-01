using SolidRpc.Abstractions.OpenApi.Invoker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SolidRpc.OpenApi.Binder.Invoker
{
    /// <summary>
    /// The memory queue bus is responsible for lifting messages between containers.
    /// </summary>
    public class MemoryQueueBus
    {
        private IDictionary<string, IList<MemoryQueueBusMessage>> _messages = new Dictionary<string, IList<MemoryQueueBusMessage>>();
        private IDictionary<string, Func<MemoryQueueBusMessage, Task>> _handlers = new Dictionary<string, Func<MemoryQueueBusMessage, Task>>();

        /// <summary>
        /// Sends a message to supplied queue
        /// </summary>
        /// <param name="queueName"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public Task HandleMessage(string queueName, string message, InvocationOptions options)
        {
            if (_messages.TryGetValue(queueName, out IList<MemoryQueueBusMessage> messages))
            {
                lock (messages)
                {
                    messages.Add(new MemoryQueueBusMessage() { Message = message, Options = options});
                }
                return Task.CompletedTask;
            }
            else
            {
                throw new ArgumentException("Cannot find queue " + queueName);
            }
        }

        public void AddHandler(string queueName, Func<MemoryQueueBusMessage, Task> handler)
        {
            _handlers.Add(queueName, handler);
            _messages.Add(queueName, new List<MemoryQueueBusMessage>());
        }

        public void RemoveHandler(string queueName)
        {
            _handlers.Remove(queueName);
            _messages.Remove(queueName);
        }

        public async Task<int> DispatchAllMessagesAsync()
        {
            var dispatchedMessages = 0;
            foreach(var messageList in _messages)
            {
                var handler = _handlers[messageList.Key];
                var messages = new List<MemoryQueueBusMessage>();
                lock(messageList.Value)
                {
                    messages.AddRange(messageList.Value);
                    messageList.Value.Clear();
                }
                foreach (var message in messages)
                {
                    await handler(message);
                    dispatchedMessages++;
                }
            }
            return dispatchedMessages;
        }

        public IEnumerable<MemoryQueueBusMessage> GetMessages()
        {
            return _messages.Values.SelectMany(o => o);
        }
    }
}
