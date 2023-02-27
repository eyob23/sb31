using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Configuration;
using System.Text.Json;
using System.Threading.Tasks;

namespace sendMessage.ServiceBus
{
    public interface IServiceBusSendMessage
    {
        public Task SendMessage<T>(T payload);
    }
    public class ServiceBusSendMessage : IServiceBusSendMessage
    {
        private readonly ServiceBusClient _client;
        private readonly ServiceBusSender _clientSender;

        public ServiceBusSendMessage(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("ServiceBusConnectionString");
            var _queueName = configuration.GetValue<string>("QueueName");
            _client = new ServiceBusClient(connectionString);
            _clientSender = _client.CreateSender(_queueName);

        }


        public async Task SendMessage<T>(T payload)
        {
            string messagePayload = JsonSerializer.Serialize(payload);
            ServiceBusMessage message = new ServiceBusMessage(messagePayload);
            await _clientSender.SendMessageAsync(message).ConfigureAwait(false);
        }
    }
}