
using Azure.Messaging.ServiceBus;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mango.MessageBus
{
    public class AzureServiceBusMessageBus : IMessageBus
    {
        //can be improved, add it to the appsettings
        private string connectionString = "Endpoint=sb://mangorestaurantsafaa.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=fRNcnNWIFaGEOtGe+zn+iSh+AOmZWqwOlqWschmxuMQ=";
        public async Task PublishMessage(BaseMessage baseMessage, string topicName)
        {
            //where the topic is, to send the msg
            await using var client = new ServiceBusClient(connectionString);

            ServiceBusSender sender= client.CreateSender(topicName);

            var jsonMessage=JsonConvert.SerializeObject(baseMessage);
            ServiceBusMessage finalMessage = new ServiceBusMessage(Encoding.UTF8.GetBytes(jsonMessage))
            {
                CorrelationId = Guid.NewGuid().ToString()
            };

            await sender.SendMessageAsync(finalMessage);
            await client.DisposeAsync();
        }
    }
}
