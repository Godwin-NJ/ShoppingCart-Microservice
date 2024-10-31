using Azure.Messaging.ServiceBus;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Mango.MessageBus
{
    public class MessageBus : IMessageBus
    {
        public async Task PublishMessage(object message, string topic_queue_name)
        {
            await using var client = new ServiceBusClient("service bus connection string");
            ServiceBusSender sender = client.CreateSender(topic_queue_name);
            var jsonMsg = JsonConvert.SerializeObject(message);
            ServiceBusMessage Finalmsg = new ServiceBusMessage(Encoding.UTF8.GetBytes(jsonMsg))
            {
                CorrelationId = Guid.NewGuid().ToString(),
            };
            sender.SendMessageAsync(Finalmsg).Wait();
            await client.DisposeAsync();
        }
    }
}
