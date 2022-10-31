using System;
using System.Text;
using RabbitMQ.Client;

var factory = new ConnectionFactory { HostName = "localhost" };

using var connection = factory.CreateConnection();

using var channel = connection.CreateModel();

channel.ExchangeDeclare(exchange: "myTopicExchange", ExchangeType.Topic);


var userPaymentMessage = "A european user paid for something";

var userPaymentBody = Encoding.UTF8.GetBytes(userPaymentMessage);

channel.BasicPublish(exchange: "myTopicExchange", "user.europe.payments", null, userPaymentBody);

Console.WriteLine($"Published message: {userPaymentMessage}");


var businessOrderMessage = "A european  business ordered goods";

var businessOrderBody = Encoding.UTF8.GetBytes(businessOrderMessage);

channel.BasicPublish(exchange: "myTopicExchange", "business.europe.order", null, businessOrderBody);

Console.WriteLine($"Published message: {businessOrderMessage}");