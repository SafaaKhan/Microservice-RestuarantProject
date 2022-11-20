using Azure.Messaging.ServiceBus;
using Mango.Services.Email.Messages;
using Mango.Services.Email.Repository;
using Newtonsoft.Json;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json.Serialization;

namespace Mango.Services.Email.Messaging
{
    public class AzureServiceBusConsumer : IAzureServiceBusConsumer
    {
        private readonly string serviceBusConnectionString;
        private readonly string orderUpdatePaymentResultTopic;
        private readonly string subscriptionEmail;

        private readonly EmailRepository _emailRepository;

        private readonly ServiceBusProcessor orderUpdatePaymentStatusProcessor;


        private readonly IConfiguration _configuration;

        public AzureServiceBusConsumer(EmailRepository emailRepository,
            IConfiguration configuration)
        {
            _emailRepository = emailRepository;
            _configuration = configuration;


            serviceBusConnectionString = _configuration.GetValue<string>("ServiceBusConnectionString");
            orderUpdatePaymentResultTopic = _configuration.GetValue<string>("OrderUpdatePaymentResultTopic");
            subscriptionEmail = _configuration.GetValue<string>("MangoEmailSubscription");


            var client = new ServiceBusClient(serviceBusConnectionString);
            orderUpdatePaymentStatusProcessor = client.CreateProcessor(orderUpdatePaymentResultTopic, subscriptionEmail);
        }

        public async Task Start()
        {
            orderUpdatePaymentStatusProcessor.ProcessMessageAsync += onPaymentUpdateRecieved;
            orderUpdatePaymentStatusProcessor.ProcessErrorAsync += ErrorHandler;
            await orderUpdatePaymentStatusProcessor.StartProcessingAsync();
        }

        public async Task Stop()
        {
            await orderUpdatePaymentStatusProcessor.StopProcessingAsync();
            await orderUpdatePaymentStatusProcessor.DisposeAsync();
        }

        Task ErrorHandler(ProcessErrorEventArgs args)
        {
            Console.WriteLine(args.Exception.ToString());
            return Task.CompletedTask;
        }



        private async Task onPaymentUpdateRecieved(ProcessMessageEventArgs args)
        {
            var message = args.Message;

            var body = Encoding.UTF8.GetString(message.Body);

            UpdatePaymentResultMessage updatePaymentResultMessage = JsonConvert.DeserializeObject<UpdatePaymentResultMessage>(body);


            try
            {
                await _emailRepository.SendAndLogEmailAsync(updatePaymentResultMessage);
                await args.CompleteMessageAsync(args.Message);
            }
            catch (Exception ex)
            {
                //write log error
                throw;
            }
        }
    }
}
