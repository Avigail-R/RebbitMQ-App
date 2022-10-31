using System;
using System.Text;
using RabbitMQ.Client;

var factory = new ConnectionFactory { HostName = "localhost" };

using var connection = factory.CreateConnection();

using var channel = connection.CreateModel();

channel.ExchangeDeclare(exchange: "myRoutingExchange", ExchangeType.Direct);


var message = "This message needs to be routed";

var encodedMessage = Encoding.UTF8.GetBytes(message);

channel.BasicPublish("myRoutingExchange", "both", null, encodedMessage);

Console.WriteLine($"Published message: {message}");