using Azure.Messaging.ServiceBus;
using Mango.MessageBus;
using Mango.Services.PaymentAPI.Messages;
using Newtonsoft.Json;
using PaymentProcessor;
using System.Text;
using System.Text.Json.Serialization;

namespace Mango.Services.PaymentAPI.Messaging
{
    public class AzureServiceBusConsumer : IAzureServiceBusConsumer
    {
        private readonly string serviceBusConnectionString;
        private readonly string orderPaymentProcessTopic;
        private readonly string subscriptionPayment;
        private readonly string orderUpdatePaymentResultTopic;

        private readonly ServiceBusProcessor orderPaymentProcessor;
        private readonly IProcessPayment _processPayment;
        private readonly IMessageBus _messageBus;

        private readonly IConfiguration _configuration;

        public AzureServiceBusConsumer(IProcessPayment processPayment,
            IConfiguration configuration,
            IMessageBus messageBus)
        {
            _configuration = configuration;
            _messageBus = messageBus;
            _processPayment = processPayment;

            serviceBusConnectionString = _configuration.GetValue<string>("ServiceBusConnectionString");
            orderPaymentProcessTopic = _configuration.GetValue<string>("OrderPaymentProcessTopic");
            subscriptionPayment = _configuration.GetValue<string>("OrderPaymentProcessSubscription");
            orderUpdatePaymentResultTopic = _configuration.GetValue<string>("orderupdatepaymentresulttopic");


            var client = new ServiceBusClient(serviceBusConnectionString);
            orderPaymentProcessor = client.CreateProcessor(orderPaymentProcessTopic, subscriptionPayment);
        }

        public async Task Start()
        {
            orderPaymentProcessor.ProcessMessageAsync += ProcessPayments;
            orderPaymentProcessor.ProcessErrorAsync += ErrorHandler;
            await orderPaymentProcessor.StartProcessingAsync();
        }

        public async Task Stop()
        {
            await orderPaymentProcessor.StopProcessingAsync();
            await orderPaymentProcessor.DisposeAsync();
        }

        Task ErrorHandler(ProcessErrorEventArgs args)
        {
            Console.WriteLine(args.Exception.ToString());
            return Task.CompletedTask;
        }
        private async Task ProcessPayments(ProcessMessageEventArgs args)
        {
            var message = args.Message;

            var body = Encoding.UTF8.GetString(message.Body);

            PaymentRequestMessage paymentRequestMessage = JsonConvert.DeserializeObject<PaymentRequestMessage>(body);

            var result = _processPayment.PaymentProcessor(); //in real world, the payment gateway must check the payment info
            UpdatePaymentResultMessage updatePaymentResultMessage = new UpdatePaymentResultMessage()
            {
                OrderId=paymentRequestMessage.OrderId,
                Status=result,
                Email=paymentRequestMessage.Email
            };


            
            try
            {
                await _messageBus.PublishMessage(updatePaymentResultMessage, orderUpdatePaymentResultTopic);
                await args.CompleteMessageAsync(args.Message);
            }
            catch(Exception ex)
            {
                //write log error
                throw;
            }
        }
    }
}
