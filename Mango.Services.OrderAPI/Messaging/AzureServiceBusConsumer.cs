﻿using Azure.Messaging.ServiceBus;
using Mango.Services.OrderAPI.Messages;
using Mango.Services.OrderAPI.Models;
using Mango.Services.OrderAPI.Repository;
using Newtonsoft.Json;
using System.Text;
using System.Text.Json.Serialization;

namespace Mango.Services.OrderAPI.Messaging
{
    public class AzureServiceBusConsumer: IAzureServiceBusConsumer
    {
        private readonly string serviceBusConnectionString;
        private readonly string checkoutMessageTopic;
        private readonly string subscriptionCheckout;
        private readonly OrderRepository _orderRepository;

        private readonly ServiceBusProcessor checkoutProcessor;

        private readonly IConfiguration _configuration;

        public AzureServiceBusConsumer(OrderRepository orderRepository, IConfiguration configuration)
        {
            _orderRepository = orderRepository;
            _configuration = configuration;

            serviceBusConnectionString = _configuration.GetValue<string>("ServiceBusConnectionString");
            checkoutMessageTopic = _configuration.GetValue<string>("CheckoutMessageTopic");
            subscriptionCheckout = _configuration.GetValue<string>("SubscriptionCheckout");


            var client = new ServiceBusClient(serviceBusConnectionString);
            checkoutProcessor = client.CreateProcessor(checkoutMessageTopic, subscriptionCheckout);
        }

        public async Task Start()
        {
            checkoutProcessor.ProcessMessageAsync += onCheckMessageRecieved;
            checkoutProcessor.ProcessErrorAsync += ErrorHandler;
            await checkoutProcessor.StartProcessingAsync();
        }

        public async Task Stop()
        {
            await checkoutProcessor.StopProcessingAsync();
            await checkoutProcessor.DisposeAsync();
        }

        Task ErrorHandler(ProcessErrorEventArgs args)
        {
            Console.WriteLine(args.Exception.ToString());
            return Task.CompletedTask;
        }
        private async Task onCheckMessageRecieved(ProcessMessageEventArgs args)
        {
            var message = args.Message;

            var body = Encoding.UTF8.GetString(message.Body);

            CheckoutHeaderDto checkoutHeaderDto= JsonConvert.DeserializeObject<CheckoutHeaderDto>(body);

            OrderHeader orderHeader = new OrderHeader()
            {
                CardNumber=checkoutHeaderDto.CardNumber,
                OrderDetails=new List<OrderDetails>(),
                CouponCode= checkoutHeaderDto.CouponCode,
                CVV= checkoutHeaderDto.CVV,
                DiscoutTotal=checkoutHeaderDto.DiscoutTotal,
                Email= checkoutHeaderDto.Email,
                ExpiryMonthYear=checkoutHeaderDto.ExpiryMonthYear,
                FirstName=checkoutHeaderDto.FirstName,
                LastName=checkoutHeaderDto.LastName,
                OrderTime=DateTime.Now,
                OrderTotal=checkoutHeaderDto.OrderTotal,
                PaymentStatus=false,
                UserId=checkoutHeaderDto.UserId,
                PickupDateTime= checkoutHeaderDto.PickupDateTime,
                Phone= checkoutHeaderDto.Phone
            };
            
            foreach(var detailsList in checkoutHeaderDto.CartDetails)
            {
                OrderDetails orderDetails = new OrderDetails()
                {
                    ProductId=detailsList.ProductId,
                    Count=detailsList.Count,
                    Price=detailsList.Product.Price,
                    ProdcutName=detailsList.Product.Name
                };
                orderHeader.CartTotalItems += detailsList.Count;
                orderHeader.OrderDetails.Add(orderDetails);
            }

            await _orderRepository.AddOrderAsync(orderHeader);
        }
    }
}
